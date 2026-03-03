using Exemplo1_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exemplo1_MVC.Controllers {
    public class ProdutoController : Controller {

        public static IList<Produto> Produtos = new List<Produto>() {
            new Produto() {
                ProdutoId = 1,
                Nome = "TV",
                Preco = 3500.55f,
                categoria = CategoriaController.categorias.First(cat => cat.CategoriaId == 1)
            },

            new Produto() {
                ProdutoId = 2,
                Nome = "Iphone 17",
                Preco = 10200.35f,
                categoria = CategoriaController.categorias.First(cat => cat.CategoriaId == 2)
            },

        };
        public IActionResult Index() {
            return View(Produtos.OrderBy(prod => prod.Nome));
        }
    }
}
