# Introduction

This is a sample code to debug async tasks using Microsoft Coyotee tool.

In order to run this:

- install Coyotee globally
```
dotnet tool install --global Microsoft.Coyote.CLI
```

- dotnet build
- run 

````
coyote test ./bin/Debug/netcoreapp2.0/coyote-sample.dll

````


- You will see following output in the prompt.

```
..... Writing ./bin/Debug/netcoreapp2.0/Output/coyote-sample.dll/CoyoteOutput/coyote-sample_0_0.txt
..... Writing ./bin/Debug/netcoreapp2.0/Output/coyote-sample.dll/CoyoteOutput/coyote-sample_0_0.schedule
```

open the files that will contain the stacktrace information!

For more information visit https://microsoft.github.io/coyote/