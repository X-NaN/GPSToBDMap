using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;

namespace GPSToBDMap
{
    //要想调用JS的类都需要添加一下两句  

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]

    [System.Runtime.InteropServices.ComVisibleAttribute(true)]  
    public partial class BDMap : Form
    {
        public static List<Model.GPS> points_list = new List<Model.GPS>();
        public BDMap()
        {
            InitializeComponent();
            webBrowser1.ObjectForScripting = this;    //这句是必不可少的，是调用JS的前提  
        }

        private void BDMap_Load(object sender, EventArgs e)
        {
            try
            {

                DataTable dt_FixInfos_BD = new DataTable();//坐标转换成百度的fix文件

                dt_FixInfos_BD = CSVHelper.ReadFromCSV("32", "Fix_Infos", true);
                int count = dt_FixInfos_BD.Rows.Count;

                for (int i = 0; i < dt_FixInfos_BD.Rows.Count; i++)
                {
                    string fix_quality = dt_FixInfos_BD.Rows[i]["fix_quality"].ToString();
                    //定位成功
                    if (!fix_quality.Equals("0") && !fix_quality.Equals("6"))
                    {
                        Model.GPS gps = new Model.GPS();
                        double latitude = Convert.ToDouble(dt_FixInfos_BD.Rows[i]["latitude"].ToString());
                        double longitude = Convert.ToDouble(dt_FixInfos_BD.Rows[i]["longitude"].ToString());

                        gps.Latitude = latitude;
                        gps.Longitude = longitude;

                        points_list.Add(gps);


                    }
                }

                int length = points_list.Count;

                //webBrowser1.Url = new Uri("https://www.baidu.com");
                //这个文件于可执行文件放在同一目录
                webBrowser1.Url = new Uri(Path.Combine(Application.StartupPath, "实例.html"));
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //绘制轨迹
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //这里传入x、y的值，调用JavaScript脚本 珠海经纬度113.571207,22.379602
            //webBrowser1.Document.InvokeScript("setLocation", new object[] { 113.571207, 22.379602 });
            string json_points_list = JsonConvert.SerializeObject(points_list);//设备信息json串

            json_points_list = "[{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5738433},{\"Latitude\":22.37538,\"Longitude\":113.5938433},{\"Latitude\":22.33538,\"Longitude\":113.5238433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433}]";
            var result = webBrowser1.Document.InvokeScript("setData", new string[] { json_points_list });
            if (webBrowser1.ReadyState < WebBrowserReadyState.Complete) return;
        }

        //定位
        private void btn_Map_Click(object sender, EventArgs e)
        {
            string json_points_list = JsonConvert.SerializeObject(points_list);//设备信息json串
            json_points_list = "[{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5738433},{\"Latitude\":22.37538,\"Longitude\":113.5938433},{\"Latitude\":22.33538,\"Longitude\":113.5238433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433},{\"Latitude\":22.37538,\"Longitude\":113.5638433}]";
            for (int i = 0; i < points_list.Count; i++)
            {
                Model.GPS gps = new Model.GPS();
                gps.Longitude = points_list[i].Longitude;
                gps.Latitude = points_list[i].Latitude;
                webBrowser1.Document.InvokeScript("setLocation", new object[] { gps.Longitude,gps.Latitude });
            }
                
        }
    }
}
