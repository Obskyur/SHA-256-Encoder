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
    public App() {
        this.State = new State();
        this.Server = new Server(this.State);
    }

    public async Task Submit()
    {
        await Server.Authenticate(State.Username, State.Password);
        await Server.Verify(State.SessionId, State.Username, State.Password);
    }
}
