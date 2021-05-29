using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Models;

namespace SGP.Controllers
{
    public class UsuarioController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        public UsuarioController(IHttpContextAccessor httpContextAccessor) { HttpContextAccessor = httpContextAccessor; }

        [HttpGet]
        public IActionResult Login(int? id)
        {
            if (id == 0)
            {
                HttpContext.Session.SetString("IdUsuarioLogado", string.Empty);
                HttpContext.Session.SetString("NomeUsuarioLogado", string.Empty);
            }

            return View();
        }

        [HttpPost]
        public IActionResult ValidarLogin(UsuarioModel usuario)
        {
            var login = usuario.ValidarLogin();
            if (login)
            {
                HttpContext.Session.SetString("IdUsuarioLogado", usuario.Id.ToString());
                HttpContext.Session.SetString("NomeUsuarioLogado", usuario.Login);
                HttpContext.Session.SetString("PerfilAcesso", usuario.PerfilAcesso.ToString());
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ERRO_LOGIN"] = "Usuário ou Senha incorreta.";
                return RedirectToAction("Login");
            }
        }   

        [HttpPost]
        public IActionResult ObterUsuario(int? IdFuncionario)
        {
            if (IdFuncionario != null)
            {
                var usuario = new UsuarioModel(HttpContextAccessor);
                ViewBag.UsuarioBanco = usuario.ObterUsuario(IdFuncionario);
            }

            return View();
        }

        public IActionResult Index()
        {
            var usuario = new UsuarioModel(HttpContextAccessor);
            ViewBag.ListaUsuario = usuario.ListaUsuario();
            return View();
        }

        [HttpPost]
        public IActionResult NovoUsuario(UsuarioModel NovoUsuario)
        {
            if (ModelState.IsValid)
            {
                NovoUsuario.HttpContextAccessor = HttpContextAccessor;
                NovoUsuario.GravarUsuario();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult NovoUsuario(int? id)
        {
            var entity = new UsuarioModel(HttpContextAccessor);
            ViewBag.ListaFuncionario = entity.ListaFuncionario();

            if (id != null)
            {        
                ViewBag.UsuarioBanco = entity.ObterUsuario(id);
            }

            return View();
        }

        [HttpGet]
        public IActionResult ExcluirUsuario(int id)
        {
            var conta = new UsuarioModel(HttpContextAccessor);
            conta.Excluir(id);
            return RedirectToAction("Index");
        }

        public IActionResult Sucesso()
        {
            return View();
        }
    }
}
