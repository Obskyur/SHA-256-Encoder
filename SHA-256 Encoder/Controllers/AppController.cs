using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SHA_256_Encoder.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHA_256_Encoder.Controllers
{
    internal class AppController :INotifyPropertyChanged
    {
        private Models.App App { get; set; } = new Models.App();
        private BoolToBrushConverter BoolToBrushConverter { get; set; } = new BoolToBrushConverter();

        public AppController()
        {
            App.State.PropertyChanged += OnStatePropertyChanged;
        }

        public bool IsAuthenticated => App.State.IsAuthenticated;
        public bool IsVerified => App.State.IsVerified;

        public int Score => App.State.Score;

        public async Task SubmitAsync()
        {
            await App.Server.Authenticate();
            await App.Server.Verify();
        }

        public void SetUsername(string username)
        {
            App.State.Username = username;
        }

        public void SetPassword(string password)
        {
            App.State.Password = password;
        }

        public RadialGradientBrush GetColor(bool isTrue)
        {
            return (RadialGradientBrush)BoolToBrushConverter.Convert(
                isTrue,
                typeof(RadialGradientBrush),
                null,
                "");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnStatePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
    }
}
