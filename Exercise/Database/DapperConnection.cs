using Dapper;
using Exercise.Database.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace Exercise.Database
{
    public class DapperConnection
    {
        private string _connectionString = "server=DESKTOP-9EQ67PK; database=ExerciseDB;integrated security=true";
        public IDbConnection _connect;

        public DapperConnection()
        {
            _connect = new SqlConnection(_connectionString);
        }

        public IDbConnection GetDbConnection()
        {
            return _connect;
        }

        //public List<T> GetAll<T>(string sqlQuery)
        //{
        //    try
        //    {
        //        _connect.Open();

        //        List<T> records = _connect.Query<T>(sqlQuery).ToList();

        //        _connect.Close();

        //        return records;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
