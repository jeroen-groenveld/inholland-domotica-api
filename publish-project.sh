#!/bin/bash

project="./ASP.NET Solution/Web API/Web_API.csproj"
publish_dir="C:\inetpub\wwwroot\api.domotica.j20.nl"

#stop the website in iis
appcmd stop site api.domotica.j20.nl

#publish project
dotnet publish "$project" --output "$publish_dir"

#start the website in iis
appcmd start site api.domotica.j20.nl

echo "Project has been published."