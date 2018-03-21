using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pc.BudapestHotel.Interfaces
{
     enum RoomType { single = 1, @double, juniorSuite, suite, presedentialSuite }
     enum Extras { kettle = 1, boathrobes, slippers, champaign, freshflowers, dentalHygineKits }
     interface IRoomReservation
    {
        string FullName { get; set; }
        DateTime? CheckInDate { get; set; }
        DateTime? CheckOutDate { get; set; }
        RoomType RoomType { get; set; }
        int Occupency { get; set; }
        List<Extras>Extras { get; set; }
        bool VIPMembership { get; set; }
    }
}