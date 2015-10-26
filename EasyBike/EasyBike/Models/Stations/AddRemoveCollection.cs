using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBike.Models.Stations
{
    public class AddRemoveCollection
    {
        public List<Station> ToAdd { get; set; }
        public List<Station> ToRemove { get; set; }
    }
}
