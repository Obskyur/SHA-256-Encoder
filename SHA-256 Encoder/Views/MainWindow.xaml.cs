using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;

namespace SHA_256_Encoder
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public new static MainWindow Current { get; private set; }
        private Controllers.AppController AppController { get; set; } = new Controllers.AppController();
        public MainWindow()
        {
            this.InitializeComponent();
            this.Width = 400;
            this.Height = 400;
            Current = this;
            this.authIcon.Fill = AppController.GetColor(AppController.IsAuthenticated);
            this.verifiedIcon.Fill = AppController.GetColor(AppController.IsVerified);
            AppController.PropertyChanged += AppController_PropertyChanged;
        }

        public double Width
        {
            get { return this.Bounds.Width; }
            set { this.AppWindow.Resize(new Windows.Graphics.SizeInt32((int)value, (int)this.Height)); }
        }

        public double Height
        {
            get { return this.Bounds.Height; }
            set { this.AppWindow.Resize(new Windows.Graphics.SizeInt32((int)this.Width, (int)value)); }
        }

        private void TrySetFill(string propertyName)
        {
            if (propertyName == nameof(AppController.IsAuthenticated))
            {
                this.authIcon.Fill = AppController.GetColor(AppController.IsAuthenticated);
            }
            else if (propertyName == nameof(AppController.IsVerified))
            {
                this.verifiedIcon.Fill = AppController.GetColor(AppController.IsVerified);
            }
            else
            {
                throw new ArgumentException("Invalid property name", nameof(propertyName));
            }
        }

        private void UsernameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AppController.SetUsername(this.usernameBox.Text);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            AppController.SetPassword(this.passwordBox.Password);
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            await AppController.SubmitAsync();
        }

        private void ShowPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            passwordBox.PasswordRevealMode = PasswordRevealMode.Visible;
        }

        private void ShowPasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            passwordBox.PasswordRevealMode = PasswordRevealMode.Hidden;
        }
        private void AppController_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AppController.Score))
                scoreTextBlock.Text = "Score: " + AppController.Score.ToString();
            else
                TrySetFill(e.PropertyName);
        }
    }
}
