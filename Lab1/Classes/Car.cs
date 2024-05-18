using System;
using System.IO;
using System.Xml.Serialization;

namespace Lab1
{
        public class Car
        {
            public string CarNumber { get; set; }
            public CarBrand CarBrand { get; set; } // Modified property name to match XAML binding
            public double StartLatitude { get; set; }
            public double StartLongitude { get; set; }
            public double DestinationLatitude { get; set; }
            public double DestinationLongitude { get; set; }
            public double Distance { get; set; }
            public DateTime DateTime { get; set; }
            public double FuelConsumption { get; set; }
            public double Fare { get; set; }
        }    
}