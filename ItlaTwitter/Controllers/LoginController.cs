using AutoMapper;
using EmailHandler;
using ItlaTwitter.Models;
using ItlaTwitter.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace ItlaTwitter.Controllers
{
    public class LoginController : Controller
    {

        private readonly itlaTwitterContext _context;
        private readonly Iemailsender _iemailsender;
        private readonly IMapper _mapper;

        public LoginController(itlaTwitterContext context, Iemailsender iemailsender, IMapper mapper)
        {
            _context = context;
            _iemailsender = iemailsender;
            this._mapper = mapper;
        }

        public IActionResult Index()
        {
            var userTokens = HttpContext.Session.GetInt32("TokenUser");
            if (userTokens != null)
            {
                return RedirectToAction("Index", "Usuarios");
            }


          ViewBag.mostrar = "none";


            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Index(LoginViewModel vm)
        {

            ViewBag.mostrar = "none";

            if (ModelState.IsValid)
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(c =>
                c.Usuario== vm.usuario  && c.Contraseña == PasswordEncryption(vm.Contraseña));
                
                if (user != null)
                {
                    if (user.Estado.Trim() == "Activo")
                    {
                        HttpContext.Session.SetInt32("TokenUser", user.Id);
                        return RedirectToAction("Index", "Usuarios");

                    }
                   if (user.Estado.Trim() == "Inactivo")
                    {
                        ViewBag.mostrar = "block";

                    }

                } else
                {
                    ModelState.AddModelError("UsuarioOcontraseñaerronaea", "El usuario ingresado o contraseña estan erroneos");
                }
               
            }


            return View(vm);
        }


        [HttpGet]

        public IActionResult Registrar()
        {
            var userTokens = HttpContext.Session.GetInt32("TokenUser");
            if (userTokens != null)
            {
                return RedirectToAction("Index", "Usuarios");
            }
            ViewBag.mostrar = "none";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(RegisterViewModel vm)
        {
            ViewBag.mostrar = "none";

            if (!ModelState.IsValid)
            {
                return View();
            }
            vm.Telefono = string.Format("{0}-{1}", vm.indicetel, vm.Telefono).Trim();
            vm.Contraseña = PasswordEncryption(vm.Contraseña).Trim();
            var usuarioentity = _mapper.Map<Usuarios>(vm);

            string url = Request.GetDisplayUrl().Replace("Registrar", "Activador");

            _context.Usuarios.Add(usuarioentity);
            await _context.SaveChangesAsync();
            var message = new Message(new string[] { vm.Correo }, "Activacion de Cuenta", "¡Bienvenido a Itla Twitter!, \n " + usuarioentity.Nombre + " " + usuarioentity.Apellido + " \n Gracias por ingresar a la familia ITLA, esperamos que disfrutes de esta aplicacion y puedas sacarle todo el potencial que tiene escondio \n Este es el link para activar la cuenta del Usuario " +  usuarioentity.Usuario+":"+ " \n" +url+"/"+ usuarioentity.Id);
            await _iemailsender.SendMailAsync(message);
            ViewBag.mostrar = "block";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Activador(int? id)
        {
            var userTokens = HttpContext.Session.GetInt32("TokenUser");
            if (userTokens != null)
            {
                return RedirectToAction("Index", "Usuarios");
            }
            if (id == null)
            {
                return NotFound();
            }

            var Usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id);

            if (Usuario == null)
            {
                return NotFound();
            }
            Usuario.Estado = "Activo".Trim();
            _context.Usuarios.Update(Usuario);
              await _context.SaveChangesAsync();
            
            


            return  View();
        }
        [HttpGet]
        public IActionResult Recuperar()
        {
            var userTokens = HttpContext.Session.GetInt32("TokenUser");
            if (userTokens != null)
            {
                return RedirectToAction("Index", "Usuarios");
            }
            ViewBag.mostrar = "none";

            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Recuperar(RecuperarViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = _context.Usuarios.FirstOrDefault(c =>
              c.Usuario == vm.usuario);
            Random rnd = new Random();
            int nuevacontraseña = rnd.Next(100000, 999999);

            user.Contraseña = PasswordEncryption(Convert.ToString( nuevacontraseña));
            _context.Usuarios.Update(user);
            await _context.SaveChangesAsync();
            var message = new Message(new string[] {user.Correo}, "Recuperacion de Cuenta", "Hemos sidos notificados que que has perdido tu contraseña para acceder a ITLA Twiiter \nEsta es tu nueva Contraseña " + user.Usuario.Trim()+ ":\n" + nuevacontraseña); ;
            await _iemailsender.SendMailAsync(message);
            
            return View();
        }
        public IActionResult Salir()
        {

            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        public IActionResult Redirect()
        {
            
            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public  IActionResult VerifyUser(string usuario)
        {
            var user =  _context.Usuarios.FirstOrDefault(c =>
                c.Usuario == usuario );
            if (user != null)
            {
                return Json($"El Usuaurio {usuario} ya esta en uso.");
            }

            return Json(true);
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult ReturnUser(string usuario)
        {
            var user = _context.Usuarios.FirstOrDefault(c =>
               c.Usuario == usuario);
            if (user != null)
            {
                return Json(true);

            }

            return Json($"Este Usuario {usuario} no existe");
        }
        private string PasswordEncryption (string password)
        {
            using (SHA256 sha256hash = SHA256.Create())
            {

                byte[] bytes = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (Byte t in bytes)
                {

                    builder.Append(t.ToString("x2"));
                }
                return builder.ToString();
            }

        }


    }

}
