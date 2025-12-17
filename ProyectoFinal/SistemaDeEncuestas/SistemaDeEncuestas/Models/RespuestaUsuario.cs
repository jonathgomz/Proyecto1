using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeEncuestas.Models
{
    public class RespuestaUsuario
    {
        public int Id { get; set; }

        [Required]
        public int EncuestaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Usuario { get; set; }

        public DateTime Fecha { get; set; }

        [Required]
        public int TiempoSegundos { get; set; }

        public virtual Encuesta Encuesta { get; set; }
        public virtual ICollection<RespuestaDetalle> Detalles { get; set; }

        public RespuestaUsuario()
        {
            Fecha = DateTime.Now;
            Detalles = new List<RespuestaDetalle>();
        }
    }
}