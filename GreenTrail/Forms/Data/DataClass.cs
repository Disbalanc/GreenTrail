using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenTrail.Forms.Data
{
    internal class DataClass
    {
    }

    public class PollutionData
    {
        public long id { get; set; }
        public string levels { get; set; }
        public string source { get; set; }
        public string geographicalcoordinates { get; set; }
    }

    public class StudyData
    {
        public string fullname { get; set; }
        public string articulSample { get; set; }
        public string region { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public double result { get; set; }
    }
}