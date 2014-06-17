using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ServersInventoryAPI1
{
    public static class ConnectionManager
    {
        public static string DefaultConnectionString { get; private set; }

        static ConnectionManager()
        {
            ConnectionManager.DefaultConnectionString =
                WebConfigurationManager.ConnectionStrings["ServersInventoryDB"].ToString();
        }

        public static SqlConnection Create()
        {
            return new SqlConnection(ConnectionManager.DefaultConnectionString);
        }
    }

    public class LoggingManager
    {
        private readonly ILog logger;

        private static LoggingManager instance;

        private LoggingManager()
        {
            log4net.Config.XmlConfigurator.Configure();
            logger = LogManager.GetLogger("FileTransfer");
        }

        public static LoggingManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoggingManager();
                }
                return instance;
            }
        }

        public ILog GetLogger
        {
            get { return logger; }
        }
    }
}