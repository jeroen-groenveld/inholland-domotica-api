// Decompiled with JetBrains decompiler
// Type: DaHaus.SocketEventArgs
// Assembly: DaHaus, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F5293888-287F-47C1-B6A9-B2BD840DD288
// Assembly location: C:\Users\jeroe\AppData\Local\Apps\2.0\D9T6E6NY.OPP\BECXCBMJ.759\daha..tion_47687aec7c164033_0001.0000_61d211aba69693e5\DaHaus.exe

using System;
using System.Net.Sockets;
using System.Text;

namespace DaHaus
{
  public class SocketEventArgs
  {
    public const int BufferSize = 1024;
    public Socket workSocket;
    public byte[] buffer;
    public StringBuilder dataString;
    public int bytesPending;
    public bool keepConnection;
    internal SocketListener slParent;

    public SocketEventArgs(SocketListener parent, Socket socket)
    {
      this.workSocket = socket;
      this.buffer = new byte[1024];
      this.dataString = new StringBuilder();
      this.bytesPending = 0;
      this.keepConnection = false;
      this.slParent = parent;
    }

    public void Send(string data, bool keepConnection = false)
    {
      this.SendRaw(Encoding.ASCII.GetBytes(data), keepConnection);
    }

    public void SendRaw(byte[] byteData, bool keepConnection = false)
    {
      this.bytesPending += byteData.Length;
      this.keepConnection = keepConnection;
      this.workSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(this.slParent.SendCallback), (object) this);
    }
  }
}
