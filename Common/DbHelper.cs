using Microsoft.Extensions.Logging;
using Npgsql;

namespace Common
{
    public static class DbHelper
    {
        public static async Task RunSeedSqlFileAsync(ILogger logger, string? connectionString, List<string> fileNames)
        {
            foreach (var fileName in fileNames)
            {
                try
                {
                    logger.LogInformation("Seeding data for {FileName}", fileName);
                    string sql = await File.ReadAllTextAsync($"/app/SqlScripts/{fileName}.sql");

                    await using (var connection = new NpgsqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        await using (var command = new NpgsqlCommand(sql, connection))
                        {
                            await command.ExecuteNonQueryAsync();
                        }
                    }

                    logger.LogInformation("Seeding was successful");
                }
                catch (Exception e)
                {
                    logger.LogError("Seeding error for {FileName}, {EMessage}", fileName, e.Message);
                }
            }
        }
    }
}