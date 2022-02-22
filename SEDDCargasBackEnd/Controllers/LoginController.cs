using Newtonsoft.Json.Linq;
using SEDDCargasBackEnd.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SEDDCargasBackEnd.Controllers
{
    public class LoginController : ApiController
    {
        public class ParametorsEntrada
        {
            public string Usuario { get; set; }
            public string Pass { get; set; }

}

     
            
        public JObject Post(ParametorsEntrada Datos)
        {

            try
            {
                string Mensaje = "";
                int Estatus = 0;

                    SqlCommand comando2 = new SqlCommand("Cargas.Login");
                    comando2.CommandType = CommandType.StoredProcedure;

                    //Declaracion de parametros 
                    comando2.Parameters.Add("@Usuario", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Pass", SqlDbType.VarChar);

                    //Asignacion de valores a parametros
                    comando2.Parameters["@Usuario"].Value = Datos.Usuario;// Datos.IDHoles;
                    comando2.Parameters["@Pass"].Value = Datos.Pass;// Datos.IDHoles;

                    comando2.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                    comando2.CommandTimeout = 0;
                    comando2.Connection.Open();

                    DataTable DT2 = new DataTable();
                    SqlDataAdapter DA2 = new SqlDataAdapter(comando2);
                    comando2.Connection.Close();
                    DA2.Fill(DT2);

                    int contador = DT2.Rows.Count;

                    if (DT2.Rows.Count > 0)
                    {

                        foreach (DataRow row in DT2.Rows)
                        {
                            Mensaje = Convert.ToString(row["mensaje"]);
                            Estatus = Convert.ToInt32(row["Estatus"]);
                        }

                      

                    }
                   
               

                JObject Resultado = JObject.FromObject(new
                {
                    mensaje = Mensaje,
                    estatus = Estatus,
                  
                });

                return Resultado;

            }
            catch (Exception ex)
            {

                JObject Resultado = JObject.FromObject(new
                {
                    mensaje = ex.ToString(),
                    estatus = 0,

                });

                return Resultado;
            }

        }
    }
}