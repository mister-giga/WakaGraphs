using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakaGraphs.Models
{
    public class AllTimeSinceToday
    {
        public Data Data { get; set; }
    }
    public class Data
    {
        public bool Is_up_to_date { get; set; }
        public Range Range { get; set; }
        public string Text { get; set; }
        public int Timeout { get; set; }
        public double Total_seconds { get; set; }
    }

    public class Range
    {
        public DateTime End { get; set; }
        public string End_date { get; set; }
        public string End_text { get; set; }
        public DateTime Start { get; set; }
        public string Start_date { get; set; }
        public string Start_text { get; set; }
        public string Timezone { get; set; }
    }
}
