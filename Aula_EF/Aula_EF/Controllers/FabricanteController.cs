using Aula_EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExemploEF.Controllers {
    public class FabricanteController : Controller {
        public Context context;

        public FabricanteController(Context ctx) {
            context = ctx;
        }

        //metodo que mostra todos os fabricantes cadastrados no BD
        public IActionResult Index() {
            return View(context.Fabricantes);
        }

        //metodo que abre a página com o form para cadastrar um fabricante
        public IActionResult Create() {
            return View();
        }

        //metodo que recebe os dados do form e salva no BD
        [HttpPost]
        public IActionResult Create(Fabricante fabricante) {
            context.Add(fabricante);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        //metodo que mostra os detalhes de um fabricante específico
        public IActionResult Details(int id) {
            var fabricante = context.Fabricantes
                .Include(f => f.Produtos)
                .FirstOrDefault(f => f.FabricanteId == id);
            return View(fabricante);
        }

        //metodo que carrega os dados do fabricante p/ aparecer no form de edicao.
        public IActionResult Edit(int id) {
            var fabricante = context.Fabricantes.Find(id);
            return View(fabricante);
        }

        //metodo que salva no BD as alterações feitas no form de edicao
        [HttpPost]
        public IActionResult Edit(Fabricante fabricante) {
            context.Entry(fabricante).State = EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        //exibe confirmação do delete
        public IActionResult Delete(int id) {
            var fabricante = context.Fabricantes
                .FirstOrDefault(f => f.FabricanteId == id);
            return View(fabricante);
        }

        //remove do banco
        [HttpPost]
        public IActionResult Delete(Fabricante fabricante) {
            context.Fabricantes.Remove(fabricante);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}