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
    public class CompetenciaController : ApiController
    {
        public class ParametorsEntrada
        {
            public string Arreglo { get; set; }

        }

        public class ParametrosSalida
        {
            public int Estatus1 { get; set; }
            public string Error { get; set; }

        }

        public JObject Post(ParametorsEntrada Datos)
        {

            try
            {
                string Mensaje = "";
                int Estatus = 0;

                string Arreglover = Datos.Arreglo;

                string ArregloTratado0 = Arreglover.Replace("'", "");
                string ArregloTratado1 = ArregloTratado0.Replace("[", "");
                string ArregloTratado2 = ArregloTratado1.Replace("]", "");

                string[] ArregloFinal = ArregloTratado2.Split('{');

                List<ParametrosSalida> lista = new List<ParametrosSalida>();


                for (int i = 1; i < ArregloFinal.Length; i++)
                {
                    string ArregloSimple = ArregloFinal[i];

                    string EliminaParte1 = ArregloSimple.Replace("{", "");
                    string EliminaParte2 = EliminaParte1.Replace("},", "");
                    string EliminaParte3 = EliminaParte2.Replace("}", "");

                    string[] Valores = EliminaParte3.Split(',');

                    string DeMando = Convert.ToString(Valores[0]);
                    string DescripcionTipoCompetencia = Convert.ToString(Valores[1]);
                    string NombreCompetencia = Convert.ToString(Valores[2]);
                    string DescripcionCompetencia = Convert.ToString(Valores[3]);
                    string Idioma = Convert.ToString(Valores[4]);
                    string Empresa = Convert.ToString(Valores[5]);
                    string ClaveCompetenciaAluprint = Convert.ToString(Valores[6]);

      

                    SqlCommand comando2 = new SqlCommand("Cargas.AltaCompetencias");
                    comando2.CommandType = CommandType.StoredProcedure;

   
                    //Declaracion de parametros 
                    comando2.Parameters.Add("@DeMando", SqlDbType.VarChar);
                    comando2.Parameters.Add("@DescripcionTipoCompetencia", SqlDbType.VarChar);
                    comando2.Parameters.Add("@NombreCompetencia", SqlDbType.VarChar);
                    comando2.Parameters.Add("@DescripcionCompetencia", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Idioma", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Empresa", SqlDbType.VarChar);
                    comando2.Parameters.Add("@ClaveCompetenciaAluprint", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Fila", SqlDbType.Int);

                    //Asignacion de valores a parametros
                    comando2.Parameters["@DeMando"].Value = DeMando;
                    comando2.Parameters["@DescripcionTipoCompetencia"].Value = DescripcionTipoCompetencia;
                    comando2.Parameters["@NombreCompetencia"].Value = NombreCompetencia;
                    comando2.Parameters["@DescripcionCompetencia"].Value = DescripcionCompetencia;
                    comando2.Parameters["@Idioma"].Value = Idioma;
                    comando2.Parameters["@Empresa"].Value = Empresa;
                    comando2.Parameters["@ClaveCompetenciaAluprint"].Value = ClaveCompetenciaAluprint;
                    comando2.Parameters["@Fila"].Value = i;

                    comando2.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                    comando2.CommandTimeout = 0;
                    comando2.Connection.Open();

                    DataTable DT2 = new DataTable();
                    SqlDataAdapter DA2 = new SqlDataAdapter(comando2);
                    comando2.Connection.Close();
                    DA2.Fill(DT2);

                    if (DT2.Rows.Count > 0)
                    {

                        foreach (DataRow row in DT2.Rows)
                        {
                            Mensaje = Convert.ToString(row["mensaje"]);
                            Estatus = Convert.ToInt32(row["Estatus"]);
                        }

                        if (Estatus == 0)
                        {
                            ParametrosSalida ent = new ParametrosSalida
                            {
                                Estatus1 = Estatus,
                                Error = Mensaje

                            };

                            lista.Add(ent);

                        }

                    }
                    else
                    {
                        ParametrosSalida ent = new ParametrosSalida
                        {
                            Estatus1 = 0,
                            Error = "No se encontraron Registros"

                        };

                        lista.Add(ent);

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