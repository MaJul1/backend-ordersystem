using Microsoft.AspNetCore.Identity;
using OrderSystemWebApi.DTO.User;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Interfaces;

public interface IUserRepositoryService
{
    Task<IdentityResult> RegisterUserAsync(RegisterUserRequestDTO request);
    Task<IdentityResult> RegisterModeratorAsync(RegisterUserRequestDTO request);
    Task<User?> LogInUserAsync(LogInRequest request);
    Task<IdentityResult> UpdateUserInformation(Guid id, RegisterUserRequestDTO request);
    Task<IdentityResult> DeleteUser(Guid Id);
    Task<List<string>> GetRoles(User user);
}
