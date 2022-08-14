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
            int error = 0;
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
                    error = i;
                    string ArregloSimple = ArregloFinal[i];

                    string EliminaParte1 = ArregloSimple.Replace("{", "");
                    string EliminaParte2 = EliminaParte1.Replace("},", "");
                    string EliminaParte3 = EliminaParte2.Replace("}", "");

                    string[] Valores = EliminaParte3.Split(',');

                    Int64 ObjetivoAluprint = Convert.ToInt64(Valores[0]);
                    Int64 Periodo = Convert.ToInt64(Valores[1]);
                    Int64 Empresa = Convert.ToInt64(Valores[2]);
                    double RealFinal = Convert.ToDouble(Valores[3]);

                    SqlCommand comando2 = new SqlCommand("Cargas.AltaAñoAnterior");
                    comando2.CommandType = CommandType.StoredProcedure;

                    //Declaracion de parametros 
                    comando2.Parameters.Add("@ObjetivoAluprint", SqlDbType.BigInt);
                    comando2.Parameters.Add("@Periodoid", SqlDbType.Int);
                    comando2.Parameters.Add("@Empresaid", SqlDbType.Int);
                    comando2.Parameters.Add("@RealFinal", SqlDbType.Float);
                    comando2.Parameters.Add("@Fila", SqlDbType.VarChar);

                    //Asignacion de valores a parametros
                    comando2.Parameters["@ObjetivoAluprint"].Value = ObjetivoAluprint;
                    comando2.Parameters["@Periodoid"].Value = Periodo;
                    comando2.Parameters["@Empresaid"].Value = Empresa;
                    comando2.Parameters["@RealFinal"].Value = RealFinal;
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
                    error1=error

                });

                return Resultado;
            }

        }
    }
}