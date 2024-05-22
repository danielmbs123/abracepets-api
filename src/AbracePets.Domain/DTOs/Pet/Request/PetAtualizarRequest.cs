﻿using AbracePets.Domain.Enumerators;

namespace AbracePets.Domain.DTOs.Pet.Request;

public class PetAtualizarRequest
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Foto { get; set; }
    public string Especie { get; set; }
    public EnumSexo Sexo { get; set; }
    public string Raca { get; set; }
    public string Cor { get; set; }
}