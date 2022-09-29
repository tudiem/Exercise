using Exercise.Database.Entities;
using Exercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise.Database
{
    public interface IProductService
    {
        List<ProductView> GetAllProductToShowView();
        List<Product> GetAll();
        bool CreateProductAndCopyPhoto(Product product, Image photo);
        Product GetProductByName(string name);
        bool UpdateProducts(List<Product> products);
        bool UpdateProduct(Product product, Image photo);
    }
}
