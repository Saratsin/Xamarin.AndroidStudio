using Mono.Addins;
using Mono.Addins.Description;
using MonoDevelop;

[assembly: Addin("Xamarin.AndroidStudio", Version = "0.7.0")]

[assembly: AddinName("Xamarin.AndroidStudio")]
[assembly: AddinCategory("IDE extensions")]
[assembly: AddinDescription("Xamarin.AndroidStudio is a Visual Studio extension that allows you to manage your Xamarin.Android project resources with Android Studio")]
[assembly: AddinAuthor("Taras Shevchuk")]
[assembly: AddinUrl("https://github.com/Saratsin/Xamarin.AndroidStudio")]

[assembly: AddinDependency("::MonoDevelop.Core", BuildInfo.Version)]
[assembly: AddinDependency("::MonoDevelop.Ide", BuildInfo.Version)]
[assembly: AddinDependency("::MonoDevelop.TextEditor", BuildInfo.Version)]