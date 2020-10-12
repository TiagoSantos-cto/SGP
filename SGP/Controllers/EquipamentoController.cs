using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Models;
using System.IO;

namespace SGP.Controllers
{
    public class EquipamentoController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        public EquipamentoController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment) { HttpContextAccessor = httpContextAccessor; this.environment = environment; }

        private readonly IWebHostEnvironment environment;

        public IActionResult Index()
        {
            var equipamento = new EquipamentoModel(HttpContextAccessor);
            ViewBag.ListaEquipamento = equipamento.ListaEquipamento();
            return View();
        }

        [HttpGet]
        public IActionResult Registrar(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var equipamento = new EquipamentoModel(HttpContextAccessor);
                ViewBag.Registro = equipamento.CarregarRegistro(id);
            }

            var estacao = new EstacaoModel(HttpContextAccessor);
            ViewBag.ListaEstacao = estacao.ListaEstacao();

            return View();
        }

        [HttpPost]
        public IActionResult Registrar(EquipamentoModel equipamento)
        {
            if (ModelState.IsValid)
            {
                GravarImagem(equipamento);
                equipamento.GravarEquipamento();
                return RedirectToAction("Sucesso");
            }

            var estacao = new EstacaoModel(HttpContextAccessor);
            ViewBag.ListaEstacao = estacao.ListaEstacao();
           
            return View();
        }

        private void GravarImagem(EquipamentoModel equipamento)
        {
            if (equipamento.Imagem != null)
            {             
                var imageName = Path.GetFileName(equipamento.Imagem.FileName);
                var imagePath = Path.Combine(environment.WebRootPath + "/upload/", imageName) ;
                using var fileStream = new FileStream(imagePath, FileMode.Create, FileAccess.Write);
                equipamento.Imagem.CopyTo(fileStream);
                equipamento.ImagemPath = "/upload/" + imageName;
            }
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
