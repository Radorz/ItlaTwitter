using AutoMapper;
using ItlaTwitter.Models;
using ItlaTwitter.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItlaTwitter.Controllers
{
    public class AmigosController : Controller
    {


        private readonly itlaTwitterContext _context;
        private readonly IMapper _mapper;


        public AmigosController(itlaTwitterContext context, IMapper mapper)
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
            var amigos = await _context.Amigos.Where(m => m.Idenvia == userTokens).ToListAsync();
            var listOfListCom = new List<List<ComentariosViewModel>>();
            var Listdatoamigos = new List<Usuarios>();

            foreach (Amigos idp in amigos)
            {
                var datoamigos =  _context.Usuarios.FirstOrDefault(m => m.Id == idp.Idrecibe);
              var publicacionesdeltipo  = await _context.Publicaciones.Where(m => m.Idusuario == idp.Idrecibe).OrderByDescending(a => a.Fecha).ToListAsync();
                foreach (Publicaciones pub in publicacionesdeltipo)
                {
                    var comentarios = _context.Comentario.Where(b => (b.Idpublicacion == pub.Id)).ToList();
                    List<ComentariosViewModel> cometariosview = new List<ComentariosViewModel>();

                    foreach (var Comento in comentarios)
                    {

                        var sabercomentarista = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == Comento.Idusuario);
                        var cvms = _mapper.Map<ComentariosViewModel>(Comento);
                        cvms.Comentarista = sabercomentarista.Usuario;

                        cometariosview.Add(cvms);

                    }
                    listOfListCom.Add(cometariosview);

                  
                }

                Listdatoamigos.Add(datoamigos);

            }
            
           var pubs = _context.Publicaciones2.FromSqlRaw("SELECT I.id,I.contenido,I.idusuario, Nombre, Apellido, Usuario,Fecha FROM Usuarios O JOIN Publicaciones I ON O.id = I.idusuario JOIN Amigos P ON P.idrecibe = I.idusuario where P.idenvia ={0} ORDER BY i.Fecha desc", userTokens).ToList() ;
            
            AmigosViewModel vms = new AmigosViewModel();
            vms.idusuario = userTokens.GetValueOrDefault() ;
            vms.Amigos = Listdatoamigos;
            vms.publicacion = pubs;
            vms.comentario = listOfListCom;

            return View(vms);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AmigosViewModel vms, string submit, string coment)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var userTokens = HttpContext.Session.GetInt32("TokenUser");
            vms.idusuario = userTokens.GetValueOrDefault();
            switch (submit)
            {
                case "AñadirA":
                    var añadir= await _context.Usuarios.FirstOrDefaultAsync(a => a.Usuario == vms.Añadiruser.Trim());

                    var añadiramigo = new Amigos();
                    añadiramigo.Idenvia = vms.idusuario;
                    añadiramigo.Idrecibe = añadir.Id;

                    await _context.Amigos.AddAsync(añadiramigo);
                    await _context.SaveChangesAsync();

                    break;
                case "BorrarAmix":
                    var Borrar = await _context.Amigos.FirstOrDefaultAsync(a => a.Idenvia == vms.idusuario && a.Idrecibe == vms.BorrarAmix);

                    if (Borrar != null)
                    {
                        _context.Amigos.Remove(Borrar);
                        await _context.SaveChangesAsync();
                    }


                    break;
                case "Comentar":
                    if (coment != null)
                    {
                        vms.nuevoCom.Contenido = coment;
                        vms.nuevoCom.Idusuario = userTokens;
                        _context.Comentario.Add(vms.nuevoCom);
                        await _context.SaveChangesAsync();
                    }
                    break;
             
                default:
                    throw new Exception();

            }

            var amigos = await _context.Amigos.Where(m => m.Idenvia == userTokens).ToListAsync();
            var listOfListCom = new List<List<ComentariosViewModel>>();
            var Listdatoamigos = new List<Usuarios>();

            foreach (Amigos idp in amigos)
            {
                var datoamigos = _context.Usuarios.FirstOrDefault(m => m.Id == idp.Idrecibe);
                var publicacionesdeltipo = await _context.Publicaciones.Where(m => m.Idusuario == idp.Idrecibe).OrderByDescending(a => a.Fecha).ToListAsync();
                foreach (Publicaciones pub in publicacionesdeltipo)
                {
                    var comentarios = _context.Comentario.Where(b => (b.Idpublicacion == pub.Id)).ToList();
                    List<ComentariosViewModel> cometariosview = new List<ComentariosViewModel>();

                    foreach (var Comento in comentarios)
                    {

                        var sabercomentarista = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == Comento.Idusuario);
                        var cvms = _mapper.Map<ComentariosViewModel>(Comento);
                        cvms.Comentarista = sabercomentarista.Usuario;

                        cometariosview.Add(cvms);

                    }
                    listOfListCom.Add(cometariosview);

              

                }

                Listdatoamigos.Add(datoamigos);

            }

            var pubs = _context.Publicaciones2.FromSqlRaw("SELECT I.id,I.contenido,I.idusuario, Nombre, Apellido, Usuario,Fecha FROM Usuarios O JOIN Publicaciones I ON O.id = I.idusuario JOIN Amigos P ON P.idrecibe = I.idusuario where P.idenvia ={0} ORDER BY i.Fecha desc", userTokens).ToList();

            vms.Amigos = Listdatoamigos;
            vms.publicacion = pubs;
            vms.comentario = listOfListCom;

            return View(vms);

        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult ReturnUser(string Añadiruser)
        {
            var userTokens = HttpContext.Session.GetInt32("TokenUser");

            var user = _context.Usuarios.FirstOrDefault(c =>
               c.Usuario == Añadiruser);
           
            if (user != null)
            {
              var revisaramigo= _context.Amigos.FirstOrDefault(a => a.Idenvia == userTokens && a.Idrecibe == user.Id);
                if (revisaramigo != null)
                {

                    return Json($"Ya tienes añadido este Usuario {Añadiruser}");

                }
                else {

                    return Json(true);


                }

            }
            
            return Json($"Este Usuario {Añadiruser} no existe");
        }


    }
}
   

