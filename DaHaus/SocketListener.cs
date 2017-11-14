// Decompiled with JetBrains decompiler
// Type: DaHaus.SocketListener
// Assembly: DaHaus, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F5293888-287F-47C1-B6A9-B2BD840DD288
// Assembly location: C:\Users\jeroe\AppData\Local\Apps\2.0\D9T6E6NY.OPP\BECXCBMJ.759\daha..tion_47687aec7c164033_0001.0000_61d211aba69693e5\DaHaus.exe

using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DaHaus
{
  public class SocketListener : Component
  {
    private ManualResetEvent allDone;
    private Thread thrListener;

    public IPAddress BindAddress { get; set; }

    public int LocalPort { get; set; }

    public AddressFamily AddressFamily { get; set; }

    public SocketType SocketType { get; set; }

    public ProtocolType ProtocolType { get; set; }

    public int Backlog { get; set; }

    public event SocketListener.ConnectionDataEvent OnNewConnection;

    public event SocketListener.ConnectionDataEvent OnConnectionEnded;

    public event SocketListener.MessageReceivedEvent OnMessageReceived;

    public SocketListener()
    {
      this.allDone = new ManualResetEvent(false);
      this.thrListener = new Thread(new ThreadStart(this.StartListening));
      this.BindAddress = IPAddress.Any;
      this.LocalPort = 11000;
      this.AddressFamily = AddressFamily.InterNetwork;
      this.SocketType = SocketType.Stream;
      this.ProtocolType = ProtocolType.Tcp;
      this.Backlog = 100;
    }

    public void Start()
    {
      this.thrListener.Start();
    }

    public void Stop()
    {
      this.thrListener.Abort();
    }

    private void StartListening()
    {
      IPEndPoint ipEndPoint = new IPEndPoint(this.BindAddress, this.LocalPort);
      Socket socket = new Socket(this.AddressFamily, this.SocketType, this.ProtocolType);
      try
      {
        socket.Bind((EndPoint) ipEndPoint);
        socket.Listen(this.Backlog);
        while (true)
        {
          this.allDone.Reset();
          socket.BeginAccept(new AsyncCallback(this.AcceptCallback), (object) socket);
          this.allDone.WaitOne();
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void AcceptCallback(IAsyncResult ar)
    {
      this.allDone.Set();
      Socket socket = ((Socket) ar.AsyncState).EndAccept(ar);
      SocketEventArgs sea = new SocketEventArgs(this, socket);
      this.Invoke(this.OnNewConnection, (object) this, sea);
      socket.BeginReceive(sea.buffer, 0, 1024, SocketFlags.None, new AsyncCallback(this.ReadCallback), (object) sea);
    }

    private void ReadCallback(IAsyncResult ar)
    {
      SocketEventArgs asyncState = (SocketEventArgs) ar.AsyncState;
      int count = -1;
      if (asyncState.workSocket.Connected)
        count = asyncState.workSocket.EndReceive(ar);
      if (count > 0)
      {
        if (count != 21 || (int) asyncState.buffer[0] != (int) byte.MaxValue)
          asyncState.dataString.Append(Encoding.ASCII.GetString(asyncState.buffer, 0, count));
        string str = asyncState.dataString.ToString();
        if (str.EndsWith("\n"))
        {
          string message = str.Trim();
          this.Invoke(this.OnMessageReceived, (object) this, asyncState, message);
        }
        asyncState.workSocket.BeginReceive(asyncState.buffer, 0, 1024, SocketFlags.None, new AsyncCallback(this.ReadCallback), (object) asyncState);
      }
      else
      {
        if (count != 0)
          return;
        this.CloseSocket(ar);
      }
    }

    internal void SendCallback(IAsyncResult ar)
    {
      try
      {
        SocketEventArgs asyncState = (SocketEventArgs) ar.AsyncState;
        int num = asyncState.workSocket.EndSend(ar);
        asyncState.bytesPending -= num;
        this.CloseSocket(ar);
      }
      catch (Exception ex)
      {
      }
    }

    private void CloseSocket(IAsyncResult ar)
    {
      SocketEventArgs asyncState = (SocketEventArgs) ar.AsyncState;
      if ((!ar.IsCompleted || asyncState.keepConnection) && asyncState.workSocket.Connected)
        return;
      this.Invoke(this.OnConnectionEnded, (object) this, asyncState);
      if (!asyncState.workSocket.Connected)
        return;
      asyncState.workSocket.Shutdown(SocketShutdown.Both);
      asyncState.workSocket.Close();
    }

    public void Invoke(SocketListener.ConnectionDataEvent safeEvent, object sender, SocketEventArgs sea)
    {
      if (safeEvent.Target is Control && ((Control) safeEvent.Target).InvokeRequired)
      {
        ((Control) safeEvent.Target).Invoke((Delegate) new SafeInvokeDelegate(this.Invoke), (object) safeEvent, sender, (object) sea);
      }
      else
      {
        if (safeEvent == null)
          return;
        safeEvent(sender, sea);
      }
    }

    public void Invoke(SocketListener.MessageReceivedEvent safeEvent, object sender, SocketEventArgs sea, string message)
    {
      if (safeEvent.Target is Control && ((Control) safeEvent.Target).InvokeRequired)
      {
        ((Control) safeEvent.Target).Invoke((Delegate) new SafeInvokeDelegate2(this.Invoke), (object) safeEvent, sender, (object) sea, (object) message);
      }
      else
      {
        if (safeEvent == null)
          return;
        safeEvent(sender, sea, message);
      }
    }

    public delegate void ConnectionDataEvent(object sender, SocketEventArgs sea);

    public delegate void MessageReceivedEvent(object sender, SocketEventArgs sea, string message);
  }
}
