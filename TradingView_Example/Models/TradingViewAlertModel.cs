using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using TradingView_Example.Models.Enums;
using TradingView_Example.Properties;

namespace TradingView_Example.Models
{
    [Serializable]
    public class TradingViewAlertModel
    {
        public int Number { get; set; }

        public string Name { get; set; }

        public string Symbol { get; set; }

        public decimal Volume { get; set; }

        public TradingViewAlertStatus Status { get; set; }

        public DateTime? ExirationTime { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public static async Task SaveAsync(IList<TradingViewAlertModel> alerts)
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, alerts);
                ms.Position = 0;
                var buffer = new byte[(int)ms.Length];
                await ms.ReadAsync(buffer, 0, buffer.Length);

                Settings.Default.Alerts = Convert.ToBase64String(buffer);
                Settings.Default.Save();
            }
        }

        public static IList<TradingViewAlertModel> LoadAlerts()
        {
            var serializedAlerts = Settings.Default.Alerts;
            if (!serializedAlerts.HasValue())
                return new List<TradingViewAlertModel>();

            using (var ms = new MemoryStream(Convert.FromBase64String(serializedAlerts)))
            {
                var bf = new BinaryFormatter();

                return (IList<TradingViewAlertModel>)bf.Deserialize(ms);
            }
        }
    }
}