using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumManager
{
    public class MessageHandler
    {
        public static void ExceptionMessage(Exception err)
        {
            AutomationStart.ConsoleWriteLineWithColor(ConsoleColor.Black, ConsoleColor.Red, "--Não foi possível executar o comando: " + err.Message +
                              " - StackTrace: " + err.StackTrace +
                              " - InnerException: " + err.InnerException +
                              " - Message: " + err.Source);
            throw new Exception(err.Message);
        }
    }
}
