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
            
            ViewBag.ListaItem = requisicoes.ListaItem(); //teste
            
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
                //var requisicao = new RequisicaoModel(HttpContextAccessor);
                ViewBag.Registro = requisicao.CarregarRegistro(id);
            }

            var usuario = new UsuarioModel(HttpContextAccessor);
            ViewBag.ListaUsuario = usuario.ListaUsuario();

            ViewBag.ListaItem = requisicao.ListaItem(); //teste

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

            ViewBag.ListaItem = entity.ListaItem(); //teste

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
