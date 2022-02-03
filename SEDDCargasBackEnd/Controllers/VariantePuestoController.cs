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
    public class VariantePuestoController : ApiController
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

                string ArregloTratado0 = Arreglover.Replace("'", "");
                string ArregloTratado1 = ArregloTratado0.Replace("[", "");
                string ArregloTratado2 = ArregloTratado1.Replace("]", "");

                string[] ArregloFinal = ArregloTratado2.Split('{');

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
                    string Puesto = Convert.ToString(Valores[4]);
                    string Idioma = Convert.ToString(Valores[5]);
                    string Variante = Convert.ToString(Valores[6]);
                    string DescripcionVariante = Convert.ToString(Valores[7]);

                    SqlCommand comando2 = new SqlCommand("Cargas.AltaVariantePuesto");
                    comando2.CommandType = CommandType.StoredProcedure;

                    //Declaracion de parametros 
                    comando2.Parameters.Add("@Empresa", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Direccion", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Gerencia", SqlDbType.VarChar);
                    comando2.Parameters.Add("@CentroCostos", SqlDbType.BigInt);
                    comando2.Parameters.Add("@Puesto", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Idioma", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Variante", SqlDbType.VarChar);
                    comando2.Parameters.Add("@DescripcionVariante", SqlDbType.VarChar);

                    //Asignacion de valores a parametros
                    comando2.Parameters["@Empresa"].Value = Empresa;
                    comando2.Parameters["@Direccion"].Value = Direccion;
                    comando2.Parameters["@Gerencia"].Value = Gerencia;
                    comando2.Parameters["@CentroCostos"].Value = CentroCostos;
                    comando2.Parameters["@Puesto"].Value = Puesto;
                    comando2.Parameters["@Idioma"].Value = Idioma;
                    comando2.Parameters["@Variante"].Value = Variante;
                    comando2.Parameters["@DescripcionVariante"].Value = DescripcionVariante;

                    comando2.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                    comando2.CommandTimeout = 0;
                    comando2.Connection.Open();

                    DataTable DT2 = new DataTable();
                    SqlDataAdapter DA2 = new SqlDataAdapter(comando2);
                    comando2.Connection.Close();
                    DA2.Fill(DT2);

                    Mensaje = "OK";
                    Estatus = 1;

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