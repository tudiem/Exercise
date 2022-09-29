using DevExpress.XtraEditors;
using Exercise.Database;
using Exercise.Database.Entities;
using Exercise.Database.Implements;
using Exercise.Enums;
using Exercise.Utilities;
using Exercise.ValidateModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Exercise.Forms.ProductForm
{
    public partial class formCreatedProduct : Form
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private string _sourcePath = string.Empty;
        private List<Category> _categories = new List<Category>();
        private Image _photo = null;
        private List<FieldInValid> _inValidFields = new List<FieldInValid>();
        public Product _productEdit;

        public formCreatedProduct()
        {
            InitializeComponent();
            _productService = new ProductService();
            _categoryService = new CategoryService();
        }

        private void formCreatedProduct_Load(object sender, EventArgs e)
        {
            _categories = _categoryService.GetAll();
            var dataLookupCategory = _categories.Select(c => c.Name);
            lookUpECategory.Properties.DataSource = dataLookupCategory;
            if (_productEdit != null)
            {
                txtProductName.Text = _productEdit.Name;
                memoDescription.Text = _productEdit.Description;
                lookUpECategory.EditValue = _categories.FirstOrDefault(c => c.Id == (_productEdit.CategoryId)).Name;
                spinEQuantity.Value = _productEdit.Quantity;
                dateECreatedDate.EditValue = _productEdit.CreatedDate;
                checkEIsActive.Checked = _productEdit.IsActive;
                if (_productEdit.Type == ProductType.Single) radioSingle.Checked = true;
                else if (_productEdit.Type == ProductType.Other) radioOther.Checked = true;
                else radioPackage.Checked = false;
                picPhoto.Image = _productEdit.LoadImage();
                txtPrice.Text = _productEdit.Price.ToString();
            }
        }

        private void simpleBtnSave_Click(object sender, EventArgs e)
        {
            _inValidFields = new List<FieldInValid>();
            var type = ProductType.Single;
            if (radioSingle.Checked) { type = ProductType.Single; }
            else if (radioOther.Checked) { type = ProductType.Other; }
            else if (radioPackage.Checked) { type = ProductType.Package; }

            // check field photo/category/price before
            ValidateFieldsBeforeBindingDataToProduct();
            if (_inValidFields.Count != 0)
            {
                var message = string.Empty;
                _inValidFields.ForEach(f => message += string.Join("", f.FieldName, " :", f.Message, "\n"));
                MessageBox.Show(message, ProductValidate.Caption_Message_InValid_Some_Fields, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // get categoryId
            var categoryId = _categories.Where(c => c.Name == lookUpECategory.EditValue?.ToString()).FirstOrDefault()?.Id ?? 0;

            var product = new Product()
            {
                Name = txtProductName.Text.Trim(),
                CategoryId = categoryId,
                Description = memoDescription.Text.Trim(),
                IsActive = checkEIsActive.Checked,
                Photo = string.Empty,
                Price = decimal.Parse(txtPrice.Text),
                Quantity = (int)spinEQuantity.Value,
                Type = type,
                CreatedDate = (dateECreatedDate.EditValue != null ? (DateTime)dateECreatedDate.EditValue : DateTime.UtcNow)
            };
            _inValidFields = product.GetInValidFields();
            if (_inValidFields.Count == 0 && _productEdit == null) // Create product
            {
                _productService.CreateProductAndCopyPhoto(product, _photo);
                this.Close();
            }
            else if (_inValidFields.Count == 0 && _productEdit != null) // Update product
            {
                product.Id = _productEdit.Id;
                _productService.UpdateProduct(product, _photo);
                this.Close();
            }
            else
            {
                var message = string.Empty;
                _inValidFields.ForEach(f => message += string.Join("", f.FieldName, " :", f.Message, "\n"));
                MessageBox.Show(message, ProductValidate.Caption_Message_InValid_Some_Fields, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleBtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEPhoto_Click(object sender, EventArgs e)
        {
            var diaglog = new OpenFileDialog();
            diaglog.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (diaglog.ShowDialog() == DialogResult.OK)
            {
                _photo = new Bitmap(diaglog.FileName);
                _sourcePath = diaglog.FileName;
                picPhoto.Image = _photo;
            }
        }

        private void ValidateFieldsBeforeBindingDataToProduct()
        {
            if (_photo == null)
            {
                _inValidFields.Add(new FieldInValid() { FieldName = nameof(Product.Photo), Message = ProductValidate.Message_Not_Selected_Photo });
            }

            if (string.IsNullOrEmpty(lookUpECategory.EditValue?.ToString()))
            {
                _inValidFields.Add(new FieldInValid() { FieldName = nameof(Category), Message = ProductValidate.Message_Not_Selected_Category });
            }

            decimal price;
            if (string.IsNullOrEmpty(txtPrice.Text) || !decimal.TryParse(txtPrice.Text, out price))
            {
                _inValidFields.Add(new FieldInValid() { FieldName = nameof(Product.Price), Message = ProductValidate.Message_InValid_Price });
            }
        }
    }
}
