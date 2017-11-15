using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
     [Serializable]
    /// <summary>
    /// GNSS卫星信息表
    /// </summary>
    public class Satellite_Info
    {
        public Satellite_Info()
        {
            Elevation = null;
            Azimuth = null;
        }
        //卫星信息ID
        public int Satellite_Info_Id { get; set; }
        //定位信息ID
        public int Fix_Info_Id { get; set; }
        //GPS/GNSS设备 ID
        public int Device_Id { get; set; }
        //GNSS设备名
        public string Device_Name { get; set; }
        //当前utc时间
        public string Time { get; set; }
        //卫星PRN（伪随机噪声码）
        public int Prn { get; set; }
        //所属卫星系统（GP/GL/BD/GA）
        public string Satellite_Class { get; set; }
        //卫星是否可用
        public int Used { get; set; }
        //卫星仰角（00 - 90）度
        public int? Elevation { get; set; }
        //卫星方位角（00 - 359）度
        public int? Azimuth { get; set; }
        //信噪比（00－99）dB
        public decimal Snr { get; set; }


        //2017.9.22 xnn 卫星信息表列名

        //数据库表的列名
        public List<string> Column_Name_List;

        public List<string> ColumnName_List
        {
            set
            {
                Column_Name_List = value;

            }
            get { return Column_Name_List; }


        }


    }
}
