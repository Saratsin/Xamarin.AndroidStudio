# Xamarin.AndroidStudio

Standard projects created for work with Android Resources of 
the Xamarin.Android project inside the Android Studio

Requirements:
1. All resource files names should contain only lower case english letters, digits or underscore. Other symbols are prohibited (you'll get an error during the build in Android Studio)
2. All layouts should have .xml extension, .axml extension is not supported by AndroidStudio

Instruction:
1. Copy the AndroidStudio folder to your Android project folder (where you have Resources folder).
 
   Note: use AndroidX.AndroidStudio folder if you have AndroidX
2. Open Android Studio, tap File -> Open -> Select AndroidStudio folder inside your Android project folder and open it
3. If you have some non-default views: add related libraries references to the build.gradle file
4. Build the project (green hammer button)
5. Enjoy using it
