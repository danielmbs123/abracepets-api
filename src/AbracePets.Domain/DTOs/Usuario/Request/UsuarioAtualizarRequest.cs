namespace AbracePets.Domain.DTOs.Usuario.Request;

public class UsuarioAtualizarRequest
{
    public Guid Id { get; set; }
    public string SenhaAtual { get; set; }
    public string SenhaNova { get; set; }
    public string Telefone { get; set; }
    public string Facebook { get; set; }
    public string Instagram { get; set; }
}