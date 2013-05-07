using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Data;
using System.Xml;

namespace Infrastructure.Crosscutting.Utility.CommomHelper
{
    /// <summary>
    /// 文件读写实用类
    /// </summary>
    public static class FileHelper
    {
        #region Validation

        /// <summary>
        /// 检测指定目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        /// <returns></returns>
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// 检测指定文件是否存在,如果存在则返回true。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }

        #endregion

        #region Read
         
        /// <summary>
        /// 获取指定目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetFileNames(string directoryPath)
        {
            if (!FileHelper.IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }
            return Directory.GetFiles(directoryPath);
        }

        /// <summary>
        /// 获取指定目录及子目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            if (!FileHelper.IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }
            string[] files;
            try
            {
                if (isSearchChild)
                {
                    files = Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    files = Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
            return files;
        }

        /// <summary>
        /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetDirectories(string directoryPath)
        {
            string[] directories;
            try
            {
                directories = Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
            return directories;
        }

        /// <summary>
        /// 检测指定目录中是否存在指定的文件,若要搜索子目录请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>        
        public static bool Contains(string directoryPath, string searchPattern)
        {
            bool result;
            try
            {
                string[] fileNames = FileHelper.GetFileNames(directoryPath, searchPattern, false);
                if (fileNames.Length == 0)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 检测指定目录中是否存在指定的文件
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param> 
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            bool result;
            try
            {
                string[] fileNames = FileHelper.GetFileNames(directoryPath, searchPattern, true);
                if (fileNames.Length == 0)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 获取文本文件的行数
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static int GetLineCount(string filePath)
        {
            string[] array = File.ReadAllLines(filePath);
            return array.Length;
        }

        /// <summary>
        /// 获取一个文件的长度,单位为Byte
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static int GetFileSize(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return (int)fileInfo.Length;
        }

        /// <summary>
        /// 获取指定目录及子目录中所有子目录列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            string[] directories;
            try
            {
                if (isSearchChild)
                {
                    directories = Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    directories = Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
            return directories;
        }

        /// <summary>
        /// 读取文件,返回一个byte的数组
        /// </summary>
        /// <param name="fileName">要读取的文件路径</param>
        /// <returns>返回一个byte的数组</returns>
        public static byte[] ReadFile(string fileName)
        {
            FileStream pFileStream = null;
            byte[] pReadByte = new byte[0];
            try
            {
                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(pFileStream);
                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                pReadByte = r.ReadBytes((int)r.BaseStream.Length);
                return pReadByte;
            }
            catch
            {
                return pReadByte;
            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
        }

        /// <summary>
        /// 读取文件的内容到List中
        /// </summary>
        /// <param name="fileName">文件名+文件路径</param>
        /// <returns></returns>
        public static List<string> ReadFileLine(string fileName)
        {
            List<string> lst = new List<string>();
            if (string.IsNullOrEmpty(fileName))
                return lst;

            StreamReader sr = new StreamReader(fileName, Encoding.Default);

            string content = string.Empty;

            while (content != null)
            {
                content = sr.ReadLine();

                if (content != null)
                {
                    lst.Add(content);
                }
            }

            sr.Close();

            return lst;
        }

        /// <summary>
        /// 读取整个文件内容
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadFileContent(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(fileName))
                return "";
            StreamReader sr = new StreamReader(fileName, Encoding.Default);

            string content = sr.ReadToEnd();
            sr.Close();

            return content;
        }

        /// <summary>
        /// 读取Excel到DataSet中
        /// </summary>
        /// <param name="excelFilePath"></param>
        /// <returns></returns>
        public static DataSet ReadFileByExcel(string excelFilePath)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(excelFilePath))
                return ds;
            string connStr = "Provider=Microsoft.Jet.Oledb.4.0;Data Source=" + excelFilePath +
                                ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";

            List<string> lstTables = OleDbHelper.GetExcelTableNameList(connStr);

            if (lstTables == null || lstTables.Count == 0)
                return ds;

            string baseSql = "select * from [{0}]";


            foreach (string table in lstTables)
            {
                string sql = string.Format(baseSql, table);

                DataTable dt = OleDbHelper.ExecuteDataTable(connStr, CommandType.Text, sql);

                ds.Tables.Add(dt);
            }
            return ds;
        }

        public static Dictionary<string, string> ReadConfig()
        {
            Dictionary<string, string> setting = new Dictionary<string, string>();
            XmlDocument xdoc = new XmlDocument();
            XmlElement xroot = null;

            try
            {
                xdoc.Load("Connection.xml");
                xroot = xdoc.DocumentElement;

                foreach (XmlElement xChild in xroot.ChildNodes.OfType<XmlElement>())
                    setting.Add(xChild.Attributes["name"].InnerText, xChild.FirstChild.InnerText);
            }
            catch
            {
            }

            return setting;
        }
         
        #endregion

        #region Write

        /// <summary>
        /// 复制文件夹(递归)
        /// </summary>
        /// <param name="varFromDirectory">源文件夹路径</param>
        /// <param name="varToDirectory">目标文件夹路径</param>
        public static void CopyFolder(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);
            if (!Directory.Exists(varFromDirectory))
            {
                return;
            }
            string[] directories = Directory.GetDirectories(varFromDirectory);
            if (directories.Length > 0)
            {
                string[] array = directories;
                for (int i = 0; i < array.Length; i++)
                {
                    string text = array[i];
                    FileHelper.CopyFolder(text, varToDirectory + text.Substring(text.LastIndexOf("\\")));
                }
            }
            string[] files = Directory.GetFiles(varFromDirectory);
            if (files.Length > 0)
            {
                string[] array2 = files;
                for (int j = 0; j < array2.Length; j++)
                {
                    string text2 = array2[j];
                    File.Copy(text2, varToDirectory + text2.Substring(text2.LastIndexOf("\\")), true);
                }
            }
        }

        /// <summary>
        /// 检查文件,如果文件不存在则创建  
        /// </summary>
        /// <param name="FilePath">路径,包括文件名</param>
        public static void ExistsFile(string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                FileStream fileStream = File.Create(FilePath);
                fileStream.Close();
            }
        }
       
        /// <summary>
        /// 向文本文件中写入内容,如果文件存在则覆盖
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">写入的内容</param>
        /// <param name="encoding">编码</param>
        public static void WriteText(string filePath, string text, Encoding encoding)
        {
            File.WriteAllText(filePath, text, encoding);
        }

        /// <summary>
        /// 向文本文件的尾部追加内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="content">写入的内容</param>
        public static void AppendText(string filePath, string content)
        {
            File.AppendAllText(filePath, content);
        }

        /// <summary>
        /// 删除指定目录及其所有子目录
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void DeleteDirectory(string directoryPath)
        {
            if (FileHelper.IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

        /// <summary>
        /// 清空指定目录下所有文件及子目录,但该目录依然保存.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void ClearDirectory(string directoryPath)
        {
            if (FileHelper.IsExistDirectory(directoryPath))
            {
                string[] fileNames = FileHelper.GetFileNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    File.Delete(fileNames[i]);
                }
                string[] directories = FileHelper.GetDirectories(directoryPath);
                for (int j = 0; j < directories.Length; j++)
                {
                    FileHelper.DeleteDirectory(directories[j]);
                }
            }
        }

        /// <summary>
        /// 清空文件内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void ClearFile(string filePath)
        {
            File.Delete(filePath);
            FileHelper.CreateFile(filePath);
        }
         
        /// <summary>
        /// 写byte[]到fileName
        /// </summary>
        /// <param name="pReadByte">要写入的数据</param>
        /// <param name="fileName">要写到的文件路径和文件名</param>
        /// <returns>若写入成功返回true,否则返回false</returns>
        public static bool WriteFile(byte[] pReadByte, string fileName)
        {

            FileStream pFileStream = null;
            try
            {
                pFileStream = new FileStream(fileName, FileMode.OpenOrCreate);
                pFileStream.Write(pReadByte, 0, pReadByte.Length);
            }

            catch
            {
                return false;
            }

            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();

            }

            return true;

        }

        /// <summary>
        /// 创建文件目录
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <returns></returns>
        public static bool CreateDirectory(string strPath)
        {
            try
            {
                if (strPath.Length == 0)
                { return false; }

                if (Directory.Exists(strPath))
                { return true; }

                Directory.CreateDirectory(strPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CreateFile(string strPath)
        {
            try
            {
                if (string.IsNullOrEmpty(strPath))
                    return false;
                if (File.Exists(strPath))
                    return true;

                File.CreateText(strPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
         
        /// <summary>
        /// 写文本日志
        /// </summary>
        /// <param name="strFile">文件名及路径</param>
        /// <param name="strLog">记录日志内容</param>
        public static void WriteFileLog(string strFile, string strLog)
        {
            try
            { 
                string fullPath = Path.GetDirectoryName(strFile);

                if (string.IsNullOrEmpty(fullPath))
                {
                    strFile = AppDomain.CurrentDomain.BaseDirectory + "\\" + strFile;
                }
                else
                {
                    CreateDirectory(strFile); 
                }

                StreamWriter sw = new StreamWriter(strFile, true);
                sw.WriteLine("[" + DateTime.Now.ToString() + "]：" + strLog);
                sw.Flush();
                sw.Close(); 
            }
            catch
            {
                throw;
            }
        }
         
        #endregion
    }
}
