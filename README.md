# Serilog.Sinks.NLog

[![Build status](https://ci.appveyor.com/api/projects/status/5h48rc07j90bhwda/branch/master?svg=true)](https://ci.appveyor.com/project/serilog/serilog-sinks-nlog/branch/master)

Adapts Serilog to write events through existing NLog infrastructure.

**Package** - [Serilog.Sinks.NLog](http://nuget.org/packages/serilog.sinks.nlog)
| **Platforms** - .NET 4.5

You'll need to configure NLog, too. 

```csharp
var log = new LoggerConfiguration()
    .WriteTo.NLog()
    .CreateLogger();
```
