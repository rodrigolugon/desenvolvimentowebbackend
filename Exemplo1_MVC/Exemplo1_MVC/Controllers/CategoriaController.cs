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
        
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        
        public IActionResult Create(Categoria categoria) {

            categorias.Add(categoria); //adc a nova categoria a lista
            //busca o ult Id e incrementa 1 para a nova categoria
            categoria.CategoriaId = categorias.Select(cat => cat.CategoriaId).Max() + 1;
            return RedirectToAction("Index");

        }

        public IActionResult Details(int id) { 

            //retorna uma view com os dados da caterogia cujo id
            //foi passado como parametro
        return View(categorias.Where(cat => cat.CategoriaId == id).First());
        }

        public IActionResult Edit(int id) {
            return View(categorias.Where(cat => cat.CategoriaId == id).First());
        }

            [HttpPost]

            public IActionResult Edit(Categoria categoria) {
                
            //remove
            categorias.Remove(categorias.Where(cat => cat.CategoriaId == categoria.CategoriaId).First());
            //add
            categorias.Add(categoria);
            //redireciona
                return RedirectToAction("Index");
            }
        //Delete
        public IActionResult Delete(int id) {
            return View(categorias.Where(cat => cat.CategoriaId == id).First());
        }

        [HttpPost]

        public IActionResult Delete(Categoria categoria) {

            categorias.Remove(categorias.Where(cat => cat.CategoriaId == categoria.CategoriaId).First());
            return RedirectToAction("Index");
        }
    }
}