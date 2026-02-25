using Exemplo1_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Exemplo1_MVC.Controllers {
    public class CategoriaController : Controller {
        public static IList<Categoria> categorias = new List<Categoria>() {
            new Categoria() {
                CategoriaId = 1,
                Nome = "Vestuario"
            },
            new Categoria() {
                CategoriaId = 2,
                Nome = "Eletronicos"
            },
            new Categoria() {
                CategoriaId = 3,
                Nome = "Utilidades Domésticas"
            },
        };


        public IActionResult Index() {
            return View(categorias.OrderBy(cat => cat.CategoriaId));
        }
    }
}