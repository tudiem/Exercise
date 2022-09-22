using Dapper;
using Exercise.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
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

        public List<ProductView> GetAll()
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
    }
}
