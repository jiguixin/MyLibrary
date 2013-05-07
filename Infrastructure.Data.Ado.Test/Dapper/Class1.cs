using System;
using System.Linq;
using Infrastructure.Crosscutting.Utility;
using NUnit.Framework;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using Infrastructure.Data.Ado.Dapper; 

namespace Infrastructure.Data.Ado.Test.Dapper
{
    [TestFixture]
    public class Class1
    {
        private string connString = "Data Source=jiguixin\\sqlexpress;Initial Catalog=DBMainBoundedContextTests;Uid=jiguixin;pwd=123456;";
        
        [Test]
        public void TestDapper()
        {
            CodeTimer.Time("Dapper", 1, () =>
                                        {
                                            using (SqlConnection conn = new SqlConnection(connString))
                                            {
                                                conn.Open();
                                                 
                                                //var lst = conn.Query<Customer>("select * from Customers ");

                                                //Console.WriteLine(lst.Count());

                                                string sql =
                                                    string.Format(
                                                        "insert into Customers Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                                        Guid.NewGuid(), "sds", "sf", "sf", "sf", "sf", 212, true);

                                                conn.Execute(sql);
                                            } 
                                        });
        }
        [Test]
        public void TestDbExecutor()
        {
            CodeTimer.Time("DbExecutor", 1, () =>
            {
                using (DbExecutor db = new DbExecutor(new SqlConnection(connString)))
                {
                    //var lst = db.Select<Customer>("select * from Customers "); 

                    //Console.WriteLine(lst.Count());
                    string sql =
                                                   string.Format(
                                                       "insert into Customers Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                                       Guid.NewGuid(), "sds", "sf", "sf", "sf", "sf", 212, true);
                    db.ExecuteNonQuery(sql);
                }
            });
        } 
    }

    public class Customer 
    {
        #region Members

        bool _IsEnabled;

        #endregion

        #region Properties

        public Guid Id { get; set; }

        /// <summary>
        /// Get or set the Given name of this customer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Get or set the surname of this customer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Get or set the full name of this customer
        /// </summary>
        public string FullName
        {
            get
            {
                return string.Format("{0}, {1}", this.LastName, this.FirstName);
            }
            set { }

        }

        /// <summary>
        /// Get or set the telephone 
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Get or set the company name
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Get or set the current credit limit for this customer
        /// </summary>
        public decimal CreditLimit { get; set; }

        /// <summary>
        /// Get or set if this customer is enabled
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            private set
            {
                _IsEnabled = value;
            }
        }

        #endregion

    }
}
