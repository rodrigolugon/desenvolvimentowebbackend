namespace Exemplo1_MVC.Models {
    public class Produto {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public float Preco { get; set; }
        public int CategoriaId { get; set; }
        public Categoria categoria { get; set; }
    }
}
