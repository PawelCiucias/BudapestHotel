using pc.BudapestHotel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pc.BudapestHotel.ExtensionMethods
{
    public static class EntityParserExtensions
    {
        internal static IEntityParser AddParsers(this IEntityParser firstParser, params IEntityParser[] parsers)
        {
            IEntityParser current = firstParser;

            foreach (var p in parsers)
                current = (current.Next = p);

            return firstParser;
        }
    }
}