using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace ItlaTwitter.ViewModels
{
    public class RecuperarViewModel
    {
      
        [Required(ErrorMessage = "Este campo es requerido")]
        [Remote(action: "ReturnUser", controller: "Login")]
        public string usuario { get; set; }
            
        }
    }



