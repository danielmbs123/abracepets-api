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
        public ICollection<Evento> Eventos { get; private set; }
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
            Guid usuarioId,
            params Evento[] eventos
)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Foto = foto;
            Especie = especie;
            Sexo = sexo;
            Raca = raca;
            Cor = cor;            
            UsuarioId = usuarioId;
            Eventos = eventos ?? [];
        }

        public void Atualizar(
            string nome,
            string foto,
            string especie,
            EnumSexo sexo,
            string raca,
            string cor)
        {
            Nome = nome;
            Foto = foto;
            Especie = especie;
            Sexo = sexo;
            Raca = raca;
            Cor = cor;
        }

        public void AdicionarEvento(Evento evento)
        {
            if (Eventos == null)
            {
                Eventos = new List<Evento>();
            }

            Eventos.Add(evento);
        }

        public void RemoveEvento(Guid eventId)
        {
            Eventos = Eventos.ToList().Where(e => e.Id != eventId).ToList();
        }

        public EnumStatus GetLastStatus()
        {
            if (Eventos == null || Eventos.Count == 0) 
                return EnumStatus.Cadastrado;

            return Eventos.ToList().OrderBy(e => e.Data).LastOrDefault().Status;
        }
    }
}
