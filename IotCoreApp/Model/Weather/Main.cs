using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotCoreApp.Model
{
    public class Main
    {
        public double tempCelsius {
            get {
                return ConvertFahrenheitToCelsius(temp);
            }
        }

        public double temp { get; set; }
        public int humidity { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }

        public double ConvertFahrenheitToCelsius(double f)
        {
            return (f - 273);
        }
    }

   
}
