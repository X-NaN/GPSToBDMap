using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PositionUtility
    {
        /// <summary>
        /// GPS左边转换成百度坐标
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="lng">经度</param>
        /// <param name="la">纬度</param>
        /// <returns></returns>
        public static GPS getBaiducoor(double lng,double la)//坐标转换的方法  
        {
            double longitude = lng;//经度
            double latitude = la;//纬度
            //需要转的gps经纬度  
            string convertUrl = "http://api.map.baidu.com/ag/coord/convert?from=0&to=4&x=" + longitude + "&y=" + latitude + "";

            HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(convertUrl);//创建http请求  
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            string responseTxt = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();

            CoorConvert mapconvert = new CoorConvert();//创建存放结果的对象  
            mapconvert = JsonConvert.DeserializeObject<CoorConvert>(responseTxt);//赋值  

            string lon = mapconvert.x;
            string lat = mapconvert.y;

            byte[] xBuffer = Convert.FromBase64String(lon);//坐标base64解密  
            string strX = Encoding.UTF8.GetString(xBuffer, 0, xBuffer.Length);

            byte[] yBuffer = Convert.FromBase64String(lat);
            string strY = Encoding.UTF8.GetString(yBuffer, 0, xBuffer.Length);

            double[] coor = new double[2];
            coor[0] = Convert.ToDouble(strX);
            coor[1] = Convert.ToDouble(strY);

            GPS gps = new GPS();
            gps.Latitude = coor[1];
            gps.Longitude = coor[0];

            return gps;
        }

        //创建一个对象存储结果  
        public class CoorConvert
        {
            public string error { get; set; }
            public string x { get; set; }
            public string y { get; set; }
        }  

    }
}
