﻿using Newtonsoft.Json.Linq;
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
    public class EsquemaObjetivosController : ApiController
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

            Int64 ClaveVariante =0;
            Int64 ClaveObjetivo = 0;
            float Peso =0;

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

                     ClaveVariante = Convert.ToInt64(Valores[0]);
                     ClaveObjetivo = Convert.ToInt64(Valores[1]);
                     Peso = Convert.ToSingle(Valores[2]);

                    SqlCommand comando2 = new SqlCommand("Cargas.AltaEsquemaObjetivo");
                    comando2.CommandType = CommandType.StoredProcedure;


                    //Declaracion de parametros 
                    comando2.Parameters.Add("@ClaveVariantePuesto", SqlDbType.BigInt);
                    comando2.Parameters.Add("@ClaveObjetivo", SqlDbType.BigInt);
                    comando2.Parameters.Add("@Peso", SqlDbType.Float);
                    comando2.Parameters.Add("@Fila", SqlDbType.Int);

                    //Asignacion de valores a parametros
                    comando2.Parameters["@ClaveVariantePuesto"].Value = ClaveVariante;// Datos.IDHoles;
                    comando2.Parameters["@ClaveObjetivo"].Value = ClaveObjetivo;// Datos.IDHoles;
                    comando2.Parameters["@Peso"].Value = Peso;// Datos.IDHoles;
                    comando2.Parameters["@Fila"].Value = i;// Datos.IDHoles;

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
                    ClaveVariante = ClaveVariante,
                    ClaveObjetivo = ClaveObjetivo,
                    Peso = Peso
                });

                return Resultado;
            }

        }
    }
}