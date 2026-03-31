using Aula_EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;


namespace Aula_EF.Controllers {
    public class ProdutoController : Controller {
        public Context context;
        public ProdutoController(Context ctx) {
            context = ctx;
        }
        public IActionResult Index(int pagina = 1) {
            //retorna uma view com todos os produtos
            //o métoto Include carrega a associação com Fabricante
            return View(context.Produtos
                .Include(f => f.Fabricante)

            //converte o resultado 
            .ToPagedList(pagina, 3));
        }


        public IActionResult Create() {
            //utiliza uma Viewbag para gerar uma lista com os nomes dos fabricantes
            ViewBag.FabricanteID = new SelectList(context.Fabricantes
                .OrderBy(f => f.Nome), "FabricanteId", "Nome");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Produto produto) {
            context.Add(produto);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id) {
            var produto = context.Produtos
                .Include(f => f.Fabricante)
                .FirstOrDefault(p => p.ProdutoID == id);
            return View(produto);
        }

        public IActionResult Edit(int id) {
            var produto = context.Produtos.Find(id);
            ViewBag.FabricanteID = new SelectList(context.Fabricantes.OrderBy(f => f.Nome), "FabricanteId", "Nome");
            return View(produto);
        }

        [HttpPost]
        public IActionResult Edit(Produto produto) {
            //avisa a EF que o registro será modificado
            context.Entry(produto).State = EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id) {
            var produto = context.Produtos
                .Include(f => f.Fabricante)
                .FirstOrDefault(p => p.ProdutoID == id);
            return View(produto);
        }

        [HttpPost]
        public IActionResult Delete(Produto produto) {
            context.Produtos.Remove(produto);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}