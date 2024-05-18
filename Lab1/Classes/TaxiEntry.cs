using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public class TaxiEntry
    {
        public string CarNumber { get; set; }
        public string CarBrand { get; set; }
        public string StartLatitude { get; set; }
        public string StartLongitude { get; set; }
        public string DestLatitude { get; set; }
        public string DestLongitude { get; set; }
        public string Distance { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string FuelConsumption { get; set; }
        public string Fare { get; set; }
    }
}
