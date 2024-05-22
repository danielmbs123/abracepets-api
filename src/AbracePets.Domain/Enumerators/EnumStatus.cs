using System.ComponentModel;

namespace AbracePets.Domain.Enumerators
{
    public  enum EnumStatus
    {
        [Description("Cadastrado")]
        Cadastrado,

        [Description("Para Adoção")]
        ParaAdocao,

        [Description("Adotado")]
        Adotado,

        [Description("Desaparerecido")]
        Desaparecido
    }
}
