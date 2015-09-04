using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts
{
    class BarclayBikes : Contract
    {
        public override Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
