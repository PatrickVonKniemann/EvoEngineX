#!/bin/bash

ECS_CLUSTER_NAME=$1
ECR_REPOSITORY_NAME=$2
IAM_ROLE_NAME=$3
LOAD_BALANCER_NAME=$4
TARGET_GROUP_NAME=$5
AWS_REGION=$6
AWS_ACCOUNT_ID=$7

# Fetch the Load Balancer ARN
LOAD_BALANCER_ARN=$(aws elbv2 describe-load-balancers --names "$LOAD_BALANCER_NAME" --query 'LoadBalancers[0].LoadBalancerArn' --output text)

# Fetch the Target Group ARN
TARGET_GROUP_ARN=$(aws elbv2 describe-target-groups --names "$TARGET_GROUP_NAME" --query 'TargetGroups[0].TargetGroupArn' --output text)

# Delete ECS service
echo "Deleting ECS service..."
SERVICE_NAME=$(aws ecs list-services --cluster "$ECS_CLUSTER_NAME" --query 'serviceArns[0]' --output text)
if [ "$SERVICE_NAME" != "None" ]; then
    aws ecs update-service --cluster "$ECS_CLUSTER_NAME" --service "$SERVICE_NAME" --desired-count 0
    aws ecs delete-service --service "$SERVICE_NAME" --cluster "$ECS_CLUSTER_NAME" --force
else
    echo "No ECS service found."
fi

# Wait for ECS service to be deleted
echo "Waiting for ECS service to be deleted..."
aws ecs wait services-inactive --cluster "$ECS_CLUSTER_NAME" --services "$SERVICE_NAME"

# Delete ECS cluster
echo "Deleting ECS cluster..."
aws ecs delete-cluster --cluster "$ECS_CLUSTER_NAME"

# Delete ECR repository
echo "Deleting ECR repository..."
aws ecr delete-repository --repository-name "$ECR_REPOSITORY_NAME" --force

# Delete Load Balancer
if [ "$LOAD_BALANCER_ARN" != "None" ]; then
    echo "Deleting Load Balancer..."
    aws elbv2 delete-load-balancer --load-balancer-arn "$LOAD_BALANCER_ARN"
else
    echo "Load Balancer not found."
fi

# Wait for Load Balancer deletion
echo "Waiting for Load Balancer to be deleted..."
aws elbv2 wait load-balancers-deleted --load-balancer-arns "$LOAD_BALANCER_ARN"

# Delete Target Group
if [ "$TARGET_GROUP_ARN" != "None" ]; then
    echo "Deleting Target Group..."
    aws elbv2 delete-target-group --target-group-arn "$TARGET_GROUP_ARN"
else
    echo "Target Group not found."
fi

# Delete IAM Role
echo "Deleting IAM Role..."
aws iam delete-role --role-name "$IAM_ROLE_NAME"
