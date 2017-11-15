using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    /// <summary>
    /// GNSS定位信息表表
    /// </summary>
    public class Fix_Info
    {
        public Fix_Info()
        {
            Latitude = null;
            Longitude = null;
            Distance = null;
        }
        //定位信息ID
        public int Fix_Info_Id { get; set; }
        //GNSS设备 ID
        public int Device_Id { get; set; }
        //GNSS设备名
        public string Device_Name { get; set; }
        //当前utc日期
        public string Date { get; set; }
        //当前utc时间
        public string Time { get; set; }
        //GPS状态，0=未定位，1=非差分定位，2=差分定位，3=无效PPS，6=正在估算
        public int Fix_Quality { get; set; }
        //定位模式，A=自动选择，M=手动选择
        public string Fix_Mode1 { get; set; }
        //定位类型，1=未定位，2=2D定位，3=3D定位
        public int Fix_Mode2 { get; set; }
        //纬度，±dd.ddddddd°,南纬前面加“-” ddmm.mmmm格式转换为此格式为dd+mm.mmmm/60 
        public decimal? Latitude { get; set; }
        //经度，±ddd.ddddddd°,西经前面加“-” ddmm.mmmm格式转换为此格式为dd+mm.mmmm/60
        public decimal? Longitude { get; set; }
        //正在使用的卫星数量
        public int Num_Of_Used { get; set; }
        //正在使用的GPS卫星数量
        public int Gps_Num_Of_Used { get; set; }
        //正在使用的Glonass卫星数量
        public int Glonass_Num_Of_Used { get; set; }
        //正在使用的Bds卫星数量
        public int Bds_Num_Of_Used { get; set; }
        //正在使用的Galileo卫星数量
        public int Galileo_Num_Of_Used { get; set; }
        //可见卫星数量
        public int Num_Of_Visable { get; set; }
        //可见Gps卫星数量
        public int Gps_Num_Of_Visable { get; set; }
        //可见Glonass卫星数量
        public int Glonass_Num_Of_Visable { get; set; }
        //可见Bds卫星数量
        public int Bds_Num_Of_Visable { get; set; }
        //可见Galileo卫星数量
        public int Galileo_Num_Of_Visable { get; set; }
        //瞬时平均信号强度
        public decimal Snr_Avg { get; set; }
        //Gps瞬时平均信号强度
        public decimal Gps_Snr_Avg { get; set; }
        //Glonass瞬时平均信号强度
        public decimal Glonass_Snr_Avg { get; set; }
        //Bds瞬时平均信号强度
        public decimal Bds_Snr_Avg { get; set; }
        //Galileo瞬时平均信号强度
        public decimal Galileo_Snr_Avg { get; set; }
        //与对比机距离
        public decimal? Distance { get; set; }

        /*
        //海拔高度,单位：M/米
        public float Altitude { get; set; }
        //大地水准面的高度 基于WGS84，单位：M/米
        public float Height_Of_Geoid { get; set; }
        //PDOP综合位置精度因子（0.5 - 99.9）
        public float Pdop { get; set; }
        //HDOP水平精度因子（0.5 - 99.9）
        public float Hdop { get; set; }
        //VDOP垂直精度因子（0.5 - 99.9）
        public float Vdop { get; set; }
         */


        //2017.9.22 xnn 定位信息表列名

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
