variable "ecr_repository_name" {
  description = "The name of the ECR repository"
  default     = "simpledotnetapi"
}

variable "ecs_cluster_name" {
  description = "The name of the ECS cluster"
  default     = "simpledotnetapi-cluster"
}

variable "iam_role_name" {
  description = "The name of the IAM role"
  default     = "ecsTaskExecutionRole"
}

variable "service_group_name" {
  description = "The name of the security group for the ECS service"
  default     = "ecs-service-sg"
}
