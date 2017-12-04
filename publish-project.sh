#!/bin/bash

project="./inholland-domotica-api.csproj"
publish_dir="C:\inetpub\wwwroot\api.inholland.it"

#stop the website in iis
NET STOP "W3SVC" 

#publish project
dotnet publish "$project" --output "$publish_dir"

#start the website in iis
NET START "W3SVC" 

echo "Project has been published."