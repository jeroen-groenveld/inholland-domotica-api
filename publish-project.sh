#!/bin/bash

project="./C# Solution - Good/C# Solution.sln"
publish_dir="C:\inetpub\wwwroot\domotica.j20.nl"

#stop the website in iis
appcmd stop site domotica.j20.nl

#publish project
dotnet publish "$project" --output "$publish_dir"

#start the website in iis
appcmd start site domotica.j20.nl

echo "Project has been published."