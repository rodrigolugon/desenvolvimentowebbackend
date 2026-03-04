namespace Exemplo1_MVC.Models {
    public class DataStore {
        public static List<Categoria> categorias = new List<Categoria>() {
            new Categoria(){ CategoriaId = 1, Nome = "Vestuario"},
            new Categoria(){ CategoriaId = 2, Nome = "Eletronicos"},
            new Categoria(){ CategoriaId = 3, Nome = "Utilidades Domésticas"}
        };
        public static List<Produto> produtos = new List<Produto>() {
            new Produto(){
                ProdutoId = 1,
                Nome = "TV",
                Preco = 3500.55f,
                categoria = categorias.First(cat => cat.CategoriaId == 1)
            },

            new Produto(){
                ProdutoId = 2,
                Nome = "Iphone 17",
                Preco = 10200.35f,
                categoria = categorias.First(cat => cat.CategoriaId == 2)
            }
        };
    }
}