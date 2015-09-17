using GalaSoft.MvvmLight;
using EasyBike.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using EasyBike.Notification;
using System;

namespace EasyBike.Models
{
    public abstract class Contract : ObservableObject
    {
        public bool StationRefreshGranularity { get; set; } = false;
        public string AvailabilityUrl { get; set; }
        public string StationsUrl { get; set; }
        public string ServiceProvider { get; set; }
        public string Name { get; set; }
        private int retryStation;
        private int retryContract;
        private string technicalName;
        public string TechnicalName
        {
            get
            {
                if (string.IsNullOrEmpty(technicalName))
                    return Name;
                else
                    return technicalName;
            }
            set { technicalName = value; }
        }

        private string storageName;
        public string StorageName
        {
            get
            {
                if (string.IsNullOrEmpty(storageName))
                    return Name + GetType().Name;
                else
                    return storageName;
            }
            set { storageName = value; }
        }
        public string Id { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }

        public bool DownloadingAvailability { get; set; }

        private bool downloading;
        public bool Downloading
        {
            get { return downloading; }
            set
            {
                Set(() => Downloading, ref downloading, value);
            }
        }

        private bool downloaded;
        public bool Downloaded
        {
            get { return downloaded; }
            set
            {
                Set(() => Downloaded, ref downloaded, value);
            }
        }

        public string ISO31661 { get; set; }

        public List<Station> Stations { get; set; }


        // because of dispatcher cross threading
        public int stationCounter;
        public int StationCounter
        {
            get { return stationCounter; }
            set
            {
                Set(() => StationCounter, ref stationCounter, value);
                RaisePropertyChanged("StationCounterStr");
            }
        }
        public string StationCounterStr
        {
            get
            {
                return $"{stationCounter} {(stationCounter <= 1 ? "station" : "stations")}";
            }
        }

        public override int GetHashCode()
        {
            return StorageName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Contract))
            {
                return false;
            }
            return StorageName == ((obj as Contract).StorageName);
        }

        public async Task<List<Station>> GetStationsAsync()
        {
            var serviceProviderModels = await InnerGetStationsAsync().ConfigureAwait(false);
            var stations = new List<Station>(serviceProviderModels.Count);
            foreach (var serviceProviderModel in serviceProviderModels)
            {
                stations.Add(new Station()
                {
                    Latitude = serviceProviderModel.Latitude,
                    Longitude = serviceProviderModel.Longitude,
                    AvailableBikes = serviceProviderModel.AvailableBikes,
                    AvailableBikeStands = serviceProviderModel.AvailableBikeStands,
                    ContractStorageName = StorageName,
                    Id = serviceProviderModel.Id,
                    IsUiRefreshNeeded = true,
                    Loaded = true
            });
            }
            return stations;
        }

        public virtual async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            return await Task.FromResult(new List<StationModelBase>());
        }

        public async Task<bool> RefreshAsync()
        {
            try
            {
                var refreshedStations = await InnerRefreshAsync().ConfigureAwait(false);

                for (int i = 0; i < Stations.Count; i++)
                {
                    Stations[i].Loaded = true;
                    for (int y = refreshedStations.Count - 1; y >= 0; y--)
                    {
                        if (Stations[i].Latitude == refreshedStations[y].Latitude && Stations[i].Longitude == refreshedStations[y].Longitude)
                        {
                            if (Stations[i].AvailableBikes != refreshedStations[y].AvailableBikes || Stations[i].AvailableBikeStands != refreshedStations[y].AvailableBikeStands)
                            {
                                Stations[i].IsUiRefreshNeeded = true;
                                Stations[i].AvailableBikes = refreshedStations[y].AvailableBikes;
                                Stations[i].AvailableBikeStands = refreshedStations[y].AvailableBikeStands;
                            }
                            refreshedStations.Remove(refreshedStations[y]);
                            break;
                        }
                    }
                   

                    await Task.Delay(1).ConfigureAwait(false);
                }
                retryContract = 0;
            }
            catch (Exception e)
            {
                retryContract++;
                if (retryContract == 3)
                {
                    var notificationService = SimpleIoc.Default.GetInstance<INotificationService>();
                    try
                    {
                        notificationService.Notify(new RefreshFailureNotification()
                        {
                            Body = "You seems to struggle getting the stations information. It may be either that your network connection isn't healthy, the service provider is down or it has changed. If you think it could be the last option, then contact us and we will investigate. Thumbs up !",
                            Subject = "Hey !",
                            ContractName = Name,
                            Exception = e
                        });
                    }
                    catch
                    {
                        // Ignore
                    }
                }
            }
            return true;
        }

        public async Task<bool> RefreshAsync(Station station)
        {
            if(retryStation > 10 && retryStation%30 != 0)
            {
                retryStation++;
                return false;
            }
            try
            {
                var refreshedStation = await InnerRefreshAsync(station).ConfigureAwait(false);
                // case where the availability comes from images (China)
                if(refreshedStation == null)
                {
                    retryStation = 0;
                    return true;
                }
                if (station.AvailableBikes != refreshedStation.AvailableBikes || station.AvailableBikeStands != refreshedStation.AvailableBikeStands)
                {
                    station.IsUiRefreshNeeded = true;
                    station.AvailableBikes = refreshedStation.AvailableBikes;
                    station.AvailableBikeStands = refreshedStation.AvailableBikeStands;
                }
                if (!station.Loaded)
                {
                    station.Loaded = true;
                    station.IsUiRefreshNeeded = true;
                }
               
                retryStation = 0;
                return true;
            }
            catch(Exception e)
            {
                retryStation++;
                if(retryStation == 10) 
                {
                    //var contractService = SimpleIoc.Default.GetInstance<IContractService>();
                    //contractService.
                    var notificationService = SimpleIoc.Default.GetInstance<INotificationService>();
                    try
                    {
                        notificationService.Notify(new RefreshFailureNotification()
                        {
                            Body = "You seems to struggle getting the stations information. It may be either that your network connection isn't healthy, the service provider is down or it has changed. If you think it could be the last option, then contact us and we will investigate. Thumbs up !",
                            Subject = "Hey !",
                            ContractName = station.Contract.Name,
                            Exception = e
                        });
                    }
                    catch
                    {
                        // Ignore
                    }
                }
            }

            return true;
        }

        public virtual async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            return await Task.FromResult(new List<StationModelBase>());
        }

        public virtual async Task<StationModelBase> InnerRefreshAsync(Station station)
        {
            return null;
        }
    }
}