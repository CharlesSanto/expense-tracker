using ExpenseTracker.Data.Models;
using ExpenseTracker.DTOs.Auth;
using ExpenseTracker.DTOs.UserDTOs;
using ExpenseTracker.Repositories.Interfaces;
using ExpenseTracker.Security;
using ExpenseTracker.Services;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Options;
using Xunit;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();

        var options = Options.Create(new ExpenseTracker.Configurations.JwtOptions
        {
            Key = "super_secret_key_12345678901234567890",
            Issuer = "test_issuer",
            Audience = "test_audience"
        });

        _authService = new AuthService(_userRepositoryMock.Object, options);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        var password = "123456";
        var user = new User
        {
            Id = 1,
            Email = "test@email.com",
            PasswordHash = PasswordHasher.HashPassword(password)
        };

        _userRepositoryMock
            .Setup(r => r.GetUserByEmailAsync(user.Email))
            .ReturnsAsync(user);

        var loginDto = new LoginDto
        {
            Email = user.Email,
            Password = password
        };

        var result = await _authService.LoginAsync(loginDto);

        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrEmpty();
        result.User.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        var loginDto = new LoginDto
        {
            Email = "fake@email.com",
            Password = "123456"
        };

        _userRepositoryMock
            .Setup(r => r.GetUserByEmailAsync(loginDto.Email))
            .ReturnsAsync((User)null);

        var result = await _authService.LoginAsync(loginDto);

        result.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnNull_WhenPasswordIsInvalid()
    {
        var user = new User
        {
            Id = 1,
            Email = "test@email.com",
            PasswordHash = PasswordHasher.HashPassword("correct_password")
        };

        _userRepositoryMock
            .Setup(r => r.GetUserByEmailAsync(user.Email))
            .ReturnsAsync(user);

        var loginDto = new LoginDto
        {
            Email = user.Email,
            Password = "wrong_password"
        };

        var result = await _authService.LoginAsync(loginDto);

        result.Should().BeNull();
    }

    [Fact]
    public void GenerateJwtToken_ShouldReturnToken()
    {
        var user = new User
        {
            Id = 1,
            Name = "Test User",
            Email = "test@email.com"
        };

        var userDto = new UserResponseDto(user);

        var token = _authService.GenerateJwtToken(userDto);

        token.Should().NotBeNullOrEmpty();
    }
}