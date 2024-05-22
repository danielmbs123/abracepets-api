using AbracePets.Domain.Enumerators;

namespace AbracePets.Domain.DTOs.Evento.Request
{
    public class AdicionarEventoRequest
    {
        public EnumStatus Status { get; set; }
        public DateTime? Data { get; set; }
        public string Local { get; set; }
        public string Descricao { get; set; }
    }
}
