using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;

namespace GPSToBDMap
{
    public partial class BDConvert : Form
    {
        public BDConvert()
        {
            InitializeComponent();
        }

        private void btn_AddFile_Click(object sender, EventArgs e)
        {
            try
            {
                string nmea_path = "";
                DataTable dt_nmea_path = new DataTable();
                DataTable dt = new DataTable();              

                dt.Columns.Add("nmea_path");

                OpenFileDialog op = new OpenFileDialog();
                //过滤器           
                op.Filter = "csv(*.csv)|*.csv;";//只能是.csv
                DialogResult res = op.ShowDialog();
                nmea_path = op.FileName;

                this.txt_NMEAFile.Text = nmea_path;

            }
            catch (Exception ex)
            {
                Utility.Log.WriteSysLog("BDMap," + MethodBase.GetCurrentMethod().Name, ex);
                MessageBox.Show("操作不成功,请重试或与管理员联系!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //坐标转换
        private void btn_Convert_Click(object sender, EventArgs e)
        {
            try
            {               
                DataTable dt_FixInfos = new DataTable();
                DataTable dt_FixInfos_BD = new DataTable();//坐标转换成百度的fix文件

                dt_FixInfos = CSVHelper.ReadFromCSV("32", "Fix_Infos", true);
                int count = dt_FixInfos.Rows.Count;

                dt_FixInfos_BD = dt_FixInfos;
                for (int i = 0; i < dt_FixInfos.Rows.Count; i++)
                {
                    string fix_quality = dt_FixInfos.Rows[i]["fix_quality"].ToString();
                    //定位成功
                    if (!fix_quality.Equals("0") && !fix_quality.Equals("6"))
                    {
                        double latitude = Convert.ToDouble(dt_FixInfos.Rows[i]["latitude"].ToString());
                        double longitude = Convert.ToDouble(dt_FixInfos.Rows[i]["longitude"].ToString());

                        Model.GPS gps = new GPS();
                        gps = PositionUtil.getBaiducoor(longitude, latitude);

                        dt_FixInfos_BD.Rows[i]["latitude"] = gps.Latitude;
                        dt_FixInfos_BD.Rows[i]["longitude"] = gps.Longitude;
                    }
                    
                }

                CSVHelper.SaveCsv(dt_FixInfos_BD, "32", "Fix_Infos_BD");
                MessageBox.Show("坐标转换完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;


            }
            catch (Exception ex)
            {
                Utility.Log.WriteSysLog("BDMap," + MethodBase.GetCurrentMethod().Name, ex);
                MessageBox.Show("操作不成功,请重试或与管理员联系!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BDMap_Load(object sender, EventArgs e)
        {

        }
    }
}
