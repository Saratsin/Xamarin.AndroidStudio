namespace Xamarin.AndroidStudio.Extension.Mac.Plugins.AndroidStudio
{
    public interface IAndroidStudioPlugin
    {
        void OpenAndroidStudioProject(string androidResourcesDirectoryPath,
                                      string androidStudioProjectDirectoryPath,
                                      string specificFilePath = null);
    }
}