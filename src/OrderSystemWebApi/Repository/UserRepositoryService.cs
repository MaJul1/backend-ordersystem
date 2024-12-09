using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderSystemWebApi.DTO;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Mapper;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Repository;

public class UserRepositoryService : IUserRepositoryService
{
    private UserManager<User> _userManager;
    private SignInManager<User> _signInManager;
    private ILogger<UserRepositoryService> _logger;
    public UserRepositoryService (UserManager<User> userManager, SignInManager<User> signInManager, ILogger<UserRepositoryService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<IdentityResult> DeleteUser(Guid Id)
    {
        var user = await _userManager.FindByIdAsync(Id.ToString());

        if (user == null)
            return IdentityResult.Failed(new IdentityError {Description = "User not found"});

        return await _userManager.DeleteAsync(user);
    }

    public async Task<User?> LogInUserAsync(LogInRequest request)
    {

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            _logger.LogDebug("User cannot be found using the email '{Email}'", request.Email);
            return null;
        }
        
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (result.Succeeded == false)
        {
            _logger.LogDebug("Incorrect Password is being typed");
            return null;
        }

        _logger.LogDebug("User LogIn Successful with an id of {id}", user.Id);
        return user;
    }

    public Task<IdentityResult> RegisterModeratorAsync(WriteUserRequestDTO request)
    {
        var result = Register(request, "Moderator");
        return result;
    }

    public async Task<IdentityResult> UpdateUserInformation(Guid id, WriteUserRequestDTO request)
    {
        var updatedUser = request.ToUser(id);

        return await _userManager.UpdateAsync(updatedUser);
    }

    public Task<IdentityResult> RegisterUserAsync(WriteUserRequestDTO request)
    {
        var result = Register(request, "User");
        return result;
    }

    private async Task<IdentityResult> Register(WriteUserRequestDTO request, string role)
    {
        var user = request.ToUser();

        var resultCreation = await _userManager.CreateAsync(user, request.Password);
        if (resultCreation.Succeeded == false)
            return IdentityResult.Failed([.. resultCreation.Errors]);

        var resultRole = await _userManager.AddToRoleAsync(user, role);
        if (resultRole.Succeeded == false)
        {
            await _userManager.DeleteAsync(user);
            return IdentityResult.Failed([.. resultRole.Errors]);
        }

        return IdentityResult.Success;
    }

    public async Task<List<string>> GetRoles(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return [.. roles];
    }

    private void DetachUser(User user)
    {
        var store = _userManager as IUserStore<User>;
        if (store is UserStore<User> userStore)
        {
            var context = userStore.Context;
            if (context != null)
            {
                context.Entry(user).State = EntityState.Detached;
            }
        }
    }
}
