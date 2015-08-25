using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts
{
    class BarclayBikes : Contract
    {
        public override Task<List<Station>> GetStationsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
