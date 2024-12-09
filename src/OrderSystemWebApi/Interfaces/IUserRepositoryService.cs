using System;
using Microsoft.AspNetCore.Identity;
using OrderSystemWebApi.DTO;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Interfaces;

public interface IUserRepositoryService
{
    Task<IdentityResult> RegisterUserAsync(WriteUserRequestDTO request);
    Task<IdentityResult> RegisterModeratorAsync(WriteUserRequestDTO request);
    Task<User?> LogInUserAsync(LogInRequest request);
    Task<IdentityResult> UpdateUserInformation(Guid id, WriteUserRequestDTO request);
    Task<IdentityResult> DeleteUser(Guid Id);
    Task<List<string>> GetRoles(User user);
}
