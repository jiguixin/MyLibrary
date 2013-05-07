using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Crosscutting.Tests.Dtos
{
    public class CustomerDto
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
        /// Get or set the city of this address 
        /// </summary>
        public string CusAddressCity { get; set; }

        /// <summary>
        /// Get or set the zip code
        /// </summary>
        public string CusAddressZipCode { get; set; }

        /// <summary>
        /// Get or set address line 1
        /// </summary>
        public string CusAddressAddressLine1 { get; set; }

        /// <summary>
        /// Get or set address line 2  像此属性必须加Customer 的Address 的变量名的前缀在加上该类的属性名。
        /// </summary>
        public string CusAddressAddressLine2 { get; set; }

        //
        public string FilePath { get; set; }

        public List<OrderDto> OrderListDto { get; set; }
    }
}
