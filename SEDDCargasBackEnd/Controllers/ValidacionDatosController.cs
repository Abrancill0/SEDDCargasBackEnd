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
    public class ValidacionDatosController : ApiController
    {
        public class ParametrosSalida
        {
            public string Direccion { get; set; }
            public string Gerencia { get; set; }
            public string Departamento { get; set; }
            public string NumeroEmpleado { get; set; }

            public string NombreEmpleado { get; set; }

            public int Acciones { get; set; }

            public int Peso { get; set; }

            public string Puesto { get; set; }

            public string Nombre { get; set; }

            public int Competencia { get; set; }

            public int PesoTotal { get; set; }

            //public string PuestoVariante { get; set; }

            //public string NombreVariante { get; set; }
            public string NivelPuesto { get; set; }

            public int Objetivos { get; set; }

            public int PesoTotalobj { get; set; }
            public string Condicion { get; set; }

        }



        public JObject Post()
        {

            try
            {
                string Mensaje = "";
                int Estatus = 0;

                SqlCommand comando2 = new SqlCommand("Cargas.ValidaInformacionUsuarios");
                comando2.CommandType = CommandType.StoredProcedure;

                comando2.Connection = new SqlConnection(VariablesGlobales.CadenaConexion);
                comando2.CommandTimeout = 0;
                comando2.Connection.Open();

                DataTable DT2 = new DataTable();
                SqlDataAdapter DA2 = new SqlDataAdapter(comando2);
                comando2.Connection.Close();
                DA2.Fill(DT2);

                int contador = DT2.Rows.Count;

                List<ParametrosSalida> lista = new List<ParametrosSalida>();

                if (DT2.Rows.Count > 0)
                {

                    foreach (DataRow row in DT2.Rows)
                    {
                        Mensaje = Convert.ToString(row["mensaje"]);
                        Estatus = Convert.ToInt32(row["Estatus"]);

                        ParametrosSalida ent = new ParametrosSalida
                        {
                            Direccion = Convert.ToString(row["Direccion"]),
                            Gerencia = Convert.ToString(row["Gerencia"]),
                            Departamento = Convert.ToString(row["Departamento"]),
                            NumeroEmpleado = Convert.ToString(row["NumeroEmpleado"]),
                            NombreEmpleado = Convert.ToString(row["NombreEmpleado"]),
                            NivelPuesto = Convert.ToString(row["NivelPuesto"]),
                            Acciones = Convert.ToInt32(row["Acciones"]),
                            Peso = Convert.ToInt32(row["Peso"]),
                            Puesto = Convert.ToString(row["Puesto"]),
                            Nombre = Convert.ToString(row["Nombre"]),
                            Competencia = Convert.ToInt32(row["Competencia"]),
                            PesoTotal = Convert.ToInt32(row["PesoTotal"]),
                            //PuestoVariante = Convert.ToString(row["PuestoVariante"]),
                            //NombreVariante = Convert.ToString(row["NombreVariante"]),
                            Objetivos = Convert.ToInt32(row["Objetivos"]),
                            PesoTotalobj = Convert.ToInt32(row["PesoTotalobj"]),
                            Condicion = Convert.ToString(row["Condicion"])
                        };
                        lista.Add(ent);

                    }

                }


                JObject Resultado = JObject.FromObject(new
                {
                    mensaje = Mensaje,
                    estatus = Estatus,
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