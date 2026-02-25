namespace Exemplo1_MVC.Models {
    public class Categoria {
        public int CategoriaId { get; set; }
        public string Nome { get; set; }

        public ICollection<Produto> produtos { get; set; }
    }
}
