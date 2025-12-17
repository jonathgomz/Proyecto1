using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeEncuestas.Models
{
    public class RespuestaDetalle
    {
        public int Id { get; set; }

        [Required]
        public int RespuestaUsuarioId { get; set; }

        [Required]
        public int PreguntaId { get; set; }

        [Required]
        public int OpcionId { get; set; }

        public virtual RespuestaUsuario RespuestaUsuario { get; set; }
        public virtual Pregunta Pregunta { get; set; }
        public virtual OpcionPregunta Opcion { get; set; }
    }
}