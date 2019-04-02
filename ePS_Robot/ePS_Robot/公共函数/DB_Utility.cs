using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;//手工引用
using System.Data.OleDb;//手工引用
using System.Data.SqlClient;

namespace ePS_Robot
{
    class DB_Utility
    {
        public static string MyConnectionString()
        {
            //返回连接字串

            //从注册表读取
            Microsoft.Win32.RegistryKey reg = Microsoft.Win32.Registry.CurrentUser;
            reg = reg.OpenSubKey("Software", true);
            return reg.GetValue("ePS").ToString();
        }

        public string Dlookup(string sqlstr)
        {
            //成功返回值,失败返回空值
            OleDbConnection CN = new OleDbConnection();
            CN.ConnectionString = MyConnectionString();
            OleDbCommand CM = new OleDbCommand();
            CM.Connection = CN;
            CM.CommandType = CommandType.Text;
            CM.CommandText = sqlstr;
            CM.CommandTimeout = 15;
            CN.Open();
            try
            {
                OleDbDataReader RD = CM.ExecuteReader();
                RD.Read();
                return RD.GetValue(0).ToString();
            }
            catch
            {
                return null;
            }
            finally
            {
                CN.Close();
            }
        }

        public string Dexecute(string sqlstr)
        {
            //执行SQL,成功返回"",错误返回错误信息
            OleDbConnection CN = new OleDbConnection();
            CN.ConnectionString = MyConnectionString();
            OleDbCommand CM = new OleDbCommand();
            CM.Connection = CN;
            CM.CommandText = sqlstr;
            CM.CommandType = CommandType.Text;
            CM.CommandTimeout = 15;
            CN.Open();
            OleDbTransaction TA = CN.BeginTransaction();
            CM.Transaction = TA;
            try
            {
                CM.ExecuteNonQuery();
                TA.Commit();
                return null;
            }
            catch (Exception e)
            {
                TA.Rollback();
                return e.Message.ToString();
            }
            finally
            {
                CN.Close();
            }
        }

        /// <summary>
        /// 执行SQL 返回DateSet数据集
        /// </summary>
        public DataSet Dtable(string _sqlstr)
        {
            //返回数据集
            OleDbConnection CN = new OleDbConnection();
            CN.ConnectionString = MyConnectionString();
            OleDbCommand CM = new OleDbCommand();

            CM.Connection = CN;

            CM.CommandType = CommandType.Text;
            CM.CommandText = _sqlstr;
            CM.CommandTimeout = 90;
            CN.Open();
            try
            {
                OleDbDataReader RD = CM.ExecuteReader();
                DataSet DS = new DataSet("DataSet0");
                DataTable DT = new DataTable("DataTable0");
                DataRow DR;
                //列名
                for (int i = 0; i < RD.FieldCount; i++)
                {
                    DT.Columns.Add(RD.GetName(i));
                    //DT.Columns.Add("c" +i.ToString());
                }
                //内容
                while (RD.Read())
                {
                    DR = DT.NewRow();
                    for (int i = 0; i < RD.FieldCount; i++)
                    {
                        DR[i] = RD.GetValue(i).ToString();
                    }
                    DT.Rows.Add(DR);
                }
                DS.Tables.Add(DT);
                return DS;
            }
            catch
            {
                return null;
            }
            finally
            {
                CN.Close();
            }
        }

        /// <summary>
        /// 将DataTable中数据放入数据库对应表中
        /// </summary>
        /// <param name="DT">需导入数据库的表</param>
        /// <param name="TableName">导入数据库中的表名</param>
        /// <returns></returns>
        public string DataTableToSql(DataTable DT, string TableName)
        {
            try
            {
                if (DT.TableName.ToUpper() != "PRODUCT")
                {
                    for (int j = DT.Rows.Count - 1; j >= 0; j--)
                    {
                        if (DT.Rows[j][1].ToString() == null)
                        {
                            DT.Rows.RemoveAt(j);
                            continue;
                        }
                        if (DT.Rows[j][1].ToString() == "")
                        {
                            DT.Rows.RemoveAt(j);
                            continue;
                        }
                    }
                }
                string conn = MyConnectionString();
                int i = conn.IndexOf(";");   //去掉Provider=SQLOLEDB.1;
                conn = conn.Substring(i + 1, conn.Length - i - 1);
                SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.UseInternalTransaction);
                sqlbulkcopy.DestinationTableName = TableName;
                sqlbulkcopy.WriteToServer(DT);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="_UserID"></param>
        /// <param name="_Content"></param>
        /// <returns></returns>
        public Boolean AppendLog(string _UserID, string _Content, string LogType)
        {
            //添加日志
            OleDbConnection CN = new OleDbConnection();
            CN.ConnectionString = MyConnectionString();
            OleDbCommand CM = new OleDbCommand();
            CM.Connection = CN;
            CN.Open();
            CM.CommandText = "insert into Log(UserID,UpdateTime,Content,LogType) select '" + _UserID + "-自动','" + YearMonthDaYTime() + "','" + _Content + "','" + LogType + "'";
            try
            {
                CM.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                CN.Close();
            }
        }

        /// <summary>
        /// 返回当前日期的字符串 格式为 yy-MM-dd HH:mm:ss
        /// </summary>
        /// <returns></returns>
        private string YearMonthDaYTime()
        {
            string year = DateTime.Now.Year.ToString();//.Substring(2, 2);
            string month = DateTime.Now.Month.ToString();
            if (month.Length < 2) { month = "0" + month; }
            string day = DateTime.Now.Day.ToString();
            if (day.Length < 2) { day = "0" + day; }
            string hour = DateTime.Now.Hour.ToString();
            if (hour.Length < 2) { hour = "0" + hour; }
            string minute = DateTime.Now.Minute.ToString();
            if (minute.Length < 2) { minute = "0" + minute; }
            string second = DateTime.Now.Second.ToString();
            if (second.Length < 2) { second = "0" + second; }
            string s = year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;
            return s;
        }



    }
}
