using ADO.net.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows;


namespace ADO.net
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly String _connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\My resources\Work\WPF\ADO\ADO.NET\ADO.NET\Database.mdf;Integrated Security=True";
        internal static readonly ILogger _logger = new FileLogger();
    }
}
