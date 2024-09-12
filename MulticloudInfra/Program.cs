using Pulumi;
using Pulumi.Aws.S3;
using Pulumi.Aws.Lambda;
using Pulumi.Aws.ApiGateway;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pulumi.Aws.Lambda.Inputs;

return await Pulumi.Deployment.RunAsync(() =>
{
    // Create an AWS resource (S3 Bucket)
    var bucket = new Bucket("my-bucket");

    // Get the version from the environment variable
    var version = System.Environment.GetEnvironmentVariable("VERSION") ?? "1.0.0";

    // ---------- AWS Deployment: Lambda + API Gateway ----------
    
    // AWS Lambda Function (Deploy your API as a Lambda function)
    var lambdaFunction = new Function("apiLambda", new FunctionArgs
    {
        Runtime = "dotnet8", // .NET 8 runtime
        Handler = "index.handler",
        Code = new FileArchive("path-to-your-lambda-code.zip"), // Update with your actual Lambda code path
        Environment = new FunctionEnvironmentArgs
        {
            Variables = { { "VERSION", version } }
        }
    });

    // Create AWS API Gateway
    var api = new RestApi("versionApi", new RestApiArgs
    {
        Name = "Version API",
    });

    var resource = new Pulumi.Aws.ApiGateway.Resource("versionResource", new Pulumi.Aws.ApiGateway.ResourceArgs
    {
        ParentId = api.RootResourceId,
        PathPart = "version",
        RestApi = api.Id
    });

    var method = new Pulumi.Aws.ApiGateway.Method("versionMethod", new Pulumi.Aws.ApiGateway.MethodArgs
    {
        HttpMethod = "GET",
        ResourceId = resource.Id,
        RestApi = api.Id,
        Authorization = "NONE"
    });

    var integration = new Pulumi.Aws.ApiGateway.Integration("versionIntegration", new Pulumi.Aws.ApiGateway.IntegrationArgs
    {
        RestApi = api.Id,
        ResourceId = resource.Id,
        HttpMethod = method.HttpMethod,
        IntegrationHttpMethod = "POST",
        Type = "AWS_PROXY",
        Uri = lambdaFunction.InvokeArn
    });

    // Export the API Gateway Deployment
    var deployment = new Pulumi.Aws.ApiGateway.Deployment("versionDeployment", new Pulumi.Aws.ApiGateway.DeploymentArgs
    {
        RestApi = api.Id
    });

    // Export AWS API URL
    var awsApiEndpoint = Output.Format($"{api.ExecutionArn}/version");

    // ---------- Azure Deployment: App Service ----------

    // Create Azure Resource Group
    var resourceGroup = new ResourceGroup("azure-rg");

    // Create Azure App Service Plan
    var appServicePlan = new AppServicePlan("azure-appservice-plan", new AppServicePlanArgs
    {
        ResourceGroupName = resourceGroup.Name,
        Sku = new SkuDescriptionArgs
        {
            Tier = "Free",
            Size = "F1"
        }
    });

    // Create Azure App Service to deploy API
    var appService = new WebApp("azure-api-service", new WebAppArgs
    {
        ResourceGroupName = resourceGroup.Name,
        ServerFarmId = appServicePlan.Id,
        SiteConfig = new SiteConfigArgs
        {
            LinuxFxVersion = "DOTNETCORE|8.0", // Correct runtime stack for .NET 6 (adjust for .NET 8 if needed)
            AppSettings = 
            {
                new NameValuePairArgs
                {
                    Name = "VERSION",
                    Value = version
                }
            }
        }
    });

    // Export Azure API URL
    var azureApiEndpoint = appService.DefaultHostName.Apply(hostname => $"https://{hostname}/version");

    // Export the name of the bucket, AWS API URL, and Azure API URL
    return new Dictionary<string, object?>
    {
        ["bucketName"] = bucket.Id,
        ["awsApiEndpoint"] = awsApiEndpoint,
        ["azureApiEndpoint"] = azureApiEndpoint
    };
});
