using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Models;

namespace SGP.Controllers
{
    public class ItemRequisicaoController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        public ItemRequisicaoController(IHttpContextAccessor httpContextAccessor) { HttpContextAccessor = httpContextAccessor; }

        public IActionResult Index()
        {
            //var ItemRequisicao = new ItemRequisicaoModel(HttpContextAccessor);
            //ViewBag.ListaItemRequisicao = ItemRequisicao.ListaItemRequisicao();
            return View();
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(ItemRequisicaoModel ItemRequisicao)
        {
            //if (ModelState.IsValid)
            //{
            //    ItemRequisicao.GravarItemRequisicao();
            //    return RedirectToAction("Sucesso");
            //}

            return View();
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
            //var ItemRequisicao = new ItemRequisicaoModel(HttpContextAccessor);
            //ItemRequisicao.ExcluirItemRequisicao(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Sucesso()
        {
            return View();
        }

    }
}
