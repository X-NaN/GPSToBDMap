using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*----------------------------------------------------------------
// Copyright (C) 2017 魅族科技有限公司
// 版权所有。 
//
// 文件名：Log
// 文件功能描述：操作日志逻辑，将日志记录进log文件，或写入数据库（留给以后扩充）
//
// 
// 创建标识：邢娜娜 2017.7.27
//
// 修改标识：
// 修改描述：
//  
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
namespace Utility
{
    /// <summary>
    /// 日志操作
    /// </summary>
    public class Log
    {

        private static string logFormat;
        private static String sysLogFormat = "{0} / {1} / {2} / {3}\r\n";
        private static String sysExeLogFormat = "{0} / {1} \r\n";

        /// <summary>
        /// 日志，写入Log文件，出错记录
        /// </summary>
        /// <param name="formName">窗体名称</param>
        /// <param name="e">异常</param>
        public static void WriteSysLog(String formName, Exception e)
        {
            try
            {
                String time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //此处使用本地时间，如果服务器连不上，自然也不能获取到服务器时间。错误日志也并不需要与系统实际对应。czq
                String str = String.Format(sysLogFormat, time, formName, e.TargetSite.ToString(), e.Message);
                String dirPath = Utility.Common.GetDirPath();
                String filePath = dirPath + "\\log.log";
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                }
                FileStream fs = new FileStream(filePath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.WriteLine(str);
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {
                //2017.28  xnn  记录日志出错
            }
        }

        /// <summary>
        /// 2017.10.18 xnn
        /// 上传消息
        /// </summary>
        /// <param name="formName">窗体名</param>
        /// <param name="code">消息代码</param>
        /// <param name="message">消息</param>
        public static void WriteSysLog(String formName, string code, string message)
        {
            try
            {
                String time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //此处使用本地时间，如果服务器连不上，自然也不能获取到服务器时间。错误日志也并不需要与系统实际对应。czq
                String str = String.Format(sysLogFormat, time, formName, code, message);
                String dirPath = Utility.Common.GetDirPath();
                String filePath = dirPath + "\\log.log";
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                }
                FileStream fs = new FileStream(filePath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.WriteLine(str);
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {
                //2017.28  xnn  记录日志出错
            }
        }

        /// <summary>
        /// 记录方法执行时间
        /// </summary>
        /// <param name="information"></param>
        /// <param name="e"></param>
        public static void WriteSysExeTimeLog(String information)
        {
            try
            {
                String time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                String str = String.Format(sysExeLogFormat, time, information);
                String dirPath = Utility.Common.GetDirPath();
                String filePath = dirPath + "\\exeTime.log";
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                }
                FileStream fs = new FileStream(filePath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.WriteLine(str);
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {
                //2017.28  xnn  记录日志出错
            }
        }



    }
}
