using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.net.EFCore
{
    public class Product
    {
        public Guid Id { get; set; }  // UNIQUEIDENTIFIER NOT NULL.
        public String? Name { get; set; }  // NVARCHAR(50).
        public double Price { get; set; }  // FLOAT  NOT NULL.
        public DateTime? DeleteData { get; set; }  // DATETINE.
    }
}