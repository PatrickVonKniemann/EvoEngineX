provider "aws" {
  region = "us-east-1"
}

# Create an ECS cluster
resource "aws_ecs_cluster" "test_api_cluster" {
  name = "test-api-cluster"
}

# Create an IAM Role for ECS task execution
resource "aws_iam_role" "ecsTaskExecutionRole" {
  name = "ecsTaskExecutionRole"

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
}

# Attach the required Amazon ECS Task Execution Role policy
resource "aws_iam_role_policy_attachment" "ecsTaskExecutionRolePolicy" {
  role       = aws_iam_role.ecsTaskExecutionRole.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

# Create the ECR Repository
resource "aws_ecr_repository" "test_api" {
  name = "test-api"
  force_delete = true
  lifecycle {
    prevent_destroy = true  # Prevents Terraform from destroying this resource
    ignore_changes  = [name]  # Ignores changes in name, preventing recreation
  }
}

# ECS Task Definition
resource "aws_ecs_task_definition" "test_api_task" {
  family                   = "test-api"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  execution_role_arn       = aws_iam_role.ecsTaskExecutionRole.arn
  memory                   = "512"
  cpu                      = "256"

  container_definitions = jsonencode([{
    name      = "test-api"
    image     = "${aws_ecr_repository.test_api.repository_url}:latest"
    essential = true
    portMappings = [{
      containerPort = 80
      hostPort      = 80
      protocol      = "tcp"
    }]
    logConfiguration = {
      logDriver = "awslogs"
      options = {
        "awslogs-group"         = "/ecs/test-api"
        "awslogs-region"        = "us-east-1"
        "awslogs-stream-prefix" = "ecs"
      }
    }
  }])
}

# ECS Service
resource "aws_ecs_service" "test_api_service" {
  name            = "test-api-service"
  cluster         = aws_ecs_cluster.test_api_cluster.id
  task_definition = aws_ecs_task_definition.test_api_task.arn
  desired_count   = 1
  launch_type     = "FARGATE"

  network_configuration {
    subnets         = ["subnet-0a9f456c125753831", "subnet-075e54dfa8cf31286"]
    security_groups = ["sg-097a6a7e63727eb39"]
    assign_public_ip = true
  }

  load_balancer {
    target_group_arn = aws_lb_target_group.test_api_target_group.arn
    container_name   = "test-api"
    container_port   = 80
  }

  depends_on = [aws_lb_listener.test_api_listener]
}

# Application Load Balancer
resource "aws_lb" "test_api_lb" {
  name               = "test-api-lb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = ["sg-097a6a7e63727eb39"]
  subnets            = ["subnet-0a9f456c125753831", "subnet-075e54dfa8cf31286"]
  lifecycle {
    prevent_destroy = true  # Prevents Terraform from destroying this resource
    ignore_changes  = [name]  # Ignores changes in name, preventing recreation
  }
}

# Target Group for the Load Balancer
resource "aws_lb_target_group" "test_api_target_group" {
  name        = "test-api-tg"
  port        = 80
  protocol    = "HTTP"
  vpc_id      = "vpc-0b40a65925c8d2210"
  target_type = "ip"
  lifecycle {
    prevent_destroy = true  # Prevents Terraform from destroying this resource
    ignore_changes  = [name]  # Ignores changes in name, preventing recreation
  }
}

# Listener for the Load Balancer
resource "aws_lb_listener" "test_api_listener" {
  load_balancer_arn = aws_lb.test_api_lb.arn
  port              = "80"
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.test_api_target_group.arn
  }
}
