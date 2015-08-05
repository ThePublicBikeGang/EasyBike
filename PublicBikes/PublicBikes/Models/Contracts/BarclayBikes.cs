using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicBikes.Models.Contracts
{
    class BarclayBikes : Contract
    {
        public override Task<List<Station>> GetStationsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
