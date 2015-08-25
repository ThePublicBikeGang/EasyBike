using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using EasyBike.Config;
using EasyBike.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBike.Models
{
    public abstract class Contract : ObservableObject
    {

        public bool DirectDownloadAvailability { get; set; } = true;
        public string AvailabilityUrl { get; set; }
        public string StationsUrl { get; set; }
        public string ServiceProvider { get; set; }
        public string Name { get; set; }
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
                    return Name + this.GetType().Name;
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



        private int stationCounter;
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
            if (obj == null || ! (obj is Contract))
            {
                return false;
            }
            return StorageName == ((obj as Contract).StorageName);
        }

        public abstract Task<List<Station>> GetStationsAsync();

        public async Task<bool> RefreshAsync()
        {
            try
            {
                var refreshedStations = await InnerRefreshAsync().ConfigureAwait(false);

                for (int i = 0; i < Stations.Count; i++)
                {
                    for (int y = refreshedStations.Count - 1; y >= 0; y--)
                    {
                        if (Stations[i].Latitude == refreshedStations[y].Latitude && Stations[i].Longitude == refreshedStations[y].Longitude)
                        {
                            if (Stations[i].AvailableBikes != refreshedStations[y].AvailableBikes || Stations[i].AvailableBikeStands != refreshedStations[y].AvailableBikeStands)
                            {
                                Stations[i].IsUiRefreshNeeded = true;
                            }
                            Stations[i].AvailableBikes = refreshedStations[y].AvailableBikes;
                            Stations[i].AvailableBikeStands = refreshedStations[y].AvailableBikeStands;
                            Stations[i].Loaded = true;
                            refreshedStations.Remove(refreshedStations[y]);
                            break;
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }

            return true;
        }

        public virtual async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            return await Task.FromResult<List<StationModelBase>>(new List<StationModelBase>());
        }
    }
}