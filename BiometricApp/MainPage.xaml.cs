using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiometricApp.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BiometricApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var animation = new Animation(v => btnThrone.Scale = v, 0, 2);
            animation.Commit(this, "PulseAnimation", 16, 2000, Easing.SinOut, (v, c) => btnThrone.Scale = 0, () => true);
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (DependencyService.Get<IBiometricService>().IsDeviceSupportBiometry())
            {
                var result = await DependencyService.Get<IBiometricService>().LoginWithBiometrics();
                if (result)
                {
                    await DisplayAlert("", "May your reign be long!", "OK");
                }
                else
                    await DisplayAlert("", "Returning to the Night Watch.", "OK");
            }
        }
    }
}
