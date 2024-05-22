using AbracePets.Domain.Enumerators;

namespace AbracePets.Domain.DTOs.Evento.Response
{
    public class EventoResponse
    {
        public Guid Id { get; set; }
        public EnumStatus Status { get; set; }
        public DateTime Data { get; set; }
        public string Local { get; set; }
        public string Descricao { get; set; }
        public Guid PetId { get; set; }
    }
}
