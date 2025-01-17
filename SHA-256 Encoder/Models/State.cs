using Microsoft.Windows.ApplicationModel.DynamicDependency;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHA_256_Encoder.Models;
internal class State : INotifyPropertyChanged
{
    private bool isAuthenticated = false;
    private bool isVerified = false;
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string SessionId { get; set; } = "";
    public int Score { get; set; } = 0;
    public bool IsAuthenticated
    {
        get { return isAuthenticated; }
        set
        {
            if (isAuthenticated != value)
            {
                isAuthenticated = value;
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }
    }

    public bool IsVerified
    {
        get { return isVerified; }
        set
        {
            if (isVerified != value)
            {
                isVerified = value;
                OnPropertyChanged(nameof(IsVerified));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
