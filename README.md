## EasyBike: A cross platform app for Public Bikes powered by Xamarin.

![](https://github.com/ThePublicBikeGang/EasyBike/blob/master/EasyBike/EasyBike/Assets/phones.png?raw=true)


The objective of the project is to store on a shared library all the integration logic of public bike services.
All services can be easily accessed with common interfaces.

### Sources to help integration
##### The Bike-sharing map
* https://www.google.com/maps/d/viewer?mid=zGPlSU9zZvZw.kmqv_ul1MfkI

##### The Bike-sharing map twitter
* https://twitter.com/BikesharingMap

### List of cities 
https://docs.google.com/spreadsheets/d/13Tim2Nvd41jKQim8dkglOsFcO_iTW7OBWGuAcSQrQUo/pubhtml

## ContractService
This service provide access to the different contracts already implemented within the common library.

```C#
// Returns the list of contracts available.
List<Contract> GetStaticContracts

// Returns the list of the conctracts already downloaded on the device.
Task<List<Contract>> GetContractsAsync() 

// Remove the specified contract from the device.
Task RemoveContractAsync(Contract contract)

// Store the specified contract in the device.
Task AddContractAsync(Contract contract)

// Get all stations across all downloaded contracts.
List<Station> GetStations()
```

## RefreshService
This service is responsible to refresh the stations. It will automatically refresh stations on a regular basis.
The only thing you have to do it to attach to the event *ContractRefreshed*

```C#
// This event will be triggered when a contract as been refreshed. The sender is the refreshed contract reference.
// Attach to it to refresh the UI when required.
EventHandler ContractRefreshed
```

## ViewModels

more to come... ;)

## Packages 

PublicBikes takes advantage of multiple third parties plugins.

* MVVMLight: https://mvvmlight.codeplex.com/ (doc: http://www.mvvmlight.net/doc)
* Akavache: https://github.com/akavache/Akavache
* JSON.NET: http://www.newtonsoft.com/json
* Splat: https://github.com/paulcbetts/splat
* PCLCrypto: https://github.com/AArnott/PCLCrypto


## Services already integrated


## Helpful links
* Xamarin plugins: https://github.com/xamarin/plugins
* Android icons generator: http://fa2png.io/r/ionicons
* Online diagram tool: http://asciiflow.com/
