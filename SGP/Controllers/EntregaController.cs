using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Models;
using SGP.Util;
using System.Collections.Generic;
using static SGP.Models.EntregaModel;

namespace SGP.Controllers
{
    public class EntregaController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        
        public EntregaController(IHttpContextAccessor httpContextAccessor) { HttpContextAccessor = httpContextAccessor; }

        [HttpGet]
        public IActionResult Registrar(int? id)
        {
            var entrega = new EntregaModel(HttpContextAccessor);
            
            if (id != null)
            {
                ViewBag.Registro = entrega.CarregarRegistro(id);
            }
           
            var ListaEmbarcacao = new EmbarcacaoModel(HttpContextAccessor);
            ViewBag.ListaEmbarcacao = ListaEmbarcacao.ListaEmbarcacao();

            ViewBag.ListaStatus = new List<string>(new string[] { StatusEntrega.Processamento.GetDescription(), StatusEntrega.Transito.GetDescription(), StatusEntrega.Encerrada.GetDescription() });

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
