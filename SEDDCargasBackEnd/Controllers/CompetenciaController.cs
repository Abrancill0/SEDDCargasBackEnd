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

        public JObject Post(ParametorsEntrada Datos)
        {

            try
            {
                string Mensaje = "";
                int Estatus = 0;

                string Arreglover = Datos.Arreglo;

                string[] ArregloFinal = Arreglover.Split('{');

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

                    SqlCommand comando2 = new SqlCommand("Cargas.AltaCompetencias");
                    comando2.CommandType = CommandType.StoredProcedure;

   
                    //Declaracion de parametros 
                    comando2.Parameters.Add("@DeMando", SqlDbType.VarChar);
                    comando2.Parameters.Add("@DescripcionTipoCompetencia", SqlDbType.VarChar);
                    comando2.Parameters.Add("@NombreCompetencia", SqlDbType.VarChar);
                    comando2.Parameters.Add("@DescripcionCompetencia", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Idioma", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Empresa", SqlDbType.VarChar);

                    //Asignacion de valores a parametros
                    comando2.Parameters["@DeMando"].Value = DeMando;
                    comando2.Parameters["@DescripcionTipoCompetencia"].Value = DescripcionTipoCompetencia;
                    comando2.Parameters["@NombreCompetencia"].Value = NombreCompetencia;
                    comando2.Parameters["@DescripcionCompetencia"].Value = DescripcionCompetencia;
                    comando2.Parameters["@Idioma"].Value = Idioma;
                    comando2.Parameters["@Empresa"].Value = Empresa;

                    comando2.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                    comando2.CommandTimeout = 0;
                    comando2.Connection.Open();

                    DataTable DT2 = new DataTable();
                    SqlDataAdapter DA2 = new SqlDataAdapter(comando2);
                    comando2.Connection.Close();
                    DA2.Fill(DT2);

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