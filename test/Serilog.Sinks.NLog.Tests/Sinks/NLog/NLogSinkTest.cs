using NLog;
using NLog.Config;
using NLog.Targets;
using Serilog.Core;
using System.Linq;
using Xunit;

namespace Serilog.Sinks.NLog.Tests.Sinks.NLog
{
    public class NLogSinkTest
    {
        [Fact]
        public void LogsAreSentToNLog()
        {
            // Fixture setup
            SetupNLogConfiguration();
            // Exercise system
            var log = new LoggerConfiguration()
                .WriteTo.NLog()
                .CreateLogger();
            log.Warning("The quick brown fox jumps over the lazy dog");
            // Verify outcome
            var result = GetLastLoggedLineFromNLogMemotyTarget();
            Assert.Equal("Default|Warn|The quick brown fox jumps over the lazy dog", result);
            // Teardown 
            TeardownNLogConfiguration();
        }

        [Fact]
        public void SourceContextIsUsedForNLogLogName()
        {
            // Fixture setup
            SetupNLogConfiguration();
            // Exercise system
            var log = new LoggerConfiguration()
                .Enrich.WithProperty(Constants.SourceContextPropertyName, "Test")
                .MinimumLevel.Verbose()
                .WriteTo.NLog()
                .CreateLogger();
            log.Debug("The quick brown fox jumps over the lazy dog");
            // Verify outcome
            var result = GetLastLoggedLineFromNLogMemotyTarget();
            Assert.Equal("Test|Debug|The quick brown fox jumps over the lazy dog", result);
            // Teardown 
            TeardownNLogConfiguration();
        }

        [Fact]
        public void TestOutputTemplate()
        {
            // Fixture setup
            SetupNLogConfiguration();
            // Exercise system
            var log = new LoggerConfiguration()
                .WriteTo.NLog(outputTemplate: "[{Level}] - {Message}")
                .CreateLogger();
            log.Information("The quick brown fox jumps over the lazy dog");
            // Verify outcome
            var result = GetLastLoggedLineFromNLogMemotyTarget();
            Assert.Equal("Default|Info|[Information] - The quick brown fox jumps over the lazy dog", result);
            // Teardown
            TeardownNLogConfiguration();
        }

        private const string MemoryTargetName = "memory";
        private const string DefaultNLogLayout = @"${logger}|${level}|${message}";

        private void SetupNLogConfiguration()
        {
            var config = new LoggingConfiguration();
            var memoryTarget = new MemoryTarget();
            config.AddTarget(MemoryTargetName, memoryTarget);
            memoryTarget.Layout = DefaultNLogLayout;
            var rule = new LoggingRule("*", LogLevel.Trace, memoryTarget);
            config.LoggingRules.Add(rule);
            LogManager.Configuration = config;
        }

        private void TeardownNLogConfiguration()
        {
            LogManager.Configuration = null;
        }

        private string GetLastLoggedLineFromNLogMemotyTarget()
        {
            var target = (MemoryTarget)LogManager.Configuration.FindTargetByName(MemoryTargetName);
            return target.Logs.Last();
        }
    }
}
