using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakaGraphs.Templates.Models
{
    public class AllTimeData : TemplateModelBase
    {
        public DateTime MemberSince { get; set; }
        public DateTime LastActive { get; set; }
        public TimeSpan TotalTimeSpent { get; set; }
    }
}
