using ADO.net.Entity;
using ADO.net.Service;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADO.net.DAL
{
    internal class ManagerApi
    {
        private readonly SqlConnection _connection;
        private readonly ILogger _logger;
        private readonly DataContext _context;

        public ManagerApi(SqlConnection connection, DataContext context)
        {
            _connection = connection;
            this._logger = App._logger;
            _context = context;
        }

        public List<Entity.Manager> GetAll(bool include_delete = false)
        {
            var manager = new List<Entity.Manager>();

            try
            {
                using SqlCommand command = new SqlCommand("SELECT M.* FROM Managers M " + (include_delete ? "" : "WHERE M.FiredDt IS NULL"), _connection);
                using var reader = command.ExecuteReader();


                while (reader.Read())
                    manager.Add(new Manager(reader) { DataContext = _context });
            }
            catch (Exception ex)
            {
                this._logger.Log(ex.Message, "SERVER", this.GetType().Name, MethodInfo.GetCurrentMethod()?.Name ?? "");
            }
            return manager;
        }
    }
}
