using Dapper;
using DevExpress.CodeParser;
using DevExpress.XtraLayout.Customization;
using Exercise.Database.Entities;
using Exercise.Models;
using Exercise.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Exercise.Database.Implements
{
    public class ProductService : IProductService
    {
        private IDbConnection _connect;

        public ProductService()
        {
            _connect = new SqlConnection(DapperConnection.ConnectionString);
        }

        public List<ProductView> GetAllProductToShowView()
        {
            try
            {
                _connect.Open();

                var sqlQuery = "Select p.Id[ProductId], p.Name[ProductName], p.Quantity, p.Price, p.CreatedDate, p.Description, p.IsActive, p.Type, p.Photo, p.CategoryId, c.Name[CategoryName] from Product p, Category c where p.CategoryId = c.Id";
                List<ProductView> records = _connect.Query<ProductView>(sqlQuery).ToList();

                _connect.Close();

                return records;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Product> GetAll()
        {
            try
            {
                _connect.Open();

                var sqlQuery = "SELECT * FROM Product";
                List<Product> records = _connect.Query<Product>(sqlQuery).ToList();

                _connect.Close();

                return records;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Product GetProductByName(string name)
        {
            try
            {
                _connect.Open();

                var sqlQuery = "Select Top 1 * from Product Where [Name] = @Name";
                List<Product> records = _connect.Query<Product>(sqlQuery, new { Name = name }).ToList();

                _connect.Close();

                return records.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CreateProductAndCopyPhoto(Product product, Image photo)
        {
            try
            {
                _connect.Open();

                var sqlInsert = "INSERT INTO Product([Name], Quantity, Price, CategoryId, CreatedDate, Description, IsActive, [Type], Photo) " +
                    "VALUES (@Name, @Quantity, @Price, @CategoryId, @CreatedDate, @Description, @IsActive, @Type, @Photo)";
                var isSuccess = _connect.Execute(sqlInsert, product) == 1 ? true : false;
                
                // Copy photo
                if (isSuccess)
                {
                    var sqlQuery = "Select Top 1 * from Product Order By Id Desc";
                    List<Product> records = _connect.Query<Product>(sqlQuery).ToList();
                    var productInsert = records.FirstOrDefault();
                    var newName = string.Join(".", productInsert.Id.ToString(), ImageFormat.Png.ToString());
                    var targetPath = PhotoUtiities.GetPathToPhoto(newName);
                    photo.Save(targetPath, ImageFormat.Png);

                    var sqlUpdate = "UPDATE Product SET Photo = @Photo WHERE Id = @Id";
                    _connect.Execute(sqlUpdate, new { Id = productInsert.Id, Photo = newName });
                }

                _connect.Close();
                return isSuccess;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateProducts(List<Product> products)
        {
            try
            {
                _connect.Open();
                foreach(Product product in products)
                {
                    var sqlUpdate = "UPDATE Product SET Price = @Price, CategoryId = @CategoryId, CreatedDate = @CreatedDate, IsActive = @IsActive WHERE Id = @Id";
                    _connect.Execute(sqlUpdate, new { Id = product.Id, Price = product.Price, CategoryId = product.CategoryId, CreatedDate = product.CreatedDate, IsActive = product.IsActive });
                }
                _connect.Close();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }            
        }

        public bool UpdateProduct(Product product, Image photo)
        {
            try
            {
                _connect.Open();
                var sqlUpdate = "UPDATE Product SET Price = @Price, " +
                    "CategoryId = @CategoryId, " +
                    "CreatedDate = @CreatedDate, " +
                    "IsActive = @IsActive, " +
                    "[Name] = @Name, " +
                    "Description = @Description, " +
                    "Quantity = @Quantity, " +
                    "Type = @Type, " +
                    "Photo = @Photo " +
                    "WHERE Id = @Id";
                var isSuccess = _connect.Execute(sqlUpdate, product) == 1 ? true : false;

                // Copy photo
                if (isSuccess)
                {
                    var sqlQuery = "Select Top 1 * from Product WHERE Id = @Id";
                    List<Product> records = _connect.Query<Product>(sqlQuery, new { Id = product.Id }).ToList();
                    var productUpdate = records.FirstOrDefault();
                    var newName = string.Join(".", productUpdate.Id.ToString(), ImageFormat.Png.ToString());
                    var targetPath = PhotoUtiities.GetPathToPhoto(newName);
                    photo.Save(targetPath, ImageFormat.Png);

                    sqlUpdate = "UPDATE Product SET Photo = @Photo WHERE Id = @Id";
                    _connect.Execute(sqlUpdate, new { Id = productUpdate.Id, Photo = newName });
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
