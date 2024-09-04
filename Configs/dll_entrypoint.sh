#!/bin/sh
if [ -z "$SERVICE_NAME" ]; then
  echo "SERVICE_NAME is not set. Exiting."
  exit 1
fi

/app/wait-for-it.sh ${DB_HOST}:5432 --timeout=60 --strict -- echo "PostgreSQL is up - executing command"
/app/wait-for-it.sh ${RABBITMQ_HOST}:5672 --timeout=60 --strict -- echo "RabbitMQ is up - executing command"
/app/wait-for-it.sh ${MONGO_HOST}:27017 --timeout=60 --strict -- echo "Mongo is up - executing command"


echo "----------executing----------------"
exec dotnet "$SERVICE_NAME.dll"