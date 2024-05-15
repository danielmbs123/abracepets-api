using AbracePets.Domain.Entities.Base;
using AbracePets.Domain.Enumerators;

namespace AbracePets.Domain.Entities
{
    public class Pet : EntityBase
    {
        public string Nome { get; private set; }
        public string Foto { get; private set; }
        public string Especie { get; private set; }
        public EnumSexo Sexo { get; private set; }
        public string Raca { get; private set; }
        public string Cor { get; private set; }
        public EnumStatus Status { get; private set; }
        public Guid UsuarioId { get; private set; }
        public virtual Usuario Usuario { get; private set; }

        protected Pet() {}

        public Pet(
            string nome,
            string foto,
            string especie,
            EnumSexo sexo,
            string raca,
            string cor,
            EnumStatus status,
            Guid usuarioId
)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Foto = foto;
            Especie = especie;
            Sexo = sexo;
            Raca = raca;
            Cor = cor;
            Status = status;
            UsuarioId = usuarioId;
        }

        public void Atualizar(
            string nome,
            string foto,
            string especie,
            EnumSexo sexo,
            string raca,
            string cor,
            EnumStatus status)
        {
            Nome = nome;
            Foto = foto;
            Especie = especie;
            Sexo = sexo;
            Raca = raca;
            Cor = cor;
            Status = status;
        }
    }
}
