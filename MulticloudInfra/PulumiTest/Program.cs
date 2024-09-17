using System.Threading.Tasks;
using Pulumi;
using Pulumi.Aws.Ec2;
using Pulumi.Aws.Ec2.Inputs;
using Pulumi.Aws.Ecr;
using Pulumi.Aws.Ecs;
using Pulumi.Aws.Ecs.Inputs;
using Pulumi.Docker;
using Pulumi.Aws.Iam; // Add this for IAM role creation
using Service = Pulumi.Aws.Ecs.Service;
using ServiceArgs = Pulumi.Aws.Ecs.ServiceArgs;

class AwsEcsStack : Stack
{
    public AwsEcsStack()
    {
        // Define the Docker image name
        var imageName = "infrastructuretestcode";

        // Define the environment variable
        var environment = "staging";

        // Create an ECR repository to store the Docker image
        var repo = new Repository("infrastructuretestcode-repo");

        // Build and push the Docker image to ECR
        var awsImage = new Image("MultiCloudAwsImage", new ImageArgs
        {
            Build = new DockerBuild 
            { 
                Context = "../InfrastructureTestCode",  // The build context where your source files are
                Dockerfile = "../InfrastructureTestCode/Dockerfile"  // Path to your Dockerfile
            },
            ImageName = Output.Format($"{repo.RepositoryUrl}:v1.0.0"),
            Registry = new ImageRegistry
            {
                Server = repo.RepositoryUrl,
                Username = repo.RegistryId.Apply(id => id),
                Password = repo.RegistryId.Apply(id => "YOUR_AWS_ACCESS_KEY_SECRET"),
            }
        });



        // Create the IAM Role for ECS Task Execution
        var executionRole = new Role("ecsTaskExecutionRole", new RoleArgs
        {
            AssumeRolePolicy = @"{
                ""Version"": ""2012-10-17"",
                ""Statement"": [
                    {
                        ""Action"": ""sts:AssumeRole"",
                        ""Principal"": {
                            ""Service"": ""ecs-tasks.amazonaws.com""
                        },
                        ""Effect"": ""Allow"",
                        ""Sid"": """"
                    }
                ]
            }"
        });

        // Attach the ECS Task Execution Role policy
        var executionPolicy = new RolePolicyAttachment("ecsTaskExecutionRolePolicy", new RolePolicyAttachmentArgs
        {
            Role = executionRole.Name,
            PolicyArn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
        });

        // Create a VPC for the ECS service
        var vpc = new Vpc("ecsVpc", new VpcArgs
        {
            CidrBlock = "10.0.0.0/16"
        });

        // Create a public subnet within the VPC
        var subnet = new Subnet("ecsSubnet", new SubnetArgs
        {
            VpcId = vpc.Id,
            CidrBlock = "10.0.1.0/24",
            MapPublicIpOnLaunch = true
        });

        // Create a security group for the ECS service
        var securityGroup = new SecurityGroup("ecsSecurityGroup", new SecurityGroupArgs
        {
            VpcId = vpc.Id,
            Ingress = 
            {
                new SecurityGroupIngressArgs
                {
                    Protocol = "tcp",
                    FromPort = 80,
                    ToPort = 80,
                    CidrBlocks = { "0.0.0.0/0" }
                }
            }
        });

        // Create an ECS cluster
        var cluster = new Cluster("ecsCluster");

        // Define the ECS task definition with the created execution role and environment variable
        var taskDefinition = new TaskDefinition("appTask", new TaskDefinitionArgs
        {
            Family = "myAppTaskFamily",
            Cpu = "256",
            Memory = "512",
            NetworkMode = "awsvpc",
            RequiresCompatibilities = { "FARGATE" },
            ExecutionRoleArn = executionRole.Arn,  // Use the IAM role created above
            ContainerDefinitions = awsImage.ImageName.Apply(imageUri => $@"
        [
            {{
                ""name"": ""appContainer"",
                ""image"": ""{imageUri}"",
                ""essential"": true,
                ""portMappings"": [
                    {{
                        ""containerPort"": 80,
                        ""hostPort"": 80
                    }}
                ],
                ""environment"": [
                    {{
                        ""name"": ""ENVIRONMENT"",
                        ""value"": ""{environment}""
                    }}
                ]
            }}
        ]")
        });

        // Create the ECS service
        var ecsService = new Service("appService", new ServiceArgs
        {
            Cluster = cluster.Arn,
            DesiredCount = 1,
            LaunchType = "FARGATE",
            TaskDefinition = taskDefinition.Arn,
            NetworkConfiguration = new ServiceNetworkConfigurationArgs
            {
                AssignPublicIp = true,
                Subnets = { subnet.Id },
                SecurityGroups = { securityGroup.Id }
            }
        });

        // Output the ECS service URL
        this.Url = ecsService.Id.Apply(id => $"http://{ecsService.DesiredCount}.ecs.amazonaws.com");
    }

    [Output]
    public Output<string> Url { get; set; }
}
