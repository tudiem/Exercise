using DevExpress.Utils.Filtering;
using Exercise.Contants;
using Exercise.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise.Models
{
    public class ProductView
    {
        public int ProductId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ErrorMessage.ValueIsNotNull)]
        [MaxLength(255, ErrorMessage = "Length of name product must less 255")]
        public string ProductName { get; set; }

        [Range(0, 100, ErrorMessage = ErrorMessage.ValueIsInRange)]
        public int Quantity { get; set; }

        [RegularExpression(@"^\d{1,2}(\.\d{0,2})$", ErrorMessage = "Value contains more than 2 decimal places")]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ErrorMessage.ValueIsNotNull)]
        [MaxLength(255)]
        public int CategoryId { get; set; }

        [DisplayFormat(DataFormatString = "mm/dd/yyyy", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required(AllowEmptyStrings = false)]
        [MaxLength(1000)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public ProductType Type { get; set; }
        public string CategoryName { get; set; }
        public string Photo { get; set; }
    }
}
