using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Models;

namespace SGP.Controllers
{
    public class EquipamentoController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        public EquipamentoController(IHttpContextAccessor httpContextAccessor) { HttpContextAccessor = httpContextAccessor; }

       
        [HttpGet]
        public IActionResult Registrar()
        {
            var estacao = new EstacaoModel(HttpContextAccessor);
            ViewBag.ListaEstacao = estacao.ListaEstacao();

            return View();
        }

        [HttpPost]
        public IActionResult Registrar(EquipamentoModel Equipamento)
        {
            if (ModelState.IsValid)
            {
                Equipamento.GravarEquipamento();
                return RedirectToAction("Sucesso");
            }

            var estacao = new EstacaoModel(HttpContextAccessor);
            ViewBag.ListaEstacao = estacao.ListaEstacao();
           
            return View();
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
            var equipamento = new EquipamentoModel(HttpContextAccessor);
            equipamento.ExcluirEquipamento(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Sucesso()
        {
            return View();
        }

    }
}
