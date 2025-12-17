using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeEncuestas.Models
{
    public class EncuestaConPreguntasViewModel
    {
        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [StringLength(4000)]
        public string Descripcion { get; set; }

        [Required]
        [StringLength(100)]
        public string Creador { get; set; }

        public List<PreguntaViewModel> Preguntas { get; set; }

        public EncuestaConPreguntasViewModel()
        {
            Preguntas = new List<PreguntaViewModel>();
        }
    }

    public class PreguntaViewModel
    {
        [Required]
        public string Texto { get; set; }

        [Required]
        public string TipoPregunta { get; set; }

        public int Orden { get; set; }

        public List<string> Opciones { get; set; }

        public PreguntaViewModel()
        {
            TipoPregunta = "Unica";
            Opciones = new List<string> { "", "" };
        }
    }
}