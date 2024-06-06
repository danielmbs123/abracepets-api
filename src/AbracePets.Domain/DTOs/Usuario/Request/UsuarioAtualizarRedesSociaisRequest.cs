namespace AbracePets.Domain.DTOs.Usuario.Request
{
    public class UsuarioAtualizarRedesSociaisRequest
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
    }
}
