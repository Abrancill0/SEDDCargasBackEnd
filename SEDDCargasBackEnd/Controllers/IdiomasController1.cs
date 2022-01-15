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
    public class IdiomasController1 : ApiController
    {
        public class ParametorsEntrada
        {
            public string Arreglo { get; set; }

        }

        public JObject Post(ParametorsEntrada Datos)
        {

            try
            {
                //Materiales
                string Arreglover = Datos.Arreglo;

                string[] ArregloFinal = Arreglover.Split('{');

                for (int i = 1; i < ArregloFinal.Length; i++)
                {
                    string ArregloSimple = ArregloFinal[i];

                    string EliminaParte1 = ArregloSimple.Replace("{", "");
                    string EliminaParte2 = EliminaParte1.Replace("},", "");
                    string EliminaParte3 = EliminaParte2.Replace("}", "");

                    string[] Valores = EliminaParte3.Split(',');

                    int IdiomaId = Convert.ToInt32(Valores[0]);
                    string Nombre = Convert.ToString(Valores[1]);
                    DateTime FechaCreacion = Convert.ToDateTime(Valores[2]);
                    DateTime FechaModificacion = Convert.ToDateTime(Valores[3]);

                    SqlCommand comando2 = new SqlCommand("Semaltec_AgregarMateriales");
                    comando2.CommandType = CommandType.StoredProcedure;

                    //Declaracion de parametros 
                    comando2.Parameters.Add("@DCOT_IDCotizacion", SqlDbType.Int);
                    comando2.Parameters.Add("@DCOT_IDMaterial", SqlDbType.Int);
                    comando2.Parameters.Add("@DCOT_Cantidad", SqlDbType.Decimal);
                    comando2.Parameters.Add("@DCOT_PrecioUnitario", SqlDbType.Float);
                    comando2.Parameters.Add("@DCOT_ImporteTotal", SqlDbType.Float);

                    //Asignacion de valores a parametros
                    comando2.Parameters["@DCOT_IDCotizacion"].Value = COT_IDCotizacion;// Datos.IDHoles;
                    comando2.Parameters["@DCOT_IDMaterial"].Value = DCOT_IDMaterial;// Datos.IDHoles;
                    comando2.Parameters["@DCOT_Cantidad"].Value = DCOT_Cantidad;// Datos.Ho_Par;
                    comando2.Parameters["@DCOT_PrecioUnitario"].Value = DCOT_PrecioUnitario;// Datos.Ho_Advantage;
                    comando2.Parameters["@DCOT_ImporteTotal"].Value = DCOT_ImporteTotal;// Datos.Ho_Yards;

                    comando2.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                    comando2.CommandTimeout = 0;
                    comando2.Connection.Open();

                    DataTable DT2 = new DataTable();
                    SqlDataAdapter DA2 = new SqlDataAdapter(comando2);
                    comando2.Connection.Close();
                    DA2.Fill(DT2);

                }
                //

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