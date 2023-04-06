using ADO.net.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.net.Entity
{
    public class Manager
    {
        public Manager()
        {
            Id = Guid.NewGuid();
            Surname = null!;
            Name = null!;
            Secname = null!;
        }

        public Manager(SqlDataReader reader)
        {
            Id = reader.GetGuid(0);
            Name = reader.GetString(1);
            Surname = reader.GetString(2);
            Secname = reader.GetString(3);
            IdMainDep = reader.GetGuid(4);
            IdSecDep = reader.GetValue(5) == DBNull.Value ? null : reader.GetGuid(5);
            IdChief = reader.IsDBNull(6) ? null : reader.GetGuid(6);
            FiredDt = reader.IsDBNull(7) ? null : reader.GetDateTime(7);
        }

        public Guid Id { get; set; }  // UNIQUEIDENTIFIER NOT NULL.
        public String? Surname { get; set; }  // NVARCHAR(50).
        public String? Name { get; set; }  // NVARCHAR(50).
        public String? Secname { get; set; }  // NVARCHAR(50).
        public Guid IdMainDep { get; set; }  // UNIQUEIDENTIFIER NOT NULL.
        public Guid? IdSecDep { get; set; }  // UNIQUEIDENTIFIER REFERENCES.
        public Guid? IdChief { get; set; }  // UNIQUEIDENTIFIER.
        public DateTime? FiredDt { get; set; }  // UNIQUEIDENTIFIER.

        //////////////////////NAVIGATION PROPERTIES////////////////////
        internal DataContext DataContext { get; set; }
        public Department? MainDep { get { return DataContext?.Departments.GetAll().Find(dep => dep.Id == this.IdMainDep); } }
        public Department? SecDep { get { return DataContext?.Departments.GetAll().Find(dep => dep.Id == this.IdSecDep); } }
    }
}