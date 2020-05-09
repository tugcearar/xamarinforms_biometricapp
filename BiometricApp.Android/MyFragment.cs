using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Java.Lang;

namespace BiometricApp.Droid
{
    public class MyFragment : AndroidX.Fragment.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);
        }
    }

    public class MyCallback : AndroidX.Biometric.BiometricPrompt.AuthenticationCallback
    {
        private TaskCompletionSource<bool> _taskCompletionSource;

        public MyCallback()
        {
            _taskCompletionSource = new TaskCompletionSource<bool>();
        }

        public Task<bool> GetTask()
        {
            return _taskCompletionSource.Task;
        }

        private void SetResultSafe(bool result)
        {
            if (!(_taskCompletionSource.Task.IsCanceled || _taskCompletionSource.Task.IsCompleted || _taskCompletionSource.Task.IsFaulted))
            {
                _taskCompletionSource.SetResult(result);
            }
        }

        public override void OnAuthenticationError(int errorCode, ICharSequence errString)
        {
            base.OnAuthenticationError(errorCode, errString);
            var faResult = false;
            SetResultSafe(faResult);
        }

        public override void OnAuthenticationSucceeded(AndroidX.Biometric.BiometricPrompt.AuthenticationResult result)
        {
            base.OnAuthenticationSucceeded(result);
            var faResult = true;
            SetResultSafe(faResult);

        }

        public override void OnAuthenticationFailed()
        {
            base.OnAuthenticationFailed();
            var faResult = false;
            SetResultSafe(faResult);
        }
    }
}