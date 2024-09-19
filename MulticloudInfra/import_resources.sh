#!/bin/bash

# Import ECS cluster
terraform import aws_ecs_cluster.test_api_cluster arn:aws:ecs:us-east-1:<AWS_ACCOUNT_ID>:cluster/test-api-cluster

# Import ECR repository
terraform import aws_ecr_repository.test_api arn:aws:ecr:us-east-1:<AWS_ACCOUNT_ID>:repository/test-api

# Import ECS task definition
terraform import aws_ecs_task_definition.test_api_task test-api

# Import ECS service
terraform import aws_ecs_service.test_api_service arn:aws:ecs:us-east-1:<AWS_ACCOUNT_ID>:service/test-api-service

# Import Load Balancer
terraform import aws_lb.test_api_lb arn:aws:elasticloadbalancing:us-east-1:<AWS_ACCOUNT_ID>:loadbalancer/app/test-api-lb/<ID>

# Import Target Group
terraform import aws_lb_target_group.test_api_target_group arn:aws:elasticloadbalancing:us-east-1:<AWS_ACCOUNT_ID>:targetgroup/test-api-tg/<ID>

# Import Listener
terraform import aws_lb_listener.test_api_listener arn:aws:elasticloadbalancing:us-east-1:<AWS_ACCOUNT_ID>:listener/app/test-api-lb/<ID>/<LISTENER_ID>
