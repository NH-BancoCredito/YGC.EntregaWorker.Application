using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntregaWorker.CrossCutting.Configs
{
    public class AppConfiguration
    {
        private readonly IConfiguration _configInfo;
        public AppConfiguration(IConfiguration configInfo)
        {
            _configInfo = configInfo;
        }

        public string ConexionDBEntregas
        {
            get
            {
                return _configInfo["dbEntregas-cnx"];
            }
            private set { }
        }

        
        public string KafkaDbCollection
        {
            get
            {
                return _configInfo["BootstrapServers"];
            }
            private set { }
        }


    }
}
