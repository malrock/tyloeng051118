using NLog;
using NLog.Conditions;
using NLog.Config;
using NLog.Targets;

namespace FaceSort
{
    public class LogService : ILogService
    {
        public LogService()
        {
            if (LogManager.Configuration != null)
            {
                Logger.Info("Configuration present, ignoring built-in defaults.");
                return;
            }

            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget {Name = "ColorConsole"};
            var highlightRule = new ConsoleRowHighlightingRule
            {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Info"),
                ForegroundColor = ConsoleOutputColor.Blue
            };
            consoleTarget.RowHighlightingRules.Add(highlightRule);
            var errorHighlightRule = new ConsoleRowHighlightingRule
            {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Error"),
                ForegroundColor = ConsoleOutputColor.Red
            };
            consoleTarget.RowHighlightingRules.Add(errorHighlightRule);

            config.AddTarget(consoleTarget);
            config.AddRuleForAllLevels(consoleTarget);
            LogManager.Configuration = config;
            Logger.Info("Initialized logger from built-in defaults.");
        }

        public ILogger Logger { get; } = LogManager.GetCurrentClassLogger();
    }
}