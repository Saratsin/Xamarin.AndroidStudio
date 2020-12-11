using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.AndroidStudio.Extension.Mac.Extensions;

namespace Xamarin.AndroidStudio.Extension.Mac.Plugins.AndroidStudio
{
    public class AndroidStudioPlugin : IAndroidStudioPlugin
    {
        public const string AndroidStudioProcessName = "studio";
        public const string AndroidStudioExecutableFilePath = "/Applications/Android Studio.app/Contents/MacOS/studio";

        private static readonly string[] ExcludedFileNames =
        {
            ".DS_Store",
            "Resource.designer.cs",
            "Resource.Designer.cs"
        };

        private string[] _androidResourceFilePathsSnapshot;
        private bool _isSubscribedForAndroidStudioProcessExit;
        private string _currentStatusBarMessage;

        public void OpenAndroidStudioProject(string androidResourcesDirectoryPath,
                                             string androidStudioProjectDirectoryPath,
                                             string specificFilePath = null)
        {
            _ = ShowOpeningStatusBarMessageAsync(specificFilePath);

            var arguments = specificFilePath is null
                ? androidStudioProjectDirectoryPath :
                $"{androidStudioProjectDirectoryPath} {specificFilePath}";

            var androidStudioProcess = Process.GetProcessesByName(AndroidStudioProcessName)
                                              .FirstOrDefault(process => process.MainModule.FileName is AndroidStudioExecutableFilePath);
            if (androidStudioProcess is null)
            {
                androidStudioProcess = Process.Start(AndroidStudioExecutableFilePath, arguments);
            }
            else
            {
                // TODO Investigate about way to pass command to the existing android studio process
                Process.Start(AndroidStudioExecutableFilePath, arguments);
            }

            if (_isSubscribedForAndroidStudioProcessExit)
            {
                return;
            }
            
            _ = ObserveAndroidStudioProcessAsync(androidResourcesDirectoryPath, androidStudioProcess);
        }

        private async Task ObserveAndroidStudioProcessAsync(string androidResourcesDirectoryPath, Process androidStudioProcess)
        {
            _androidResourceFilePathsSnapshot = GetAndroidResourceFilePaths(androidResourcesDirectoryPath);

            _isSubscribedForAndroidStudioProcessExit = true;
            IdeApp.FocusIn += OnVisualStudioFocused;

            await androidStudioProcess.SafeWaitForExitAsync();

            IdeApp.FocusIn -= OnVisualStudioFocused;
            _isSubscribedForAndroidStudioProcessExit = false;

            await SyncResourcesAsync(androidResourcesDirectoryPath);

            _androidResourceFilePathsSnapshot = null;

            void OnVisualStudioFocused(object sender, EventArgs e)
            {
                _ = SyncResourcesAsync(androidResourcesDirectoryPath);
            }
        }

        private async Task SyncResourcesAsync(string androidResourcesDirectoryPath)
        {
            ShowStatusBarMessage("Checking changes from Android Studio");

            var currentAndroidResourceFilePaths = GetAndroidResourceFilePaths(androidResourcesDirectoryPath);
            var isNothingChanged = currentAndroidResourceFilePaths.SequenceEqual(_androidResourceFilePathsSnapshot);

            await Task.Delay(1000);

            if (isNothingChanged)
            {
                await ShowNoAddedOrRemovedResourcesStatusBarMessageAsync();
                return;
            }

            ShowStatusBarMessage("Some files were added or removed by Android Studio. Please, update your csproj manually", Stock.StatusWarning);

            //var synchronizingMessage = "Synchronizing changes from Android Studio";
            //ShowStatusBarMessage(synchronizingMessage);

            _androidResourceFilePathsSnapshot = currentAndroidResourceFilePaths;

            //await Task.Delay(1000);
            //if (_currentStatusBarMessage != synchronizingMessage)
            //{
            //    return;
            //}
            //
            //await ShowStatusBarMessageAsync("Updated Resources from Android Studio", 2, Stock.StatusSuccess);
        }

        private string[] GetAndroidResourceFilePaths(string androidResourcesDirectoryPath)
        {
            var androidResourcesDirectoryInfo = new DirectoryInfo(androidResourcesDirectoryPath);
            var androidResourceFilePaths = androidResourcesDirectoryInfo.EnumerateFiles("*.*", SearchOption.AllDirectories)
                                                                        .Where(fileInfo => !ExcludedFileNames.Contains(fileInfo.Name))
                                                                        .Select(fileInfo => fileInfo.FullName)
                                                                        .ToArray();
            return androidResourceFilePaths;
        }

        private Task ShowOpeningStatusBarMessageAsync(string specificFilePath = null)
        {
            var message = specificFilePath is null
                ? "Opening Resources in Android Studio"
                : $"Opening {new FileInfo(specificFilePath).Name}";

            return ShowStatusBarMessageAsync(message, 3);
        }

        private Task ShowNoAddedOrRemovedResourcesStatusBarMessageAsync()
        {
            var message = "No added or removed resources from Android Studio. Everything is synchronized";
            return ShowStatusBarMessageAsync(message, 3, Stock.StatusSuccess);
        }

        private async Task ShowStatusBarMessageAsync(string message, int seconds, IconId? image = null)
        {
            ShowStatusBarMessage(message, image);

            await Task.Delay(TimeSpan.FromSeconds(seconds));

            if (_currentStatusBarMessage != message)
            {
                return;
            }

            ResetStatusBarMessage();
        }

        private void ShowStatusBarMessage(string message, IconId? image = null)
        {
            _currentStatusBarMessage = message;
            IdeApp.Workbench.StatusBar.ShowMessage(image ?? Stock.StatusWorking, message);
        }

        private void ResetStatusBarMessage()
        {
            IdeApp.Workbench.StatusBar.ShowReady();
        }
    }
}