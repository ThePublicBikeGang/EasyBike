With the Google Maps Android API, you can add maps based on Google Maps data to your application. The API automatically handles access to Google Maps servers, data downloading, map display, and response to map gestures. You can also use API calls to add markers, polygons, and overlays to a basic map, and to change the user's view of a particular map area. These objects provide additional information for map locations, and allow user interaction with the map.



Required Android API Levels
===========================

We recommend setting your app's *Target Framework* and *Target Android version* to **Android 5.0 (API Level 21)** or higher in your app project settings.

This Google Play Service SDK's requires a *Target Framework* of at least Android 4.1 (API Level 16) to compile.

You may still set a lower *Minimum Android version* (as low as Android 2.3 - API Level 9) so your app will run on older versions of Android, however you must ensure you do not use any API's at runtime that are not available on the version of Android your app is running on.




Google Developers Console Setup
=================================

Many of the Google Play Services SDK's require that you create an application inside the [Google Developers Console][1].  Visit the [Google Developers Console][1] to create a project for your application.

Once you have created a project for your Android app, enable the necessary APIs in the developer console for the Google Play Services APIs you will be using in your app.



The *Google Maps Android API v2* API must be enabled in the Developers Console to use the Maps APIs in your app.

If you are using the Places APIs, the *Google Places API for Android* API must also be enabled in the Developers Console.



Credentials
-----------

Some Google Play Services APIs require an *API Key* or an *OAuth 2.0 Client ID* (or both) to be setup to allow your app to make authenticated calls against the API.

In the Developers Console, in your app's Project, under the *APIs & auth* section, go to *Credentials*.





### API Key

If the Google Play Services API you are using requires an API Key:

  1. *Add credentials* button and then *API key*
  2. Choose *Android key*
  3. Click *Add package name and fingerprint*
  4. Enter your android app's package name as found in your *AndroidManifest.xml* file
  5. [Find your SHA-1 fingerprints][2]
  6. Enter your SHA-1 fingerprint of your app's debug keystore
  7. Repeat steps 4-6 with the package name and SHA-1 of the keystore file you will be signing your app's Release builds with
  8. Click *Create*
  9. Note the *API key* value you generated

Once you have your API key value, you will need to add this to your *AndroidManifest.xml* as a metadata value either by directly editing the manifest file, or using an assembly level attribute which will generate the value in the manifest file for you.  The metadata key will be different for each Google Play Services API.  For example, if you are adding it for Maps, you could add this assembly level attribute to your project:

```csharp
[assembly: MetaData ("com.google.android.maps.v2.API_KEY", Value="YOUR-API-KEY")]
```


Once you have created your API key, you must add it as a metadata value in your *AndroidManifest.xml* file.  You can add it by including the following assembly level attribute in your app:

```csharp
[assembly: MetaData ("com.google.android.maps.v2.API_KEY", Value="YOUR-API-KEY")]
```



Android Manifest 
================

Some Google Play Services APIs require specific metadata, attributes, permissions or features to be declared in your *AndroidManifest.xml* file.

These can be added manually to the *AndroidManifest.xml* file, or merged in through the use of assembly level attributes.


The SDK requires the *Internet*, *AccessNetworkState*, *AccessCourseLocation*, *AccessFineLocation*, *WriteExternalStorage* and *AccessMockLocation* permissions to work correctly.  You can have these automatically added to your *AndroidManifest.xml* file by including the following assembly level attributes:

```csharp
[assembly: UsesPermission (Android.Manifest.Permission.Internet)]
[assembly: UsesPermission (Android.Manifest.Permission.AccessNetworkState)]
[assembly: UsesPermission (Android.Manifest.Permission.AccessCourseLocation)]
[assembly: UsesPermission (Android.Manifest.Permission.AccessFineLocation)]
[assembly: UsesPermission (Android.Manifest.Permission.AccessMockLocation)]
[assembly: UsesPermission (Android.Manifest.Permission.WriteExternalStorage)]
```



Samples
=======

You can find a Sample Application within each Google Play Services component.  The sample will demonstrate the necessary configuration and some basic API usages.





Attribution Requirements
========================

If you use the Google Maps Android API in your application, you must include the Google Play Services attribution text as part of a "Legal Notices" section in your application. Including legal notices as an independent menu item, or as part of an "About" menu item, is recommended.

The attribution text is available by making a call to `GoogleApiAvailability.OpenSourceSoftwareLicenseInfo`




Learn More
==========

You can learn more about the various Google Play Services SDKs & APIs by visiting the official [Google APIs for Android][3] documentation


You can learn more about Google Play Services Maps by visiting the official [Google Maps for Android API](https://developers.google.com/maps/documentation/android-api/) documentation.



[1]: https://console.developers.google.com/ "Google Developers Console"
[2]: https://developer.xamarin.com/guides/android/deployment,_testing,_and_metrics/MD5_SHA1/ "Finding your SHA-1 Fingerprints"
[3]: https://developers.google.com/android/ "Google APIs for Android"

