using SistemaDeEncuestas.Data;
using SistemaDeEncuestas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace SistemaDeEncuestas.Controllers
{
    public class RespuestaController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Responder(int? id)
        {
            if (id == null) return HttpNotFound();

            var encuesta = db.Encuestas
                .Include(e => e.Preguntas.Select(p => p.Opciones))
                .FirstOrDefault(e => e.Id == id);

            if (encuesta == null) return HttpNotFound();
            if (!encuesta.Preguntas.Any())
            {
                TempData["Error"] = "Esta encuesta no tiene preguntas disponibles.";
                return RedirectToAction("Index", "Encuesta");
            }

            return View(encuesta);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Responder(int encuestaId, string usuario, int tiempoSegundos, FormCollection form)
        {
            if (string.IsNullOrWhiteSpace(usuario))
            {
                TempData["Error"] = "Debe ingresar su nombre.";
                return RedirectToAction("Responder", new { id = encuestaId });
            }

            var encuesta = db.Encuestas
                .Include(e => e.Preguntas.Select(p => p.Opciones))
                .FirstOrDefault(e => e.Id == encuestaId);

            if (encuesta == null) return HttpNotFound();

            var respuestaUsuario = new RespuestaUsuario
            {
                EncuestaId = encuestaId,
                Usuario = usuario.Trim(),
                Fecha = DateTime.Now,
                TiempoSegundos = tiempoSegundos
            };

            db.RespuestasUsuario.Add(respuestaUsuario);
            db.SaveChanges();

            foreach (var pregunta in encuesta.Preguntas)
            {
                var key = "pregunta_" + pregunta.Id;
                var valores = form[key];

                if (!string.IsNullOrEmpty(valores))
                {
                    var opcionesIds = valores.Split(',').Select(int.Parse).ToList();

                    foreach (var opcionId in opcionesIds)
                    {
                        db.RespuestasDetalle.Add(new RespuestaDetalle
                        {
                            RespuestaUsuarioId = respuestaUsuario.Id,
                            PreguntaId = pregunta.Id,
                            OpcionId = opcionId
                        });
                    }
                }
            }

            db.SaveChanges();
            TempData["Success"] = "¡Gracias por responder la encuesta!";
            return RedirectToAction("Completada", new { id = respuestaUsuario.Id });
        }

        public ActionResult Completada(int? id)
        {
            if (id == null) return HttpNotFound();

            var respuesta = db.RespuestasUsuario
                .Include(r => r.Encuesta)
                .FirstOrDefault(r => r.Id == id);

            if (respuesta == null) return HttpNotFound();
            return View(respuesta);
        }

        public ActionResult Resultados(int? id)
        {
            if (id == null) return HttpNotFound();

            var encuesta = db.Encuestas
                .Include(e => e.Preguntas.Select(p => p.Opciones))
                .Include(e => e.Respuestas.Select(r => r.Detalles))
                .FirstOrDefault(e => e.Id == id);

            if (encuesta == null) return HttpNotFound();

            return View(encuesta);
        }

        public ActionResult ListaRespuestas(int? id)
        {
            if (id == null) return HttpNotFound();

            var encuesta = db.Encuestas
                .Include(e => e.Respuestas)
                .FirstOrDefault(e => e.Id == id);

            if (encuesta == null) return HttpNotFound();

            ViewBag.EncuestaTitulo = encuesta.Titulo;
            return View(encuesta.Respuestas.OrderByDescending(r => r.Fecha).ToList());
        }

        public ActionResult DetalleRespuesta(int? id)
        {
            if (id == null) return HttpNotFound();

            var respuesta = db.RespuestasUsuario
                .Include(r => r.Encuesta)
                .Include(r => r.Detalles.Select(d => d.Pregunta))
                .Include(r => r.Detalles.Select(d => d.Opcion))
                .FirstOrDefault(r => r.Id == id);

            if (respuesta == null) return HttpNotFound();

            return View(respuesta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}