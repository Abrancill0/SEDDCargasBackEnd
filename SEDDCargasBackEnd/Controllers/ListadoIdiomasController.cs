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
    public class ListadoIdiomasController : ApiController
    {
      

        public class ParametrosSalida
        {
            public int IdiomaId { get; set; }
            public string Nombre { get; set; }

        }

            
        public JObject Post()
        {

            try
            {
                string Mensaje = "";
                int Estatus = 0;

                List<ParametrosSalida> lista = new List<ParametrosSalida>();

                    SqlCommand comando2 = new SqlCommand("Cargas.ListaIdioma");
                    comando2.CommandType = CommandType.StoredProcedure;

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

                        if (Estatus == 1)
                        {
                            ParametrosSalida ent = new ParametrosSalida
                            {
                                IdiomaId = Convert.ToInt32(row["IdiomaId"]),
                                Nombre = Convert.ToString(row["Nombre"]),


                            };

                            lista.Add(ent);

                        }

                    }

                }

                JObject Resultado = JObject.FromObject(new
                {
                    mensaje = "OK",
                    estatus = 1,
                    Resultado = lista
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