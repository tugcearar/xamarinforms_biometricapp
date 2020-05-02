using System;
using System.Threading.Tasks;
using Android.Widget;
using AndroidX.Biometric;
using AndroidX.Core.Content;
using AndroidX.Fragment.App;
using BiometricApp.Droid;
using BiometricApp.Services;
using Java.Util.Concurrent;
using Xamarin.Forms;

[assembly: Dependency(typeof(BiometricService))]
namespace BiometricApp.Droid
{
    public class BiometricService : IBiometricService
    { 
        private BiometricPrompt biometricPrompt;
        private BiometricPrompt.PromptInfo promptInfo;

        public BiometricManager manager = BiometricManager.From(MainActivity.CurrentActivity);


        public bool IsDeviceSupportBiometry()
        {
            switch (manager.CanAuthenticate())
            {
                case BiometricManager.BiometricErrorHwUnavailable:
                    return false;
                case BiometricManager.BiometricErrorNoHardware:
                    return false;
                case BiometricManager.BiometricErrorNoneEnrolled:
                    return false;
                case BiometricManager.BiometricSuccess:
                    return true;
                default:
                    return false;
            }


        }

        public async Task<bool> LoginWithBiometrics()
        {
            if (IsDeviceSupportBiometry())
            {
                var callBack = new MyCallback();
                var executor = Executors.NewSingleThreadExecutor();
                biometricPrompt = new BiometricPrompt(MainActivity.CurrentActivity as FragmentActivity, executor, callBack);
                promptInfo = new BiometricPrompt.PromptInfo.Builder()
                    .SetTitle("Put your finger on screen to be the King!")
                    .SetNegativeButtonText("I don't want it")
                    .Build();
                biometricPrompt.Authenticate(promptInfo);
                return await callBack.GetTask();
            }
            else
                return false;
        }
    }
}
