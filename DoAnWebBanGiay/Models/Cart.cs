using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace DoAnWebBanGiay.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int ProId { get; set; }
        public int ProductSizeId { get; set; }
        public int SoLuong { get; set; }
        public int UserId { get; set; }


        public virtual Product Product { get; set; }
        public virtual ProductSize ProductSizes { get; set; }
        public virtual User Users { get; set; }


    }
}