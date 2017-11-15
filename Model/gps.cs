using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class GPS
    {
        public GPS() { }

        //纬度
        private double latitude; 
        //经度
        private double longitude;

        //纬度
        public double Latitude
        {
            set { latitude = value; }
            get { return latitude; }
        }

        //经度
        public double Longitude
        {
            set { longitude = value; }
            get { return longitude; }
        }

    }
}
