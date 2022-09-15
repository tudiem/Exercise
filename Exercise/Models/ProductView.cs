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
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ErrorMessage.ValueIsNotNull)]
        [MaxLength(255, ErrorMessage = "Length of name product must less 255")]
        public string Name { get; set; }

        public int Quantity { get; set; }

        [RegularExpression(@"^\d{1,2}(\.\d{0,2})$", ErrorMessage = "Value contains more than 2 decimal places")]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ErrorMessage.ValueIsNotNull)]
        [MaxLength(255)]
        public int CategoryId { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(1000)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public ProductType Type { get; set; }
        public string CategoryName { get; set; }
    }
}
