using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHA_256_Encoder.Models;
internal class App
{
    public State State { get; private set; }
    public Server Server { get; private set; }
    public App()
    {
        this.State = new State();
        this.Server = new Server(this.State);
    }

    public string GetHash(string s)
    {
        return Encoder.ComputeSha256Hash(s);
    }
}
