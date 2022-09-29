using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise.ValidateModels
{
    public static class ProductValidate
    {
        public static string Caption_Message_InValid_Some_Fields = "Invalid Some Fields";

        public static string Message_Not_Selected_Photo = "Please upload the photo";

        public static string Message_Not_Selected_Category = "Please select the category";

        public static string Message_InValid_Price = "Please enter the correct value to price";

        public static int Max_Length_Name = 255;
        public static string Message_InValid_Max_Length_Name = string.Format("Name must less {0}", Max_Length_Name);

        public static int Max_Length_Description = 1000;
        public static string Message_InValid_Max_Length_Description = string.Format("Description must less {0}", Max_Length_Description);

        public static int Min_Quantity = 0;
        public static int Max_Quantity = 100;
        public static string Message_InValid_Min_Max_Quantity = string.Format("Quantity must in {0} to {1}", Min_Quantity, Max_Quantity);
    }
}
