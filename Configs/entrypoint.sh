#!/bin/sh
if [ -z "$SERVICE_NAME" ]; then
  echo "SERVICE_NAME is not set. Exiting."
  exit 1
fi

exec dotnet "$SERVICE_NAME.dll"
