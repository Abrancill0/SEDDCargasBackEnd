using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SEDDCargasBackEnd.Clases
{
    public class VariablesGlobales
    {
        public static string CadenaConexion = ConfigurationManager.ConnectionStrings["SEEDConnectionString"].ConnectionString;
    }
}