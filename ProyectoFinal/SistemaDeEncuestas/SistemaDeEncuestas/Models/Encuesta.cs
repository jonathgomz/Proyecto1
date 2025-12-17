using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeEncuestas.Models
{
    public class Encuesta
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [StringLength(4000)]
        public string Descripcion { get; set; }

        [Required]
        [StringLength(100)]
        public string Creador { get; set; }

        public DateTime FechaCreacion { get; set; }

        public virtual ICollection<Pregunta> Preguntas { get; set; }
        public virtual ICollection<RespuestaUsuario> Respuestas { get; set; }

        public Encuesta()
        {
            FechaCreacion = DateTime.Now;
            Preguntas = new List<Pregunta>();
            Respuestas = new List<RespuestaUsuario>();
        }
    }
}