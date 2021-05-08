using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.Models;
using System;
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
            var Sinistro = new SinistroModel();
            
            if (id != null && id > 0)
            {
                ViewBag.Registro = Sinistro.CarregarRegistro(id);
            }

            ViewBag.ListaStatus = new List<string>(new string[] { Enum.GetName(typeof(StatusSinistro), 0),  StatusSinistro.Aberto.ToString(), StatusSinistro.Analise.ToString(), StatusSinistro.Finalizado.ToString() });
            var usuario = new UsuarioModel(HttpContextAccessor);
            ViewBag.ListaUsuario = usuario.ListaUsuario();

            return View();
        }
       
        [HttpPost]
        public IActionResult Registrar(SinistroModel Sinistro)
        {
            if (ModelState.IsValid)
            {
                Sinistro.Gravar();
                return RedirectToAction("Sucesso", Sinistro);
            }

            return View();
        }
       
        public IActionResult Sucesso(SinistroModel Sinistro)
        {
            ViewBag.Registro = Sinistro;

            return View();
        }

    }
}
