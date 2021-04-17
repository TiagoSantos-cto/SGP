using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Models;
using System.Collections.Generic;

namespace SGP.Controllers
{
    public class EntregaController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        
        public EntregaController(IHttpContextAccessor httpContextAccessor) { HttpContextAccessor = httpContextAccessor; }

       
        [HttpGet]
        public IActionResult Registrar(int? id)
        {
            var entrega = new EntregaModel();
            ViewBag.Registro = entrega.CarregarRegistro(id);

            var ListaEmbarcacao = new EmbarcacaoModel(HttpContextAccessor);
            ViewBag.ListaEmbarcacao = ListaEmbarcacao.ListaEmbarcacao();

            ViewBag.ListaStatus = new List<string>(new string[] { "Processamento", "Trânsito", "Encerrada"});

            return View();
        }

        [HttpPost]
        public IActionResult Registrar(EntregaModel Entrega)
        {
            if (ModelState.IsValid)
            {
                Entrega.GravarEntrega();
                return RedirectToAction("Sucesso", Entrega);
            }

            return View();
        }
       
        public IActionResult Sucesso(EntregaModel Entrega)
        {
            ViewBag.Registro = Entrega;

            return View();
        }

    }
}
