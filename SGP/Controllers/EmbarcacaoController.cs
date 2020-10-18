using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Models;

namespace SGP.Controllers
{
    public class EmbarcacaoController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        public EmbarcacaoController(IHttpContextAccessor httpContextAccessor) { HttpContextAccessor = httpContextAccessor; }

        public IActionResult Index()
        {
            var embarcacao = new EmbarcacaoModel(HttpContextAccessor);
            ViewBag.Listaembarcacao = embarcacao.ListaEmbarcacao();
            return View();
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(EmbarcacaoModel Embarcacao)
        {
            if (ModelState.IsValid)
            {
                Embarcacao.GravarEmbarcacao();
                return RedirectToAction("Sucesso");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
            var Embarcacao = new EmbarcacaoModel(HttpContextAccessor);
            Embarcacao.ExcluirEmbarcacao(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Sucesso()
        {
            return View();
        }

    }
}
