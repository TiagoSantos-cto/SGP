using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Models;
using SGP.Util;
using System.Collections.Generic;
using static SGP.Models.EmbarcacaoModel;


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
        public IActionResult Registrar(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var embarcacao = new EmbarcacaoModel(HttpContextAccessor);
                ViewBag.Registro = embarcacao.CarregarRegistro(id);
                ViewBag.ListaCategoria = new List<string>(new string[] { EnumCategoria.PSV.GetDescription(), EnumCategoria.AHTS.GetDescription(), EnumCategoria.P.GetDescription(), EnumCategoria.UT.GetDescription() });
            }

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
