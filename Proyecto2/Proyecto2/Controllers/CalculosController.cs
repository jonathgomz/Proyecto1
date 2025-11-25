using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Proyecto2.Models;

namespace Proyecto2.Controllers
{
    public class CalculosController : ApiController
    {
        private Calculadora db = new Calculadora();

        [HttpGet]
        [Route("api/Calculos/Existentes")]
        public IHttpActionResult GetExistentes()
        {
            try
            {
                var calculos = db.Calculos.ToList();
                return Ok(calculos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Calculos/Sumas")]
        public IHttpActionResult GetSumas()
        {
            try
            {
                var sumas = db.Calculos.Where(c => c.Operacion.Contains("+")).ToList();
                return Ok(sumas);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Calculos/Restas")]
        public IHttpActionResult GetRestas()
        {
            try
            {
                var restas = db.Calculos.Where(c => c.Operacion.Contains("-")).ToList();
                return Ok(restas);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Calculos/Multiplicaciones")]
        public IHttpActionResult GetMultiplicaciones()
        {
            try
            {
                var multiplicaciones = db.Calculos.Where(c => c.Operacion.Contains("*")).ToList();
                return Ok(multiplicaciones);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Calculos/Divisiones")]
        public IHttpActionResult GetDivisiones()
        {
            try
            {
                var divisiones = db.Calculos.Where(c => c.Operacion.Contains("/")).ToList();
                return Ok(divisiones);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Calculos/Porcentaje")]
        public IHttpActionResult GetPorcentaje()
        {
            try
            {
                var porcentajes = db.Calculos.Where(c =>
                c.Operacion.Contains("0.07") ||
                c.Operacion.Contains("0.05") ||
                c.Operacion.Contains("0.10") ||
                c.Operacion.Contains("0.15") ||
                c.Operacion.Contains("0.20") ||
                c.Operacion.Contains("0.25") ||
                c.Operacion.Contains("0.30") ||
                c.Operacion.Contains("0.50")
                ).ToList();

                return Ok(porcentajes);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}