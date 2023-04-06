using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.net.EFCore
{
    public class Department
    {
        public Guid Id { get; set; }  // UNIQUEIDENTIFIER NOT NULL.
        public String? Name { get; set; }  // NVARCHAR(50).
        public DateTime? DeleteDt { get; set; }
    }
}
