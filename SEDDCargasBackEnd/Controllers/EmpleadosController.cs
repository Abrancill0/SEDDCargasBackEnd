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



                    string Nombre = Convert.ToString(Valores[0]);
                    string ApellidoPaterno = Convert.ToString(Valores[1]);
                    string ApellidoMaterno = Convert.ToString(Valores[2]);
                    string RFC = Convert.ToString(Valores[3]);
                    string Sexo = Convert.ToString(Valores[4]);
                    Int64 IMSS = Convert.ToInt64(Valores[5]);
                    Int64 CentroCosto = Convert.ToInt64(Valores[6]);
                    Int64 Nomina = Convert.ToInt64(Valores[7]);
                    string Puesto = Convert.ToString(Valores[8]);
                    string VariantePuesto = Convert.ToString(Valores[9]);
                    Int64 NominaJefe = Convert.ToInt64(Valores[10]);
                    string JerarquiaEmpleado = Convert.ToString(Valores[11]);
                    Int64 TipoEmpleadoCFLEX = Convert.ToInt64(Valores[12]);
                    Int64 TipoUsuario = Convert.ToInt64(Valores[13]);
                    DateTime FechaAntiguedad = Convert.ToDateTime(Valores[14]);
                    int ActivoEvaluacion = Convert.ToInt32(Valores[15]);
                    string Idioma = Convert.ToString(Valores[16]);
                    string CorreoElectronico = Convert.ToString(Valores[17]);
                    DateTime FechaPuesto = Convert.ToDateTime(Valores[18]);
                    string TipoArea = Convert.ToString(Valores[19]);
                    string NominaJefeInvitado = Convert.ToString(Valores[20]);

                    SqlCommand comando2 = new SqlCommand("Cargas.AltaEmpleado");
                    comando2.CommandType = CommandType.StoredProcedure;

                    //Declaracion de parametros 
                    comando2.Parameters.Add("@Nombre", SqlDbType.VarChar);
                    comando2.Parameters.Add("@ApellidoPaterno", SqlDbType.VarChar);
                    comando2.Parameters.Add("@ApellidoMaterno", SqlDbType.VarChar);
                    comando2.Parameters.Add("@RFC", SqlDbType.VarChar);
                    comando2.Parameters.Add("@Sexo", SqlDbType.VarChar);
                    comando2.Parameters.Add("@IMSS", SqlDbType.BigInt);
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
                    comando2.Parameters.Add("@CorreoElectronico", SqlDbType.VarChar);
                    comando2.Parameters.Add("@FechaPuesto", SqlDbType.DateTime);
                   
                    comando2.Parameters.Add("@TipoArea", SqlDbType.VarChar);
                    comando2.Parameters.Add("@NominaJefeInvitado", SqlDbType.VarChar);

                    //Asignacion de valores a parametros
                    comando2.Parameters["@Nombre"].Value = Nombre;
                    comando2.Parameters["@ApellidoPaterno"].Value = ApellidoPaterno;
                    comando2.Parameters["@ApellidoMaterno"].Value = ApellidoMaterno;
                    comando2.Parameters["@RFC"].Value = RFC;
                    comando2.Parameters["@Sexo"].Value = Sexo;
                    comando2.Parameters["@IMSS"].Value = IMSS;
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
                    comando2.Parameters["@CorreoElectronico"].Value = CorreoElectronico;
                    comando2.Parameters["@FechaPuesto"].Value = FechaPuesto;
                  
                    comando2.Parameters["@TipoArea"].Value = TipoArea;
                    comando2.Parameters["@NominaJefeInvitado"].Value = NominaJefeInvitado;

                    comando2.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                    comando2.CommandTimeout = 0;
                    comando2.Connection.Open();

                    DataTable DT2 = new DataTable();
                    SqlDataAdapter DA2 = new SqlDataAdapter(comando2);
                    comando2.Connection.Close();
                    DA2.Fill(DT2);

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