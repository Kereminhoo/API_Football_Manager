using Npgsql;
using FootManager.Models;
using System.Data;

namespace FootManager.Services;

public class EquipeService
{
    private readonly NpgsqlConnection _connection;

    public EquipeService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public List<Equipe> GetAll()
    {
        var equipes = new List<Equipe>();
        
        if (_connection.State != ConnectionState.Open) _connection.Open();

        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT id, nom, ville FROM equipes ORDER BY nom ASC";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            equipes.Add(new Equipe
            {
                Id = reader.GetInt32(0),
                Nom = reader.GetString(1),
                Ville = reader.GetString(2)
            });
        }
        return equipes;
    }
}