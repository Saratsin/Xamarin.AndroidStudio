using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using MonoDevelop.Projects;
using System.IO;
using System.Linq;

namespace Xamarin.AndroidStudio.Extension.Mac.CommandHandlers
{
    public class OpenResourcesInAndroidStudioCommandHandler : CommandHandler
    {
        protected override void Run()
        {
            var androidResourcesProjectFolder = IdeApp.Workspace.CurrentSelectedItem as ProjectFolder;
            var androidResourcesDirectoryPath = androidResourcesProjectFolder.Path;
            var projectDirectoryPath = Path.GetDirectoryName(androidResourcesDirectoryPath);
            var androidStudioProjectDirectoryPath = FindAndroidStudioProjectDirectoryPathOrDefault(projectDirectoryPath);
            if (androidStudioProjectDirectoryPath is null)
            {
                IdeApp.Workbench.StatusBar.ShowMessage(Stock.StatusError, "No Android Studio project found, please add it to your Xamarin.Android project folder");
                return;
            }

            Container.AndroidStudioPlugin.OpenAndroidStudioProject(androidResourcesDirectoryPath, androidStudioProjectDirectoryPath);
        }

        private string FindAndroidStudioProjectDirectoryPathOrDefault(string projectDirectoryPath)
        {
            var projectDirectoryInfo = new DirectoryInfo(projectDirectoryPath);
            var projectSubdirectoriesInfo = projectDirectoryInfo.EnumerateDirectories();
            foreach (var subdirectoryInfo in projectSubdirectoriesInfo)
            {
                var subdirectoryGradleFiles = subdirectoryInfo.GetFiles("build.gradle", SearchOption.TopDirectoryOnly);
                if (subdirectoryGradleFiles.Any())
                {
                    return subdirectoryInfo.FullName;
                }
            }

            return null;
        }

        protected override void Update(CommandInfo info)
        {
            var isAndroidResourcesFolderSelected = GetIsAndroidResourcesFolderSelected();

            info.Enabled = isAndroidResourcesFolderSelected;
            info.Visible = isAndroidResourcesFolderSelected;
        }

        private bool GetIsAndroidResourcesFolderSelected()
        {
            var currentSelectedItem = IdeApp.Workspace.CurrentSelectedItem;
            if (currentSelectedItem is not ProjectFolder currentProjectFolder)
            {
                return false;
            }

            var currentProject = currentProjectFolder.Parent as DotNetProject;
            if (currentProject is null)
            {
                return false;
            }

            var targetFrameworkIdentifier = currentProject.TargetFramework.Id.Identifier;
            if (targetFrameworkIdentifier is not "MonoAndroid")
            {
                return false;
            }

            return currentProjectFolder.Name is "Resources";
        }
    }
}