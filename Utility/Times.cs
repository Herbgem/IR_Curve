using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    [Serializable]
    public class Times
    {
        public Times(DateTime startDate, DateTime endDate, string irType)
        {
            StartDate = startDate;
            EndDate = endDate;
            IRType = irType;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string IRType { get; set; }
    }
}
