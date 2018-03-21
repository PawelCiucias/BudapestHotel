using pc.BudapestHotel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pc.BudapestHotel.Models
{
    public class DateRange : IDateRange
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime   {get;set;}

        public DateRange(DateTime startDateTime, DateTime endDateTime, bool flipOnException = false)
        {
            if (endDateTime < startDateTime)
            {
                if (flipOnException)
                {
                    var temp = new DateTime(startDateTime.Ticks);
                    startDateTime = endDateTime;
                    endDateTime = temp;
                }
                else
                {
                    var msg = $"{endDateTime.ToShortTimeString()} must be after {startDateTime.ToShortTimeString()}";
                    throw new ArgumentOutOfRangeException(nameof(endDateTime), msg);
                }
            }
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
        }
    }
}