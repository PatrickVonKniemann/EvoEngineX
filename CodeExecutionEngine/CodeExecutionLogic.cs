using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace CodeExecutionService;

// Inject the MongoDB database instance through the constructor
public class CodeExecutionLogic(IMongoDatabase mongoDatabase) : ICodeExecutionLogic
{

    public async Task ExecuteAsync(string code)
    {
        code = @"
        using System;
        public class MyConsoleApp
        {
            public static void Run()
            {
                Console.WriteLine(""Hello from dynamic code!"");
                Console.WriteLine(""Another line of output!"");
            }
        }
        MyConsoleApp.Run();
        ";

        // Capture Console output
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter); // Redirect Console output to StringWriter

        try
        {
            // Execute the dynamic code
            await CSharpScript.RunAsync(code);

            // Capture the output written to the Console
            string consoleOutput = stringWriter.ToString();
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput())
                { AutoFlush = true }); // Reset to default Console output

            // Write captured output to the database
            await WriteOutputToDatabaseAsync(consoleOutput);
        }
        catch (CompilationErrorException e)
        {
            Console.WriteLine("Compilation errors: " + string.Join(Environment.NewLine, e.Diagnostics));
        }
    }

    // Function to write captured output to the MongoDB database
    private async Task WriteOutputToDatabaseAsync(string consoleOutput)
    {
        var collection = mongoDatabase.GetCollection<BsonDocument>("ExecutionLogs");

        // Create a BSON document to insert
        var logDocument = new BsonDocument
        {
            { "Timestamp", DateTime.UtcNow },
            { "LogMessage", consoleOutput }
        };

        // Insert the document into the MongoDB collection
        await collection.InsertOneAsync(logDocument);
        Console.WriteLine("Log written to MongoDB successfully.");
    }
}
