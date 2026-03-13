using ExpenseTracker.Data.Models;
using ExpenseTracker.DTOs.Auth;
using ExpenseTracker.DTOs.UserDTOs;
using ExpenseTracker.Repositories.Interfaces;
using ExpenseTracker.Services;
using FluentAssertions;
using Moq;
using Xunit;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        var user = new User
        {
            Id = 1,
            Name = "Charles",
            Email = "test@email.com"
        };

        _userRepositoryMock
            .Setup(r => r.GetUserByIdAsync(user.Id))
            .ReturnsAsync(user);

        var result = await _userService.GetUserByIdAsync(user.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(user.Id);
        result.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        _userRepositoryMock
            .Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((User)null);

        var result = await _userService.GetUserByIdAsync(1);

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateUserAsync_ShouldCreateUser_WhenEmailIsValid()
    {
        var registerDto = new RegisterDto
        {
            Name = "Charles",
            Email = "test@email.com",
            Password = "123456"
        };

        _userRepositoryMock
            .Setup(r => r.GetUserByEmailAsync(registerDto.Email.ToLower()))
            .ReturnsAsync((User)null);

        _userRepositoryMock
            .Setup(r => r.CreateUserAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);

        var result = await _userService.CreateUserAsync(registerDto);

        result.Should().NotBeNull();
        result.Email.Should().Be(registerDto.Email.ToLower());
    }

    [Fact]
    public async Task CreateUserAsync_ShouldThrowException_WhenEmailAlreadyExists()
    {
        var registerDto = new RegisterDto
        {
            Name = "Charles",
            Email = "test@email.com",
            Password = "123456"
        };

        var existingUser = new User
        {
            Id = 1,
            Email = registerDto.Email
        };

        _userRepositoryMock
            .Setup(r => r.GetUserByEmailAsync(registerDto.Email.ToLower()))
            .ReturnsAsync(existingUser);

        Func<Task> action = async () => await _userService.CreateUserAsync(registerDto);

        await action.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldUpdateUser_WhenUserExists()
    {
        var user = new User
        {
            Id = 1,
            Name = "Old Name",
            Email = "old@email.com"
        };

        var updateDto = new UpdateUserDto
        {
            Name = "New Name"
        };

        _userRepositoryMock
            .Setup(r => r.GetUserByIdAsync(user.Id))
            .ReturnsAsync(user);

        var result = await _userService.UpdateUserAsync(user.Id, updateDto);

        result.Should().NotBeNull();
        result.Name.Should().Be("New Name");
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserIsDeleted()
    {
        _userRepositoryMock
            .Setup(r => r.DeleteUserAsync(It.IsAny<int>()))
            .ReturnsAsync(true);

        var result = await _userService.DeleteUserAsync(1);

        result.Should().BeTrue();
    }
}