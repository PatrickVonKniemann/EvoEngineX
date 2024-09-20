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
