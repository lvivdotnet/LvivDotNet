#!/bin/bash

dotnet ./LvivDotNet.Tests/output/LvivDotNet.Tests.dll

nginx -g "daemon off;"