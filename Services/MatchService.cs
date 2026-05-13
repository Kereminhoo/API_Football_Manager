using Npgsql;
using FootManager.Models;
using System.Data;

namespace FootManager.Services;

public class MatchService
{
    private readonly NpgsqlConnection _connection;

    public MatchService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public List<Match> GetAll()
    {
        var matchs = new List<Match>();
        if (_connection.State != ConnectionState.Open) _connection.Open();

        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            SELECT m.id, m.date_match, m.score_domicile, m.score_exterieur, 
                   ed.nom as nom_dom, ee.nom as nom_ext
            FROM matchs m
            JOIN equipes ed ON m.equipe_domicile_id = ed.id
            JOIN equipes ee ON m.equipe_exterieur_id = ee.id
            ORDER BY m.date_match DESC";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            matchs.Add(new Match
            {
                Id = reader.GetInt32(0),
                DateMatch = reader.GetDateTime(1),
                ScoreDomicile = reader.GetInt32(2),
                ScoreExterieur = reader.GetInt32(3),
                NomEquipeDomicile = reader.GetString(4),
                NomEquipeExterieur = reader.GetString(5)
            });
        }
        return matchs;
    }

    public void Add(Match m)
    {
        if (_connection.State != ConnectionState.Open) _connection.Open();

        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO matchs (date_match, equipe_domicile_id, equipe_exterieur_id, score_domicile, score_exterieur) 
            VALUES (@date, @dom, @ext, @sdom, @sext)";
        
        cmd.Parameters.AddWithValue("date", m.DateMatch);
        cmd.Parameters.AddWithValue("dom", m.EquipeDomicileId);
        cmd.Parameters.AddWithValue("ext", m.EquipeExterieurId);
        cmd.Parameters.AddWithValue("sdom", m.ScoreDomicile);
        cmd.Parameters.AddWithValue("sext", m.ScoreExterieur);
        
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        if (_connection.State != ConnectionState.Open) _connection.Open();

        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "DELETE FROM matchs WHERE id = @id";
        cmd.Parameters.AddWithValue("id", id);
    
        cmd.ExecuteNonQuery();
    }
}