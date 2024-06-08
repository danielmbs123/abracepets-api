using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using AbracePets.CrossCutting;
using AbracePets.Data.Context;
using AbracePets.Domain.DTOs.Pet.Request;
using AbracePets.Domain.DTOs.Pet.Response;
using AbracePets.Domain.DTOs.Usuario.Request;
using AbracePets.Domain.DTOs.Usuario.Response;
using AbracePets.Domain.Entities;
using AbracePets.Domain.Extensions;
using AbracePets.API;
using AbracePets.Domain.DTOs.Evento.Request;
using AbracePets.Domain.DTOs.Evento.Response;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
});
builder.Services.AddDbContext<AbracePetsContext>();
builder.Services.AddCors();
//builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.ConfigureAuthentication();
builder.Services.ConfigureAuthenticateSwagger();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("admin", policy => policy.RequireClaim("IsAdmin", "admin"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//(Cross-Origin Resource Sharing - Compartilhamento de recursos com origens diferentes)
app.UseCors(cors => cors
    .AllowAnyOrigin()
    .AllowAnyMethod() // Get, Post, Put, Delete, etc...
    .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

#region Endpoints de Pet

app.MapGet("/pet/listar", (AbracePetsContext context) =>
    {
        var pets = context.PetSet.Select(pet => new PetObterResponse
        {
            Id = pet.Id,
            Nome = pet.Nome,
            Foto = pet.Foto,
            Especie = pet.Especie,
            Sexo = pet.Sexo,
            Raca = pet.Raca,
            Cor = pet.Cor,
            Status = pet.GetLastStatus(),
            UsuarioId = pet.UsuarioId
        });

        return Results.Ok(pets);
    })
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para obter todos os pets cadastrados";
        operation.Summary = "Listar todos os pets";
        return operation;
    })
    .Produces<List<PetObterResponse>>()
    .WithTags("Pets");

app.MapGet("/pet/{petId}", (AbracePetsContext context, Guid petId) =>
    {
        var pet = context.PetSet.Find(petId);
        if (pet is null)
            return Results.BadRequest("Pet não Localizado.");

        var petDto = new PetObterResponse
        {
            Id = pet.Id,
            Nome = pet.Nome,
            Foto = pet.Foto,
            Especie = pet.Especie,
            Sexo = pet.Sexo,
            Raca = pet.Raca,
            Cor = pet.Cor,
            Status = pet.GetLastStatus(),
            UsuarioId = pet.UsuarioId
        };

        return Results.Ok(petDto);
    })
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para obter um pet com base no ID informado";
        operation.Summary = "Obter um Pet";
        operation.Parameters[0].Description = "Id do Pet";
        return operation;
    })
    .Produces<PetObterResponse>()
    .WithTags("Pets");

app.MapPost("/pet/adicionar", (AbracePetsContext context, PetAdicionarRequest petAdicionarRequest, ClaimsPrincipal principal) =>
    {
        try
        {
            var pet = new Pet(
                petAdicionarRequest.Nome,
                petAdicionarRequest.Foto,
                petAdicionarRequest.Especie,
                petAdicionarRequest.Sexo,
                petAdicionarRequest.Raca,
                petAdicionarRequest.Cor,
                TokenUtil.GetUsuarioId(principal)
            );

            context.PetSet.Add(pet);
            context.SaveChanges();

            return Results.Created("Created", $"Pet Registrado com Sucesso. {pet.Id}");
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.InnerException?.Message ?? ex.Message);
        }
    })
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para Cadastrar um Pet";
        operation.Summary = "Novo Pet";
        return operation;
    })
    .Produces<string>(201)
    .WithTags("Pets")
    .RequireAuthorization();

app.MapPut("/pet/atualizar", (AbracePetsContext context, PetAtualizarRequest petAtualizarRequest, ClaimsPrincipal principal) =>
    {
        try
        {
            var usuarioId = TokenUtil.GetUsuarioId(principal);

            var pet = context.PetSet.Find(petAtualizarRequest.Id);
            if (pet is null)
                return Results.BadRequest("Pet não Localizado.");


            if (pet.UsuarioId != usuarioId)
                //TODO: maybe return 403?
                return Results.BadRequest("Não é permitido alterar um pet que não lhe pertença");

            pet.Atualizar(
                petAtualizarRequest.Nome,
                petAtualizarRequest.Foto,
                petAtualizarRequest.Especie,
                petAtualizarRequest.Sexo,
                petAtualizarRequest.Raca,
                petAtualizarRequest.Cor
            );

            context.PetSet.Update(pet);
            context.SaveChanges();


            return Results.Ok("Pet Atualizado com Sucesso.");
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.InnerException?.Message ?? ex.Message);
        }
    })
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para Atualizar um Pet";
        operation.Summary = "Atualizar Pet";
        return operation;
    })
    .Produces<string>()
    .WithTags("Pets")
    .RequireAuthorization();



app.MapDelete("/pet/{petId}", (AbracePetsContext context, Guid petId, ClaimsPrincipal principal) =>
    {
        try
        {
            var usuarioId = TokenUtil.GetUsuarioId(principal);

            var pet = context.PetSet.Find(petId);
            if (pet is null)
                return Results.BadRequest("Pet não Localizado.");

            if (pet.UsuarioId != usuarioId)
                //TODO: maybe return 403?
                return Results.BadRequest("Não é permitido alterar um pet que não lhe pertença");

            context.PetSet.Remove(pet);
            context.SaveChanges();

            return Results.Ok("Pet Removido com Sucesso.");
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.InnerException?.Message ?? ex.Message);
        }
    })
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para Excluir um Pet";
        operation.Summary = "Excluir Pet";
        operation.Parameters[0].Description = "Id do Pet";
        return operation;
    })
    .Produces<string>()
    .WithTags("Pets")
    .RequireAuthorization();

#endregion

#region Endpoints de Eventos de Pet
app.MapPost("/pet/{petId}/evento", (AbracePetsContext context, Guid petId, AdicionarEventoRequest request, ClaimsPrincipal principal) =>
{
    try
    {
        var usuarioId = TokenUtil.GetUsuarioId(principal);

        var pet = context.PetSet.Find(petId);
        if (pet is null)
            return Results.BadRequest("Pet não Localizado.");


        /*if (pet.UsuarioId != usuarioId)
            //TODO: maybe return 403?
            return Results.BadRequest("Não é permitido alterar um pet que não lhe pertença");
        */

        pet.AdicionarEvento(
            new Evento(
                request.Status,
                request.Data ?? DateTime.Now,
                request.Local,
                request.Descricao,
                petId
            )
        );

        context.PetSet.Update(pet);
        context.SaveChanges();


        return Results.Ok("Pet Atualizado com Sucesso.");
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.InnerException?.Message ?? ex.Message);
    }
})
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para Adicionar Eventos no Pet";
        operation.Summary = "Adicionar Evento no Pet";
        return operation;
    })
    .Produces<string>()
    .WithTags("Eventos Pets")
    .RequireAuthorization();

app.MapGet("/pet/evento", (AbracePetsContext context) =>
{
    try
    {
        var eventos = context.EventoSet.Select(e => new EventoResponse
        {
            Data = e.Data,
            Descricao = e.Descricao,
            Id = e.Id,
            Local = e.Local,
            PetId = e.PetId,
            Status = e.Status
        }).ToList();

        return Results.Ok(eventos);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.InnerException?.Message ?? ex.Message);
    }
})
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para Listar Eventos de Todos os Pets";
        operation.Summary = "Listar Eventos de Todos os Pets";
        return operation;
    })
    .Produces<List<EventoResponse>>()
    .WithTags("Eventos Pets");

app.MapGet("/pet/{petId}/evento", (AbracePetsContext context, Guid petId) =>
{
    try
    {
        // Busca os eventos relacionados ao pet específico
        var eventos = context.EventoSet
            .Where(e => e.PetId == petId)
            .Select(e => new EventoResponse
            {
                Data = e.Data,
                Descricao = e.Descricao,
                Id = e.Id,
                Local = e.Local,
                PetId = e.PetId,
                Status = e.Status
            })
            .ToList();

        return Results.Ok(eventos);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.InnerException?.Message ?? ex.Message);
    }
})
.WithOpenApi(operation =>
{
    operation.Description = "Endpoint para Listar Eventos de um Pet específico";
    operation.Summary = "Listar Eventos de um Pet";
    operation.Parameters[0].Description = "Id do Pet";
    return operation;
})
.Produces<List<EventoResponse>>()
.WithTags("Eventos Pets");

app.MapDelete("/pet/{petId}/evento/{eventoId}", (AbracePetsContext context, Guid petId, Guid eventoId, ClaimsPrincipal principal) =>
{
    try
    {
        var usuarioId = TokenUtil.GetUsuarioId(principal);

        var pet = context.PetSet.Find(petId);
        if (pet is null)
            return Results.BadRequest("Pet não Localizado.");


        /*if (pet.UsuarioId != usuarioId)
            //TODO: maybe return 403?
            return Results.BadRequest("Não é permitido alterar um pet que não lhe pertença");*/

        pet.RemoveEvento(eventoId);

        context.PetSet.Update(pet);
        context.SaveChanges();


        return Results.Ok("Pet Atualizado com Sucesso.");
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.InnerException?.Message ?? ex.Message);
    }
})
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para Adicionar Eventos no Pet";
        operation.Summary = "Adicionar Evento no Pet";
        return operation;
    })
    .Produces<string>()
    .WithTags("Eventos Pets")
    .RequireAuthorization();
#endregion

#region Endpoints de Usuários

app.MapGet("/usuario", (AbracePetsContext context) =>
    {
        var usuarios = context.UsuariosSet.Select(usuario => new UsuarioListarResponse
        {
            Id = usuario.Id,
            Nome = usuario.Nome
        });

        return Results.Ok(usuarios);
    })
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para obter todos os usu rios cadastrados";
        operation.Summary = "Listar todos os Usuários";
        return operation;
    })
    .Produces<List<UsuarioListarResponse>>()
    .WithTags("Usuários")
    .RequireAuthorization();

app.MapGet("/usuario/{usuarioId}", (AbracePetsContext context, Guid usuarioId) =>
    {
        var usuario = context.UsuariosSet.Find(usuarioId);
        if (usuario is null)
            return Results.BadRequest("Usuário não Localizado.");

        var usuarioDto = new UsuarioObterResponse
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            EmailLogin = usuario.EmailLogin,
            Telefone = usuario.Telefone,      
            Facebook = usuario.Facebook,
            Instagram = usuario.Instagram,
            IsAdmin = usuario.IsAdmin,
        };

        return Results.Ok(usuarioDto);
    })
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para obter um usuário com base no ID informado";
        operation.Summary = "Obter um Usuário";
        operation.Parameters[0].Description = "Id do Usuário";
        return operation;
    })
    .Produces<UsuarioObterResponse>()
    .WithTags("Usuários")
    .RequireAuthorization();

app.MapPost("/usuario", (AbracePetsContext context, UsuarioAdicionarRequest usuarioAdicionarRequest) =>
    {
        try
        {
            if (usuarioAdicionarRequest.EmailLogin != usuarioAdicionarRequest.EmailLoginConfirmacao)
                return Results.BadRequest("Email de Login não Confere.");

            if (usuarioAdicionarRequest.Senha != usuarioAdicionarRequest.SenhaConfirmacao)
                return Results.BadRequest("Senha não Confere.");

            if (context.UsuariosSet.Any(p => p.EmailLogin == usuarioAdicionarRequest.EmailLogin))
                return Results.BadRequest("Email já utilizado para Login em outro Usuário.");

            var usuario = new Usuario(
                usuarioAdicionarRequest.Nome,
                usuarioAdicionarRequest.EmailLogin,
                usuarioAdicionarRequest.Senha.EncryptPassword(), 
                usuarioAdicionarRequest.Telefone, 
                usuarioAdicionarRequest.Facebook, 
                usuarioAdicionarRequest.Instagram
            );

            context.UsuariosSet.Add(usuario);
            context.SaveChanges();

            return Results.Created("Created", $"Usuário Registrado com Sucesso. {usuario.Id}");
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.InnerException?.Message ?? ex.Message);
        }
    })
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para Cadastrar um Usuário";
        operation.Summary = "Novo Usuário";
        return operation;
    })
    .Produces<string>()
    .WithTags("Usuários");

app.MapPut("/usuario/alterar-senha", (AbracePetsContext context, UsuarioAtualizarRequest usuarioAtualizarRequest) =>
    {
        try
        {
            var usuario = context.UsuariosSet.Find(usuarioAtualizarRequest.Id);
            if (usuario is null)
                return Results.BadRequest("Usuário não Localizado.");

            if (usuarioAtualizarRequest.SenhaAtual.EncryptPassword() == usuario.Senha)
            {
                usuario.AlterarSenha(usuarioAtualizarRequest.SenhaNova.EncryptPassword());
                context.UsuariosSet.Update(usuario);
                context.SaveChanges();

                return Results.Ok("Senha Altera com Sucesso.");
            }

            return Results.BadRequest("Ocorreu um Problema ao Alterar a Senha do Usuário.");
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.InnerException?.Message ?? ex.Message);
        }
    })
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para Alterar a Senha do Usuário";
        operation.Summary = "Alterar Senha";
        return operation;
    })
    .Produces<string>()
    .WithTags("Usuários")
    .RequireAuthorization();

app.MapPut("/usuario/atualizar-redes-sociais", (AbracePetsContext context, UsuarioAtualizarRedesSociaisRequest usuarioAtualizarRedesSociaisRequest) =>
{
    try
    {
        var usuario = context.UsuariosSet.Find(usuarioAtualizarRedesSociaisRequest.Id);
        if (usuario is null)
            return Results.BadRequest("Usuário não Localizado.");

        usuario.AlterarNome(usuarioAtualizarRedesSociaisRequest.Nome);
        usuario.AlterarRedesSociais(usuarioAtualizarRedesSociaisRequest.Facebook, usuarioAtualizarRedesSociaisRequest.Instagram);
        usuario.AlterarTelefone(usuarioAtualizarRedesSociaisRequest.Telefone);

        context.UsuariosSet.Update(usuario);
        context.SaveChanges();

        return Results.Ok("Redes Sociais Atualizadas com Sucesso.");
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.InnerException?.Message ?? ex.Message);
    }
})
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para Atualizar as Redes Sociais do Usuário";
        operation.Summary = "Atualizar Redes Sociais";
        return operation;
    })
    .Produces<string>()
    .WithTags("Usuários")
    .RequireAuthorization();


#endregion

#region Autenticação

app.MapPost("/autenticar", (AbracePetsContext context, UsuarioAutenticarRequest usuarioAutenticarRequest) =>
    {
        var usuario = context.UsuariosSet.FirstOrDefault(p =>
            p.EmailLogin == usuarioAutenticarRequest.EmailLogin &&
            p.Senha == usuarioAutenticarRequest.Senha.EncryptPassword());
        if (usuario is null)
            return Results.BadRequest("N o foi Poss vel Efetuar o Login.");

        var claims = new[]
        {
            new Claim("UsuarioId", usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim("IsAdmin", "admin123")
        };

        //Recebe uma instância da Classe SymmetricSecurityKey
        //armazenando a chave de criptografia usada na cria  o do Token
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("{ccdc511d-23f0-4a30-995e-ebc3658e901d}"));

        //Recebe um objeto do tipo SigninCredentials contendo a chave de
        //criptografia e o algoritimo de seguran a empregados na gera  o
        //de assinaturas digitais para tokens
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "abrace.pet",
            audience: "abrace.pet",
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return Results.Ok(new UsuarioAutenticarResponse(
            usuario.Id,
            usuario.Nome,
            new JwtSecurityTokenHandler().WriteToken(token)
        ));
    })
    .WithOpenApi(operation =>
    {
        operation.Description = "Endpoint para Autenticar um Usuário na API";
        operation.Summary = "Autenticar Usuário";
        return operation;
    })
    .Produces<UsuarioAutenticarResponse>()
    .WithTags("Segurança");

#endregion

app.Run();