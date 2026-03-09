using Exemplo1_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Exemplo1_MVC.Controllers {
    public class ProdutoController : Controller {

        //public static IList<Produto> Produtos = new List<Produto>() {
        //    new Produto() {
        //        ProdutoId = 1,
        //        Nome = "TV",
        //        Preco = 3500.55f,
        //        categoria = DataStore.categorias.First(cat => cat.CategoriaId == 1)
        //    },

        //    new Produto() {
        //        ProdutoId = 2,
        //        Nome = "Iphone 17",
        //        Preco = 10200.35f,
        //        categoria = DataStore.categorias.First(cat => cat.CategoriaId == 2)
        //    },

        //};

        //Lista os produtos
        public IActionResult Index() {
            return View(DataStore.produtos.OrderBy(prod => prod.Nome));
        }

        //Mostra os detalhes de um produto
        public IActionResult Details(int id) {
            return View(DataStore.produtos.Where(prod => prod.ProdutoId == id).First());
        }

        //Abre o formulário de criação
        public IActionResult Create() {
            ViewBag.Categorias = new SelectList(DataStore.categorias, "CategoriaId", "Nome");
            return View(); //busca o produto
        }

        //Cria um novo produto
        [HttpPost]
        public IActionResult Create(Produto produto) {
            produto.ProdutoId = DataStore.produtos.Select(prod => prod.ProdutoId).Max() + 1;
            produto.categoria = DataStore.categorias.First(cat => cat.CategoriaId == produto.CategoriaId);
            DataStore.produtos.Add(produto);
            return RedirectToAction("Index");


        }

        //Abre o formulário de edição
        public IActionResult Edit(int id) {
            return View(DataStore.produtos.Where(prod => prod.ProdutoId == id).First());
        }

        //Salva a edição
        [HttpPost]
        public IActionResult Edit(Produto produto) {

            //remove o produto antigo
            DataStore.produtos.Remove(
                DataStore.produtos.Where(prod => prod.ProdutoId == produto.ProdutoId).First()
            );

            //adiciona o produto atualizado
            DataStore.produtos.Add(produto);

            return RedirectToAction("Index");
        }

        //Abre a tela de confirmação de exclusão
        public IActionResult Delete(int id) {
            return View(DataStore.produtos.Where(prod => prod.ProdutoId == id).First());
        }

        //Remove o produto
        [HttpPost]
        public IActionResult Delete(Produto produto) {

            DataStore.produtos.Remove(
                DataStore.produtos.Where(prod => prod.ProdutoId == produto.ProdutoId).First()
            );

            return RedirectToAction("Index");
        }
    }
}