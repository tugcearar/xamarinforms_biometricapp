using System;
using System.Threading.Tasks;
using BiometricApp.iOS;
using BiometricApp.Services;
using Foundation;
using LocalAuthentication;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(BiometricService))]
namespace BiometricApp.iOS
{
    public class BiometricService : IBiometricService
    {
        LAContext context;

        public BiometricService()
        {
            CreateLaContext();
        }
        private void CreateLaContext()
        {
            var info = new NSProcessInfo();

            if (!UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                return;

            if (Class.GetHandle(typeof(LAContext)) == IntPtr.Zero)
                return;

            context = new LAContext();
        }
        private void CancelAuthentication()
        {
            CreateNewContext();
        }

        private void CreateNewContext()
        {
            if (context != null)
            {
                if (context.RespondsToSelector(new Selector("invalidate")))
                {
                    context.Invalidate();
                }
                context.Dispose();
            }

            CreateLaContext();
        }

        public async Task<bool> LoginWithBiometrics()
        {
            try
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
                            message = "Put your finger on screen to be the King!";
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
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                CreateNewContext();
            }
        }

    }
}
