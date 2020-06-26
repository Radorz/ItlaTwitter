using System;
using System.Collections.Generic;

namespace ItlaTwitter.Models
{
    public partial class Amigos
    {
        public int Id { get; set; }

        public int? Idenvia { get; set; }
        public int? Idrecibe { get; set; }

        public virtual Usuarios IdenviaNavigation { get; set; }
        public virtual Usuarios IdrecibeNavigation { get; set; }
    }
}
