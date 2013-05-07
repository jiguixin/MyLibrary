using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace Infrastructure.Crosscutting.Utility.CommomHelper
{
    public partial class ExcelHelper
    {
        private const string connStringExcel03 =
            "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=YES\"";

        /// <summary>
        /// 得到EXCEL的第一张表格的数据
        /// </summary>
        /// <param name="filePath">文件路径 可以是EXCEL2003 也可以是EXCEL2007</param> 
        /// <returns>第一个表的内容</returns>
        public static DataTable GetExcelData(string filePath)
        {
            List<string> lst;
            OleDbConnection conn = GetTableNames(filePath, out lst);

            conn.Open();

            DataSet ds = OleDbHelper.ExecuteDataset(conn, CommandType.Text, "select * from [" + lst[0] + "]");

            if (ds == null || ds.Tables.Count <= 0)
            {
                conn.Close();
                return null;
            }

            DataTable dt = ds.Tables[0];

            conn.Close();
            return dt;
        }

        /// <summary>
        /// 得到EXCEL下的所有表名
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="lst">一组表名</param>
        /// <returns></returns>
        private static OleDbConnection GetTableNames(string filePath, out List<string> lst)
        {
            string connStr = string.Empty;

            connStr = string.Format(connStringExcel03, filePath);

            var conn = new OleDbConnection(connStr);
            try
            {
                conn.Open();
            }
            catch
            {
            }
            DataTable dtb = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                                                     new object[] {null, null, null, "Table"});
            lst = new List<string>();
            foreach (DataRow dr in dtb.Rows)
            {
                string tbname = dr["TABLE_NAME"].ToString();
                lst.Add(tbname);
            }
            conn.Close();
            return conn;
        }

        /// <summary>
        /// 得到EXCEL的指定表格的数据
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="sheetName">可以查整个Sheet， 注：如果只想读取A1到B2的内容 Sheet1$A1:B2 。 表名不需要加[]</param>
        /// <returns></returns>
        public static DataTable GetExcelData(string filePath, string sheetName)
        {
            string connStr = string.Empty;

            connStr = string.Format(connStringExcel03, filePath);

            DataSet ds = OleDbHelper.ExecuteDataset(connStr, CommandType.Text,
                                                    string.Format("select * from [{0}]", sheetName));

            if (ds == null || ds.Tables.Count <= 0)
            {
                return null;
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 得到所有的SHEET表格
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static DataSet GetExcelDataSet(string filePath)
        {
            List<string> lst;
            OleDbConnection conn = GetTableNames(filePath, out lst);
            conn.Open();
            var ds = new DataSet("test");
            foreach (string s in lst)
            {
                OleDbDataAdapter myCommand = null;
                var dt = new DataTable();

                //从指定的表明查询数据,可先把所有表明列出来供用户选择
                string strExcel = "select * from [" + s + "]";
                myCommand = new OleDbDataAdapter(strExcel, conn);
                dt = new DataTable();
                if (!string.IsNullOrEmpty(s) && s.Length > 1)
                    dt.TableName = s.Remove(s.Length - 1, 1);
                myCommand.Fill(dt);
                ds.Tables.Add(dt);
            }
            conn.Close();
            return ds;
        }


    }
}