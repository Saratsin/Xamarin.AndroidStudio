using System;
using Xamarin.AndroidStudio.Extension.Mac.Plugins.AndroidStudio;

namespace Xamarin.AndroidStudio.Extension.Mac
{
    public static class Container
    {
        private static readonly Lazy<IAndroidStudioPlugin> _androidStudioPluginLazy = new Lazy<IAndroidStudioPlugin>(() => new AndroidStudioPlugin());

        public static IAndroidStudioPlugin AndroidStudioPlugin => _androidStudioPluginLazy.Value;

    }
}
