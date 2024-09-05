#!/bin/sh

/app/wait-for-it.sh ${RABBITMQ_HOST}:5672 --timeout=60 --strict -- echo "RabbitMQ is up - executing command"


echo "----------executing----------------"
exec dotnet "CodeFormaterService.dll"