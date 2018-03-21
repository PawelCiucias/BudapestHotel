using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pc.BudapestHotel.Interfaces
{
    internal interface IEntityParser
    {
        object Parse(EntityRecommendation entity);
        IEntityParser Next { get; set; }
    }
}
