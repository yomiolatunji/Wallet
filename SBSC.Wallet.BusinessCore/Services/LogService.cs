using log4net;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SBSC.Wallet.BusinessCore.Services
{
    public class LogService
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogInfo(string message,  [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
        {
            log.Error($"\r\n Executing Operation: {fileName} Method Name: {memberName}  Line Number: {lineNumber} \r\nMessage: {message}\r\n");
        }

        public static void LogError(Exception ex, [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
        {
            log.Error($"\r\nExecuting Operation: {fileName} Method Name: {memberName}  Line Number: {lineNumber} \r\nMessage: {ex}\r\n");
        }
    }
}
