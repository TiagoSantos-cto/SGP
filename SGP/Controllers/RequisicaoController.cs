using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Models;

namespace SGP.Controllers
{
    public class RequisicaoController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;

        public RequisicaoController(IHttpContextAccessor httpContextAccessor) { HttpContextAccessor = httpContextAccessor; }

        public IActionResult Index()
        {
            var requisicoes = new RequisicaoModel(HttpContextAccessor);
            ViewBag.ListaRequisicao = requisicoes.ListaRequisicao();
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
            if (id != null)
            {
                var transacao = new RequisicaoModel(HttpContextAccessor);
                ViewBag.Registro = transacao.CarregarRegistro(id);
            }

            var usuario = new UsuarioModel(HttpContextAccessor);
            ViewBag.ListaUsuario = usuario.ListaUsuario();

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
            entity.HttpContextAccessor = HttpContextAccessor;
            ViewBag.ListaRequisicao = entity.ListaRequisicao();
           
            var usuario = new UsuarioModel(HttpContextAccessor);
            ViewBag.ListaUsuario = usuario.ListaUsuario();
           
            return View();
        }
    }
}
