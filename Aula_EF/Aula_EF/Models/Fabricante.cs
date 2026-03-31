namespace Aula_EF.Models {
    public class Fabricante {
        public int FabricanteId { get; set; }
        public string Nome { get; set; }
        public ICollection<Produto> Produtos { get; set; } //associacao de 1 p/ mts
    }
}
