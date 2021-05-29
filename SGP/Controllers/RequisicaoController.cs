using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Models;
using SGP.Util;
using System.Collections.Generic;
using static SGP.Models.RequisicaoModel;

namespace SGP.Controllers
{
    public class RequisicaoController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;

        public RequisicaoController(IHttpContextAccessor httpContextAccessor) { HttpContextAccessor = httpContextAccessor; }

        public IActionResult Index(RequisicaoModel requisicao)
        {
            ViewBag.Registro = requisicao;

            return View();
        }

        [HttpPost]
        public IActionResult Informacao(RequisicaoModel requisicao)
        {
            if (ModelState.IsValid)
            {
                requisicao.HttpContextAccessor = HttpContextAccessor;
                requisicao.Gravar();
                return RedirectToAction("Index", requisicao);
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
                ViewBag.ItemRequisicao = requisicao.ObterItensRequisicao(id);
            }

            ViewBag.ListaStatus = new List<string>(new string[] { StatusRequisicao.Solicitar.GetDescription(), StatusRequisicao.Liberar.GetDescription(), StatusRequisicao.Coletar.GetDescription(), StatusRequisicao.Processar.GetDescription(), StatusRequisicao.Cancelar.GetDescription(), StatusRequisicao.Programar.GetDescription()});

            var usuario = new UsuarioModel(HttpContextAccessor);
            ViewBag.ListaUsuario = usuario.ListaUsuario();

            var equipamento = new EquipamentoModel(HttpContextAccessor);
            ViewBag.ListaEquipamento = equipamento.ListaEquipamento();

            var estacao = new EstacaoModel(HttpContextAccessor);
            ViewBag.ListaEstacao = estacao.ListaEstacao();


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
        public IActionResult Encerrar(int id)
        {
            var conta = new RequisicaoModel(HttpContextAccessor);
            conta.Encerrar(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Cancelar(int id)
        {
            var conta = new RequisicaoModel(HttpContextAccessor);
            conta.Cancelar(id);
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
