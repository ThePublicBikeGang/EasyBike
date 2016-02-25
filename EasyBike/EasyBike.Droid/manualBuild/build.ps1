# First clean the Release target.
msbuild.exe ../EasyBike.Droid.csproj /p:Configuration=Release /t:Clean

# Now build the project, using the Release target.
msbuild.exe ../EasyBike.Droid.csproj /p:Configuration=Release /t:PackageForAndroid

# At this point there is only the unsigned APK - sign it.
# The script will pause here as jarsigner prompts for the password.
# It is possible to provide they keystore password for jarsigner.exe by adding an extra command line parameter -storepass, for example
#    -storepass <MY_SECRET_PASSWORD>
# If this script is to be checked in to source code control then it is not recommended to include the password as part of this script.
& 'C:\Program Files (x86)\Java\jdk1.7.0_55\bin\jarsigner.exe' -verbose -sigalg SHA1withRSA -digestalg SHA1  -keystore ./easybike.keystore -signedjar ../bin/Release/com.easybikeapp-armeabi-v7a-signed.apk ../bin/Release/com.easybikeapp-armeabi-v7a.apk easybike


# Now zipalign it.  The -v parameter tells zipalign to verify the APK afterwards.
& 'C:\Program Files (x86)\Android\android-sdk\build-tools\22.0.1\zipalign.exe' -f -v 4 ../bin/Release/com.easybikeapp-armeabi-v7a-signed.apk ./easybike.apk
