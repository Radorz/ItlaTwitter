using AutoMapper;
using ItlaTwitter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItlaTwitter.ViewModels;
namespace ItlaTwitter.Infraestructure.Automapper
{
    public class AutomapperConfi : Profile
    {

        public AutomapperConfi ()
        {

            Configureregister();
            ConfigureUsuarios();
            ConfigureComents();
        
            }

        private void Configureregister()
        {
            CreateMap<RegisterViewModel, Usuarios>().ReverseMap().ForMember(dest => dest.indicetel, opt => opt.Ignore()).ForMember
                (dest => dest.RepeatContraseña, opt => opt.Ignore());
            ;

        }
        private void ConfigureUsuarios()
        {
            CreateMap<Usuarios, RegisterViewModel>().ReverseMap();
            ;

        }
        private void ConfigureComents()
        {
            CreateMap<ComentariosViewModel, Comentario>().ReverseMap().ForMember(dest => dest.Comentarista, opt => opt.Ignore());
            ;

        }
    }
}
