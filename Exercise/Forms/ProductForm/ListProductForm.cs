using DevExpress.Entity.Model.Metadata;
using DevExpress.XtraGrid.Views.Grid;
using Exercise.Database;
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
        private List<ProductView> _products;
        public formProducts()
        {
            InitializeComponent();
            _productService = new ProductService();
            _products = new List<ProductView>();
            
        }

        private void formProducts_Load(object sender, EventArgs e)
        {
            try
            {
                _products = _productService.GetAll();
                gridProducts.DataSource = _products;
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

                var product = _products.Where(x => x.ProductId == productId).FirstOrDefault();
                Image tempImage = null;
                var path = PhotoUtiities.GetPathToPhoto("PictureEdit-TakePictureDialog.png");
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    tempImage = Image.FromStream(fs);
                }
                txtType.Text = product.Type.ToString();
                txtQuantity.Text = product.Quantity.ToString();
                picPhoto.Image = tempImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
