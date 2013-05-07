using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Infrastructure.Crosscutting.Tests.Utility.CommomHelper
{
    /// <summary>
    /// ExcelHelperTest 的摘要说明
    /// </summary>
    [TestClass]
    public class ExcelHelperTest
    {
        public ExcelHelperTest()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ReadExcel()
        {
            using (ExcelHelper excel = new ExcelHelper("abc.xlsx"))
            {
                excel.Hdr = "YES";
                DataTable dt = excel.ReadTable("Sheet1");
                Console.WriteLine(dt.Columns[0].ToString()); 
            }
            
        }

        [TestMethod]
        public void WriteExcel()
        {
            using (ExcelHelper excel = new ExcelHelper("abcds.xls"))
            {
                excel.Hdr = "YES";
                excel.Imex = "0"; 
                Dictionary<string, string> tableDefinition = new Dictionary<string, string>();
                tableDefinition.Add("Name", "ntext");
                tableDefinition.Add("Age", "ntext");
                tableDefinition.Add("Class", "ntext");

                excel.WriteTable("Student", tableDefinition);

                StringBuilder sb = new StringBuilder();
//                //for (int i = 0;i <100; i++)
//                //{
                    sb.Append(" insert into [Student] (Name,Age,Class) values (");
////                }
// 
                    sb.Append("'"); sb.Append("a"); sb.Append("',");
                    sb.Append("'"); sb.Append("b"); sb.Append("',");
                    sb.Append("'"); sb.Append("c"); sb.Append("'");
                    sb.Append(")");
                    Console.WriteLine(sb.ToString());
                    excel.ExecuteCommand(sb.ToString());

//                excel.ExecuteCommand(sql);
            }

        }
    }
}
