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
    public class AñoAnteriorController : ApiController
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

                    Int64 ObjetivoAluprint = Convert.ToInt64(Valores[0]);
                    float Periodo = Convert.ToSingle(Valores[1]);
                    float Enero = Convert.ToSingle(Valores[2]);
                    float Febrero = Convert.ToSingle(Valores[3]);
                    float Marzo = Convert.ToSingle(Valores[4]);
                    float Abril = Convert.ToSingle(Valores[5]);
                    float Mayo = Convert.ToSingle(Valores[6]);
                    float Junio = Convert.ToSingle(Valores[7]);
                    float Julio = Convert.ToSingle(Valores[8]);
                    float Agosto = Convert.ToSingle(Valores[9]);
                    float Septiembre = Convert.ToSingle(Valores[10]);
                    float Octubre = Convert.ToSingle(Valores[11]);
                    float Noviembre = Convert.ToSingle(Valores[12]);
                    float Diciembre = Convert.ToSingle(Valores[13]);
                    float FinalPeriodoAnterior = Convert.ToSingle(Valores[14]);


                    SqlCommand comando2 = new SqlCommand("Cargas.AltaAñoAnterior");
                    comando2.CommandType = CommandType.StoredProcedure;

                    //Declaracion de parametros 
                    comando2.Parameters.Add("@ObjetivoAluprint", SqlDbType.BigInt);
                    comando2.Parameters.Add("@Periodo", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Enero", SqlDbType.Float);
                    comando2.Parameters.Add("@Febrero", SqlDbType.Float);
                    comando2.Parameters.Add("@Marzo", SqlDbType.Float);
                    comando2.Parameters.Add("@Abril", SqlDbType.Float);
                    comando2.Parameters.Add("@Mayo", SqlDbType.Float);
                    comando2.Parameters.Add("@Junio", SqlDbType.Float);
                    comando2.Parameters.Add("@Julio", SqlDbType.Float);
                    comando2.Parameters.Add("@Agosto", SqlDbType.Float);
                    comando2.Parameters.Add("@Septiembre", SqlDbType.Float);
                    comando2.Parameters.Add("@Octubre", SqlDbType.Float);
                    comando2.Parameters.Add("@Noviembre", SqlDbType.Float);
                    comando2.Parameters.Add("@Diciembre", SqlDbType.Float);
                    comando2.Parameters.Add("@FinalPeriodoAnterior", SqlDbType.Float);
                    comando2.Parameters.Add("@Empresaid", SqlDbType.BigInt);
                    comando2.Parameters.Add("@Fila", SqlDbType.VarChar);

                    //Asignacion de valores a parametros
                    comando2.Parameters["@ObjetivoAluprint"].Value = ObjetivoAluprint;
                    comando2.Parameters["@Periodo"].Value = Periodo;
                    comando2.Parameters["@Enero"].Value = Enero;
                    comando2.Parameters["@Febrero"].Value = Febrero;
                    comando2.Parameters["@Marzo"].Value = Marzo;
                    comando2.Parameters["@Abril"].Value = Abril;
                    comando2.Parameters["@Mayo"].Value = Mayo;
                    comando2.Parameters["@Junio"].Value = Junio;
                    comando2.Parameters["@Julio"].Value = Julio;
                    comando2.Parameters["@Agosto"].Value = Agosto;
                    comando2.Parameters["@Septiembre"].Value = Septiembre;
                    comando2.Parameters["@Octubre"].Value = Octubre;
                    comando2.Parameters["@Noviembre"].Value = Noviembre;
                    comando2.Parameters["@Diciembre"].Value = Diciembre;
                    comando2.Parameters["@FinalPeriodoAnterior"].Value = FinalPeriodoAnterior;
                    comando2.Parameters["@Empresaid"].Value = 6;
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