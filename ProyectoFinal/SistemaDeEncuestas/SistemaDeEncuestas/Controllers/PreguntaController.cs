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
    public class PreguntaController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Gestionar(int? id)
        {
            if (id == null) return HttpNotFound();

            var encuesta = db.Encuestas
                .Include(e => e.Preguntas.Select(p => p.Opciones))
                .FirstOrDefault(e => e.Id == id);

            if (encuesta == null) return HttpNotFound();

            ViewBag.EncuestaId = id;
            ViewBag.EncuestaTitulo = encuesta.Titulo;

            // Forzar recarga desde BD
            var preguntas = db.Preguntas
                .Include(p => p.Opciones)
                .Where(p => p.EncuestaId == id)
                .OrderBy(p => p.Orden)
                .ToList();

            return View(preguntas);
        }

        public ActionResult Create(int? encuestaId)
        {
            if (encuestaId == null) return HttpNotFound();
            var encuesta = db.Encuestas.Find(encuestaId);
            if (encuesta == null) return HttpNotFound();

            var pregunta = new Pregunta
            {
                EncuestaId = encuestaId.Value,
                Orden = (db.Preguntas.Where(p => p.EncuestaId == encuestaId).Max(p => (int?)p.Orden) ?? 0) + 1
            };
            ViewBag.EncuestaTitulo = encuesta.Titulo;
            return View(pregunta);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(Pregunta modelo, string[] opciones)
        {
            if (ModelState.IsValid)
            {
                db.Preguntas.Add(modelo);
                db.SaveChanges();

                if (opciones != null)
                {
                    for (int i = 0; i < opciones.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(opciones[i]))
                        {
                            db.OpcionesPregunta.Add(new OpcionPregunta
                            {
                                PreguntaId = modelo.Id,
                                TextoOpcion = opciones[i].Trim(),
                                Orden = i + 1
                            });
                        }
                    }
                    db.SaveChanges();
                }

                TempData["Success"] = "Pregunta creada exitosamente.";
                return RedirectToAction("Gestionar", new { id = modelo.EncuestaId });
            }

            ViewBag.EncuestaTitulo = db.Encuestas.Find(modelo.EncuestaId)?.Titulo;
            return View(modelo);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return HttpNotFound();
            var pregunta = db.Preguntas.Include(p => p.Opciones).Include(p => p.Encuesta).FirstOrDefault(p => p.Id == id);
            if (pregunta == null) return HttpNotFound();

            ViewBag.EncuestaTitulo = pregunta.Encuesta.Titulo;
            return View(pregunta);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Pregunta modelo, string[] opciones)
        {
            if (ModelState.IsValid)
            {
                var preguntaDb = db.Preguntas.Include(p => p.Opciones).FirstOrDefault(p => p.Id == modelo.Id);
                if (preguntaDb == null) return HttpNotFound();

                preguntaDb.Texto = modelo.Texto;
                preguntaDb.TipoPregunta = modelo.TipoPregunta;
                preguntaDb.Orden = modelo.Orden;

                db.OpcionesPregunta.RemoveRange(preguntaDb.Opciones);

                if (opciones != null)
                {
                    for (int i = 0; i < opciones.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(opciones[i]))
                        {
                            db.OpcionesPregunta.Add(new OpcionPregunta
                            {
                                PreguntaId = preguntaDb.Id,
                                TextoOpcion = opciones[i].Trim(),
                                Orden = i + 1
                            });
                        }
                    }
                }

                db.SaveChanges();
                TempData["Success"] = "Pregunta actualizada exitosamente.";
                return RedirectToAction("Gestionar", new { id = preguntaDb.EncuestaId });
            }

            ViewBag.EncuestaTitulo = db.Encuestas.Find(modelo.EncuestaId)?.Titulo;
            return View(modelo);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return HttpNotFound();

            var pregunta = db.Preguntas
                .Include(p => p.Opciones)
                .Include(p => p.Encuesta)
                .FirstOrDefault(p => p.Id == id);

            if (pregunta == null)
                return HttpNotFound();

            return View(pregunta);
        }


        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var pregunta = db.Preguntas.Find(id);
            if (pregunta != null)
            {
                int encuestaId = pregunta.EncuestaId;
                db.Preguntas.Remove(pregunta);
                db.SaveChanges();
                TempData["Success"] = "Pregunta eliminada exitosamente.";
                return RedirectToAction("Gestionar", new { id = encuestaId });
            }
            return RedirectToAction("Index", "Encuesta");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}