#!/bin/bash

ECS_CLUSTER_NAME=$1
ECR_REPOSITORY_NAME=$2
IAM_ROLE_NAME=$3
LOAD_BALANCER_NAME=$4
TARGET_GROUP_NAME=$5
AWS_REGION=$6
AWS_ACCOUNT_ID=$7
SECURITY_GROUP_NAME=$8  # Add Security Group Name

# Fetch the Load Balancer ARN
LOAD_BALANCER_ARN=$(aws elbv2 describe-load-balancers --names "$LOAD_BALANCER_NAME" --query 'LoadBalancers[0].LoadBalancerArn' --output text)

# Fetch the Target Group ARN
echo "Deleting target group"
TARGET_GROUP_ARN=$(aws elbv2 describe-target-groups --names "$TARGET_GROUP_NAME" --query 'TargetGroups[0].TargetGroupArn' --output text)

# Delete ECS service
echo "Deleting ECS service..."
SERVICE_NAME=$(aws ecs list-services --cluster "$ECS_CLUSTER_NAME" --query 'serviceArns[0]' --output text)
if [ "$SERVICE_NAME" != "None" ]; then
    aws ecs update-service --cluster "$ECS_CLUSTER_NAME" --service "$SERVICE_NAME" --desired-count 0 || true
    aws ecs delete-service --service "$SERVICE_NAME" --cluster "$ECS_CLUSTER_NAME" --force || true
else
    echo "No ECS service found."
fi

# Wait for ECS service to be deleted
echo "Waiting for ECS service to be deleted..."
aws ecs wait services-inactive --cluster "$ECS_CLUSTER_NAME" --services "$SERVICE_NAME" || true

# Delete ECS cluster
echo "Deleting ECS cluster..."
aws ecs delete-cluster --cluster "$ECS_CLUSTER_NAME" || true

# Delete ECR repository
echo "Deleting ECR repository..."
aws ecr delete-repository --repository-name "$ECR_REPOSITORY_NAME" --force || true

# Delete Target Group (delete before the load balancer)
if [ "$TARGET_GROUP_ARN" != "None" ]; then
    echo "Deleting Target Group..."
    aws elbv2 delete-target-group --target-group-arn "$TARGET_GROUP_ARN" || true
else
    echo "Target Group not found."
fi

# Delete Load Balancer
if [ "$LOAD_BALANCER_ARN" != "None" ]; then
    echo "Deleting Load Balancer..."
    aws elbv2 delete-load-balancer --load-balancer-arn "$LOAD_BALANCER_ARN" || true
else
    echo "Load Balancer not found."
fi

# Wait for Load Balancer deletion
echo "Waiting for Load Balancer to be deleted..."
if [ "$LOAD_BALANCER_ARN" != "None" ]; then
    aws elbv2 wait load-balancers-deleted --load-balancer-arns "$LOAD_BALANCER_ARN" || true
fi

# Delete Security Group
echo "Deleting Security Group..."
SECURITY_GROUP_ID=$(aws ec2 describe-security-groups --filters Name=group-name,Values="$SECURITY_GROUP_NAME" --query 'SecurityGroups[0].GroupId' --output text)
if [ "$SECURITY_GROUP_ID" != "None" ]; then
    aws ec2 delete-security-group --group-id "$SECURITY_GROUP_ID" || true
else
    echo "Security Group not found."
fi

# Detach and delete IAM Role policies
echo "Detaching policies from IAM Role..."
POLICIES=$(aws iam list-attached-role-policies --role-name "$IAM_ROLE_NAME" --query 'AttachedPolicies[*].PolicyArn' --output text)
for POLICY_ARN in $POLICIES; do
    echo "Detaching policy $POLICY_ARN from role $IAM_ROLE_NAME"
    aws iam detach-role-policy --role-name "$IAM_ROLE_NAME" --policy-arn "$POLICY_ARN" || true
done

# Delete inline policies attached to IAM Role
echo "Deleting inline policies from IAM Role..."
INLINE_POLICIES=$(aws iam list-role-policies --role-name "$IAM_ROLE_NAME" --query 'PolicyNames' --output text)
for INLINE_POLICY in $INLINE_POLICIES; do
    echo "Deleting inline policy $INLINE_POLICY from role $IAM_ROLE_NAME"
    aws iam delete-role-policy --role-name "$IAM_ROLE_NAME" --policy-name "$INLINE_POLICY" || true
done

# Delete IAM Role
echo "Deleting IAM Role..."
aws iam delete-role --role-name "$IAM_ROLE_NAME" || true

echo "Deletion script completed successfully."
