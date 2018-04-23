using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psbds.LUIS.IntentCSVImport.Core.Helpers
{
    public static class StringExtensions
    {
        public static T DeserializeObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
