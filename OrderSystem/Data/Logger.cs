using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Enums;

namespace OrderSystem.Data
{
    /// <summary>
    /// Logger class to log everything
    /// </summary>
    public class Logger
    {
        private static Logger _instance;
        private readonly StreamWriter writer;
        private readonly LogCreator creator;

        private Logger()
        {
            writer = new StreamWriter(Configuration.Instance.LogFile, true);
            creator = new LogCreator();
        }

        /// <summary>
        /// Short version to log error messages
        /// </summary>
        /// <param name="message"></param>
        public static void E(string message)
        {
            Instance.Log(message, LogType.Error);
        }
    
        /// <summary>
        /// Short version to log error exceptions
        /// </summary>
        /// <param name="exception"></param>
        public static void E(Exception exception)
        {
            Instance.Log(exception, LogType.Error);
        }

        /// <summary>
        /// Short version to log warning messages
        /// </summary>
        /// <param name="message"></param>
        public static void W(string message)
        {
            Instance.Log(message, LogType.Warning);
        }

        /// <summary>
        /// Short version to log warning exceptions
        /// </summary>
        /// <param name="exception"></param>
        public static void W(Exception exception)
        {
            Instance.Log(exception, LogType.Warning);
        }

        /// <summary>
        /// Short version to log info messages
        /// </summary>
        /// <param name="message"></param>
        public static void I(string message)
        {
            Instance.Log(message, LogType.Info);
        }

        /// <summary>
        /// Short version to log info exceptions
        /// </summary>
        /// <param name="exception"></param>
        public static void I(Exception exception)
        {
            Instance.Log(exception, LogType.Info);
        }

        /// <summary>
        /// Short version to log debug messages
        /// </summary>
        /// <param name="message">The message</param>
        public static void D(string message)
        {
            Instance.Log(message, LogType.Debug);
        }

        /// <summary>
        /// Short version to log debug exceptions
        /// </summary>
        /// <param name="exception">THe exception</param>
        public static void D(Exception exception)
        {
            Instance.Log(exception, LogType.Debug);
        }

        /// <summary>
        /// Short version to log trace messages
        /// </summary>
        /// <param name="message"></param>
        public static void T(string message)
        {
            Instance.Log(message, LogType.Trace);
        }

        /// <summary>
        /// Short version to log trace exceptions
        /// </summary>
        /// <param name="exception"></param>
        public static void T(Exception exception)
        {
            Instance.Log(exception, LogType.Trace);
        }

        /// <summary>
        /// Main Method to log messages
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="type">The type of the log</param>
        public void Log(string message, LogType type)
        {
            Write(creator.ConstructLoggingLine(message, type));
        }

        /// <summary>
        /// Main Method to log excpetions
        /// </summary>
        /// <param name="exception">The exception</param>
        /// <param name="type">The type of the log</param>
        public void Log(Exception exception, LogType type)
        {
            Write(creator.ConstructLoggingLine(creator.GetLoggingMessage(exception, type), type));
        }
        
        /// <summary>
        /// Writes the logging line to the file
        /// </summary>
        /// <param name="line">The line to write</param>
        private void Write(string line)
        {
            Console.WriteLine(line);
            writer.WriteLine(line);
            writer.Flush();
        }

        /// <summary>
        /// Singleton Instance
        /// </summary>
        public static Logger Instance
        {
            get
            {
                if(_instance == null) _instance = new Logger();
                return _instance;
            }
        }
    }
}
