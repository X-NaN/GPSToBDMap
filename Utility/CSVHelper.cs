using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utility
{
    public class CSVHelper
    {
       static string filePath = Utility.Common.getFilePath();

         //读写锁，当资源处于写入模式时，其他线程写入需要等待本次写入结束之后才能继续写入
       //static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 2017.9.22 xnn
        /// 以任务为单位创建文件夹
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
       public static string createDirectoryIFNotExist(string task_id)
       {
           string file_path = filePath + task_id;
           if (!Directory.Exists(file_path))
           {
               Directory.CreateDirectory(file_path);
           }

           return file_path;
       }

        /// <summary>
        /// 2017.9.21 xnn
        /// 获得任务文件夹路径
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
       public static string getTaskFilePath(string task_id)
       {
           string file_path = createDirectoryIFNotExist(task_id);
          
           return file_path+"\\";
       }

        /// <summary>
        /// 2017.9.21 xnn
        /// 获得任务文件夹
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
       public static string getTaskFile(string task_id)
       {
           string file_path = createDirectoryIFNotExist(task_id);

           return file_path ;
       }

        /// <summary>
        /// 将DataTable转换成CSV文件
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="filePath">文件路径</param>
        public static void SaveCsv(DataTable dt,string task_id,string file_name )
        {
            FileStream fs = null;
            StreamWriter sw = null;
            string file = "";
            var data = string.Empty;
            try
            {
                file = getTaskFilePath(task_id) + file_name + ".csv";

                if (!File.Exists(file))
                {
                    fs = new FileStream(file, FileMode.Create, FileAccess.Write);
                    sw = new StreamWriter(fs, Encoding.UTF8);
                    //写出列名称
                    for (var i = 0; i < dt.Columns.Count; i++)
                    {
                        data += dt.Columns[i].ColumnName;
                        if (i < dt.Columns.Count - 1)
                        {
                            data += ",";
                        }
                    }
                    sw.WriteLine(data);
                }
                else
                {
                    fs = new FileStream(file, FileMode.Append);
                    sw = new StreamWriter(fs, Encoding.UTF8);
                }                                                          
               
                //写出各行数据
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    data = string.Empty;
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        string s = dt.Rows[i][j].ToString();
                        data += dt.Rows[i][j].ToString();
                        if (j < dt.Columns.Count - 1)
                        {
                            data += ",";
                        }
                    }
                    sw.WriteLine(data);
                }
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message, ex);
            }
            finally
            {
                if (sw != null) sw.Close();
                if (fs != null) fs.Close();
            }
        }

        /// <summary>
        /// 2017.9.21 xnn
        /// 多个线程，多次写
        /// 将定位信息表和卫星信息表写入CSV文件
        /// </summary>
        /// <param name="flag">标志是 定位信息表 还是 卫星信息表</param>
        /// <param name="data_list"></param>
        /// <param name="col_name_list"></param>
        /// <param name="csv_name"></param>
        /// <param name="task_id"></param>
        public static void SaveCsv(string flag, ArrayList data_list,List<string> col_name_list, string task_id)
        {
            FileStream fs = null;
            StreamWriter sw = null;
            string file = "";
            var data = string.Empty;

            try
            {
                file = getTaskFilePath(task_id) + flag + ".csv";

                if (!File.Exists(file))
                {
                    fs = new FileStream(file, FileMode.Create, FileAccess.Write);
                    sw = new StreamWriter(fs, Encoding.UTF8);
                    
                    //写出列名称
                    for (int i = 0; i < col_name_list.Count; i++)
                    {
                        data += col_name_list[i].ToString();

                        if (i < col_name_list.Count - 1)
                        {
                            data += ",";
                        }
                    }
                    sw.WriteLine(data);
                }
                else
                {
                    fs = new FileStream(file, FileMode.Append);//追加数据
                    sw = new StreamWriter(fs, Encoding.UTF8);
                }
               
                if (flag.Equals("Fix_Infos"))//定位信息表
                {
                    //写出各行数据
                    foreach (Fix_Info fixInfo in data_list)
                    {
                        data = string.Empty;
                        data += fixInfo.Fix_Info_Id + "," +
                                                    fixInfo.Device_Id + "," +
                                                    fixInfo.Device_Name + "," +
                                                    fixInfo.Date + "," +
                                                    fixInfo.Time + "," +
                                                    fixInfo.Fix_Quality + "," +
                                                    fixInfo.Fix_Mode1 + "," +
                                                    fixInfo.Fix_Mode2 + "," +
                                                    (fixInfo.Latitude != null ? fixInfo.Latitude.ToString() : "NULL") + "," +
                                                    (fixInfo.Longitude != null ? fixInfo.Longitude.ToString() : "NULL") + "," +
                                                    fixInfo.Num_Of_Used + "," +
                                                    fixInfo.Gps_Num_Of_Used + "," +
                                                    fixInfo.Glonass_Num_Of_Used + "," +
                                                    fixInfo.Bds_Num_Of_Used + "," +
                                                    fixInfo.Galileo_Num_Of_Used + "," +
                                                    fixInfo.Num_Of_Visable + "," +
                                                    fixInfo.Gps_Num_Of_Visable + "," +
                                                    fixInfo.Glonass_Num_Of_Visable + "," +
                                                    fixInfo.Bds_Num_Of_Visable + "," +
                                                    fixInfo.Galileo_Num_Of_Visable + "," +
                                                    fixInfo.Snr_Avg + "," +
                                                    fixInfo.Gps_Snr_Avg + "," +
                                                    fixInfo.Glonass_Snr_Avg + "," +
                                                    fixInfo.Bds_Snr_Avg + "," +
                                                    fixInfo.Galileo_Snr_Avg + "," +
                                                    (fixInfo.Distance != null ? fixInfo.Distance.ToString() : "NULL");
                                              
                        sw.WriteLine(data);
                       
                                              
                    }
                }
                else if (flag.Equals("Satellite_Infos"))//卫星信息表
                {
                    foreach (Satellite_Info satellite_Info in data_list)
                    {
                        data = string.Empty;
                        data += satellite_Info.Satellite_Info_Id + "," +
                                                    satellite_Info.Fix_Info_Id + "," +
                                                    satellite_Info.Device_Id + "," +
                                                    satellite_Info.Device_Name + "," +
                                                    satellite_Info.Time + "," +
                                                    satellite_Info.Prn + "," +
                                                    satellite_Info.Satellite_Class + "," +
                                                    satellite_Info.Used + "," +
                                                    (satellite_Info.Elevation != null ? satellite_Info.Elevation.ToString() : "NULL") + "," +
                                                    (satellite_Info.Azimuth != null ? satellite_Info.Azimuth.ToString() : "NULL") + "," +
                                                    satellite_Info.Snr;
                      
                        sw.WriteLine(data);
                       
                                              
                    }
                }                             
               
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message, ex);              
            }
            finally
            {             
                if (sw != null) sw.Close();
                if (fs != null) fs.Close();
            }
        }


        /// <summary>
        /// 将CSV文件中内容读取到DataTable中
        /// </summary>
        /// <param name="task_id">任务ID</param>
        /// <param name="file_name">文件名</param>
        /// <param name="hasTitle">是否将CSV文件的第一行读取为DataTable的列名</param>
        /// <returns></returns>
        public static DataTable ReadFromCSV(string task_id,string file_name,  bool hasTitle = false)
        {
            DataTable dt = new DataTable();           //要输出的数据表
            string file = getTaskFilePath(task_id) + file_name + ".csv";
           
            StreamReader sr = new StreamReader(file, Encoding.UTF8); //文件读入流
            bool bFirst = true;                       //指示是否第一次读取数据

            //逐行读取
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] elements = line.Split(',');

                //第一次读取数据时，要创建数据列
                if (bFirst)
                {
                    for (int i = 0; i < elements.Length; i++)
                    {
                        dt.Columns.Add();
                    }
                    bFirst = false;
                }

                //有标题行时，第一行当做标题行处理
                if (hasTitle)
                {
                    for (int i = 0; i < dt.Columns.Count && i < elements.Length; i++)
                    {
                        dt.Columns[i].ColumnName = elements[i];
                    }
                    hasTitle = false;
                }
                else //读取一行数据
                {
                    if (elements.Length == dt.Columns.Count)
                    {
                        dt.Rows.Add(elements);
                    }
                    else
                    {
                        //throw new Exception("CSV格式错误：表格各行列数不一致");
                    }
                }
            }
            sr.Close();
            int count = dt.Rows.Count;

            return dt;
        }
     
        /// <summary>
        /// 2017.10.17 xnn
        /// 读取某个卫星信息
        /// </summary>
        /// <param name="task"></param>
        /// <param name="prn">卫星编号</param>
        /// <param name="hasTitle"></param>
        /// <returns></returns>
        public static DataTable ReadFromCSV(Model.Task task, string prn, bool hasTitle = false)
        {
            DataTable dt = new DataTable();           //要输出的数据表
            string file = getTaskFilePath(task.TaskID) + "Satellite_Infos.csv";

            StreamReader sr = new StreamReader(file, Encoding.UTF8); //文件读入流
            bool bFirst = true;                       //指示是否第一次读取数据

            //逐行读取
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] elements = line.Split(',');

                //第一次读取数据时，要创建数据列
                if (bFirst)
                {
                    for (int i = 0; i < elements.Length; i++)
                    {
                        dt.Columns.Add();
                    }
                    bFirst = false;
                }

                //有标题行时，第一行当做标题行处理
                if (hasTitle)
                {
                    for (int i = 0; i < dt.Columns.Count && i < elements.Length; i++)
                    {
                        dt.Columns[i].ColumnName = elements[i];
                    }
                    hasTitle = false;
                }
                else //读取一行数据
                {
                    if (elements.Length == dt.Columns.Count)
                    {
                        if (elements[5].Equals(prn))
                        {
                            dt.Rows.Add(elements);
                        }                      
                        
                    }
                    else
                    {
                        //throw new Exception("CSV格式错误：表格各行列数不一致");
                    }
                }
            }
            sr.Close();
            int count = dt.Rows.Count;

            return dt;
        }

        /// <summary>
        /// 2017.10.17 xnn
        /// 从文件中，读取PRN编号
        /// </summary>
        /// <param name="task">任务对象</param>
        /// <param name="satellite_class">星系</param>
        /// <returns></returns>
        public static DataTable ReadPrnFromCSV(Model.Task task, string satellite_class)
        {
            DataTable dt = new DataTable();           //要输出的数据表

            List<Int32> prn_list = new List<Int32>();
            string file = getTaskFilePath(task.TaskID) + "Satellite_Infos.csv";

            StreamReader sr = new StreamReader(file, Encoding.UTF8); //文件读入流
            bool bFirst = true;                       //指示是否第一次读取数据

            //逐行读取
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] elements = line.Split(',');

                //第一次读取数据时，要创建数据列
                if (bFirst)
                {
                    dt.Columns.Add("prn", typeof(Int32));                  
                    bFirst = false;
                }

                if (elements[6].Equals(satellite_class))
                {
                    int prn = Convert.ToInt32(elements[5]);

                    if (!prn_list.Contains(prn))
                    {
                        prn_list.Add(prn);
                    }
                   
                }                              

            }
            sr.Close();
            prn_list.Sort();//排序
            for (int i = 0; i < prn_list.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["prn"] = prn_list.ToArray()[i];
                dt.Rows.Add(dr);
            }

            int count = dt.Rows.Count;

            return dt;
        }


        #region 查询

        /// <summary>
        /// 任务文件夹是否存在
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public static bool isTaskDirectoryExist(string task_id)
        {
            try
            {
                string file_path = filePath + task_id;
                if (!Directory.Exists(file_path))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new IOException(ex.Message, ex);
            }

            
        }

        /// <summary>
        /// 2017.9.29 xnn
        /// 判断文件是否存在
        /// </summary>
        /// <param name="task_id"></param>
        /// <param name="file_name"></param>
        /// <returns></returns>
        public static bool isFileExist(string task_id, string file_name)
        {
            string file = getTaskFilePath(task_id) + file_name + ".csv";
            //判断文件的存在

            if (File.Exists(file))
            {
                return true;
            }

            else
            {
                return false;

            }
        }

        #endregion

        #region 删除任务文件

        /// <summary>
        /// 2017.9.26 xnn
        /// 删除某个任务的某个文件
        /// </summary>
        /// <param name="task"></param>
        /// <param name="file_name"></param>
        public static void Delete_file(Model.Task task, string file_name)
        {
            string file = getTaskFilePath(task.TaskID) + file_name + ".csv";

            if (File.Exists(file))
            {
                File.Delete(file);
            }

        }


        #endregion        


    }
}
