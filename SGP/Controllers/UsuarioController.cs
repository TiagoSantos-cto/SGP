using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Models;

namespace SGP.Controllers
{
    public class UsuarioController : Controller
    {
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
                HttpContext.Session.SetString("NomeUsuarioLogado", usuario.Nome);
                HttpContext.Session.SetString("FuncaoUsuario", usuario.Funcao.ToString());
                return RedirectToAction("Index", "Home"); 
            }
            else
            {
                TempData["ERRO_LOGIN"] = "E-mail ou Senha incorreta.";
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public IActionResult Registrar(UsuarioModel usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.GravarUsuario();
                return RedirectToAction("Sucesso");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        public IActionResult Sucesso()
        {
            return View();
        }
    }
}
