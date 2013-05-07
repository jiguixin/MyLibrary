using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Crosscutting.Tests.Model
{
    public class Address
    {
        /// <summary>
        /// Get or set the city of this address 
        /// </summary>
        public string City { get;  set; }

        /// <summary>
        /// Get or set the zip code
        /// </summary>
        public string ZipCode { get;  set; }

        /// <summary>
        /// Get or set address line 1
        /// </summary>
        public string AddressLine1 { get;  set; }

        /// <summary>
        /// Get or set address line 2
        /// </summary>
        public string AddressLine2 { get;  set; }
    }
}
