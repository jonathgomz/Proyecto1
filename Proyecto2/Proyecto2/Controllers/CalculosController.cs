using System;
using System.Linq;
using System.Web.Http;
using Proyecto2.Models;

namespace Proyecto2.Controllers
{
    public class CalculosController : ApiController
    {
        private Calculadora db = new Calculadora();

        [HttpGet]
        public IHttpActionResult GetCalculos()
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
        [Route("api/Calculos/Existentes")]
        public IHttpActionResult GetExistentes()
        {
            try
            {
                var existentes = db.Calculos.OrderByDescending(c => c.FechaHora).Take(10).ToList();
                return Ok(existentes);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}