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
        List<ProductView> GetAll();
    }
}
