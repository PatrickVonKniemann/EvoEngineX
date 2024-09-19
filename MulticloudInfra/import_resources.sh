#!/bin/bash

# Import IAM Role if it exists
if aws iam get-role --role-name ecsTaskExecutionRole &> /dev/null; then
  terraform import aws_iam_role.ecsTaskExecutionRole ecsTaskExecutionRole
fi

# Import ECR Repository if it exists
if aws ecr describe-repositories --repository-names test-api &> /dev/null; then
  terraform import aws_ecr_repository.test_api test-api
fi

# Import Load Balancer if it exists
LB_ARN=$(aws elbv2 describe-load-balancers --names test-api-lb --query "LoadBalancers[0].LoadBalancerArn" --output text)
if [ "$LB_ARN" != "None" ]; then
  terraform import aws_lb.test_api_lb $LB_ARN
fi

# Import Target Group if it exists
TG_ARN=$(aws elbv2 describe-target-groups --names test-api-tg --query "TargetGroups[0].TargetGroupArn" --output text)
if [ "$TG_ARN" != "None" ]; then
  terraform import aws_lb_target_group.test_api_target_group $TG_ARN
fi
