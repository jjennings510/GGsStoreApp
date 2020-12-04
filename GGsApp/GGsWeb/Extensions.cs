using GGsWeb.Features;
using GGsWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GGsWeb
{
    public static class Extensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void SerializeAlerts(this ITempDataDictionary tempData, string alertKeyName, List<Alert> alerts)
        {
            tempData[alertKeyName] = JsonConvert.SerializeObject(alerts);
        }

        public static List<Alert> DeserializeAlerts(this ITempDataDictionary tempData, string alertKeyName)
        {
            var alerts = new List<Alert>();
            if (tempData.ContainsKey(alertKeyName))
            {
                alerts = JsonConvert.DeserializeObject<List<Alert>>(tempData[alertKeyName].ToString());
            }
            return alerts;
        }
    }
}
