using System;
using System.Collections.Generic;

namespace ItlaTwitter.Models
{
    public partial class Publicaciones
    {
        public int Id { get; set; }
        public string Contenido { get; set; }
        public int? Idusuario { get; set; }
        public DateTime Fecha { get; set; }
        
        public virtual Usuarios IdusuarioNavigation { get; set; }
    }
}
