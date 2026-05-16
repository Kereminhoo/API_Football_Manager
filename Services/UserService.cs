using Npgsql;
using FootManager.Models;
using System.Data;
using Microsoft.AspNetCore.Identity; 

namespace FootManager.Services;

public class UserService
{
    private readonly NpgsqlConnection _connection;
    private readonly PasswordHasher<string> _passwordHasher = new();

    public UserService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    
    public User? ValidateUser(string email, string password)
    {
        if (_connection.State != ConnectionState.Open) _connection.Open();

        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT id, email, password_hash, role FROM users WHERE email = @email";
        cmd.Parameters.AddWithValue("email", email);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            var user = new User
            {
                Id = reader.GetInt32(0),
                Email = reader.GetString(1),
                PasswordHash = reader.GetString(2),
                Role = reader.GetString(3)
            };

            
            var result = _passwordHasher.VerifyHashedPassword(user.Email, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Success)
            {
                return user;
            }
        }
        return null;
    }

    
    public string HashPassword(string email, string password)
    {
        return _passwordHasher.HashPassword(email, password);
    }
}