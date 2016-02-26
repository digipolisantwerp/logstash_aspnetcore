using System;

namespace Toolbox.Logstash.Options.Internal
{
    public static class Defaults
    {
        public static class Message
        {
            public const LogStashLevel Level = LogStashLevel.Information;
            public const string HeaderVersion = "1";
        }

        public static class ConfigKeys
        {
            public static string AppId = "AppId";
            public static string Url = "Url";
            public static string Index = "Index";
            public static string MinimumLevel = "MinimumLevel";
        }

        public static class Exceptions
        {
            public static class InvalidOptionException
            {
                public static string Message = "Invalid option.";
            }
        }
    }
}
