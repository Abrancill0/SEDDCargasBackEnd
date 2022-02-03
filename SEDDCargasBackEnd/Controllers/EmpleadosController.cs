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
    public class EmpleadosController : ApiController
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



                    string Nombre = Convert.ToString(Valores[0]);
                    string ApellidoPaterno = Convert.ToString(Valores[1]);
                    string ApellidoMaterno = Convert.ToString(Valores[2]);
                    string RFC = Convert.ToString(Valores[3]);
                    string Sexo = Convert.ToString(Valores[4]);
                    Int64 IMSS = Convert.ToInt64(Valores[5]);
                    string Empresa = Convert.ToString(Valores[6]);
                    string Direccion = Convert.ToString(Valores[7]);
                    string Gerencia = Convert.ToString(Valores[8]);
                    Int64 CentroCosto = Convert.ToInt64(Valores[9]);
                    Int64 Nomina = Convert.ToInt64(Valores[10]);
                    string Puesto = Convert.ToString(Valores[11]);
                    string VariantePuesto = Convert.ToString(Valores[12]);
                    Int64 NominaJefe = Convert.ToInt64(Valores[13]);
                    string JerarquiaEmpleado = Convert.ToString(Valores[14]);
                    Int64 TipoEmpleadoCFLEX = Convert.ToInt64(Valores[15]);
                    Int64 TipoUsuario = Convert.ToInt64(Valores[16]);
                    DateTime FechaAntiguedad = Convert.ToDateTime(Valores[17]);
                    int ActivoEvaluacion = Convert.ToInt32(Valores[18]);
                    string Idioma = Convert.ToString(Valores[19]);

                    SqlCommand comando2 = new SqlCommand("Cargas.AltaEmpleado");
                    comando2.CommandType = CommandType.StoredProcedure;

                    //Declaracion de parametros 
                    comando2.Parameters.Add("@Nombre", SqlDbType.VarChar);
                    comando2.Parameters.Add("@ApellidoPaterno", SqlDbType.VarChar);
                    comando2.Parameters.Add("@ApellidoMaterno", SqlDbType.VarChar);
                    comando2.Parameters.Add("@RFC", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Sexo", SqlDbType.VarChar);
                    comando2.Parameters.Add("@IMSS", SqlDbType.BigInt);
                    comando2.Parameters.Add("@Empresa", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Direccion", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Gerencia", SqlDbType.VarChar);
                    comando2.Parameters.Add("@CentroCosto", SqlDbType.BigInt);
                    comando2.Parameters.Add("@Nomina", SqlDbType.BigInt);
                    comando2.Parameters.Add("@Puesto", SqlDbType.VarChar);
                    comando2.Parameters.Add("@VariantePuesto", SqlDbType.VarChar);
                    comando2.Parameters.Add("@NominaJefe", SqlDbType.BigInt);
                    comando2.Parameters.Add("@JerarquiaEmpleado", SqlDbType.VarChar);
                    comando2.Parameters.Add("@TipoEmpleadoCFLEX", SqlDbType.BigInt);
                    comando2.Parameters.Add("@TipoUsuario", SqlDbType.BigInt);
                    comando2.Parameters.Add("@FechaAntiguedad", SqlDbType.DateTime);
                    comando2.Parameters.Add("@ActivoEvaluacion", SqlDbType.Int);
                    comando2.Parameters.Add("@Idioma", SqlDbType.VarChar);

                    //Asignacion de valores a parametros
                    comando2.Parameters["@Nombre"].Value = Nombre;
                    comando2.Parameters["@ApellidoPaterno"].Value = ApellidoPaterno;
                    comando2.Parameters["@ApellidoMaterno"].Value = ApellidoMaterno;
                    comando2.Parameters["@RFC"].Value = RFC;
                    comando2.Parameters["@Sexo"].Value = Sexo;
                    comando2.Parameters["@IMSS"].Value = IMSS;
                    comando2.Parameters["@Empresa"].Value = Empresa;
                    comando2.Parameters["@Direccion"].Value = Direccion;
                    comando2.Parameters["@Gerencia"].Value = Gerencia;
                    comando2.Parameters["@CentroCosto"].Value = CentroCosto;
                    comando2.Parameters["@Nomina"].Value = Nomina;
                    comando2.Parameters["@Puesto"].Value = Puesto;
                    comando2.Parameters["@VariantePuesto"].Value = VariantePuesto;
                    comando2.Parameters["@NominaJefe"].Value = NominaJefe;
                    comando2.Parameters["@JerarquiaEmpleado"].Value = JerarquiaEmpleado;
                    comando2.Parameters["@TipoEmpleadoCFLEX"].Value = TipoEmpleadoCFLEX;
                    comando2.Parameters["@TipoUsuario"].Value = TipoUsuario;
                    comando2.Parameters["@FechaAntiguedad"].Value = FechaAntiguedad;
                    comando2.Parameters["@ActivoEvaluacion"].Value = ActivoEvaluacion;
                    comando2.Parameters["@Idioma"].Value = Idioma;

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