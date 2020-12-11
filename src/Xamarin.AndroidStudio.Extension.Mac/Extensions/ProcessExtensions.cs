using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Xamarin.AndroidStudio.Extension.Mac.Extensions
{
    public static class ProcessExtensions
    {
        private const string CannotGetExitCodeFromNonChildProcessMessage = "Cannot get the exit code from a non-child process on Unix";

        public static Task<int> SafeWaitForExitAsync(this Process process)
        {
            var taskCompletionSource = new TaskCompletionSource<int>();
            process.EnableRaisingEvents = true;
            process.Exited += OnProcessExited;

            return taskCompletionSource.Task;

            void OnProcessExited(object sender, EventArgs e)
            {
                process.Exited -= OnProcessExited;
                var exitCode = process.SafeGetExitCode();
                taskCompletionSource.SetResult(exitCode);
            }
        }

        public static int SafeGetExitCode(this Process process)
        {
            try
            {
                return process.ExitCode;
            }
            catch (InvalidOperationException ex) when (ex.Message is CannotGetExitCodeFromNonChildProcessMessage)
            {
                return 0;
            }
        }
    }
}