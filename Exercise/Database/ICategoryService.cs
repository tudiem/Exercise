using Exercise.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise.Database
{
    public interface ICategoryService
    {
        List<Category> GetAll();
    }
}
