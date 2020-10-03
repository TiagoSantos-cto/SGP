using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Models;

namespace SGP.Controllers
{
    public class EstacaoController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        public EstacaoController(IHttpContextAccessor httpContextAccessor) { HttpContextAccessor = httpContextAccessor; }

       
        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(EstacaoModel Estacao)
        {
            if (ModelState.IsValid)
            {
                Estacao.GravarEstacao();
                return RedirectToAction("Sucesso");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
            var Estacao = new EstacaoModel(HttpContextAccessor);
            Estacao.ExcluirEstacao(id);
            return RedirectToAction("Index");
        }


        public IActionResult Sucesso()
        {
            return View();
        }

    }
}
