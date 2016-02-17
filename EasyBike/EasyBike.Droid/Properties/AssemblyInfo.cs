using System.Reflection;
using Android.App;

// Information about this assembly is defined by the following attributes.
// Change them to the values specific to your project.

[assembly: AssemblyTitle("EasyBike")]
[assembly: AssemblyDescription("EasyBike is a mobile app that provides public bikes availability around the world.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("The Public Bike Gang")]
[assembly: AssemblyProduct("EasyBike")]
[assembly: AssemblyCopyright(@"EasyBike is a mobile app that provides public bikes availability around the world.
    Copyright(C) 2016 Marc Magnin

    This program is free software: you can redistribute it and / or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.If not, see http://www.gnu.org/licenses/.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// The assembly version has the format "{Major}.{Minor}.{Build}.{Revision}".
// The form "{Major}.{Minor}.*" will automatically update the build and revision,
// and "{Major}.{Minor}.{Build}.*" will update just the revision.

[assembly: AssemblyVersion("1.0.0")]

// The following attributes are used to specify the signing key for the assembly,
// if desired. See the Mono documentation for more information about signing.

//[assembly: AssemblyDelaySign(false)]
//[assembly: AssemblyKeyFile("")]

// About Xamarin.Plugins/Geolocator/
// https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Geolocator
//The ACCESS_COARSE_LOCATION & ACCESS_FINE_LOCATION permissions are required, but the library will automatically add this for you.
//Additionally, if your users are running Marshmallow the Plugin will automatically prompt them for runtime permissions.
//By adding these permissions Google Play will automatically filter out devices without specific hardward. 
//You can get around this by adding the following to your AssemblyInfo.cs file in your Android project:
[assembly: UsesFeature("android.hardware.location", Required = false)]
[assembly: UsesFeature("android.hardware.location.gps", Required = false)]
[assembly: UsesFeature("android.hardware.location.network", Required = false)]

//#if DEBUG
//[assembly: Application(Debuggable=true)]
//#else
//[assembly: Application(Debuggable=false)]
//#endif