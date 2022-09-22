using DevExpress.CodeParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise.Utilities
{
    public static class PhotoUtiities
    {
        private static string Folder = "Photos";
        public static string GetPathToPhoto(string photoName)
        {
            var current = Directory.GetCurrentDirectory();
            var path = Path.Combine(Directory.GetParent(current).Parent.Parent.FullName, Folder, photoName);
            return path;
        }
    }
}
