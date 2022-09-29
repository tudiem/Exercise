using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise.Database.Entities;
using Dapper;
using Exercise.Models;

namespace Exercise.Database.Implements
{
    public class CategoryService : ICategoryService
    {
        private IDbConnection _connect;

        public CategoryService()
        {
            _connect = new SqlConnection(DapperConnection.ConnectionString);
        }

        public List<Category> GetAll()
        {
            try
            {
                _connect.Open();

                var sqlQuery = "SELECT * FROM Category";
                List<Category> records = _connect.Query<Category>(sqlQuery).ToList();

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
