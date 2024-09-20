#!/bin/bash

# Set your AWS region and account ID
AWS_REGION="us-east-1"
AWS_ACCOUNT_ID="376129885232"

# Define resource names
ECS_CLUSTER_NAME="test-dotnet-api-test-cluster"
ECR_REPOSITORY_NAME="test-api"
IAM_ROLE_NAME="TaskExecutionDotnetApiTest"
LOAD_BALANCER_NAME="test-dotnet-api-test-lb"
TARGET_GROUP_NAME="test-dotnet-api-test-tg"

# Delete ECS Service
echo "Deleting ECS Service..."
SERVICE_ARN=$(aws ecs list-services --cluster $ECS_CLUSTER_NAME --query 'serviceArns[0]' --output text)
if [ "$SERVICE_ARN" != "None" ]; then
    aws ecs update-service --cluster $ECS_CLUSTER_NAME --service $SERVICE_ARN --desired-count 0
    aws ecs delete-service --cluster $ECS_CLUSTER_NAME --service $SERVICE_ARN --force
fi

# Delete ECS Cluster
echo "Deleting ECS Cluster..."
CLUSTER_ARN=$(aws ecs list-clusters --query "clusterArns[?contains(@, '$ECS_CLUSTER_NAME')]" --output text)
if [ "$CLUSTER_ARN" != "None" ]; then
    aws ecs delete-cluster --cluster $CLUSTER_ARN
fi

# Delete IAM Role and Policy
echo "Deleting IAM Role and Policies..."
POLICY_ARN=$(aws iam list-attached-role-policies --role-name $IAM_ROLE_NAME --query 'AttachedPolicies[0].PolicyArn' --output text)
if [ "$POLICY_ARN" != "None" ]; then
    aws iam detach-role-policy --role-name $IAM_ROLE_NAME --policy-arn $POLICY_ARN
fi
aws iam delete-role --role-name $IAM_ROLE_NAME

# Delete ECR Repository
echo "Deleting ECR Repository..."
REPO_ARN=$(aws ecr describe-repositories --repository-names $ECR_REPOSITORY_NAME --query 'repositories[0].repositoryArn' --output text)
if [ "$REPO_ARN" != "None" ]; then
    aws ecr delete-repository --repository-name $ECR_REPOSITORY_NAME --force
fi

# Delete Load Balancer
echo "Deleting Load Balancer..."
LB_ARN=$(aws elbv2 describe-load-balancers --names $LOAD_BALANCER_NAME --query 'LoadBalancers[0].LoadBalancerArn' --output text)
if [ "$LB_ARN" != "None" ]; then
    # Delete listeners before deleting load balancer
    LISTENER_ARN=$(aws elbv2 describe-listeners --load-balancer-arn $LB_ARN --query 'Listeners[0].ListenerArn' --output text)
    if [ "$LISTENER_ARN" != "None" ]; then
        aws elbv2 delete-listener --listener-arn $LISTENER_ARN
    fi
    aws elbv2 delete-load-balancer --load-balancer-arn $LB_ARN
fi

# Delete Target Group
echo "Deleting Target Group..."
TG_ARN=$(aws elbv2 describe-target-groups --names $TARGET_GROUP_NAME --query 'TargetGroups[0].TargetGroupArn' --output text)
if [ "$TG_ARN" != "None" ]; then
    aws elbv2 delete-target-group --target-group-arn $TG_ARN
fi

echo "All resources deleted."
