using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Management;
using System.IO;
using System.Net.NetworkInformation;
/*----------------------------------------------------------------
// Copyright (C) 2017 魅族科技有限公司
// 版权所有。 
//
// 文件名：Common
// 文件功能描述：公共类
//
// 
// 创建标识：邢娜娜 2017.7.27
//
// 修改标识：邢娜娜 2017.8.16
// 修改描述：增加MySQL数据库连接方法，原因是本系统打算将原数据库SQLite替换为MySQL，以尝试解决图表分析速度慢的问题
//  
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/


namespace Utility
{
    /// <summary>
    /// 公共工具类，不可继承此类
    /// </summary>
    public class Common
    {

        [DllImport("kernel32.dll", EntryPoint = "GetSystemDefaultLCID")]
        public static extern int GetSystemDefaultLCID();
        [DllImport("kernel32.dll", EntryPoint = "SetLocaleInfoA")]
        public static extern int SetLocaleInfo(int Locale, int LCType, string lpLCData);
        public const int LOCALE_SLONGDATE = 0x20;
        public const int LOCALE_SSHORTDATE = 0x1F;
        public const int LOCALE_STIME = 0x1003;

        private static String sqliteConn;                                      //SQLite连接字符串  2017.7.27 xnn
        private static String encryptKey = "meizukejiyouxiangongsihaiwaizu";       //加密密钥

        private static String mysqlConn;                                     //MySQL数据库连接字符串 2017.8.16 xnn

         private static  double EARTH_RADIUS = 6378137;//赤道半径(单位m)  


        /// <summary>
        /// 保持客户端和服务器端日期格式一致
        /// </summary>
        public static void SetDateTimeFormat()
        {
            try
            {
                int x = GetSystemDefaultLCID();
                SetLocaleInfo(x, LOCALE_STIME, "HH:mm:ss");        //时间格式  
                SetLocaleInfo(x, LOCALE_SSHORTDATE, "yyyy/MM/dd");   //短日期格式    
                SetLocaleInfo(x, LOCALE_SLONGDATE, "yyyy/MM/dd");   //长日期格式   
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 获得数据库绝对路径
        /// </summary>
        /// <returns></returns>
        public static string getDBPath()
        {
            //数据库绝对路径
            string dbPath = AppDomain.CurrentDomain.BaseDirectory;
            if (dbPath.IndexOf("\\bin\\") > 0)
            {
                if (dbPath.EndsWith("\\bin\\Debug\\"))
                    dbPath = dbPath.Replace("\\bin\\Debug", "");
                if (dbPath.EndsWith("\\bin\\Release\\"))
                    dbPath = dbPath.Replace("\\bin\\Release", "");
            }
            if (!dbPath.EndsWith("App_Data\\"))
                dbPath = dbPath + "App_Data\\";
            AppDomain.CurrentDomain.SetData("DataDirectory", dbPath);

            string sqliteConn_temp = ConfigurationManager.AppSettings["sqliteConn"];
            string[] temp = sqliteConn_temp.Split('|');
            string db_filename = temp[2].Split(';')[0];
            dbPath = dbPath + db_filename;

            return dbPath;
        }

        /// <summary>
        /// 获得CSV文件路径
        /// </summary>
        /// <returns></returns>
        public static string getFilePath()
        {
            //数据库绝对路径
            string dbPath = AppDomain.CurrentDomain.BaseDirectory;
            if (dbPath.IndexOf("\\bin\\") > 0)
            {
                if (dbPath.EndsWith("\\bin\\Debug\\"))
                    dbPath = dbPath.Replace("\\bin\\Debug", "");
                if (dbPath.EndsWith("\\bin\\Release\\"))
                    dbPath = dbPath.Replace("\\bin\\Release", "");
            }
            if (!dbPath.EndsWith("App_Data\\"))
                dbPath = dbPath + "App_Data\\";

            return dbPath;
        }


        /// <summary>
        /// 获得不加密连接字符串
        /// </summary>
        /// <returns></returns>
        public static string getSQLiteConn()
        {
            string dbPath = AppDomain.CurrentDomain.BaseDirectory;
            if (dbPath.IndexOf("\\bin\\") > 0)
            {
                if (dbPath.EndsWith("\\bin\\Debug\\"))
                    dbPath = dbPath.Replace("\\bin\\Debug", "");
                if (dbPath.EndsWith("\\bin\\Release\\"))
                    dbPath = dbPath.Replace("\\bin\\Release", "");
            }
            if (!dbPath.EndsWith("App_Data\\"))
                dbPath = dbPath + "App_Data\\";
            AppDomain.CurrentDomain.SetData("DataDirectory", dbPath);

            string sqliteConn_temp = ConfigurationManager.AppSettings["sqliteConn"];
            string[] temp = sqliteConn_temp.Split('|');

            sqliteConn_temp = "Data Source=" + dbPath + temp[2];
            //sqliteConn_temp = temp[0] + dbPath + temp[2];

            return sqliteConn_temp;
        }

        /// <summary>
        /// 2017.7.27 xnn
        /// 获得加密连接字符串
        /// 获取app.config文件中连接SQLite数据库的字符串   
        /// </summary>
        /// <returns></returns>
        public static String GetSQLiteConn()
        {
            //sqliteConn = ConfigurationManager.AppSettings["sqliteConn"];     

            sqliteConn = getSQLiteConn();

            try
            {
                return Dec_DES(sqliteConn);
            }
            catch (ArgumentException)                                    //参数异常
            {
                return null;
            }
        }

        #region MySQL数据库连接

        /// <summary>
        /// 2017.8.16 xnn
        /// 获得数据库连接字符串
        /// 获取app.config文件中连接MySQL数据库的字符串   
        /// </summary>
        /// <returns></returns>
        public static String GetMySQLConn()
        {
            mysqlConn = ConfigurationManager.AppSettings["mysqlConn"];              

            try
            {
                return Dec_DES(mysqlConn);
            }
            catch (ArgumentException)                                    //参数异常
            {
                return null;
            }
        }



        #endregion




        /// <summary>
        /// DES加密字符串
        /// DES对称加密算法，密钥为encryptKey
        /// </summary>
        /// <param name="password">待加密字符串</param>
        /// <returns>加密后的字符串</returns>
        public static String Enc_DES(String password)
        {
            return EncryptDES.strEncryptDES(password, encryptKey);
        }

        /// <summary>
        /// DES解密 
        /// </summary>
        /// <param name="password">待解密字符串</param>
        /// <returns></returns>
        public static String Dec_DES(String password)
        {
            return EncryptDES.DecryptDES(password, encryptKey);

        }

        /// <summary>
        /// 用MD5将密码进行加密，返回加密后的密码
        /// </summary>
        /// <param name="decodePassword">加密前的密码</param>
        /// <returns>加密后的密码</returns>
        public static String GetMD5(String decodePassword)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] r = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(decodePassword));
            String res = null;
            for (Int32 i = 0; i < r.Length; ++i)
            {
                res += r[i].ToString("x2");
            }
            return res.ToUpper();
        }

        /// <summary>
        /// 获取执行文件路径
        /// </summary>
        /// <returns></returns>
        public static String GetDirPath()
        {
            return System.Windows.Forms.Application.StartupPath;
        }

        /// <summary>
        /// 获取服务器端的时间，返回DateTime
        /// </summary>
        /// <returns>系统时间(DateTime)</returns>
        public static DateTime GetSysTime()
        {
            //String sql = "select  (datetime('now')) as SYSDATE";
            //return Convert.ToDateTime(DbHelperSQLite.Query(sql).Tables[0].Rows[0]["SYSDATE"].ToString());
            DateTime datetime = DateTime.Now;

            return datetime;

        }

        #region 2017.10.12 xnn Ping远程服务器
        /// <summary>
        /// 2017.10.12 xnn 是否能Ping通 远程服务器 
        /// </summary>
        /// <returns>true：通，false：不通</returns>
        public static bool PingServer()
        {
            string serverIpAddress = ConfigurationManager.AppSettings["ServerIPAddress"]; //服务器IP地址
            return Ping(serverIpAddress);
        }

        /// <summary>
        /// 是否能 Ping 通指定的主机
        /// </summary>
        /// <param name="ip">ip 地址或主机名或域名</param>
        /// <returns>true 通，false 不通</returns>
        public static bool Ping(string ip)
        {
            Ping p = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            const string data = "Test data!";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            const int timeout = 1000; //Timeout 时间，单位：毫秒
            PingReply reply = p.Send(ip, timeout, buffer, options);
            return reply != null && reply.Status == IPStatus.Success;
        }

        #endregion

        /// <summary>
        /// 生成 不重复 随机数
        /// </summary>
        /// <param name="num">生成随机数 个数</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns></returns>
        public static int[] getRandomNum(int num, int minValue, int maxValue)
        {
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int[] arrNum = new int[num];
            int tmp = 0;
            for (int i = 0; i <= num - 1; i++)
            {
                tmp = ra.Next(minValue, maxValue); //随机取数
                arrNum[i] = getNum(arrNum, tmp, minValue, maxValue, ra); //取出值赋到数组中
            }
            return arrNum;
        }

        /// <summary>
        /// 判断取出的数 与 数组中的数 是否 有重复
        /// </summary>
        /// <param name="arrNum">数组</param>
        /// <param name="tmp">随机数</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="ra">随机数生成实例</param>
        /// <returns></returns>
        private static int getNum(int[] arrNum, int tmp, int minValue, int maxValue, Random ra)
        {
            int n = 0;
            while (n <= arrNum.Length - 1)
            {
                if (arrNum[n] == tmp) //利用循环判断是否有重复
                {
                    tmp = ra.Next(minValue, maxValue); //重新随机获取。
                    getNum(arrNum, tmp, minValue, maxValue, ra);//递归:如果取出来的数字和已取得的数字有重复就重新随机获取。
                }
                n++;
            }
            return tmp;
        }


        /// <summary>
        /// 将时间转换成时分秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string getHHMMSS(string time)
        {
            string hhmmss = "";
            for (int i = 0; i < 3; i++)
            {
                string temp = time.Substring(0, 2);
                hhmmss = hhmmss + temp + ":";
                time = time.Remove(0, 2);
            }
            hhmmss = hhmmss.Substring(0, 8);
            return hhmmss;
        }


        /// <summary>
        /// 程序执行时间测试
        /// </summary>
        /// <param name="dateBegin">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>返回(秒)单位，比如: 0.00239秒</returns>
        public static string ExecDateDiff(DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();
            //你想转的格式
            //return ts3.TotalMilliseconds.ToString();
            return ts3.ToString("c").Substring(0,8);   //00:00:07
        }


        #region 根据经纬度计算两点间距离

        /// <summary>
        /// 计算两点位置的距离，返回两点的距离，单位：米
        /// 该公式为GOOGLE提供，误差小于0.2米
        /// </summary>
        /// <param name="lng1">第一点经度</param>
        /// <param name="lat1">第一点纬度</param>        
        /// <param name="lng2">第二点经度</param>
        /// <param name="lat2">第二点纬度</param>
        /// <returns></returns>
        public static double GetDistance(double lng1, double lat1, double lng2, double lat2)
        {
            double radLat1 = Rad(lat1);
            double radLng1 = Rad(lng1);
            double radLat2 = Rad(lat2);
            double radLng2 = Rad(lng2);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
            return result;
        }

        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double Rad(double d)
        {
            return (double)d * Math.PI / 180d;
        }



        #endregion

        #region 获得用户机器信息

        ///<summary>
        /// 获取CPU序列号
        ///</summary>
        ///<returns></returns>
        public static string GetCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuCollection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuCollection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
            }
            return strCpu;
        }

        ///<summary>
        /// 获取硬盘卷标号
        ///</summary>
        ///<returns></returns>
        public static string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc = new ManagementClass("win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        /// <summary>
        /// 根据CPU，硬盘号，获得唯一码
        /// </summary>
        /// <returns></returns>
        public static string getComputerOnlyNum()
        {
            string strNum = GetCpu() + GetDiskVolumeSerialNumber();
            string strMNum = strNum.Substring(0, 24);    //截取前24位作为机器码
            return strMNum;
        }

        #endregion

        #region 删除文件夹及其内文件

        /// <summary>
        /// 2017.9.21 xnn
        /// 删除文件夹及文件夹内文件
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool DeleteFolder(string dir)
        {
            if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d); //直接删除其中的文件 
                    else
                        DeleteFolder(d); //递归删除子文件夹 
                }
                Directory.Delete(dir); //删除已空文件夹 
                //文件夹删除成功
                return true;
            }
            else
            {
                //该文件夹不存在
                return false;
            }
               
        } 

        #endregion
     


    }
}
