using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenTrail.Forms.ViewModel
{
    public class EventViewModels
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string RegionCoordinates { get; set; }
        public string RegionName { get; set; }
        public DateTime Date { get; set; }
    }
}
