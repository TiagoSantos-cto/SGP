using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Models;
using SGP.Util;
using System.Collections.Generic;
using static SGP.Models.SinistroModel;

namespace SGP.Controllers
{
    public class SinistroController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;
        
        public SinistroController(IHttpContextAccessor httpContextAccessor) { HttpContextAccessor = httpContextAccessor; }

        [HttpGet]
        public IActionResult Registrar(int? id)
        {
            var Sinistro = new SinistroModel(HttpContextAccessor);
            
            if (id != null)
            {
                var entity = Sinistro.CarregarRegistro(id);
                if (entity.Id > 0)
                {
                    ViewBag.Registro = entity;
                }        
            }

            ViewBag.ListaStatus = new List<string>(new string[] { StatusSinistro.Aberto.GetDescription(), StatusSinistro.Analise.GetDescription(), StatusSinistro.Finalizar.GetDescription()});
           
            var usuario = new UsuarioModel(HttpContextAccessor);
            ViewBag.ListaUsuario = usuario.ListaUsuario();

            return View();
        }
       
        [HttpPost]
        public IActionResult Registrar(SinistroModel Sinistro)
        {
            if (ModelState.IsValid)
            {
                Sinistro.HttpContextAccessor = HttpContextAccessor;
                Sinistro.Gravar();
                return RedirectToAction("Sucesso", Sinistro);
            }

            ViewBag.ListaStatus = new List<string>(new string[] { StatusSinistro.Aberto.GetDescription(), StatusSinistro.Analise.GetDescription(), StatusSinistro.Finalizar.GetDescription() });

            var usuario = new UsuarioModel(HttpContextAccessor);
            ViewBag.ListaUsuario = usuario.ListaUsuario();

            return View();
        }

        [HttpGet]
        [HttpPost]
        public IActionResult AtendimentosSinistro(SinistroModel entity)
        {
            if (entity.UsuarioAtual != 0)
            {
                entity.HttpContextAccessor = HttpContextAccessor;
                ViewBag.ListaSinistro = entity.ListaSinistro();
            }
            else
            {
                ViewBag.ListaSinistro = new List<SinistroModel>();
            }

            var usuario = new UsuarioModel(HttpContextAccessor);
            ViewBag.ListaUsuario = usuario.ListaUsuario();

            return View();
        }

        [HttpGet]
        public IActionResult Encerrar(int id)
        {
            var conta = new SinistroModel(HttpContextAccessor);
            conta.Finalizar(id);
            return RedirectToAction("Sucesso");
        }

        public IActionResult Sucesso()
        {
            return View();
        }

    }
}
