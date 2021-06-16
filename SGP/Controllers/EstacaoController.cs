using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Models;
using System;

namespace SGP.Controllers
{
    public class EstacaoController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        public EstacaoController(IHttpContextAccessor httpContextAccessor) { HttpContextAccessor = httpContextAccessor; }

        public IActionResult Index()
        {
            var estacao = new EstacaoModel(HttpContextAccessor);
            ViewBag.ListaEstacao = estacao.ListaEstacao();
            return View();
        }

        [HttpGet]
        public IActionResult Registrar(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var estacao = new EstacaoModel(HttpContextAccessor);
                ViewBag.Registro = estacao.CarregarRegistro(id);
            }
            
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
