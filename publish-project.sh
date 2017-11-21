#!/bin/bash

project="./C# Solution - Good/C# Solution.sln"
publish_dir="C:\inetpub\wwwroot\domotica.j20.nl"
dotnet publish "$project" --output "$publish_dir"
echo "Project has been published."