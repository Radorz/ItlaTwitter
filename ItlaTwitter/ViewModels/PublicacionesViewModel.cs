using ItlaTwitter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItlaTwitter.ViewModels
{
    public class PublicacionesViewModel
    {
        public int Id { get; set; }
        public string Contenido { get; set; }
        public int? Idusuario { get; set; }
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Usuario { get; set; }

        public DateTime Fecha { get; set; }

       
    }
}
