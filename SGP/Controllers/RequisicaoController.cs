using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Models;
using System.Collections.Generic;

namespace SGP.Controllers
{
    public class RequisicaoController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;

        public RequisicaoController(IHttpContextAccessor httpContextAccessor) { HttpContextAccessor = httpContextAccessor; }

        public IActionResult Index()
        {
            var requisicao = new RequisicaoModel(HttpContextAccessor);

            return View();
        }


        [HttpPost]
        public IActionResult Informacao(RequisicaoModel requisicao)
        {
            if (ModelState.IsValid)
            {
                requisicao.HttpContextAccessor = HttpContextAccessor;
                requisicao.Gravar();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Informacao(int? id)
        {
            var requisicao = new RequisicaoModel(HttpContextAccessor);

            if (id != null)
            {
                ViewBag.Registro = requisicao.CarregarRegistro(id);
            }

            ViewBag.ListaStatus = new List<string>(new string[] { "Solicitar", "Liberar", "Coletar", "Processar", "Cancelar", "Programar" });

            var usuario = new UsuarioModel(HttpContextAccessor);
            ViewBag.ListaUsuario = usuario.ListaUsuario();

            var equipamento = new EquipamentoModel(HttpContextAccessor);
            ViewBag.ListaEquipamento = equipamento.ListaEquipamento();

            var estacao = new EstacaoModel(HttpContextAccessor);
            ViewBag.ListaEstacao = estacao.ListaEstacao();

            ViewBag.ListaItem = requisicao.ListaItem();

            return View();
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
            var conta = new RequisicaoModel(HttpContextAccessor);
            conta.Excluir(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Atendimentos(RequisicaoModel entity)
        {
            if (entity.UsuarioAtual != 0)
            {
                entity.HttpContextAccessor = HttpContextAccessor;
                ViewBag.ListaRequisicao = entity.ListaRequisicao();
            }
            else
            {
                ViewBag.ListaRequisicao = new List<RequisicaoModel>();
            }

            var usuario = new UsuarioModel(HttpContextAccessor);
            ViewBag.ListaUsuario = usuario.ListaUsuario();

            ViewBag.ListaItem = entity.ListaItem();

            return View();
        }

        [HttpGet]
        public IActionResult ListaItens()
        {
            var entity = new RequisicaoModel
            {
                HttpContextAccessor = HttpContextAccessor
            };

            ViewBag.ListaItem = entity.ListaItem();

            return PartialView();
        }


        [HttpPost]
        public IActionResult ListaItens(string id, int quantidade)
        {
            if (string.IsNullOrEmpty(id))
            {
                var requisicao = new RequisicaoModel(HttpContextAccessor);
                ViewBag.ListaItens = requisicao.ListaItens(id, quantidade);
            }

            var usuario = new UsuarioModel(HttpContextAccessor);
            ViewBag.ListaUsuario = usuario.ListaUsuario();

            return View();
        }

    }
}
