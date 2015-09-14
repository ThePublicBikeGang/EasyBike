using System.Collections.Generic;

namespace EasyBike.Models
{
    public class Country
    {
        public string Name{ get; set; }
        public string ISO31661 { get; set; }
        public List<Contract> Contracts { get; set; }
    }
}
