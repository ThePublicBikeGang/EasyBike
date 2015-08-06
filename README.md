## PBike: A cross platform app for Public Bikes based on Xamarin. Join the GANG !

![](https://github.com/MarcMagnin/PublicBikes/blob/master/PublicBikes/PublicBikes/Assets/Icon.png)

The objective of the project is to store on a shared library all the integration logic of public bike services.
All services can be easily accessed with common interfaces.

### Sources to help integration
The Bike-sharing map
https://www.google.com/maps/d/viewer?mid=zGPlSU9zZvZw.kmqv_ul1MfkI
The Bike-sharing map twitter
https://twitter.com/BikesharingMap

### List of cities 
https://docs.google.com/spreadsheets/d/13Tim2Nvd41jKQim8dkglOsFcO_iTW7OBWGuAcSQrQUo/pubhtml

## ContractService
This service provide a way to access to the different contracts already implemented within the common library.

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

PublicBikes take advantage of multiple third parties plugins.

* MVVMLight: https://mvvmlight.codeplex.com/ (doc: http://www.mvvmlight.net/doc)
* Akavache: https://github.com/akavache/Akavache
* JSON.NET: http://www.newtonsoft.com/json


## Services already integrated
