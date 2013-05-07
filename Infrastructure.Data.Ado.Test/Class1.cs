using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Crosscutting.Utility;
using NUnit.Framework;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace Infrastructure.Data.Ado.Test
{
    [TestFixture]
    public class Class1
    {
        private string connString = "Data Source=jiguixin;Initial Catalog=NHibernateSample;Uid=jiguixin;pwd=123456;";
        [Test]
        public void Test()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                DbDataAdapter sqAdapter = new SqlDataAdapter("select * from Customer", conn);
                DbCommandBuilder builder = new SqlCommandBuilder();
                builder.DataAdapter = sqAdapter;

                DataTable dt = new DataTable();

                Console.WriteLine(string.Format("Fill Count:{0}", sqAdapter.Fill(dt)));

                dt.Rows[0]["Firstname"] = "jiguixin";




                Console.WriteLine(string.Format("Update Count:{0}", sqAdapter.Update(dt)));

            }

        }

        [Test]
        public void BeginGetTableTest()
        {
            DataTable dt;
            DataTable dt1;
            DataSet ds;
            dt = new DataTable();
            dt.Columns.Add("SysId");
            dt.Columns.Add("Name");
            dt.Columns.Add("Mark");
            CodeTimer.Time("ss", 1, () =>
                                        {
                                            using (DbExecutor db = new DbExecutor(new SqlConnection(connString)))
                                            {
                                                dt = db.BeginGetTable(SqlClientFactory.Instance, "select * from MyTest");

                                                //dt.Rows[0]["Name"] = "ji1111n";

                                                DataRow dr = dt.NewRow();

                                                dt.Rows.Add(dr);

                                                dr["Name"] = "jiguixin4444";
                                                dr["Mark"] = "Makr4444";
                                                //db.CommitData(dt, "MyTest");

                                                db.EndCommitTable(dt);

                                                Console.WriteLine(dr["SysId"].ToString());
                                                Console.WriteLine(dr["Name"]);

                                                var mydt = db.BeginGetTable(SqlClientFactory.Instance,
                                                                            "select * from MyTest where SysId=@SysId",
                                                                            new
                                                                                {
                                                                                    SysId
                                                                                =
                                                                                100054
                                                                                }
                                                    );
                                                mydt.Rows[0]["Name"] = "JIm";
                                                db.EndCommitTable(mydt);

                                            }
                                        });


        }


        [Test]
        public void BeginGetTableTranTest()
        {
            DataTable dt;
            DataTable dt1;
            DataSet ds;
            dt = new DataTable();
            dt.Columns.Add("SysId");
            dt.Columns.Add("Name");
            dt.Columns.Add("Mark");
            CodeTimer.Time("ss", 1, () =>
            {
                using (DbExecutor db = new DbExecutor(new SqlConnection(connString),IsolationLevel.ReadCommitted))
                {
                    dt = db.BeginGetTable(SqlClientFactory.Instance, "select * from MyTest");

                    dt.Rows[0]["Name"] = "Test" + DateTime.Now;
                     
                    dt.Rows[dt.Rows.Count - 1].Delete();

                    DataRow dr = dt.NewRow();

                    dt.Rows.Add(dr);

                    dr["Name"] = "jiguixinss"+DateTime.Now;
                    dr["Mark"] = "Makr4444"; 
                     
                    db.EndCommitTable(dt);

                    Console.WriteLine(dr["SysId"].ToString());
                    Console.WriteLine(dr["Name"]);

                    //var mydt = db.BeginGetTable(SqlClientFactory.Instance,
                    //                            "select * from MyTest where SysId=@SysId",
                    //                            new
                    //                            {
                    //                                SysId
                    //                            =
                    //                            100054
                    //                            }
                    //    );
                    //mydt.Rows[0]["Name"] = "JIm"+DateTime.Now;
                    //db.EndCommitTable(mydt);

                    db.TransactionComplete();
                }
            });


        }

        [Test]
        public void GetTablesTest()
        {  
            using (DbExecutor db = new DbExecutor(new SqlConnection(connString)))
            {
                var ds = db.GetTables(SqlClientFactory.Instance, "select * from MyTest;select * from MyTest1");
                Assert.IsNotNull(ds);
                Assert.IsNotNull(ds.Tables[0]);
                Assert.IsTrue(ds.Tables[0].Rows.Count > 0);
                Console.WriteLine(ds.Tables[0].Rows.Count);

                Assert.IsNotNull(ds.Tables[1]);
                Assert.IsTrue(ds.Tables[1].Rows.Count > 0);
                Console.WriteLine(ds.Tables[1].Rows.Count);
            }
        }
         
        [Test]
        public void GetTablesAndGetTableStaticTest()
        {
            var dt = DbExecutor.GetTable(SqlClientFactory.Instance, connString, "select * from MyTest");

            Assert.IsNotNull(dt);
            Console.WriteLine(dt.Rows.Count);

            var strs = Util.TableToArray<string>(dt, "Name");

            Assert.IsNotNull(strs);

            Console.WriteLine(strs.Length);

            var ds = DbExecutor.GetTables(SqlClientFactory.Instance, connString,
                                          "select * from MyTest; select * from Customer;");

            Assert.IsNotNull(ds);
            Console.WriteLine(ds.Tables.Count);
        }

        [Test]
        public  void DbProviderTest()
        {
            using (DbExecutor db = new DbExecutor(new SqlConnection(connString)))
            {
                var dt = db.GetTable(SqlClientFactory.Instance, "select * from MyTest");
                Assert.IsNotNull(dt);
                Assert.IsTrue(dt.Rows.Count > 0);
                Console.WriteLine(dt.Rows.Count);
            }
        }

        [Test]
        public void CommitDataTest()
        {
            using (DbExecutor db = new DbExecutor(new SqlConnection(connString)))
            {
                var dt = db.GetTable(SqlClientFactory.Instance, "select * from MyTest");

                dt.Rows[0]["Name"] = "zhangjuncui";

                DataRow newDr = dt.NewRow();
                dt.Rows.Add(newDr);
                newDr["Name"] = "ABCD";
                newDr["Mark"] = "1244";

                db.CommitData(SqlClientFactory.Instance, dt, "MyTest");

                Console.WriteLine("Id:"+newDr["SysId"]);
            }
        }
        [Test]
        public void CommitDataTranTest()
        {
            using (DbExecutor db = new DbExecutor(new SqlConnection(connString),IsolationLevel.ReadCommitted))
            {
                var dt = db.GetTable(SqlClientFactory.Instance, "select * from MyTest");

                dt.Rows[1]["Name"] = "zhangjuncui";

                DataRow newDr = dt.NewRow();
                dt.Rows.Add(newDr);
                newDr["Name"] = DateTime.Now;
                newDr["Mark"] = "1244";

                db.CommitData(SqlClientFactory.Instance, dt, "MyTest");

                Console.WriteLine("Id:" + newDr["SysId"]);
                db.TransactionComplete();
            }
        }

        [Test]
        public void ExecuteReaderTest()
        {
            using(DbExecutor db = new DbExecutor(new SqlConnection(connString)))
            {
                 var s = db.ExecuteReader("select * from MyTest").Select(dr=>new {Name=dr["Name"],mark=dr["Mark"]});
                 
                foreach (var v in s)
                {
                     Console.WriteLine(v.mark);               
                }

            }
        }

        [Test]
        public void TestStoreProc()
        { 
            using (DbExecutor db = new DbExecutor(new SqlConnection(@"Data Source=JIGUIXIN\SQLEXPRESS;Initial Catalog=MyTest;Uid=jiguixin;pwd=123456;")))
            {
                SqlParameter par = new SqlParameter("p2",SqlDbType.Int,4);
                par.Direction = ParameterDirection.Output;

                //dynamic s2 = new { p1 = 1, par };

                var s = db.ExecuteNonQuery((string) "[dbo].[sp_name]",  new {p1=1,par}, CommandType.StoredProcedure);

                Console.WriteLine(par.Value);
            }
        }

    }
}
