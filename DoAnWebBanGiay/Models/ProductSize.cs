using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnWebBanGiay.Models
{
    public class ProductSize
    {
        [Key]
        public int ProductSizeId { get; set; }
        [Required]
        public int ProId { get; set; }
        [Required]
        public int Size { get; set; }
        [Required]
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }


    }
}