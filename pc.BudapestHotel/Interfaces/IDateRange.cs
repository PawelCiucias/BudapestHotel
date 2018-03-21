using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pc.BudapestHotel.Interfaces
{
    public interface IDateRange
    {
        DateTime StartDateTime { get; set; }
        DateTime EndDateTime { get; set; }
    }
}