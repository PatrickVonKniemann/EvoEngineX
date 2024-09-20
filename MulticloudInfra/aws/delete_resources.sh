#!/bin/bash

ECS_CLUSTER_NAME=$1
ECR_REPOSITORY_NAME=$2
IAM_ROLE_NAME=$3
LOAD_BALANCER_NAME=$4
TARGET_GROUP_NAME=$5
AWS_REGION=$6
AWS_ACCOUNT_ID=$7

# Example AWS CLI commands to delete resources
aws ecs delete-service --service "${ECS_CLUSTER_NAME}-service" --cluster "${ECS_CLUSTER_NAME}" --force
aws ecs delete-cluster --cluster "${ECS_CLUSTER_NAME}"
aws ecr delete-repository --repository-name "$ECR_REPOSITORY_NAME" --force
aws elbv2 delete-load-balancer --load-balancer-arn arn:aws:elasticloadbalancing:$AWS_REGION:$AWS_ACCOUNT_ID:loadbalancer/app/$LOAD_BALANCER_NAME/<id>
aws elbv2 delete-target-group --target-group-arn arn:aws:elasticloadbalancing:$AWS_REGION:$AWS_ACCOUNT_ID:targetgroup/$TARGET_GROUP_NAME/<id>
aws iam delete-role --role-name "$IAM_ROLE_NAME"
