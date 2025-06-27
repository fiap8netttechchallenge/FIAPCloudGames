using FIAPCloudGames.Application.Requests;
using FluentValidation;

namespace FIAPCloudGames.WebAPI.Validators;

public class CreateGameRequestValidator : AbstractValidator<CreateGameRequest>
{
    public CreateGameRequestValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(20)
            .WithMessage("O 'Nome' deve ter no mínimo 20 caracteres.")
            .MaximumLength(255)
            .WithMessage("O 'Nome' deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Description)
            .MinimumLength(20)
            .WithMessage("O 'Descrição' deve ter no mínimo 20 caracteres.")
            .MaximumLength(1000)
            .WithMessage("O 'Descrição' deve ter no máximo 1000 caracteres.");

        RuleFor(x => x.Company)
            .NotNull()
            .MinimumLength(10)
            .WithMessage("A 'Empresa de fabricação' deve ter no mínimo 10 caracteres.")
            .MaximumLength(255)
            .WithMessage("A 'Empresa de fabricação' deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Price)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("O 'preço' deve ser maior que 0 Zero.")
            .PrecisionScale(5, 2, false)
            .WithMessage("O 'Preço' deve conter uam escala de 5 com 2 duas casas decimais.");

        RuleFor(x => x.UserId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("O 'usuđario' deve ser associado ao jogo cadastrado.");
    }
}

public class UpdateGameRequestValidator : AbstractValidator<UpdateGameRequest>
{
    public UpdateGameRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id é inválido.");

        RuleFor(x => x.Name)
            .MinimumLength(20)
            .WithMessage("O 'Nome' deve ter no mínimo 20 caracteres.")
            .MaximumLength(255)
            .WithMessage("O 'Nome' deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Description)
            .MinimumLength(20)
            .WithMessage("O 'Descrição' deve ter no mínimo 20 caracteres.")
            .MaximumLength(1000)
            .WithMessage("O 'Descrição' deve ter no máximo 1000 caracteres.");

        RuleFor(x => x.Company)
            .NotNull()
            .MinimumLength(10)
            .WithMessage("A 'Empresa de fabricação' deve ter no mínimo 10 caracteres.")
            .MaximumLength(255)
            .WithMessage("A 'Empresa de fabricação' deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Price)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("O 'preço' deve ser maior que 0 Zero.")
            .PrecisionScale(5, 2, false)
            .WithMessage("O 'Preço' deve conter uam escala de 5 com 2 duas casas decimais.");

        RuleFor(x => x.UserId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("O 'usuđario' deve ser associado ao jogo cadastrado.");
    }
}