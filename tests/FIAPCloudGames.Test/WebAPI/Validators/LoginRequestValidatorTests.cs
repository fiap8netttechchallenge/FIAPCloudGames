using FIAPCloudGames.Application.Requests;
using FIAPCloudGames.WebAPI.Validators;
using FluentValidation.TestHelper;

namespace FIAPCloudGames.Test.WebAPI.Validators;

public class LoginRequestValidatorTests
{
    private readonly LoginRequestValidator _validator = new();

    [Fact]
    [Trait("Category", "LoginRequestValidator")]
    public void Should_Have_Error_When_Password_Is_Null()
    {
        var model = new LoginRequest { Email = "user@email.com", Password = null };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [Trait("Category", "LoginRequestValidator")]
    [InlineData("short1A!")]
    [InlineData("NoSpecialChar123")]
    [InlineData("nouppercase1!")]
    [InlineData("NOLOWERCASE1!")]
    [InlineData("NoNumber!@#")]
    [InlineData("NoSpecialChar123A")]
    public void Should_Have_Error_When_Password_Does_Not_Meet_Requirements(string password)
    {
        var model = new LoginRequest { Email = "user@email.com", Password = password };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [Trait("Category", "LoginRequestValidator")]
    [InlineData("ValidPass1!")]
    [InlineData("Another1@Pass")]
    [InlineData("Complex#Pass12")]
    public void Should_Not_Have_Error_When_Password_Is_Valid(string password)
    {
        var model = new LoginRequest { Email = "user@email.com", Password = password };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    [Trait("Category", "LoginRequestValidator")]
    public void Should_Have_Error_When_Email_Is_Null()
    {
        var model = new LoginRequest { Email = null, Password = "ValidPass1!" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    [Trait("Category", "LoginRequestValidator")]
    public void Should_Have_Error_When_Email_Is_Too_Long()
    {
        var longEmail = new string('a', 250) + "@mail.com";
        var model = new LoginRequest { Email = longEmail, Password = "ValidPass1!" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [Trait("Category", "LoginRequestValidator")]
    [InlineData("notanemail")]
    [InlineData("missingatsign.com")]
    [InlineData("missingdomain@")]
    public void Should_Have_Error_When_Email_Is_Invalid(string email)
    {
        var model = new LoginRequest { Email = email, Password = "ValidPass1!" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    [Trait("Category", "LoginRequestValidator")]
    public void Should_Not_Have_Error_When_Email_Is_Valid()
    {
        var model = new LoginRequest { Email = "user@email.com", Password = "ValidPass1!" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }
}
