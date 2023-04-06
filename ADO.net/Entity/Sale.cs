using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ADO.net.Entity
{
    public class Sale
    {
        public Sale()
        {
            Id = Guid.NewGuid();
            IdProduct = Guid.NewGuid();
            IdManager = Guid.NewGuid();
            Cnt = 1;
            SaleDt = DateTime.Now;
            DeleteDt = null;
        }

        public Sale(SqlDataReader reader)
        {
            Id = reader.GetGuid("Id");
            IdProduct = reader.GetGuid("ProductId");
            IdManager = reader.GetGuid("ManagerId");
            Cnt = reader.GetInt32("Cnt");
            SaleDt = reader.GetDateTime("SaleDt");
            DeleteDt = reader.IsDBNull("DeleteDt") ? null : reader.GetDateTime("DeleteDt");
        }

        public Guid Id { get; set; }
        public Guid IdProduct { get; set; }
        public Guid IdManager { get; set; }
        public Int32 Cnt { get; set; }
        public DateTime SaleDt { get; set; }
        public DateTime? DeleteDt { get; set; }
    }
}
