using Dapper;
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
    public abstract class ProductService : DbConnection
    {
        private IDbConnection _connect;

        public ProductService()
        {
            _connect = new SqlConnection(DapperConnection.ConnectionString);
        }

        public List<ProductView> GetAll<ProductView>(string sqlQuery)
        {
            try
            {
                _connect.Open();

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
