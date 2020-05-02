using System;
using System.Threading.Tasks;

namespace BiometricApp.Services
{
    public interface IBiometricService
    {
        Task<bool> LoginWithBiometrics();
        
        bool IsDeviceSupportBiometry();
    }
}
