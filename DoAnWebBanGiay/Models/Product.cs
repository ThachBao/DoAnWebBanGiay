using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAnWebBanGiay.Models
{
    public class Product
    {
        [Key]
        public int ProId { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [MaxLength(100)]
        public string ProName { get; set; }

        [Required(ErrorMessage = "Mô tả là bắt buộc.")]
        [MaxLength(500)]
        public string MoTa { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc.")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal Gia { get; set; }

        [MaxLength(250)]
        public string HinhAnh { get; set; }

        public bool BanChay { get; set; }
        public bool Moi { get; set; }
        public bool KhuyenMai { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn loại sản phẩm.")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }

}