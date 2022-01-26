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
    public class EsquemaCompetenciaController : ApiController
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

                    string Empresa = Convert.ToString(Valores[0]);
                    string Direccion = Convert.ToString(Valores[1]);
                    string Gerencia = Convert.ToString(Valores[2]);
                    Int64 CentroCostos = Convert.ToInt64(Valores[3]);
                
                    string JerarquiaMinima = Convert.ToString(Valores[4]);
                    string Puesto = Convert.ToString(Valores[5]);
                    string Competencia = Convert.ToString(Valores[6]);
                    double Peso = Convert.ToDouble(Valores[7]);
                
                    string DescripcionTipoCompetencia = Convert.ToString(Valores[8]);
                    string Idioma = Convert.ToString(Valores[9]);
                    string DeMando = Convert.ToString(Valores[10]);


                    SqlCommand comando2 = new SqlCommand("Cargas.AltaEsquemaCompetencia");
                    comando2.CommandType = CommandType.StoredProcedure;

                    //Declaracion de parametros 
                    comando2.Parameters.Add("@Empresa", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Direccion", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Gerencia", SqlDbType.VarChar);
                    comando2.Parameters.Add("@CentroCostos", SqlDbType.BigInt);

                    comando2.Parameters.Add("@JerarquiaMinima", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Puesto", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Competencia", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Peso", SqlDbType.Float);

                    comando2.Parameters.Add("@DescripcionTipoCompetencia", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Idioma", SqlDbType.VarChar);
                    comando2.Parameters.Add("@DeMando", SqlDbType.VarChar);
                    
                    //Asignacion de valores a parametros
                    comando2.Parameters["@Empresa"].Value = Empresa;
                    comando2.Parameters["@Direccion"].Value = Direccion;
                    comando2.Parameters["@Gerencia"].Value = Gerencia;
                    comando2.Parameters["@CentroCostos"].Value = CentroCostos;

                    comando2.Parameters["@JerarquiaMinima"].Value = JerarquiaMinima;
                    comando2.Parameters["@Puesto"].Value = Puesto;
                    comando2.Parameters["@Competencia"].Value = Competencia;
                    comando2.Parameters["@Peso"].Value = Peso;

                    comando2.Parameters["@DescripcionTipoCompetencia"].Value = DescripcionTipoCompetencia;
                    comando2.Parameters["@Idioma"].Value = Idioma;
                    comando2.Parameters["@DeMando"].Value = DeMando;

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