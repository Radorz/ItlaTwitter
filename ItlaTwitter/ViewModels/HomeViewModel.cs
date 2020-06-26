using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ItlaTwitter.Models;
namespace ItlaTwitter.ViewModels
{
    public class HomeViewModel
    {
        public int idusuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Usuario { get; set; }
        public List<Usuarios> usuarios { get; set; }
        public List<Publicaciones> publicacion { get; set; }

        public Publicaciones nuevapub { get; set; }

        public Comentario nuevoCom { get; set; }

        public List<List<ComentariosViewModel>> comentario { get; set; }


        public int ideditpub { get; set; }
        public string editconpub { get; set; }



    }
}
