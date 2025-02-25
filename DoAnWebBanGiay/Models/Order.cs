using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnWebBanGiay.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; } 

        [Required]
        [MaxLength(250)]
        public string ShippingAddress { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } 

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal TotalAmount { get; set; }

        public virtual User User { get; set; }
    }
}
