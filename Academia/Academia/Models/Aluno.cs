using System.ComponentModel.DataAnnotations;

namespace Academia.Models {
    public class Aluno {
        public int AlunoID { get; set; }
        public string? Nome { get; set; }

        [DataType(DataType.Date)] // Isso remove a hora (o --:--) da tela
        public DateTime Data_Nascimento { get; set; }

        public string? E_Mail { get; set; }
        public string? Instagram { get; set; }
        public string? Telefone { get; set; }
        public string? Observacoes { get; set; }

        // Chave estrangeira
        public int PersonalID { get; set; }

        // Propriedades de navegação (PRECISAM DO ? PARA NÃO TRAVAR O CREATE)
        public virtual Personal? Personal { get; set; }
        public virtual ICollection<Treino>? Treinos { get; set; } = new List<Treino>();
    }
}