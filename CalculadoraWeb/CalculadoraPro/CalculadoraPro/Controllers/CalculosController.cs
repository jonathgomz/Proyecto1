using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using CalculadoraPro.Models;

namespace CalculadoraPro.Controllers
{
    [RoutePrefix("api/calculos")]
    public class CalculosController : ApiController
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // GET: api/calculos
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetTodosLosCalculos()
        {
            List<Calculo> calculos = new List<Calculo>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Calculos";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    calculos.Add(new Calculo
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Operacion = reader["Operacion"].ToString(),
                        PrimerNumero = Convert.ToDecimal(reader["PrimerNumero"]),
                        SegundoNumero = Convert.ToDecimal(reader["SegundoNumero"]),
                        ResultadoFinal = Convert.ToDecimal(reader["ResultadoFinal"]),
                        FechaCalculo = Convert.ToDateTime(reader["FechaCalculo"])
                    });
                }
            }

            return Ok(calculos);
        }

        // GET: api/calculos/sumas
        [HttpGet]
        [Route("sumas")]
        public IHttpActionResult GetSumas()
        {
            List<Calculo> calculos = new List<Calculo>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Calculos WHERE Operacion = 'Suma'";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    calculos.Add(new Calculo
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Operacion = reader["Operacion"].ToString(),
                        PrimerNumero = Convert.ToDecimal(reader["PrimerNumero"]),
                        SegundoNumero = Convert.ToDecimal(reader["SegundoNumero"]),
                        ResultadoFinal = Convert.ToDecimal(reader["ResultadoFinal"]),
                        FechaCalculo = Convert.ToDateTime(reader["FechaCalculo"])
                    });
                }
            }

            return Ok(calculos);
        }

        // GET: api/calculos/restas
        [HttpGet]
        [Route("restas")]
        public IHttpActionResult GetRestas()
        {
            List<Calculo> calculos = new List<Calculo>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Calculos WHERE Operacion = 'Resta'";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    calculos.Add(new Calculo
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Operacion = reader["Operacion"].ToString(),
                        PrimerNumero = Convert.ToDecimal(reader["PrimerNumero"]),
                        SegundoNumero = Convert.ToDecimal(reader["SegundoNumero"]),
                        ResultadoFinal = Convert.ToDecimal(reader["ResultadoFinal"]),
                        FechaCalculo = Convert.ToDateTime(reader["FechaCalculo"])
                    });
                }
            }

            return Ok(calculos);
        }

        // GET: api/calculos/multiplicaciones
        [HttpGet]
        [Route("multiplicaciones")]
        public IHttpActionResult GetMultiplicaciones()
        {
            List<Calculo> calculos = new List<Calculo>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Calculos WHERE Operacion = 'Multiplicacion'";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    calculos.Add(new Calculo
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Operacion = reader["Operacion"].ToString(),
                        PrimerNumero = Convert.ToDecimal(reader["PrimerNumero"]),
                        SegundoNumero = Convert.ToDecimal(reader["SegundoNumero"]),
                        ResultadoFinal = Convert.ToDecimal(reader["ResultadoFinal"]),
                        FechaCalculo = Convert.ToDateTime(reader["FechaCalculo"])
                    });
                }
            }

            return Ok(calculos);
        }

        // GET: api/calculos/divisiones
        [HttpGet]
        [Route("divisiones")]
        public IHttpActionResult GetDivisiones()
        {
            List<Calculo> calculos = new List<Calculo>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Calculos WHERE Operacion = 'Division'";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    calculos.Add(new Calculo
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Operacion = reader["Operacion"].ToString(),
                        PrimerNumero = Convert.ToDecimal(reader["PrimerNumero"]),
                        SegundoNumero = Convert.ToDecimal(reader["SegundoNumero"]),
                        ResultadoFinal = Convert.ToDecimal(reader["ResultadoFinal"]),
                        FechaCalculo = Convert.ToDateTime(reader["FechaCalculo"])
                    });
                }
            }

            return Ok(calculos);
        }

        // GET: api/calculos/recientes (endpoint personalizado - últimos 10 cálculos)
        [HttpGet]
        [Route("recientes")]
        public IHttpActionResult GetRecientes()
        {
            List<Calculo> calculos = new List<Calculo>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 10 * FROM Calculos ORDER BY FechaCalculo DESC";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    calculos.Add(new Calculo
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Operacion = reader["Operacion"].ToString(),
                        PrimerNumero = Convert.ToDecimal(reader["PrimerNumero"]),
                        SegundoNumero = Convert.ToDecimal(reader["SegundoNumero"]),
                        ResultadoFinal = Convert.ToDecimal(reader["ResultadoFinal"]),
                        FechaCalculo = Convert.ToDateTime(reader["FechaCalculo"])
                    });
                }
            }

            return Ok(calculos);
        }

        // POST: api/calculos (OPCIONAL - para guardar nuevos cálculos)
        [HttpPost]
        [Route("")]
        public IHttpActionResult PostCalculo([FromBody] Calculo calculo)
        {
            if (calculo == null)
                return BadRequest("Datos inválidos");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Calculos (Operacion, PrimerNumero, SegundoNumero, ResultadoFinal, FechaCalculo) 
                                VALUES (@Operacion, @PrimerNumero, @SegundoNumero, @ResultadoFinal, @FechaCalculo);
                                SELECT CAST(SCOPE_IDENTITY() as int)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Operacion", calculo.Operacion);
                cmd.Parameters.AddWithValue("@PrimerNumero", calculo.PrimerNumero);
                cmd.Parameters.AddWithValue("@SegundoNumero", calculo.SegundoNumero);
                cmd.Parameters.AddWithValue("@ResultadoFinal", calculo.ResultadoFinal);
                cmd.Parameters.AddWithValue("@FechaCalculo", DateTime.Now);

                conn.Open();
                int nuevoId = (int)cmd.ExecuteScalar();
                calculo.Id = nuevoId;
            }

            return Created($"api/calculos/{calculo.Id}", calculo);
        }
    }
}