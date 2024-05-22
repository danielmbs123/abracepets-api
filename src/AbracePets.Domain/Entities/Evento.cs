using AbracePets.Domain.Entities.Base;
using AbracePets.Domain.Enumerators;

namespace AbracePets.Domain.Entities
{
    public class Evento : EntityBase
    {
        public EnumStatus Status { get; private set; }
        public DateTime Data { get; private set; }
        public string Local { get; private set; }
        public string Descricao {  get; private set; }
        public Guid PetId { get; private set; }
        public virtual Pet Pet { get; private set; }

        public Evento(EnumStatus status, DateTime data, string local, string descricao, Guid petId)
        {
            this.Status = status;
            this.Data = data;
            this.Local = local;
            this.Descricao = descricao;
            this.PetId = petId;
        }
    }
}
