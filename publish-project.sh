#!/bin/bash

project="C:\GIT\InHolland\robotica-domotica-webdashboard\C# Solution - Good\C# Solution.sln"
publish_dir="C:\Users\Jeroen\Desktop\test"
dotnet publish "$project" --output "$publish_dir"
echo "Project has been published."