using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Java.Lang;

namespace BiometricApp.Droid
{
    public class CustomAuthCallback : AndroidX.Biometric.BiometricPrompt.AuthenticationCallback
    {
        private TaskCompletionSource<bool> _taskCompletionSource;

        public CustomAuthCallback()
        {
            _taskCompletionSource = new TaskCompletionSource<bool>();
        }

        public Task<bool> GetTask()
        {
            return _taskCompletionSource.Task;
        }

        private void SetAuthResult(bool result)
        {
            if (!(_taskCompletionSource.Task.IsCanceled || _taskCompletionSource.Task.IsCompleted || _taskCompletionSource.Task.IsFaulted))
            {
                _taskCompletionSource.SetResult(result);
            }
        }

        public override void OnAuthenticationError(int errorCode, ICharSequence errString)
        {
            base.OnAuthenticationError(errorCode, errString);
            SetAuthResult(false);
        }

        public override void OnAuthenticationSucceeded(AndroidX.Biometric.BiometricPrompt.AuthenticationResult result)
        {
            base.OnAuthenticationSucceeded(result);
            SetAuthResult(true);

        }

        public override void OnAuthenticationFailed()
        {
            base.OnAuthenticationFailed();
            SetAuthResult(false);
        }
    }
}