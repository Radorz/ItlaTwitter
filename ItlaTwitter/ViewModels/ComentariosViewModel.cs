﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItlaTwitter.ViewModels
{
    public class ComentariosViewModel
    {
        public int Id { get; set; }
        public int? Idpublicacion { get; set; }
        public string Contenido { get; set; }

        public string Comentarista { get; set; }
    }
}
