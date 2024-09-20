variable "aws_region" {
  type    = string
  default = "us-east-1"
}

variable "ecr_repository_url" {
  description = "The name of the ECR repository"
  type        = string
  default     = "376129885232.dkr.ecr.us-east-1.amazonaws.com/test-api"  # You can set a default value or leave it blank for dynamic input
}

# Variables definition
variable "ECS_CLUSTER_NAME" {
  type = string
}

variable "ECR_REPOSITORY_NAME" {
  type = string
}

variable "IAM_ROLE_NAME" {
  type = string
}

variable "LOAD_BALANCER_NAME" {
  type = string
}

variable "TARGET_GROUP_NAME" {
  type = string
}

# AWS provider configuration
provider "aws" {
  region = var.aws_region
}

# Create an ECS cluster
resource "aws_ecs_cluster" "test_dotnet_api_test_cluster" {
  name = var.ECS_CLUSTER_NAME

  lifecycle {
    create_before_destroy = true
  }
}

# Create an IAM Role for ECS task execution
resource "aws_iam_role" "TaskExecutionDotnetApiTest" {
  name = var.IAM_ROLE_NAME

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Action = "sts:AssumeRole"
      Effect = "Allow"
      Principal = {
        Service = "ecs-tasks.amazonaws.com"
      }
    }]
  })

  lifecycle {
    create_before_destroy = true
  }
}

# Attach the required Amazon ECS Task Execution Role policy
resource "aws_iam_role_policy_attachment" "TaskExecutionDotnetApiTestPolicy" {
  role       = aws_iam_role.TaskExecutionDotnetApiTest.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"

  lifecycle {
    create_before_destroy = true
  }
}

# ECS Task Definition
resource "aws_ecs_task_definition" "test_dotnet_api_test_task" {
  family                   = var.ECS_CLUSTER_NAME
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  execution_role_arn       = aws_iam_role.TaskExecutionDotnetApiTest.arn
  memory                   = "512"
  cpu                      = "256"

  container_definitions = jsonencode([{
    name      = var.ECS_CLUSTER_NAME
    image     = "${var.ECR_REPOSITORY_NAME}:latest"
    essential = true
    portMappings = [{
      containerPort = 80
      hostPort      = 80
      protocol      = "tcp"
    }]
    logConfiguration = {
      logDriver = "awslogs"
      options = {
        "awslogs-group"         = "/ecs/${var.ECS_CLUSTER_NAME}"
        "awslogs-region"        = var.aws_region
        "awslogs-stream-prefix" = "ecs"
      }
    }
  }])

  lifecycle {
    create_before_destroy = true
  }
}

# ECS Service
resource "aws_ecs_service" "test_dotnet_api_test_service" {
  name            = "${var.ECS_CLUSTER_NAME}-service"
  cluster         = aws_ecs_cluster.test_dotnet_api_test_cluster.id
  task_definition = aws_ecs_task_definition.test_dotnet_api_test_task.arn
  desired_count   = 1
  launch_type     = "FARGATE"

  network_configuration {
    subnets         = ["subnet-0a9f456c125753831", "subnet-075e54dfa8cf31286"]
    security_groups = ["sg-097a6a7e63727eb39"]
    assign_public_ip = true
  }

  load_balancer {
    target_group_arn = aws_lb_target_group.test_dotnet_api_test_target_group.arn
    container_name   = var.ECS_CLUSTER_NAME
    container_port   = 80
  }

  depends_on = [aws_lb_listener.test_dotnet_api_test_listener]

  lifecycle {
    create_before_destroy = true
  }
}

# Application Load Balancer
resource "aws_lb" "test_dotnet_api_test_lb" {
  name               = var.LOAD_BALANCER_NAME
  internal           = false
  load_balancer_type = "application"
  security_groups    = ["sg-097a6a7e63727eb39"]
  subnets            = ["subnet-0a9f456c125753831", "subnet-075e54dfa8cf31286"]

  lifecycle {
    create_before_destroy = true
  }
}

# Target Group for the Load Balancer
resource "aws_lb_target_group" "test_dotnet_api_test_target_group" {
  name        = var.TARGET_GROUP_NAME
  port        = 80
  protocol    = "HTTP"
  vpc_id      = "vpc-0b40a65925c8d2210"
  target_type = "ip"

  lifecycle {
    create_before_destroy = true
  }
}

# Listener for the Load Balancer
resource "aws_lb_listener" "test_dotnet_api_test_listener" {
  load_balancer_arn = aws_lb.test_dotnet_api_test_lb.arn
  port              = "80"
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.test_dotnet_api_test_target_group.arn
  }

  lifecycle {
    create_before_destroy = true
  }
}