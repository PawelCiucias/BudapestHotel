using Microsoft.Bot.Builder.Luis.Models;
using pc.BudapestHotel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pc.BudapestHotel.Parsers
{
    [Serializable]
     class IntegerParser : IEntityParser
    {
        const string TypeName = "builtin.number";

        public IEntityParser Next { get; set; }

        public object Parse(EntityRecommendation entity)
        {
            if (entity.Type.Equals(TypeName))
                return entity.Resolution.First().Value;
            return Next?.Parse(entity);

        }
    }
}