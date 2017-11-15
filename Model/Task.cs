using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public partial class Task
    {
        public Task() { }

        #region Model
        private string Task_Id;                 //任务ID
        private string Task_Desc;               //任务描述
        private DateTime Time;                    //创建任务时间
        private string Compare_Device_Id;       //对比机设备ID

        private string Start_Time;       //测试开始时间 2017.9.12 xnn
        private string End_Time;         //测试结束时间

        private DataTable DT_Fix_Infos;        //定位信息表  2017.9.19 xnn

        private DataTable DT_Satellite_Infos;  //卫星信息表 2017.9.19 xnn

        private DataTable DT_CEP_Infos;          //距离概率表 2017.11.1 xnn

        private string PC_ID;//上传任务的主机ID 2017.10.11 xnn

        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskID
        {
            set { Task_Id = value; }
            get { return Task_Id; }
        }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string TaskDesc
        {
            set { Task_Desc = value; }
            get { return Task_Desc; }
        }

        /// <summary>
        /// 创建任务时间
        /// </summary>
        public DateTime CreateTime
        {
            set { Time = value; }
            get { return Time; }
        }

        /// <summary>
        /// 对比机设备ID
        /// </summary>
        public string CompareDeviceID
        {
            set { Compare_Device_Id = value; }
            get { return Compare_Device_Id; }
        }

        /// <summary>
        /// 测试开始时间
        /// </summary>
        public string StartTime
        {
            set { Start_Time = value; }
            get { return Start_Time; }
        }

        /// <summary>
        /// 测试结束时间
        /// </summary>
        public string EndTime
        {
            set { End_Time = value; }
            get { return End_Time; }
        }

        /// <summary>
        /// 定位信息表
        /// </summary>
        public DataTable DT_FixInfos
        {
            set { DT_Fix_Infos = value; }
            get { return DT_Fix_Infos; }
        }

        /// <summary>
        /// 卫星信息表
        /// </summary>
        public DataTable DT_SatelliteInfos
        {
            set { DT_Satellite_Infos = value; }
            get { return DT_Satellite_Infos; }
        }

        public DataTable DT_CEP
        {
            set { DT_CEP_Infos = value; }
            get { return DT_CEP_Infos; }
        }

        /// <summary>
        /// 主机ID
        /// </summary>
        public string PCID
        {
            set { PC_ID = value; }
            get { return PC_ID; }
        }


        #endregion
    }
}
