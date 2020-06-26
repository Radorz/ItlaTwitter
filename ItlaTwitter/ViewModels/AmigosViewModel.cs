using ItlaTwitter.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItlaTwitter.ViewModels
{
    public class AmigosViewModel
    {
        public int idusuario { get; set; }
        public List<Usuarios> Amigos { get; set; }
        public List<PublicacionesViewModel> publicacion { get; set; }
        public Comentario nuevoCom { get; set; }
        public List<List<ComentariosViewModel>> comentario { get; set; }

        [Remote(action: "ReturnUser", controller: "Amigos")]
        public string Añadiruser{ get; set; }
        public int BorrarAmix { get; set; }


    }
}
