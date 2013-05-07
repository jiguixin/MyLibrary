using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Crosscutting.Tests.Model
{
    public class Customer
    {
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
        /// Get or set the address of this customer
        /// </summary>
        public virtual Address CusAddress { get; set; }

        /// <summary>
        /// Get or set associated photo for this customer
        /// </summary>
        public virtual Picture Picture { get; set; }

        public virtual List<Order> OrderList { get; set; }
    }
}
