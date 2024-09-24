provider "aws" {
  region = "us-east-1"
}

resource "aws_ecr_repository" "simpledotnetapi" {
  name = "simpledotnetapi"
}

resource "aws_ecs_cluster" "simpledotnetapi_cluster" {
  name = "simpledotnetapi-cluster"
}

resource "aws_iam_role" "ecs_task_execution_role" {
  name = "ecsTaskExecutionRole"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Principal = {
          Service = "ecs-tasks.amazonaws.com"
        }
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "ecs_task_execution_role_policy" {
  role       = aws_iam_role.ecs_task_execution_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

resource "aws_ecs_task_definition" "simpledotnetapi_task" {
  family                   = "simpledotnetapi-task"
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = "256"
  memory                   = "512"
  execution_role_arn       = aws_iam_role.ecs_task_execution_role.arn

  container_definitions = jsonencode([
    {
      name      = "simpledotnetapi"
      image     = "${aws_ecr_repository.simpledotnetapi.repository_url}:latest"
      essential = true
      portMappings = [{
        containerPort = 80
        hostPort      = 80
      }]
    }
  ])
}

resource "aws_security_group" "ecs_service_sg" {
  name        = "ecs-service-sg"
  description = "Allow inbound traffic for ECS"
  vpc_id      = "vpc-0b40a65925c8d2210"  # Replace with your actual VPC ID

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"  # Allow all outbound traffic
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_ecs_service" "simpledotnetapi_service" {
  name            = "simpledotnetapi-service"
  cluster         = aws_ecs_cluster.simpledotnetapi_cluster.id
  task_definition = aws_ecs_task_definition.simpledotnetapi_task.arn
  desired_count   = 1

  network_configuration {
    subnets          = ["subnet-0a9f456c125753831"]  # Replace with your actual Subnet ID
    security_groups  = [aws_security_group.ecs_service_sg.id]
    assign_public_ip = true
  }
}
