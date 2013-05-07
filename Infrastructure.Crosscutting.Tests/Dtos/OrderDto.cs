using System;

namespace Infrastructure.Crosscutting.Tests.Dtos
{
    public class OrderDto
    {
        /// <summary>
        /// Get or set the Order Date
        /// </summary>
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// Get or set order delivery date
        /// </summary>
        public DateTime? DeliveryDate { get; set; }
    }
}
