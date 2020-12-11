# Xamarin.AndroidStudio

Xamarin.AndroidStudio is a Visual Studio extension that allows you to manage your Xamarin.Android project resources with Android Studio

Standard projects created for work with Android Resources of 
the Xamarin.Android project inside the Android Studio

## Downloads
[![VSMAC](https://img.shields.io/badge/Visual%20Studio%20for%20Mac%202019-v0.7.0-blue.svg)](https://addins.monodevelop.com/Project/Index/409)

## Getting Started

### Requirements:
1. All resource files names should contain only lower case english letters, digits or underscore in your Xamarin.Android project. Other symbols are prohibited (you'll get an error during the build in Android Studio)
2. All layouts files in your Xamarin.Android project should have .xml extension, .axml extension is not supported by Android Studio

### Instruction:
1. Unzip the AndroidStudio archive in your Xamarin.Android project folder (where you have Resources folder).
 
   Note: use AndroidX.AndroidStudio archive if you have AndroidX
2. Open your solution. Now you have several options to open your resources with Android Studio:
   1. Press Command + Shift + A
   2. Right click on your Resources folder, then select "Open in Android Studio"
   3. Right click on any file inside your Resources folder, select Open with, then select "Android Studio (extensinon)"
3. If you have added or removed some resources within Android Studio, Visual Studio will show you kind message that you have to manually update your csproj file for Xamarin.Android project (automation of this process will come soon). Any changes inside files works instantly (Android Studio use files by references so these are the same files as in Xamarin.Android project)
4. Enjoy using it.

### Side notes:
1. There might be some issues with layouts and resources not displaying inside Android Studio. It's required to make at least one build of Android Studio project (just press the green hammer butotn and wait a little)
2. Also if you have some non-default views which were ported from native Android, then add related libraries references to the build.gradle file of your Android Studio project