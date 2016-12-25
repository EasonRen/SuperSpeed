using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.Threading;
using System.Diagnostics;
using Microsoft.Win32;

namespace SuperSpeed
{
    public partial class Form1 : Form
    {
        private const string filePath = @"Profiles\TaskDb.dat";
        public Form1()
        {
            InitializeComponent();
        }
       
        private void btnCrock_Click(object sender, EventArgs e)
        {
            string thunderDbPath = string.Empty;
            RegistryKey thunderKey = Registry.CurrentUser;

            if (string.IsNullOrEmpty(this.tbFilePath.Text))
            {
                try
                {
                    RegistryKey thun = thunderKey.OpenSubKey(@"Software\Thunder Network\Thunder", false);
                    string exePath = thun.GetValue("Path").ToString();
                    string rootPath = Path.GetDirectoryName(Path.GetDirectoryName(exePath));
                    thunderDbPath = Path.Combine(rootPath, filePath);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    thunderKey.Close();
                }

                if (string.IsNullOrEmpty(thunderDbPath))
                {
                    MessageBox.Show("没有安装目录");
                    return;
                }
                else
                {
                    this.tbFilePath.Text = thunderDbPath;
                }
            }
            else
            {
                thunderDbPath = this.tbFilePath.Text;
            }
           

            List<string> tableNameList = new List<string>();
            List<string> superspeed = new List<string>();
            int updateResult = 0;

            string sqlSelectTableName = string.Format("SELECT tbl_name FROM sqlite_master WHERE type='{0}'", "table");
            SQLiteHelper sqlite = new SQLiteHelper(thunderDbPath);

            DataTable tableName = sqlite.ExecuteDataTable(sqlSelectTableName);

            if (tableName != null && tableName.Rows.Count > 0)
            {
                for (int i = 0; i < tableName.Rows.Count; i++)
                {
                    DataRow dr = tableName.Rows[i];
                    if (!string.IsNullOrEmpty(dr["tbl_name"].ToString()))
                    {
                        tableNameList.Add(dr["tbl_name"].ToString());
                    }
                }
            }

            superspeed = tableNameList.Where(t => t.Contains("superspeed")).ToList();

            foreach (var row in superspeed)
            {
                updateResult += UpdateSpeedResult(thunderDbPath, row.ToString());
            }

            if (updateResult >= 0)
            {
                MessageBox.Show("操作成功");
            }

        }

        private int UpdateSpeedResult(string filePath, string tableName)
        {

            List<Speed> speedList = new List<Speed>();
            List<Speed> speedModifyList = new List<Speed>();

            Speed speed;
            UserData userData;

            string sqlSelect = string.Format("SELECT LocalTaskId,UserData FROM '{0}'", tableName);
            SQLiteHelper sqliteHelper = new SQLiteHelper(filePath);

            DataTable speedDataTable = sqliteHelper.ExecuteDataTable(sqlSelect);

            if (speedDataTable != null && speedDataTable.Rows.Count > 0)
            {
                for (int i = 0; i < speedDataTable.Rows.Count; i++)
                {
                    speed = new Speed();
                    DataRow dr = speedDataTable.Rows[i];

                    speed.LocalTaskId = dr[Speed.LOCAL_TASK_ID].ToString();
                    speed.UserData = new UTF8Encoding().GetString(dr[Speed.USER_DATA] as byte[]);

                    speedList.Add(speed);
                }
            }

            StringBuilder allUpdateSql = new StringBuilder();
            if (speedList.Count > 0)
            {
                foreach (var item in speedList)
                {
                    userData = new UserData();

                    userData = new JavaScriptSerializer().Deserialize<UserData>(item.UserData);
                    userData.Result = 0;

                    allUpdateSql.Append(string.Format("UPDATE {0} SET UserData = '{1}' WHERE LocalTaskId={2};", tableName, new JavaScriptSerializer().Serialize(userData), item.LocalTaskId));

                }
            }

            return sqliteHelper.ExecuteNonQuery(allUpdateSql.ToString());

        }
    }
}
