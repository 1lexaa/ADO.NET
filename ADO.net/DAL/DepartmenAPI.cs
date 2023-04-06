using ADO.net.Entity;
using ADO.net.Service;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace ADO.net.DAL
{
    internal class DepartmentAPI
    {
        private readonly SqlConnection _connection;
        private readonly ILogger _logger;
        private readonly DataContext _context;

        private List<Department> _departments;

        public DepartmentAPI(SqlConnection connection, DataContext context)
        {
            _connection = connection;
            this._logger = App._logger;
            _departments = null!;  // Lazy.
            _context = context;
        }


        public List<Entity.Department> GetAll(bool force_reload = false)
        {
            if (_departments is not null && !force_reload)
                return _departments;

            _departments = new List<Department>();

            try
            {
                using SqlCommand command = new("SELECT D.* FROM Departments D", _connection);
                using var reader = command.ExecuteReader();


                while (reader.Read())
                    _departments.Add(new Department(reader));
            }
            catch (Exception ex)
            {
                this._logger.Log(ex.Message, "SERVER", this.GetType().Name, MethodInfo.GetCurrentMethod()?.Name ?? "");
            }
            return _departments;
        }
    }
}
