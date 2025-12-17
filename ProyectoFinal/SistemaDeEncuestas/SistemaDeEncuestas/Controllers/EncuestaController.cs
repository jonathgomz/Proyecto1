using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaDeEncuestas.Data;
using SistemaDeEncuestas.Models;

namespace SistemaDeEncuestas.Controllers
{
    public class EncuestaController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Details(int? id)
        {
            if (id == null) return HttpNotFound();
            var encuesta = db.Encuestas.Include(e => e.Preguntas.Select(p => p.Opciones)).Include(e => e.Respuestas).FirstOrDefault(e => e.Id == id);
            if (encuesta == null) return HttpNotFound();
            return View(encuesta);
        }

        public ActionResult Reportes(int? id)
        {
            if (id == null)
                return HttpNotFound();

            var encuesta = db.Encuestas
                .Include(e => e.Preguntas.Select(p => p.Opciones))
                .Include(e => e.Respuestas.Select(r => r.Detalles))
                .FirstOrDefault(e => e.Id == id);

            if (encuesta == null)
                return HttpNotFound();

            return View(encuesta);
        }

        public ActionResult Create()
        {
            var modelo = new EncuestaConPreguntasViewModel();
            return View(modelo);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(EncuestaConPreguntasViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                // Crear la encuesta
                var encuesta = new Encuesta
                {
                    Titulo = modelo.Titulo,
                    Descripcion = modelo.Descripcion,
                    Creador = modelo.Creador,
                    FechaCreacion = DateTime.Now
                };

                db.Encuestas.Add(encuesta);
                db.SaveChanges();

                // Agregar las preguntas
                if (modelo.Preguntas != null && modelo.Preguntas.Any())
                {
                    foreach (var preguntaVM in modelo.Preguntas)
                    {
                        if (!string.IsNullOrWhiteSpace(preguntaVM.Texto))
                        {
                            var pregunta = new Pregunta
                            {
                                EncuestaId = encuesta.Id,
                                Texto = preguntaVM.Texto,
                                TipoPregunta = preguntaVM.TipoPregunta,
                                Orden = preguntaVM.Orden
                            };

                            db.Preguntas.Add(pregunta);
                            db.SaveChanges();

                            // Agregar opciones
                            if (preguntaVM.Opciones != null)
                            {
                                int ordenOpcion = 1;
                                foreach (var opcionTexto in preguntaVM.Opciones)
                                {
                                    if (!string.IsNullOrWhiteSpace(opcionTexto))
                                    {
                                        db.OpcionesPregunta.Add(new OpcionPregunta
                                        {
                                            PreguntaId = pregunta.Id,
                                            TextoOpcion = opcionTexto.Trim(),
                                            Orden = ordenOpcion++
                                        });
                                    }
                                }
                                db.SaveChanges();
                            }
                        }
                    }
                }

                TempData["Success"] = "Encuesta creada exitosamente con todas sus preguntas.";
                return RedirectToAction("Index");
            }

            return View(modelo);
        }



        public ActionResult Index(string orden = "fecha")
        {
            var encuestas = db.Encuestas.AsQueryable();

            switch (orden?.ToLower())
            {
                case "id":
                    encuestas = encuestas.OrderBy(e => e.Id);
                    break;
                case "id_desc":
                    encuestas = encuestas.OrderByDescending(e => e.Id);
                    break;
                case "fecha":
                    encuestas = encuestas.OrderByDescending(e => e.FechaCreacion);
                    break;
                case "fecha_asc":
                    encuestas = encuestas.OrderBy(e => e.FechaCreacion);
                    break;
                default:
                    encuestas = encuestas.OrderByDescending(e => e.FechaCreacion);
                    break;
            }

            ViewBag.OrdenActual = orden ?? "fecha";
            return View(encuestas.ToList());
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();

            var encuesta = db.Encuestas.Find(id);

            if (encuesta == null)
                return HttpNotFound();

            return View(encuesta);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Encuesta modelo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modelo).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Encuesta actualizada exitosamente.";
                return RedirectToAction("Index");
            }
            return View(modelo);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound();

            var encuesta = db.Encuestas
                .Include(e => e.Preguntas)
                .Include(e => e.Respuestas)
                .FirstOrDefault(e => e.Id == id);

            if (encuesta == null)
                return HttpNotFound();

            return View(encuesta);
        }


        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var encuesta = db.Encuestas.Find(id);
            if (encuesta != null)
            {
                db.Encuestas.Remove(encuesta);
                db.SaveChanges();
                TempData["Success"] = "Encuesta eliminada exitosamente.";
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}