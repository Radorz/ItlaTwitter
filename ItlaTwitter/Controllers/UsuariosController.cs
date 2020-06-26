using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ItlaTwitter.Models;
using Microsoft.EntityFrameworkCore;
using ItlaTwitter.ViewModels;
using ServiceStack;
using Microsoft.Data.Sql;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using AutoMapper;

namespace ItlaTwitter.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly itlaTwitterContext _context;
        private readonly IMapper _mapper;

        public UsuariosController(itlaTwitterContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
           
            var userTokens = HttpContext.Session.GetInt32("TokenUser");
            if (string.IsNullOrEmpty(userTokens.ToString()))
            {
                return RedirectToAction("Redirect", "Login");
            }
            var Usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == userTokens);
            
            
            HomeViewModel vms = new HomeViewModel();
            vms.idusuario = userTokens.GetValueOrDefault();
            vms.publicacion = await _context.Publicaciones.Where(b => (b.Idusuario == vms.idusuario)).OrderByDescending(a => a.Fecha).ToListAsync();
            var listOfList = new List<List<ComentariosViewModel>>();
            foreach (Publicaciones idp in vms.publicacion)
            {

                var coments = _context.Comentario.Where(b => (b.Idpublicacion == idp.Id)).ToList();
               
                List<ComentariosViewModel> FirtsList = new List<ComentariosViewModel>();

                foreach (var Comento in coments) {

                    var sabercomentarista = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == Comento.Idusuario);
                    var cvms = _mapper.Map<ComentariosViewModel>(Comento);
                    cvms.Comentarista = sabercomentarista.Usuario;


                    FirtsList.Add(cvms);

                }
                listOfList.Add(FirtsList);



            }

            vms.comentario = listOfList;
            vms.NombreUsuario = Usuario.Nombre + Usuario.Apellido;
            vms.Usuario = Usuario.Usuario;

            return View(vms);
        }
        [HttpPost]

        public async Task<IActionResult> Index(HomeViewModel vms, string submit, string coment, string publicacion)
        {
            var userTokens = HttpContext.Session.GetInt32("TokenUser");
            vms.idusuario = userTokens.GetValueOrDefault();
            if (!ModelState.IsValid)
            {
                return View();
            }

            switch (submit)
            {
                case "Tweet":
                    vms.nuevapub.Contenido = publicacion;
                    vms.nuevapub.Fecha = DateTime.Now;
                    _context.Publicaciones.Add(vms.nuevapub);
                    await _context.SaveChangesAsync();
                    vms.nuevapub = null;

                    break;
                case "Comentar":
                    vms.nuevoCom.Contenido = coment;
                    _context.Comentario.Add(vms.nuevoCom);
                    await _context.SaveChangesAsync();
                    vms.nuevoCom = null;
                    break;
                case "EditarCom":
                    var pub = await _context.Publicaciones.FirstOrDefaultAsync(m => m.Id == vms.ideditpub);
                    if (pub != null)
                    {
                        pub.Contenido = vms.editconpub;
                        _context.Publicaciones.Update(pub);
                        await _context.SaveChangesAsync();

                    }

                    break;
                case "BorrarPub":
                    var pubdelete = await _context.Publicaciones.FirstOrDefaultAsync(m => m.Id == vms.ideditpub);
                    var comentarios = await _context.Comentario.Where(m => m.Idpublicacion == vms.ideditpub).ToListAsync();

                    if (pubdelete != null)
                    {

                        _context.Publicaciones.Remove(pubdelete);
                        foreach(var comentss in comentarios)
                        {

                            _context.Comentario.Remove(comentss);


                        }
                        await _context.SaveChangesAsync();
                    }

                    break;
                default:
                    throw new Exception();
                   
            }

            var Usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == userTokens);
            vms.publicacion = await _context.Publicaciones.Where(b => (b.Idusuario == userTokens)).OrderByDescending(a => a.Fecha).ToListAsync();
            var listOfList = new List<List<ComentariosViewModel>>();
            foreach (Publicaciones idp in vms.publicacion)
            {

                var coments = _context.Comentario.Where(b => (b.Idpublicacion == idp.Id)).ToList();

                List<ComentariosViewModel> Firtslist = new List<ComentariosViewModel>();

                foreach (var Comento in coments)
                {

                    var sabercomentarista = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == userTokens);
                    var cvms = _mapper.Map<ComentariosViewModel>(Comento);
                    cvms.Comentarista = sabercomentarista.Usuario;

                    Firtslist.Add(cvms);

                }
                listOfList.Add(Firtslist);



            }

            vms.comentario = listOfList;
            vms.NombreUsuario = Usuario.Nombre + Usuario.Apellido;
            vms.Usuario = Usuario.Usuario;

            return View(vms);

        }

           

    }
}
