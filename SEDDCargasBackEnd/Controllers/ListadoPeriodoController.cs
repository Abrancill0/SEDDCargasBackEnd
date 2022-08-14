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
    public class ListadoPeriodoController : ApiController
    {
        public class ParametroEntrada
        {
            public int EmpresaId { get; set; }

        }

        public class ParametrosSalida
        {
            public int PeriodoId { get; set; }
            public int AnoPeriodo { get; set; }

        }

            
        public JObject Post(ParametroEntrada Datos)
        {

            try
            {
                string Mensaje = "";
                int Estatus = 0;

                List<ParametrosSalida> lista = new List<ParametrosSalida>();

                    SqlCommand comando2 = new SqlCommand("Cargas.ListaPeriodo");
                    comando2.CommandType = CommandType.StoredProcedure;

                        comando2.Parameters.Add("@EmpresaId", SqlDbType.Int);

                        //Asignacion de valores a parametros
                        comando2.Parameters["@EmpresaId"].Value = Datos.EmpresaId;

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
                                PeriodoId = Convert.ToInt32(row["PeriodoId"]),
                                AnoPeriodo = Convert.ToInt32(row["AnoPeriodo"]),


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