using FIAPCloudGames.Application.Requests;
using FIAPCloudGames.WebAPI.Validators;
using FluentValidation.TestHelper;

namespace FIAPCloudGames.Test.WebAPI.Validators;

public class UserValidatorTests
{
    private readonly CreateUserRequestValidator _createValidator = new();
    private readonly UpdateUserRequestValidator _updateValidator = new();

    // --- CreateUserRequestValidator Tests ---

    [Fact]
    [Trait("Category", "UserValidator")]
    public void CreateUser_Should_Have_Error_When_FullName_Too_Short()
    {
        var model = new CreateUserRequest { FullName = "Short", Login = "ValidLogin1", Password = "ValidPass1!", Email = "user@email.com" };
        var result = _createValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    [Trait("Category", "UserValidator")]
    public void CreateUser_Should_Have_Error_When_FullName_Too_Long()
    {
        var model = new CreateUserRequest { FullName = new string('a', 256), Login = "ValidLogin1", Password = "ValidPass1!", Email = "user@email.com" };
        var result = _createValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    [Trait("Category", "UserValidator")]
    public void CreateUser_Should_Have_Error_When_Login_Is_Null()
    {
        var model = new CreateUserRequest { FullName = "Valid FullName", Login = null, Password = "ValidPass1!", Email = "user@email.com" };
        var result = _createValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Login);
    }

    [Fact]
    [Trait("Category", "UserValidator")]
    public void CreateUser_Should_Have_Error_When_Login_Too_Short()
    {
        var model = new CreateUserRequest { FullName = "Valid FullName", Login = "short", Password = "ValidPass1!", Email = "user@email.com" };
        var result = _createValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Login);
    }

    [Fact]
    [Trait("Category", "UserValidator")]
    public void CreateUser_Should_Have_Error_When_Login_Too_Long()
    {
        var model = new CreateUserRequest { FullName = "Valid FullName", Login = new string('a', 21), Password = "ValidPass1!", Email = "user@email.com" };
        var result = _createValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Login);
    }

    [Fact]
    [Trait("Category", "UserValidator")]
    public void CreateUser_Should_Have_Error_When_Password_Is_Null()
    {
        var model = new CreateUserRequest { FullName = "Valid FullName", Login = "ValidLogin1", Password = null, Email = "user@email.com" };
        var result = _createValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [Trait("Category", "UserValidator")]
    [InlineData("short1A!")]
    [InlineData("NoSpecialChar123")]
    [InlineData("nouppercase1!")]
    [InlineData("NOLOWERCASE1!")]
    [InlineData("NoNumber!@#")]
    [InlineData("NoSpecialChar123A")]
    public void CreateUser_Should_Have_Error_When_Password_Invalid(string password)
    {
        var model = new CreateUserRequest { FullName = "Valid FullName", Login = "ValidLogin1", Password = password, Email = "user@email.com" };
        var result = _createValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    [Trait("Category", "UserValidator")]
    public void CreateUser_Should_Have_Error_When_Email_Is_Null()
    {
        var model = new CreateUserRequest { FullName = "Valid FullName", Login = "ValidLogin1", Password = "ValidPass1!", Email = null };
        var result = _createValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    [Trait("Category", "UserValidator")]
    public void CreateUser_Should_Have_Error_When_Email_Too_Long()
    {
        var model = new CreateUserRequest { FullName = "Valid FullName", Login = "ValidLogin1", Password = "ValidPass1!", Email = new string('a', 250) + "@mail.com" };
        var result = _createValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [Trait("Category", "UserValidator")]
    [InlineData("notanemail")]
    [InlineData("missingatsign.com")]
    [InlineData("missingdomain@")]
    public void CreateUser_Should_Have_Error_When_Email_Invalid(string email)
    {
        var model = new CreateUserRequest { FullName = "Valid FullName", Login = "ValidLogin1", Password = "ValidPass1!", Email = email };
        var result = _createValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    [Trait("Category", "UserValidator")]
    public void CreateUser_Should_Not_Have_Error_When_All_Valid()
    {
        var model = new CreateUserRequest { FullName = "Valid FullName", Login = "ValidLogin1", Password = "ValidPass1!", Email = "user@email.com" };
        var result = _createValidator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }

    // --- UpdateUserRequestValidator Tests ---

    [Fact]
    [Trait("Category", "UserValidator")]
    public void UpdateUser_Should_Have_Error_When_Id_Invalid()
    {
        var model = new UpdateUserRequest { Id = 0, FullName = "Valid FullName", Login = "ValidLogin1", Password = "ValidPass1!", Email = "user@email.com" };
        var result = _updateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    [Trait("Category", "UserValidator")]
    public void UpdateUser_Should_Have_Error_When_FullName_Too_Short()
    {
        var model = new UpdateUserRequest { Id = 1, FullName = "Short", Login = "ValidLogin1", Password = "ValidPass1!", Email = "user@email.com" };
        var result = _updateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    [Trait("Category", "UserValidator")]
    public void UpdateUser_Should_Have_Error_When_FullName_Too_Long()
    {
        var model = new UpdateUserRequest { Id = 1, FullName = new string('a', 256), Login = "ValidLogin1", Password = "ValidPass1!", Email = "user@email.com" };
        var result = _updateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    [Trait("Category", "UserValidator")]
    public void UpdateUser_Should_Have_Error_When_Login_Is_Null()
    {
        var model = new UpdateUserRequest { Id = 1, FullName = "Valid FullName", Login = null, Password = "ValidPass1!", Email = "user@email.com" };
        var result = _updateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Login);
    }

    [Fact]
    [Trait("Category", "UserValidator")]
    public void UpdateUser_Should_Have_Error_When_Login_Too_Short()
    {
        var model = new UpdateUserRequest { Id = 1, FullName = "Valid FullName", Login = "short", Password = "ValidPass1!", Email = "user@email.com" };
        var result = _updateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Login);
    }

    [Fact]
    [Trait("Category", "UserValidator")]
    public void UpdateUser_Should_Have_Error_When_Login_Too_Long()
    {
        var model = new UpdateUserRequest { Id = 1, FullName = "Valid FullName", Login = new string('a', 21), Password = "ValidPass1!", Email = "user@email.com" };
        var result = _updateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Login);
    }

    [Fact]
    [Trait("Category", "UserValidator")]
    public void UpdateUser_Should_Have_Error_When_Password_Is_Null()
    {
        var model = new UpdateUserRequest { Id = 1, FullName = "Valid FullName", Login = "ValidLogin1", Password = null, Email = "user@email.com" };
        var result = _updateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [Trait("Category", "UserValidator")]
    [InlineData("short1A!")]
    [InlineData("NoSpecialChar123")]
    [InlineData("nouppercase1!")]
    [InlineData("NOLOWERCASE1!")]
    [InlineData("NoNumber!@#")]
    [InlineData("NoSpecialChar123A")]
    public void UpdateUser_Should_Have_Error_When_Password_Invalid(string password)
    {
        var model = new UpdateUserRequest { Id = 1, FullName = "Valid FullName", Login = "ValidLogin1", Password = password, Email = "user@email.com" };
        var result = _updateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    [Trait("Category", "UserValidator")]
    public void UpdateUser_Should_Have_Error_When_Email_Is_Null()
    {
        var model = new UpdateUserRequest { Id = 1, FullName = "Valid FullName", Login = "ValidLogin1", Password = "ValidPass1!", Email = null };
        var result = _updateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    [Trait("Category", "UserValidator")]
    public void UpdateUser_Should_Have_Error_When_Email_Too_Long()
    {
        var model = new UpdateUserRequest { Id = 1, FullName = "Valid FullName", Login = "ValidLogin1", Password = "ValidPass1!", Email = new string('a', 250) + "@mail.com" };
        var result = _updateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [Trait("Category", "UserValidator")]
    [InlineData("notanemail")]
    [InlineData("missingatsign.com")]
    [InlineData("missingdomain@")]
    public void UpdateUser_Should_Have_Error_When_Email_Invalid(string email)
    {
        var model = new UpdateUserRequest { Id = 1, FullName = "Valid FullName", Login = "ValidLogin1", Password = "ValidPass1!", Email = email };
        var result = _updateValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    [Trait("Category", "UserValidator")]
    public void UpdateUser_Should_Not_Have_Error_When_All_Valid()
    {
        var model = new UpdateUserRequest { Id = 1, FullName = "Valid FullName", Login = "ValidLogin1", Password = "ValidPass1!", Email = "user@email.com" };
        var result = _updateValidator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
