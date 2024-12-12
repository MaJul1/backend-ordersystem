using Microsoft.AspNetCore.Identity;
using OrderSystemWebApi.DTO.User;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Mapper;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Repository;

public class UserRepositoryService : IUserRepositoryService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UserRepositoryService(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IdentityResult> DeleteUser(Guid Id)
    {
        var user = await _userManager.FindByIdAsync(Id.ToString());

        if (user == null)
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });

        return await _userManager.DeleteAsync(user);
    }

    public async Task<User?> LogInUserAsync(LogInRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null)
            return null;

        var passwordCheckResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (passwordCheckResult.Succeeded == false)
            return null;

        return user;
    }

    public Task<IdentityResult> RegisterModeratorAsync(RegisterUserRequestDTO request)
    {
        var result = Register(request, "Moderator");
        return result;
    }

    public async Task<IdentityResult> UpdateUserInformation(Guid id, RegisterUserRequestDTO request)
    {
        var updatedUser = await _userManager.FindByIdAsync(id.ToString());

        if (updatedUser == null)
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });

        return await _userManager.UpdateAsync(updatedUser);
    }

    public Task<IdentityResult> RegisterUserAsync(RegisterUserRequestDTO request)
    {
        var result = Register(request, "User");
        return result;
    }

    private async Task<IdentityResult> Register(RegisterUserRequestDTO request, string role)
    {
        var user = request.ToUser();

        var resultCreation = await _userManager.CreateAsync(user, request.Password);

        if (resultCreation.Succeeded)
            await _userManager.AddToRoleAsync(user, role);

        return resultCreation;
    }

    public async Task<List<string>> GetRoles(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return [.. roles];
    }

    public async Task<bool> IsUsernameAlreadyExists(string username)
    {
        var result = await _userManager.FindByNameAsync(username);
        return result != null;
    }
}
