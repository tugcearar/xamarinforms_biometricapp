using System;
using System.Threading.Tasks;
using BiometricApp.iOS;
using BiometricApp.Services;
using Foundation;
using LocalAuthentication;
using Xamarin.Forms;

[assembly: Dependency(typeof(BiometricService))]
namespace BiometricApp.iOS
{
    public class BiometricService : IBiometricService
    {
        LAContext context = new LAContext();

        public async Task<bool> LoginWithBiometrics()
        {
            context.LocalizedCancelTitle = "I don't want it.";
            context.LocalizedFallbackTitle = "Use password.";
            NSError error = new NSError();
            Tuple<bool, NSError> result = new Tuple<bool, NSError>(false, null);

            if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthentication, out error))
            {
                string message = "";
                switch (context.BiometryType)
                {
                    case LABiometryType.TouchId:
                        message= "Put your finger on screen to be the King!";
                        break;
                    case LABiometryType.FaceId:
                        message = "Scan your face to be the King!";
                        break;
                    case LABiometryType.None:
                        message = "Your device not support you to be King!";
                        break;
                    default:
                        message = "We can't sure your worthiness!";
                        break;
                }


                result = await context.EvaluatePolicyAsync(LAPolicy.DeviceOwnerAuthentication, message);
            }


            if (result.Item1 && result.Item2 == null)
                return true;
            else
                return false;
        }

    }
}
