using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ItlaTwitter.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Este campo es requerido")]

        public string usuario { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]

        public string Contraseña { get; set; }
    }
}
