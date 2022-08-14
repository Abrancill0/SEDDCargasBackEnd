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
    public class MetasController : ApiController
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
            string ClaveObjetivo = "";
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

                     ClaveObjetivo = Convert.ToString(Valores[0]);
                    double Aceptable = Convert.ToDouble(Valores[1]);
                    double Sobresaliente = Convert.ToDouble(Valores[2]);
                    double Excelente = Convert.ToDouble(Valores[3]);
                    int Empresaid = Convert.ToInt32(Valores[4]);
                    int Periodoid = Convert.ToInt32(Valores[5]);
                    //double Mes = Convert.ToDouble(Valores[4]);
                    //double AceptableM = Convert.ToDouble(Valores[5]);
                    //double SobresalienteM = Convert.ToDouble(Valores[6]);
                    //double ExcelenteM = Convert.ToDouble(Valores[7]);

                    SqlCommand comando2 = new SqlCommand("Cargas.AltaMeta");
                    comando2.CommandType = CommandType.StoredProcedure;

                    //Declaracion de parametros 
                    comando2.Parameters.Add("@ClaveObjetivo", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Aceptable", SqlDbType.Float);
                    comando2.Parameters.Add("@Sobresaliente", SqlDbType.Float);
                    comando2.Parameters.Add("@Excelente", SqlDbType.Float);
                    comando2.Parameters.Add("@EmpresaID", SqlDbType.Int);
                    comando2.Parameters.Add("@Periodoid", SqlDbType.Int);
                    //comando2.Parameters.Add("@Mes", SqlDbType.Float);
                    //comando2.Parameters.Add("@AceptableM", SqlDbType.Float);
                    //comando2.Parameters.Add("@SobresalienteM", SqlDbType.Float);
                    //comando2.Parameters.Add("@ExcelenteM", SqlDbType.Float);
                    comando2.Parameters.Add("@Fila", SqlDbType.VarChar);

                    //Asignacion de valores a parametros
                    comando2.Parameters["@ClaveObjetivo"].Value = ClaveObjetivo;
                    comando2.Parameters["@Aceptable"].Value = Aceptable;
                    comando2.Parameters["@Sobresaliente"].Value = Sobresaliente;
                    comando2.Parameters["@Excelente"].Value = Excelente;
                    comando2.Parameters["@EmpresaID"].Value = Empresaid;
                    comando2.Parameters["@Periodoid"].Value = Periodoid;
                    //comando2.Parameters["@Mes"].Value = Mes;
                    //comando2.Parameters["@AceptableM"].Value = AceptableM;
                    //comando2.Parameters["@SobresalienteM"].Value = SobresalienteM;
                    //comando2.Parameters["@ExcelenteM"].Value = ExcelenteM;
                    comando2.Parameters["@Fila"].Value = i;

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
                            Error = "No se encontraron Registros",
                          

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
                    ClaveObjetivo = ClaveObjetivo

                });

                return Resultado;
            }

        }
    }
}