using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;
using System.IO;
using System.Linq;

namespace Xamarin.AndroidStudio.Extension.Mac.CommandHandlers
{
    public class OpenFileInAndroidStudioCommandHandler : CommandHandler
    {
        protected override void Run()
        {
            var resourceFile = IdeApp.Workspace.CurrentSelectedItem as ProjectFile;
            var resourceFilePath = resourceFile.FilePath.ToString();
            var projectDirectoryPath = resourceFile.Project.ItemDirectory.FullPath.ToString();
            var androidResourcesDirectoryPath = Path.Combine(projectDirectoryPath, "Resources");
            var androidStudioProjectDirectoryPath = FindAndroidStudioProjectDirectoryPathOrDefault(projectDirectoryPath);
            if (androidStudioProjectDirectoryPath is null)
            {
                IdeApp.Workbench.StatusBar.ShowMessage(Stock.StatusError, "No Android Studio project found, please add it to your Xamarin.Android project folder");
                return;
            }

            Container.AndroidStudioPlugin.OpenAndroidStudioProject(androidResourcesDirectoryPath, androidStudioProjectDirectoryPath, resourceFilePath);
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
            var isAndroidResourceFileSelected = GetIsAndroidResourceFileSelected();

            info.Enabled = isAndroidResourceFileSelected;
            info.Visible = isAndroidResourceFileSelected;
        }

        private bool GetIsAndroidResourceFileSelected()
        {
            var currentSelectedItem = IdeApp.Workspace.CurrentSelectedItem;
            if (currentSelectedItem is not ProjectFile currentProjectFile)
            {
                return false;
            }

            var currentProject = currentProjectFile.Project as DotNetProject;
            if (currentProject is null)
            {
                return false;
            }

            var targetFrameworkIdentifier = currentProject.TargetFramework.Id.Identifier;
            if (targetFrameworkIdentifier is not "MonoAndroid")
            {
                return false;
            }

            var resourcesDirectoryPath = Path.Combine(currentProject.ItemDirectory.FullPath, "Resources");
            var isResourcesDirectoryFile = currentProjectFile.FilePath.ToString().StartsWith(resourcesDirectoryPath);

            return isResourcesDirectoryFile;
        }
    }
}
