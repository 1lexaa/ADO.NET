using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADO.net.DAL
{
    internal class DataContext
    {
        internal DepartmentAPI Departments;
        internal ManagerApi Managers;
        private readonly SqlConnection _connection;

        public DataContext()
        {
            _connection = new SqlConnection(App._connection_string);

            try { _connection.Open(); }
            catch (Exception ex)
            {
                App._logger.Log(ex.Message, "SERVER", this.GetType().Name, MethodInfo.GetCurrentMethod()?.Name ?? "");
                throw new Exception("Data context init error. See loga for details");
            }

            //Departments = new DepartmentAPI(_connection);
            Managers = new ManagerApi(_connection, this);
        }
    }
}
