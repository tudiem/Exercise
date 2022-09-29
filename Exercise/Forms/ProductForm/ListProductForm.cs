using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.Entity.Model.Metadata;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Exercise.Database;
using Exercise.Database.Entities;
using Exercise.Database.Implements;
using Exercise.Models;
using Exercise.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exercise.Forms.ProductForm
{
    public partial class formProducts : Form
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private List<Category> _categories = new List<Category>();
        private List<ProductView> _products;
        private List<Product> _productsNeedUpdated = new List<Product>();
        private List<Product> _productsDatabase = new List<Product>();
        private Product _selectedProduct = null;
        public formProducts()
        {
            InitializeComponent();
            _productService = new ProductService();
            _products = new List<ProductView>();
            _categoryService = new CategoryService();
        }

        private void formProducts_Load(object sender, EventArgs e)
        {
            try
            {
                // add column category in grid
                _categories = _categoryService.GetAll();
                RepositoryItemLookUpEdit columnCategory = new RepositoryItemLookUpEdit();
                columnCategory.Name = "CategoryName";
                columnCategory.DataSource = _categories;
                columnCategory.ValueMember = "Name";
                columnCategory.DisplayMember = "Name";
                columnCategory.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                columnCategory.DropDownRows = _categories.Count;
                gridProducts.RepositoryItems.Add(columnCategory);
                gridProductsView.Columns["CategoryName"].ColumnEdit = columnCategory;
                gridProductsView.Columns["CategoryName"].FieldName = "CategoryName";

                // Load data to grid view and get list product to db
                LoadProductsToGridView();
                gridProductsView.BestFitColumns();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void gridProductsView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (view == null) return;
                var data = (ProductView)view.GetRow(e.FocusedRowHandle);
                var productId = data.ProductId;
                _selectedProduct = _productsDatabase.FirstOrDefault(p => p.Id == productId);

                var product = _products.Where(x => x.ProductId == productId).FirstOrDefault();

                Image tempImage = null;
                if (!string.IsNullOrEmpty(product.Photo))
                {
                    var path = PhotoUtiities.GetPathToPhoto(product.Photo);
                    if (File.Exists(path))
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            tempImage = Image.FromStream(fs);
                        }
                    }
                }

                txtType.Text = product.Type.ToString();
                txtQuantity.Text = product.Quantity.ToString();
                picPhoto.Image = product.LoadImage();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnCreateProduct_Click(object sender, EventArgs e)
        {
            var formCreated = new formCreatedProduct();
            formCreated.FormClosed += new FormClosedEventHandler(child_FormClosed);
            formCreated.ShowDialog();
        }

        private void child_FormClosed(object sender, FormClosedEventArgs e)
        {
            //when child form is closed, this code is 
            this.Refresh();
            LoadProductsToGridView();
        }

        private void LoadProductsToGridView()
        {
            _productsDatabase = _productService.GetAll();
            _products = _productService.GetAllProductToShowView();
            _productsNeedUpdated = new List<Product>();
            gridProducts.DataSource = _products;
        }

        private void gridProductsView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            var row = e.RowHandle;
            var columnChanged = e.Column.FieldName;

            var dataAfterChanged = (ProductView)view.GetRow(row);
            var dataBeforeChanged = _productsDatabase.FirstOrDefault(p => p.Id == dataAfterChanged.ProductId);

            var categoryIdAfterChanged = _categories.FirstOrDefault(c => c.Name == dataAfterChanged.CategoryName)?.Id;

            var productToUpdate = _productsNeedUpdated.FirstOrDefault(p => p.Id == dataBeforeChanged.Id) ?? dataBeforeChanged.Clone();
            if (columnChanged == nameof(ProductView.Price) && dataAfterChanged.Price != dataBeforeChanged.Price)
            { // Check data after is different from data before => enable btn save and add to list to update
                productToUpdate.Price = dataAfterChanged.Price;
                if (!_productsNeedUpdated.Exists(p => p.Id == productToUpdate.Id))
                {
                    _productsNeedUpdated.Add(productToUpdate);
                }
            }
            else if (columnChanged == nameof(ProductView.CategoryName) && categoryIdAfterChanged != dataBeforeChanged.CategoryId)
            { // Check data after is different from data before => enable btn save and add to list to update
                productToUpdate.CategoryId = categoryIdAfterChanged.Value;
                if (!_productsNeedUpdated.Exists(p => p.Id == productToUpdate.Id))
                {
                    _productsNeedUpdated.Add(productToUpdate);
                }
            }
            else if (columnChanged == nameof(ProductView.CreatedDate) && dataAfterChanged.CreatedDate != dataBeforeChanged.CreatedDate)
            { // Check data after is different from data before => enable btn save and add to list to update
                productToUpdate.CreatedDate = dataAfterChanged.CreatedDate;
                if (!_productsNeedUpdated.Exists(p => p.Id == productToUpdate.Id))
                {
                    _productsNeedUpdated.Add(productToUpdate);
                }
            }
            else if (columnChanged == nameof(ProductView.IsActive) && dataAfterChanged.IsActive != dataBeforeChanged.IsActive)
            { // Check data after is different from data before => enable btn save and add to list to update
                productToUpdate.IsActive = dataAfterChanged.IsActive;
                if (!_productsNeedUpdated.Exists(p => p.Id == productToUpdate.Id))
                {
                    _productsNeedUpdated.Add(productToUpdate);
                }
            }
            else // Data is not changed
            {
                var productInList = _productsNeedUpdated.FirstOrDefault(p => p.Id == dataAfterChanged.ProductId);
                if (productInList != null) // Remove it in list if it is exists
                {
                    _productsNeedUpdated.Remove(productInList);
                }                
            }
            if (_productsNeedUpdated.Count() > 0)
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var result = _productService.UpdateProducts(_productsNeedUpdated);
            if (result)
            {
                LoadProductsToGridView();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var formCreated = new formCreatedProduct();
            formCreated._productEdit = _selectedProduct;
            formCreated.FormClosed += new FormClosedEventHandler(child_FormClosed);
            formCreated.ShowDialog();
        }

        private void gridProductsView_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (view == null) return;
            var data = (ProductView)view.GetRow(info.RowHandle);
            if (data != null)
            {
                _selectedProduct = _productsDatabase.FirstOrDefault(p => p.Id == data.ProductId);
                var formCreated = new formCreatedProduct();
                formCreated._productEdit = _selectedProduct;
                formCreated.FormClosed += new FormClosedEventHandler(child_FormClosed);
                formCreated.ShowDialog();
            }
        }
    }
}
