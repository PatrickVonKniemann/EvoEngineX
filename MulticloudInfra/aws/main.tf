provider "aws" {
  region = "us-east-1"
}

resource "aws_ecr_repository" "simpledotnetapi" {
  name = "${var.ecr_repository_name}"  # Matches the repository name from deploy.yml
}

resource "aws_ecs_cluster" "simpledotnetapi_cluster" {
  name = "${var.ecs_cluster_name}"  # Matches ECS cluster name from deploy.yml
}

resource "aws_iam_role" "ecs_task_execution_role" {
  name = "${var.iam_role_name}"  # Matches IAM role name from deploy.yml

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
  family                   = "${var.ecr_repository_name}-task"
  requires_compatibilities = ["FARGATE"]  # Correctly set Fargate compatibility
  network_mode             = "awsvpc"
  cpu                      = "256"
  memory                   = "512"
  execution_role_arn       = aws_iam_role.ecs_task_execution_role.arn

  container_definitions = jsonencode([
    {
      name      = "${var.ecr_repository_name}"
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
  name        = "${var.service_group_name}"  # Matches the service group name from deploy.yml
  description = "Allow inbound traffic for ECS"
  vpc_id      = "vpc-0b40a65925c8d2210"  # Replace with your actual VPC ID

  # Allow HTTP traffic from the public
  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]  # Open to all IP addresses
  }

  # Allow HTTPS traffic if needed
  ingress {
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]  # Open to all IP addresses
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"  # Allow all outbound traffic
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_ecs_service" "simpledotnetapi_service" {
  name            = "${var.ecr_repository_name}-service"
  cluster         = aws_ecs_cluster.simpledotnetapi_cluster.id
  task_definition = aws_ecs_task_definition.simpledotnetapi_task.arn
  desired_count   = 1

  network_configuration {
    subnets          = ["subnet-0a9f456c125753831"]  # Public Subnet ID
    security_groups  = [aws_security_group.ecs_service_sg.id]
    assign_public_ip = true  # Ensure this is true in public subnets
  }

  launch_type = "FARGATE"  # Ensure Fargate is correctly set
}
