using System.Data.Common;
using System.Text;
using DomainEntities;
using Generics.BaseEntities;
using Generics.Enums;
using Generics.Pagination;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CodeRunService.Infrastructure.Database;

public class CodeRunRepository(
    CodeRunDbContext context,
    ILogger<CodeRunRepository> logger,
    IMongoDatabase mongoDatabase)
    : BaseRepository<CodeRun>(context, logger), ICodeRunRepository
{
    public async Task<List<CodeRun>> GetAllByCodeBaseIdAsync(Guid codeBaseId)
    {
        return await GetAllByParameterAsync("CodeBaseId", codeBaseId);
    }

    public async Task<List<CodeRun>> GetAllByCodeBaseIdAsync(Guid codeBaseId, PaginationQuery paginationQuery)
    {
        return await GetAllByParameterAsync("CodeBaseId", codeBaseId, paginationQuery);
    }

    public async Task<int> GetCountByCodeBaseId(Guid codeBaseId)
    {
        return await GetCountByParameterAsync("CodeBaseId", codeBaseId);
    }

    public async Task<RunResult> ReadLogsFromDatabaseAsync(Guid codeRunId)
    {
        var collection = mongoDatabase.GetCollection<BsonDocument>("ExecutionLogs");

        var filter = Builders<BsonDocument>.Filter.Eq("CodeRunId", codeRunId.ToString());
        logger.LogInformation($"Using filter: {filter.ToJson()}");
        var result = await collection.Find(filter).FirstOrDefaultAsync();
        logger.LogInformation($"Result: {result?.ToJson()}");
        
        if (result != null)
        {
            // Extract the "Parameters" field as a BsonDocument
            var objectId = result["_id"].AsObjectId;
            string parameters = result["Parameters"].AsBsonDocument.ToString();
            var data = result["Data"].AsString;
            return new RunResult
            {
                Id = Guid.Empty,
                ObjectRefId = objectId,
                File = ConvertToCsv(parameters, data)
            };
        }

        logger.LogWarning("No logs found in the database.");

        return new RunResult
        {
            Id = Guid.Empty,
            ObjectRefId = new ObjectId(),
            File = []
        };
    }

    private byte[] ConvertToCsv(string parameters, string data)
    {
        var csvBuilder = new StringBuilder();

        // Add headers
        csvBuilder.AppendLine("Key,Value");

        // Handle parameters string as JSON (if applicable)
        try
        {
            var parameterDict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(parameters);
            foreach (var element in parameterDict)
            {
                csvBuilder.AppendLine($"{element.Key},{element.Value}");
            }
        }
        catch (Exception ex)
        {
            // Handle if parameters isn't in a JSON format
            csvBuilder.AppendLine($"Parameters,{parameters}");
        }

        // Handle data string as JSON (if applicable)
        try
        {
            var dataDict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            foreach (var element in dataDict)
            {
                csvBuilder.AppendLine($"{element.Key},{element.Value}");
            }
        }
        catch (Exception ex)
        {
            // Handle if data isn't in a JSON format
            csvBuilder.AppendLine($"Data,{data}");
        }

        // Convert CSV content to byte array
        byte[] csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());

        return csvBytes;
    }
}