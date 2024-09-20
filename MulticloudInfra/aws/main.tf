provider "aws" {
  region = "us-east-1"
}

# Create an ECS cluster
resource "aws_ecs_cluster" "test_api3_cluster" {
  name = "test-api3-cluster"
}

# Create an IAM Role for ECS task execution
resource "aws_iam_role" "TaskExecutionEcs3" {
  name = "TaskExecutionEcs3"

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
resource "aws_iam_role_policy_attachment" "TaskExecutionEcs3Policy" {
  role       = aws_iam_role.TaskExecutionEcs3.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

# ECS Task Definition
resource "aws_ecs_task_definition" "test_api3_task" {
  family                   = "test-api3"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  execution_role_arn       = aws_iam_role.TaskExecutionEcs3.arn
  memory                   = "512"
  cpu                      = "256"

  container_definitions = jsonencode([{
    name      = "test-api3"
    image     = "${var.ecr_repository_url}:latest"
    essential = true
    portMappings = [{
      containerPort = 80
      hostPort      = 80
      protocol      = "tcp"
    }]
    logConfiguration = {
      logDriver = "awslogs"
      options = {
        "awslogs-group"         = "/ecs/test-api3"
        "awslogs-region"        = "us-east-1"
        "awslogs-stream-prefix" = "ecs"
      }
    }
  }])
}

# ECS Service
resource "aws_ecs_service" "test_api3_service" {
  name            = "test-api3-service"
  cluster         = aws_ecs_cluster.test_api3_cluster.id
  task_definition = aws_ecs_task_definition.test_api3_task.arn
  desired_count   = 1
  launch_type     = "FARGATE"

  network_configuration {
    subnets         = ["subnet-0a9f456c125753831", "subnet-075e54dfa8cf31286"]
    security_groups = ["sg-097a6a7e63727eb39"]
    assign_public_ip = true
  }

  load_balancer {
    target_group_arn = aws_lb_target_group.test_api3_target_group.arn
    container_name   = "test-api3"
    container_port   = 80
  }

  depends_on = [aws_lb_listener.test_api3_listener]
}

# Application Load Balancer
resource "aws_lb" "test_api3_lb" {
  name               = "test-api3-lb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = ["sg-097a6a7e63727eb39"]
  subnets            = ["subnet-0a9f456c125753831", "subnet-075e54dfa8cf31286"]
}

# Target Group for the Load Balancer
resource "aws_lb_target_group" "test_api3_target_group" {
  name        = "test-api3-tg"
  port        = 80
  protocol    = "HTTP"
  vpc_id      = "vpc-0b40a65925c8d2210"
  target_type = "ip"
}

# Listener for the Load Balancer
resource "aws_lb_listener" "test_api3_listener" {
  load_balancer_arn = aws_lb.test_api3_lb.arn
  port              = "80"
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.test_api3_target_group.arn
  }
}
