using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnWebBanGiay.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        [Required]
        public int OrderId { get; set; } // Liên kết với bảng Order

        [Required]
        public int ProId { get; set; } // Liên kết với bảng Product

        [Required]
        public int ProductSizeId { get; set; } // Liên kết với bảng ProductSize

        [Required]
        public int Quantity { get; set; } // Số lượng sản phẩm

        [Required]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal Price { get; set; } // Giá sản phẩm tại thời điểm đặt hàng

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }

        public virtual ProductSize ProductSize { get; set; }
    }
}