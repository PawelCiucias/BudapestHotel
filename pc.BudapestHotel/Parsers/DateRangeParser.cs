using Microsoft.Bot.Builder.Luis.Models;
using pc.BudapestHotel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace pc.BudapestHotel.Parsers
{
    [Serializable]
    class DateRangeParser : IEntityParser
    {
        const string TypeName = "builtin.datetimeV2.daterange";

        public IEntityParser Next { get; set; }

        public object Parse(EntityRecommendation entity)
        {
            if (entity.Type == TypeName)
            {
                var s = DateTime.Parse(((IDictionary<string, object>)((IList<object>)entity.Resolution["values"]).First())["start"].ToString());
                var e = DateTime.Parse(((IDictionary<string, object>)((IList<object>)entity.Resolution["values"]).First())["end"].ToString());

                return new Models.DateRange(s, e, true);
            }

            return Next?.Parse(entity);
        }
    }
}