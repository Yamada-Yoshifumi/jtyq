using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;
using Microsoft.EntityFrameworkCore;

namespace jtyq.Models;

public class OurTeam
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<IFormFile> ProfilePicture { get; set; }

    internal jtyqContext Db { get; set; }

    public OurTeam()
    {
    }

    internal OurTeam(jtyqContext db)
    {
        Db = db;
    }

    public async Task InsertAsync()
    {
        using var cmd = Db.Connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO `OurTeam` (`Name`, `Description`) VALUES (@name, @description);";
        BindParams(cmd);
        await cmd.ExecuteNonQueryAsync();
        Id = (int) cmd.LastInsertedId;
    }

    public async Task UpdateAsync()
    {
        using var cmd = Db.Connection.CreateCommand();
        cmd.CommandText = @"UPDATE `OurTeam` SET `Name` = @name, `Description` = @description WHERE `Id` = @id;";
        BindParams(cmd);
        BindId(cmd);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync()
    {
        using var cmd = Db.Connection.CreateCommand();
        cmd.CommandText = @"DELETE FROM `OurTeam` WHERE `Id` = @id;";
        BindId(cmd);
        await cmd.ExecuteNonQueryAsync();
    }

    private void BindId(MySqlCommand cmd)
    {
        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@id",
            DbType = DbType.Int32,
            Value = Id,
        });
    }

    private void BindParams(MySqlCommand cmd)
    {
        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@name",
            DbType = DbType.String,
            Value = Name,
        });
        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@description",
            DbType = DbType.String,
            Value = Description,
        });
    }

}

public class OurTeamQuery
{
    public jtyqContext Db { get; }

    public OurTeamQuery(jtyqContext db)
    {
        Db = db;
    }

    public async Task<OurTeam> FindOneAsync(int id)
    {
        using var cmd = Db.Connection.CreateCommand();
        cmd.CommandText = @"SELECT `Id`, `Name`, `Description` FROM `OurTeam` WHERE `Id` = @id;";
        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@id",
            DbType = DbType.Int32,
            Value = id,
        });
        var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
        return result.Count > 0 ? result[0] : null;
    }

    public async Task<List<OurTeam>> AllTeamAsync()
    {
        using var cmd = Db.Connection.CreateCommand();
        cmd.CommandText = @"SELECT `Id`, `Name`, `Description` FROM `OurTeam` ORDER BY `Id`;";
        return await ReadAllAsync(await cmd.ExecuteReaderAsync());
    }

    public async Task DeleteAllAsync()
    {
        using var txn = await Db.Connection.BeginTransactionAsync();
        using var cmd = Db.Connection.CreateCommand();
        cmd.CommandText = @"DELETE FROM `OurTeam`";
        await cmd.ExecuteNonQueryAsync();
        await txn.CommitAsync();
    }

    private async Task<List<OurTeam>> ReadAllAsync(DbDataReader reader)
    {
        var team = new List<OurTeam>();
        using (reader)
        {
            while (await reader.ReadAsync())
            {
                var person = new OurTeam(Db)
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description= reader.GetString(2),
                };
                team.Add(person);
            }
        }
        return team;
    }
}