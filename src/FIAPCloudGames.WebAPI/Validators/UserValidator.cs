using FIAPCloudGames.Application.Requests;
using FluentValidation;

namespace FIAPCloudGames.WebAPI.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FullName)
            .MinimumLength(10)
            .WithMessage("O 'Nome completo' deve ter no mínimo 10 caracteres.")
            .MaximumLength(255)
            .WithMessage("O 'Nome completo' deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Login)
            .NotNull()
            .MinimumLength(10)
            .WithMessage("O 'Login' deve ter no mínimo 10 caracteres.")
            .MaximumLength(20)
            .WithMessage("O 'Login' deve ter no máximo 20 caracteres.");

        RuleFor(x => x.Password)
            .NotNull()
            .MinimumLength(10)
            .WithMessage("O 'Password' deve ter no mínimo 10 caracteres.")
            .MaximumLength(20)
            .WithMessage("O 'Password' deve ter no máximo 20 caracteres.")
            .Matches(@"[A-Z]+")
            .WithMessage("Deve conter caracteres maiúsculos.")
            .Matches(@"[a-z]+")
            .WithMessage("Deve conter caracters minúsculos.")
            .Matches(@"[0-9]+")
            .WithMessage("Deve conter números.")
            .Matches(@"[\@\#\&\!\.]+")
            .WithMessage("Deve conter caracteres especiais (@#&!.).");

        RuleFor(x => x.Email)
            .NotNull()
            .MaximumLength(255)
            .WithMessage("O 'Email' deve ter no máximo 255 caracteres.")
            .EmailAddress();
    }
}

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id é inválido.");

        RuleFor(x => x.FullName)
            .MinimumLength(10)
            .WithMessage("O 'Nome completo' deve ter no mínimo 10 caracteres.")
            .MaximumLength(255)
            .WithMessage("O 'Nome completo' deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Login)
            .NotNull()
            .MinimumLength(10)
            .WithMessage("O 'Login' deve ter no mínimo 10 caracteres.")
            .MaximumLength(20)
            .WithMessage("O 'Login' deve ter no máximo 20 caracteres.");

        RuleFor(x => x.Password)
            .NotNull()
            .MinimumLength(10)
            .WithMessage("O 'Password' deve ter no mínimo 10 caracteres.")
            .MaximumLength(20)
            .WithMessage("O 'Password' deve ter no máximo 20 caracteres.")
            .Matches(@"[A-Z]+")
            .WithMessage("Deve conter caracteres maiúsculos.")
            .Matches(@"[a-z]+")
            .WithMessage("Deve conter caracters minúsculos.")
            .Matches(@"[0-9]+")
            .WithMessage("Deve conter números.")
            .Matches(@"[\@\#\&\!\.]+")
            .WithMessage("Deve conter caracteres especiais (@#&!.).");

        RuleFor(x => x.Email)
            .NotNull()
            .MaximumLength(255)
            .WithMessage("O 'Email' deve ter no máximo 255 caracteres.")
            .EmailAddress();
    }
}