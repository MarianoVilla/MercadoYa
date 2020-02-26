using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MercadoYa.Lib.HelpersAndHandlers
{
    public class KeyValueUtil
    {
        public static Dictionary<string, object> MakeDicionary(params KeyValuePair<string, object>[] Values)
        {
            var Map = new Dictionary<string, object>(Values.Count());
            foreach(var Val in Values)
            {
                Map.Add(Val.Key, Val.Value);
            }
            return Map;
        }
    }
}
