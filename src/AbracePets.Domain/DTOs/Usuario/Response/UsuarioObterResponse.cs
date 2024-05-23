namespace AbracePets.Domain.DTOs.Usuario.Response;

public class UsuarioObterResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string EmailLogin { get; set; }
    public string Telefone { get; set; }
    public string Facebook { get; set; }
    public string Instagram { get; set; }
    public bool IsAdmin { get; set; }
}