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
    public class PlanMejoraController : ApiController
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
            int arreglo = 0;
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

                   

                    arreglo = i;

                    if (arreglo == 45)
                    {
                        DateTime fecha1 = DateTime.Today;
                    }

                    DateTime fecha = DateTime.Today;

                    string Empresa = Convert.ToString(Valores[0]);
                    string Idioma = Convert.ToString(Valores[1]);
                   // string Perido = Convert.ToString(Valores[2]);
                    string Nomina = Convert.ToString(Valores[2]);
                    string ClaveCompetenciaAluprint = Convert.ToString(Valores[3]);
                    string ClaveCursoAluprint = Convert.ToString(Valores[4]);
                    string DescripcionAccionMejora = Convert.ToString(Valores[5]);
                   // string RecursosNecesarios = Convert.ToString(Valores[7]);
                    string Peso = Convert.ToString(Valores[6]);
                    //DateTime FechaAcordada = Convert.ToDateTime(Valores[7]);


                    string FechaAcordadaSinFormato = Convert.ToString(Valores[7]);

                    string Dia1 = (FechaAcordadaSinFormato.Substring(0, 2));
                    string Mes1 = (FechaAcordadaSinFormato.Substring(3, 2));
                    string Año1 = (FechaAcordadaSinFormato.Substring(6, 4));

                    string FechaAcordadaCasiFormato = (Año1 + '-' + Mes1 + '-' + Dia1);
                    string FechaAcordada = FechaAcordadaCasiFormato;

                    // string TipoCurso = Convert.ToString(Valores[10]);
                    string TipoAccionesMejora = Convert.ToString(Valores[8]);
                    //string ClaveCursoAluprint = Convert.ToString(Valores[9]);

                    SqlCommand comando2 = new SqlCommand("Cargas.AltaPlanMejora");
                    comando2.CommandType = CommandType.StoredProcedure;

                    //Declaracion de parametros 
                    comando2.Parameters.Add("@Empresa", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Idioma", SqlDbType.VarChar);
              
                    comando2.Parameters.Add("@Nomina", SqlDbType.Float);
                    comando2.Parameters.Add("@ClaveCompetenciaAluprint", SqlDbType.Float);
                    comando2.Parameters.Add("@ClaveCursoAluprint", SqlDbType.Float);
                    comando2.Parameters.Add("@DescripcionAccionMejora", SqlDbType.VarChar);
               
                    comando2.Parameters.Add("@Peso", SqlDbType.VarChar);
                    comando2.Parameters.Add("@FechaAcordada", SqlDbType.DateTime);
            
                    comando2.Parameters.Add("@TipoAccionesMejora", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Fila", SqlDbType.VarChar);

                    //Asignacion de valores a parametros
                    comando2.Parameters["@Empresa"].Value = Empresa;
                    comando2.Parameters["@Idioma"].Value = Idioma;
                    comando2.Parameters["@Nomina"].Value = Nomina;
                    comando2.Parameters["@ClaveCompetenciaAluprint"].Value = ClaveCompetenciaAluprint;
                    comando2.Parameters["@ClaveCursoAluprint"].Value = ClaveCursoAluprint;
                    comando2.Parameters["@DescripcionAccionMejora"].Value = DescripcionAccionMejora;
                    comando2.Parameters["@Peso"].Value = Peso;
                    comando2.Parameters["@FechaAcordada"].Value = FechaAcordada;
                    comando2.Parameters["@TipoAccionesMejora"].Value = TipoAccionesMejora;
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
                    Resultado = lista,
                    
                });

                return Resultado;


            }
            catch (Exception ex)
            {

                JObject Resultado = JObject.FromObject(new
                {
                    mensaje = ex.ToString(),
                    estatus = 0,
                    arreglin = arreglo

                });

                return Resultado;
            }

        }
    }
}