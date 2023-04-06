using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;  // GetGuid(1) --> GetGuid("Id").
namespace ADO.net.Entity
{
    public class Product
    {
        public Product()
        {
            Id = Guid.NewGuid();
            Name = null!;
            Price = 0;
            DeleteData = null!;
        }

        public Product(SqlDataReader reader)
        {
            Id = reader.GetGuid("Id");
            Name = reader.GetString("Name");
            Price = reader.GetDouble("Price");
            DeleteData = reader.IsDBNull("DeleteData") ? null : reader.GetDateTime("DeleteDt");
        }

        public Guid Id { get; set; }  // UNIQUEIDENTIFIER NOT NULL.
        public String? Name { get; set; }  // NVARCHAR(50).
        public double Price { get; set; }  // FLOAT  NOT NULL.
        public DateTime? DeleteData { get; set; }  // DATETINE.
    }
}
