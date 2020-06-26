using System;
using System.Collections.Generic;

namespace ItlaTwitter.Models
{
    public partial class Usuarios
    {
        public Usuarios()
        {
            Publicaciones = new HashSet<Publicaciones>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public string Estado { get; set; }

        public virtual ICollection<Publicaciones> Publicaciones { get; set; }
    }
}
