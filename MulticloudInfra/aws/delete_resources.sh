#!/bin/bash

ECS_CLUSTER_NAME=$1
ECR_REPOSITORY_NAME=$2
IAM_ROLE_NAME=$3
LOAD_BALANCER_NAME=$4
TARGET_GROUP_NAME=$5


# Delete ECR repository
echo "Deleting ECR repository..."
aws ecr delete-repository --repository-name "$ECR_REPOSITORY_NAME" --force || true


echo "Deletion script completed successfully."
