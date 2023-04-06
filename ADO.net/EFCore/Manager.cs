using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.net.EFCore
{
    public class Manager
    {
        public Guid Id { get; set; }  // UNIQUEIDENTIFIER NOT NULL.
        public String? Surname { get; set; }  // NVARCHAR(50).
        public String? Name { get; set; }  // NVARCHAR(50).
        public String? Secname { get; set; }  // NVARCHAR(50).
        public Guid IdMainDep { get; set; }  // UNIQUEIDENTIFIER NOT NULL.
        public Guid? IdSecDep { get; set; }  // UNIQUEIDENTIFIER REFERENCES.
        public Guid? IdChief { get; set; }  // UNIQUEIDENTIFIER.
        public DateTime? FiredDt { get; set; }  // UNIQUEIDENTIFIER.
    }
}