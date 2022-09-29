using Exercise.Enums;
using Exercise.Utilities;
using Exercise.ValidateModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise.Database.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public ProductType Type { get; set; }
        public string Photo { get; set; }

        public List<FieldInValid> GetInValidFields()
        {
            var fieldsInValid = new List<FieldInValid>();
            if (string.IsNullOrEmpty(Name) || Name.Length > ProductValidate.Max_Length_Name)
            {
                fieldsInValid.Add(new FieldInValid()
                {
                    FieldName = nameof(Name),
                    Message = ProductValidate.Message_InValid_Max_Length_Name
                });
            }

            if (string.IsNullOrEmpty(Description) || Name.Length > ProductValidate.Max_Length_Description)
            {
                fieldsInValid.Add(new FieldInValid()
                {
                    FieldName = nameof(Description),
                    Message = ProductValidate.Message_InValid_Max_Length_Description
                });
            }

            if (Quantity < ProductValidate.Min_Quantity && Quantity > ProductValidate.Max_Quantity)
            {
                fieldsInValid.Add(new FieldInValid()
                {
                    FieldName = nameof(Quantity),
                    Message = ProductValidate.Message_InValid_Min_Max_Quantity
                });
            }

            return fieldsInValid;
        }

        public Product Clone()
        {
            return (Product)this.MemberwiseClone();
        }

        public Image LoadImage()
        {
            Image tempImage = null;
            if (!string.IsNullOrEmpty(this.Photo))
            {
                var path = PhotoUtiities.GetPathToPhoto(this.Photo);
                if (File.Exists(path))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        tempImage = Image.FromStream(fs);
                    }
                }
            }
            return tempImage;
        }
    }

}
