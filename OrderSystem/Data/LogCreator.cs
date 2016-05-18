using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Enums;

namespace OrderSystem.Data
{
    public class LogCreator
    {
        /// <summary>
        /// Constructs logging line
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="type">The logging level</param>
        /// <returns>logging line</returns>
        public string ConstructLoggingLine(string message, LogType type)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToLongDateString())
                .Append(" ")
                .Append(DateTime.Now.ToLongTimeString())
                .Append($" [{GetLoggingLevelName(type)}] ")
                .Append(message);

            return sb.ToString();
        }

        /// <summary>
        /// Returns the name of the logging level
        /// </summary>
        /// <param name="type">The logging level</param>
        /// <returns>The name of the logging level</returns>
        public string GetLoggingLevelName(LogType type)
        {
            switch (type)
            {
                case LogType.Debug:
                    return "Debug";
                case LogType.Error:
                    return "Error";
                case LogType.Info:
                    return "Info";
                case LogType.Trace:
                    return "Trace";
                case LogType.Warning:
                    return "Warning";
            }

            return "";
        }

        /// <summary>
        /// Creates logging message as a string for exception
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="type">The logging level</param>
        /// <returns>string representation of the exception depending on the logging level</returns>
        public string GetLoggingMessage(Exception ex, LogType type)
        {
            switch (type)
            {
                case LogType.Info:
                case LogType.Warning:
                case LogType.Debug:
                    return SimpleMessage(ex);
                case LogType.Error:
                case LogType.Trace:
                    return ExtendedMessage(ex);
            }

            return "";
        }

        /// <summary>
        /// Creates simple string representation of the exception
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns>string representation</returns>
        private string SimpleMessage(Exception ex)
        {
            return $"{ex.GetType().Name} --> {ex.Message} (from {ex.Source})";
        }

        /// <summary>
        /// Creates extended string representation of the exception
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns>string representation</returns>
        private string ExtendedMessage(Exception ex)
        {
            return $"{ex.GetType().Name} --> Message='{ex.Message}' Source='{ex.Source}' Help='{ex.HelpLink}' Stack Trace='{ex.StackTrace}'";
        }
    }
}
