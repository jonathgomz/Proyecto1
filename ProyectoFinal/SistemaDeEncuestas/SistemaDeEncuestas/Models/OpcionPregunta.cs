using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeEncuestas.Models
{
    public class OpcionPregunta
    {
        public int Id { get; set; }

        [Required]
        public int PreguntaId { get; set; }

        [Required]
        [StringLength(500)]
        public string TextoOpcion { get; set; }

        public int Orden { get; set; }

        public virtual Pregunta Pregunta { get; set; }

        public OpcionPregunta()
        {
            Orden = 1;
        }
    }
}