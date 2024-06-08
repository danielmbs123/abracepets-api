using AbracePets.Domain.Entities.Base;

namespace AbracePets.Domain.Entities;

public class Usuario : EntityBase
{
    public string Nome { get; private set; }
    public string EmailLogin { get; private set; }
    public string Senha { get; private set; }
    public string Telefone { get; private set; }
    public string Facebook { get; private set; }
    public string Instagram { get; private set; }
    public bool IsAdmin { get; private set; }

    protected Usuario() { }

    public Usuario(
        string nome, 
        string emailLogin, 
        string senha,
        string telefone,
        string facebook,
        string instagram,
        bool isAdmin = false)
    {
        Nome = nome;
        EmailLogin = emailLogin;
        Senha = senha;
        Telefone = telefone;
        Facebook = facebook;
        Instagram = instagram;
        IsAdmin = isAdmin;
    }

    public void AlterarSenha(string senha)
    {
        Senha = senha;
    }

    public void AlterarTelefone(string telefone)
    { 
        Telefone = telefone;
    }

    public void AlterarRedesSociais(string facebook, string instagram)
    {
        Facebook = facebook;
        Instagram = instagram;
    }

    public void AlterarNome(string nome)
    {
        Nome = nome;
    }

}