using System;
using System.Collections.Generic;

namespace ItlaTwitter.Models
{
    public partial class Comentario
    {
       

        public int Id { get; set; }
        public int? Idusuario { get; set; }
        public int? Idpublicacion { get; set; }
        public string Contenido { get; set; }
    }
}
