using Aula_EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExemploEF.Controllers {
    public class FabricanteController : Controller {
        public Context context;

        public FabricanteController(Context ctx) {
            context = ctx;
        }

        // INDEX — lista todos os fabricantes
        public IActionResult Index() {
            return View(context.Fabricantes);
        }

        // CREATE GET — exibe o formulário
        public IActionResult Create() {
            return View();
        }

        // CREATE POST — salva no banco
        [HttpPost]
        public IActionResult Create(Fabricante fabricante) {
            context.Add(fabricante);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        // DETAILS — exibe um fabricante pelo id
        public IActionResult Details(int id) {
            var fabricante = context.Fabricantes
                .FirstOrDefault(f => f.FabricanteId == id);
            return View(fabricante);
        }

        // EDIT GET — carrega o fabricante para edição
        public IActionResult Edit(int id) {
            var fabricante = context.Fabricantes.Find(id);
            return View(fabricante);
        }

        // EDIT POST — salva as alterações
        [HttpPost]
        public IActionResult Edit(Fabricante fabricante) {
            context.Entry(fabricante).State = EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        // DELETE GET — exibe confirmação
        public IActionResult Delete(int id) {
            var fabricante = context.Fabricantes
                .FirstOrDefault(f => f.FabricanteId == id);
            return View(fabricante);
        }

        // DELETE POST — remove do banco
        [HttpPost]
        public IActionResult Delete(Fabricante fabricante) {
            context.Fabricantes.Remove(fabricante);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}