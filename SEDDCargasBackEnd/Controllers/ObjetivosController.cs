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
    public class ObjetivosController : ApiController
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
                    Int64 NominaDueñoObjetivo = Convert.ToInt64(Valores[1]);
                    string CriterioEvaluacion = Convert.ToString(Valores[2]);
                    string Unidad = Convert.ToString(Valores[3]);
                    string MetodoEvaluacion = Convert.ToString(Valores[4]);
                    Int64 DescripcionAluprintID = Convert.ToInt64(Valores[5]);
                    Int64 NominaResponsable = Convert.ToInt64(Valores[6]);
                    string Idioma = Convert.ToString(Valores[7]);
                    string NombreObjetivo = Convert.ToString(Valores[8]);

                    SqlCommand comando2 = new SqlCommand("Cargas.AltaObjetivos");
                    comando2.CommandType = CommandType.StoredProcedure;

                    //Declaracion de parametros 
                    comando2.Parameters.Add("@Empresa", SqlDbType.VarChar);
                    comando2.Parameters.Add("@NominaDueñoObjetivo", SqlDbType.BigInt);
                    comando2.Parameters.Add("@CriterioEvaluacion", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Unidad", SqlDbType.VarChar);
                    comando2.Parameters.Add("@MetodoEvaluacion", SqlDbType.VarChar);
                    comando2.Parameters.Add("@DescripcionAluprintID", SqlDbType.BigInt);
                    comando2.Parameters.Add("@NominaResponsable", SqlDbType.BigInt);
                    comando2.Parameters.Add("@Idioma", SqlDbType.VarChar);
                    comando2.Parameters.Add("@NombreObjetivo", SqlDbType.VarChar);

                    //Asignacion de valores a parametros
                    comando2.Parameters["@Empresa"].Value = Empresa;
                    comando2.Parameters["@NominaDueñoObjetivo"].Value = NominaDueñoObjetivo;
                    comando2.Parameters["@CriterioEvaluacion"].Value = CriterioEvaluacion;
                    comando2.Parameters["@Unidad"].Value = Unidad;
                    comando2.Parameters["@MetodoEvaluacion"].Value = MetodoEvaluacion;
                    comando2.Parameters["@DescripcionAluprintID"].Value = DescripcionAluprintID;
                    comando2.Parameters["@NominaResponsable"].Value = NominaResponsable;
                    comando2.Parameters["@Idioma"].Value = Idioma;
                    comando2.Parameters["@NombreObjetivo"].Value = NombreObjetivo;


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