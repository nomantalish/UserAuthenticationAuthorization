using Microsoft.EntityFrameworkCore;
using UserCreationAPI.DTOs;
using UserCreationAPI.Models;
using UserCreationAPI.Services.Contracts;

namespace UserCreationAPI.Services;

public class UserService(
    UserAPIDBContext context, 
    TokenService tokenService,
    CurrentUser currentUser): IUserService
{
    public async Task<string?> Login(LoginDTO input)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => 
            x.Username == input.Username &&
            x.HashedPassword == MethodHelper.ComputeSHA512Hash(input.Password));

        if (user is null)
        {
            return null;
        }

        var token = tokenService.Generate(user);
        return token;
    }

    public async Task<string> Refresh()
    {
        var user = await context.Users.FirstAsync(x => 
            x.Id == int.Parse(currentUser.Id));

        var token = tokenService.Generate(user);
        return token;
    }

    public async Task<bool> ChangePassword(ChangePasswordDTO input)
    {
        var user = await context.Users.FirstOrDefaultAsync(x =>
            x.Id == int.Parse(currentUser.Id) &&
            x.HashedPassword == MethodHelper.ComputeSHA512Hash(input.CurrentPassword)
            );

        if (user is null)
        {
            return false;
        }

        user.HashedPassword = MethodHelper.ComputeSHA512Hash(input.NewPassword);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<User?> Create(CreateUserDTO input)
    {
        var exists = await context.Users.AnyAsync(x => 
            x.Username == input.Username);

        if (exists)
        {
            return null;
        }

        var user = User.CreateCustomer(input.Username);

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return user;
    }

}
