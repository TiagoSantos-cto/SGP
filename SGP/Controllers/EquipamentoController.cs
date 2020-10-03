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

            return View();
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
            var equipamento = new EquipamentoModel(HttpContextAccessor);
            equipamento.ExcluirEquipamento(id);
            return RedirectToAction("Index");
        }


        public IActionResult Sucesso()
        {
            return View();
        }

    }
}
