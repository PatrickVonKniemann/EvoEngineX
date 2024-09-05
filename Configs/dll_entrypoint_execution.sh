#!/bin/sh

/app/wait-for-it.sh ${RABBITMQ_HOST}:5672 --timeout=60 --strict -- echo "RabbitMQ is up - executing command"
/app/wait-for-it.sh ${MONGO_HOST}:27017 --timeout=60 --strict -- echo "Mongo is up - executing command"

echo "----------executing----------------"
exec dotnet "CodeExecutionEngine.dll"