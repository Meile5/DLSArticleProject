using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SubscriberService.AppOptionsPattern;
using SubscriberService.Entities;

namespace SubscriberService.Database;

public class SubscriberDatabase : ISubscriberRepository
{
    private readonly string _connectionString;

    public SubscriberDatabase(IOptions<AppOptions> options)
    {
        _connectionString = options.Value.DbConnectionString;
    }

    public async Task<Subscriber> CreateAsync(Subscriber subscriber)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Subscribers (SubscriberId, Email, SubscribedAt, IsActive)
            VALUES (@SubscriberId, @Email, @SubscribedAt, @IsActive)";

        cmd.Parameters.AddWithValue("@SubscriberId", subscriber.SubscriberId);
        cmd.Parameters.AddWithValue("@Email", subscriber.Email);
        cmd.Parameters.AddWithValue("@SubscribedAt", subscriber.SubscribedAt);
        cmd.Parameters.AddWithValue("@IsActive", subscriber.IsActive);

        await cmd.ExecuteNonQueryAsync();
        return subscriber;
    }

    public async Task<Subscriber?> GetByEmailAsync(string email)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Subscribers WHERE Email = @Email";
        cmd.Parameters.AddWithValue("@Email", email);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Subscriber
            {
                SubscriberId = reader.GetGuid(reader.GetOrdinal("SubscriberId")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                SubscribedAt = reader.GetDateTime(reader.GetOrdinal("SubscribedAt")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }

        return null;
    }

    public async Task<IEnumerable<Subscriber>> GetAllActiveAsync()
    {
        var list = new List<Subscriber>();

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Subscribers WHERE IsActive = 1";

        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new Subscriber
            {
                SubscriberId = reader.GetGuid(reader.GetOrdinal("SubscriberId")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                SubscribedAt = reader.GetDateTime(reader.GetOrdinal("SubscribedAt")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            });
        }

        return list;
    }

    public async Task UnsubscribeAsync(string email)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Subscribers SET IsActive = 0 WHERE Email = @Email";
        cmd.Parameters.AddWithValue("@Email", email);

        await cmd.ExecuteNonQueryAsync();
    }
}