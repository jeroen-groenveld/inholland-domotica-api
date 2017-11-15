// Decompiled with JetBrains decompiler
// Type: DaHaus.frmHuis
// Assembly: DaHaus, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F5293888-287F-47C1-B6A9-B2BD840DD288
// Assembly location: C:\Users\jeroe\AppData\Local\Apps\2.0\D9T6E6NY.OPP\BECXCBMJ.759\daha..tion_47687aec7c164033_0001.0000_61d211aba69693e5\DaHaus.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Text;

namespace DaHaus
{
  public class frmHuis : Form
  {
    private IContainer components;
    private SplitContainer splitContainer1;
    private Lamp lamp1;
    private Lamp lamp2;
    private Lamp lamp4;
    private Lamp lamp3;
    private Lamp lamp5;
    private PictureBox pictureBox1;
    private ListBox lbConnected;
    private SocketListener socketListener1;
    private Label label1;
    private Window window1;
    private Window window2;
    private Heater heater2;

        private Dictionary<string, SocketEventArgs> clients = new Dictionary<string, SocketEventArgs>();

        //private Dictionary<>
    public frmHuis()
    {
      this.InitializeComponent();
      this.socketListener1.Start();
    }

    private void frmHuis_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.socketListener1.Stop();
    }

    private void socketListener1_OnNewConnection(object Sender, SocketEventArgs sea)
    {
        string address = sea.workSocket.RemoteEndPoint.ToString();
        this.lbConnected.Items.Add(address);

            sea.dataString.Clear();
            //sea.workSocket.Send(UTF8Encoding.UTF8.GetBytes("Hello\r\n"));
            this.clients.Add(address, sea);
    }

    private void socketListener1_OnConnectionEnded(object Sender, SocketEventArgs sea)
    {
            object address = (object)sea.workSocket.RemoteEndPoint.ToString();
            this.lbConnected.Items.Remove(address.ToString());
            this.clients.Remove(address.ToString());
        }

    private void socketListener1_OnMessageReceived(object Sender, SocketEventArgs sea, string message)
    {
     //       MessageBox.Show(message);
      string[] strArray = message.Split(' ');
      object[] objArray = new object[strArray.Length];
      for (int index = 0; index < strArray.Length; ++index)
      {
        int result1;
        Decimal result2;
        objArray[index] = !int.TryParse(strArray[index], out result1) ? (!Decimal.TryParse(strArray[index], out result2) ? (object) strArray[index] : (object) result2) : (object) result1;
      }
      sea.dataString.Clear();
      string str = "";
      if (objArray[0] is string)
        str = (string) objArray[0];
      int num1 = -1;
      Lamp[] controlsByType1 = this.getControlsByType<Lamp>(new Lamp());
      Window[] controlsByType2 = this.getControlsByType<Window>(new Window());
      switch (str.ToLower())
      {
        case "help":
          sea.Send("Commands available are:\r\nexit                        Close the connection\r\nhelp                        This text\r\nlamps                       Number of lamps available in this system\r\nlamp <index> [on|off]       Turn lamp #index on or off or return the status\r\nwindows                     Number of windows available in this system\r\nwindow <index> [open|close] Open or close window #index\r\nheater [<temperature>]      Gets/Sets the heater temperature\r\nwhereis lamp|window <index> Get the physical location of the object in the house\r\n", true);
          return;
        case "exit":
          sea.Send("Bye bye\r\n", false);
          return;
        case "lamps":
          sea.Send(string.Format("{0}\r\n", (object) controlsByType1.Length), true);
          return;
        case "lamp":
          num1 = -1;
          if (objArray.Length > 1 && objArray[1] is int)
          {
            int index = (int) objArray[1];
            if (index >= 0 && index < controlsByType1.Length)
            {
              if (objArray[objArray.Length - 1] is string)
              {
                bool flag = (string) objArray[objArray.Length - 1] == "on";
                controlsByType1[index].State = flag ? Lamp.LampState.On : Lamp.LampState.Off;
              }
              sea.Send(string.Format("Lamp {0} is {1}\r\n", (object) index, (object) controlsByType1[index].State), true);
              return;
            }
            break;
          }
          sea.Send(string.Format("Lamp WTF?\r\n"), true);
          return;
        case "windows":
          sea.Send(string.Format("{0}\r\n", (object) controlsByType2.Length), true);
          return;
        case "window":
          num1 = -1;
          if (objArray.Length > 1 && objArray[1] is int)
          {
            int index = (int) objArray[1];
            if (index >= 0 && index < controlsByType2.Length)
            {
              if (objArray[objArray.Length - 1] is string)
              {
                bool flag = (string) objArray[objArray.Length - 1] == "open";
                controlsByType2[index].State = flag ? Window.WindowState.Open : Window.WindowState.Closed;
              }
              sea.Send(string.Format("Window {0} is {1}\r\n", (object) index, (object) controlsByType2[index].State), true);
              return;
            }
            break;
          }
          sea.Send(string.Format("Window WTF?\r\n"), true);
          return;
        case "heater":
          if (objArray.Length > 1)
          {
            if (objArray[1] is int)
              this.heater2.Temperature = (Decimal) ((int) objArray[1]);
            else if (objArray[1] is Decimal)
              this.heater2.Temperature = (Decimal) objArray[1];
          }
          sea.Send(string.Format("Heater is set to {0}\r\n", (object) this.heater2.Temperature), true);
          return;
        case "whereis":
          num1 = -1;
          if (objArray.Length > 2 && objArray[1] is string)
          {
            if (objArray[2] is int)
            {
              frmHuis.ObjectType objectType;
              try
              {
                objectType = (frmHuis.ObjectType) Enum.Parse(typeof (frmHuis.ObjectType), CultureInfo.CurrentCulture.TextInfo.ToTitleCase((string) objArray[1]));
              }
              catch
              {
                break;
              }
              int index = (int) objArray[2];
              if (index >= 0)
              {
                int num2 = 0;
                frmHuis.Alignment alignment = (frmHuis.Alignment) 0;
                Control control = (Control) null;
                switch (objectType)
                {
                  case frmHuis.ObjectType.Lamp:
                    if (index < controlsByType1.Length)
                    {
                      control = (Control) controlsByType1[index];
                      break;
                    }
                    break;
                  case frmHuis.ObjectType.Window:
                    if (index < controlsByType2.Length)
                    {
                      control = (Control) controlsByType2[index];
                      break;
                    }
                    break;
                }
                if (control != null)
                {
                  if (control.Anchor.HasFlag((Enum) AnchorStyles.Bottom) && !control.Anchor.HasFlag((Enum) AnchorStyles.Top))
                    num2 = 0;
                  else if (control.Anchor.HasFlag((Enum) AnchorStyles.Bottom) && control.Anchor.HasFlag((Enum) AnchorStyles.Top))
                    num2 = 1;
                  else if (!control.Anchor.HasFlag((Enum) AnchorStyles.Bottom) && control.Anchor.HasFlag((Enum) AnchorStyles.Top))
                    num2 = 2;
                  if (control.Anchor.HasFlag((Enum) AnchorStyles.Left) && !control.Anchor.HasFlag((Enum) AnchorStyles.Right))
                    alignment = frmHuis.Alignment.Left;
                  else if (control.Anchor.HasFlag((Enum) AnchorStyles.Left) && control.Anchor.HasFlag((Enum) AnchorStyles.Right))
                    alignment = frmHuis.Alignment.Centre;
                  else if (!control.Anchor.HasFlag((Enum) AnchorStyles.Left) && control.Anchor.HasFlag((Enum) AnchorStyles.Right))
                    alignment = frmHuis.Alignment.Right;
                  sea.Send(string.Format("{0} {1} @ {2} {3}\r\n", (object) objectType, (object) index, (object) num2, (object) alignment), true);
                  return;
                }
                break;
              }
              break;
            }
            break;
          }
          break;
      }
      sea.Send("Computer says \"no\"\r\n", true);
    }

    private T[] getControlsByType<T>(T o)
    {
      List<T> objList = new List<T>();
      foreach (object control in (ArrangedElementCollection) this.splitContainer1.Panel1.Controls)
      {
        if (control is T)
          objList.Add((T) control);
      }
      return objList.ToArray();
    }

    private void lamp_Click(object sender, EventArgs e)
    {
      if (!(sender is Lamp))
        return;

    foreach (KeyValuePair<string, SocketEventArgs> client in this.clients)
    {
                // do something with entry.Value or entry.Key
                SocketEventArgs sea = client.Value;
                sea.workSocket.Send(UTF8Encoding.UTF8.GetBytes("Lamp turned on: " + ((Control)sender).Name + "\r\n"));
    }

      ((Lamp) sender).Toggle();
    }

    private void window_Click(object sender, EventArgs e)
    {
      if (!(sender is Window))
        return;
      Window window = (Window) sender;
      switch (window.State)
      {
        case Window.WindowState.Open:
          window.State = Window.WindowState.Closed;
          break;
        case Window.WindowState.Closed:
          window.State = Window.WindowState.Open;
          break;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (frmHuis));
      this.splitContainer1 = new SplitContainer();
      this.pictureBox1 = new PictureBox();
      this.label1 = new Label();
      this.lbConnected = new ListBox();
      this.heater2 = new Heater();
      this.window1 = new Window();
      this.window2 = new Window();
      this.lamp1 = new Lamp();
      this.lamp2 = new Lamp();
      this.lamp3 = new Lamp();
      this.lamp4 = new Lamp();
      this.lamp5 = new Lamp();
      this.socketListener1 = new SocketListener();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer1.Location = new Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.heater2);
      this.splitContainer1.Panel1.Controls.Add((Control) this.window1);
      this.splitContainer1.Panel1.Controls.Add((Control) this.window2);
      this.splitContainer1.Panel1.Controls.Add((Control) this.lamp1);
      this.splitContainer1.Panel1.Controls.Add((Control) this.lamp2);
      this.splitContainer1.Panel1.Controls.Add((Control) this.lamp3);
      this.splitContainer1.Panel1.Controls.Add((Control) this.lamp4);
      this.splitContainer1.Panel1.Controls.Add((Control) this.lamp5);
      this.splitContainer1.Panel1.Controls.Add((Control) this.pictureBox1);
      this.splitContainer1.Panel2.Controls.Add((Control) this.label1);
      this.splitContainer1.Panel2.Controls.Add((Control) this.lbConnected);
      this.splitContainer1.Size = new Size(582, 562);
      this.splitContainer1.SplitterDistance = 388;
      this.splitContainer1.TabIndex = 6;
      this.pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pictureBox1.BackColor = Color.Transparent;
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(3, 3);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(382, 558);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
      this.pictureBox1.TabIndex = 1;
      this.pictureBox1.TabStop = false;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(3, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(95, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Connected clients:";
      this.lbConnected.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lbConnected.FormattingEnabled = true;
      this.lbConnected.Location = new Point(3, 25);
      this.lbConnected.Name = "lbConnected";
      this.lbConnected.Size = new Size(184, 537);
      this.lbConnected.TabIndex = 0;
      this.heater2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.heater2.BackColor = Color.Transparent;
      this.heater2.BackgroundImage = (Image) componentResourceManager.GetObject("heater2.BackgroundImage");
      this.heater2.BackgroundImageLayout = ImageLayout.Zoom;
      this.heater2.Location = new Point(130, 347);
      this.heater2.Name = "heater2";
      this.heater2.Size = new Size((int) sbyte.MaxValue, 124);
      this.heater2.State = Heater.HeaterState.On;
      this.heater2.TabIndex = 13;
      this.heater2.Temperature = new Decimal(new int[4]
      {
        21,
        0,
        0,
        0
      });
      this.window1.BackColor = SystemColors.Control;
      this.window1.BackgroundImage = (Image) componentResourceManager.GetObject("window1.BackgroundImage");
      this.window1.BackgroundImageLayout = ImageLayout.Zoom;
      this.window1.Location = new Point(100, 173);
      this.window1.Name = "window1";
      this.window1.Size = new Size(76, 67);
      this.window1.State = Window.WindowState.Open;
      this.window1.TabIndex = 11;
      this.window1.Click += new EventHandler(this.window_Click);
      this.window2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.window2.BackColor = SystemColors.Control;
      this.window2.BackgroundImage = (Image) componentResourceManager.GetObject("window2.BackgroundImage");
      this.window2.BackgroundImageLayout = ImageLayout.Zoom;
      this.window2.Location = new Point(211, 173);
      this.window2.Name = "window2";
      this.window2.Size = new Size(76, 67);
      this.window2.State = Window.WindowState.Open;
      this.window2.TabIndex = 12;
      this.window2.Click += new EventHandler(this.window_Click);
      this.lamp1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.lamp1.BackColor = Color.Transparent;
      this.lamp1.BackgroundImage = (Image) componentResourceManager.GetObject("lamp1.BackgroundImage");
      this.lamp1.BackgroundImageLayout = ImageLayout.Zoom;
      this.lamp1.Location = new Point(248, 477);
      this.lamp1.Name = "lamp1";
      this.lamp1.Size = new Size(65, 65);
      this.lamp1.State = Lamp.LampState.Off;
      this.lamp1.TabIndex = 10;
      this.lamp1.Click += new EventHandler(this.lamp_Click);
      this.lamp2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lamp2.BackColor = Color.Transparent;
      this.lamp2.BackgroundImage = (Image) componentResourceManager.GetObject("lamp2.BackgroundImage");
      this.lamp2.BackgroundImageLayout = ImageLayout.Zoom;
      this.lamp2.Location = new Point(71, 477);
      this.lamp2.Name = "lamp2";
      this.lamp2.Size = new Size(65, 65);
      this.lamp2.State = Lamp.LampState.Off;
      this.lamp2.TabIndex = 9;
      this.lamp2.Click += new EventHandler(this.lamp_Click);
      this.lamp3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.lamp3.BackColor = Color.Transparent;
      this.lamp3.BackgroundImage = (Image) componentResourceManager.GetObject("lamp3.BackgroundImage");
      this.lamp3.BackgroundImageLayout = ImageLayout.Zoom;
      this.lamp3.Location = new Point(248, 276);
      this.lamp3.Name = "lamp3";
      this.lamp3.Size = new Size(65, 65);
      this.lamp3.State = Lamp.LampState.Off;
      this.lamp3.TabIndex = 7;
      this.lamp3.Click += new EventHandler(this.lamp_Click);
      this.lamp4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.lamp4.BackColor = Color.Transparent;
      this.lamp4.BackgroundImage = (Image) componentResourceManager.GetObject("lamp4.BackgroundImage");
      this.lamp4.BackgroundImageLayout = ImageLayout.Zoom;
      this.lamp4.Location = new Point(71, 276);
      this.lamp4.Name = "lamp4";
      this.lamp4.Size = new Size(65, 65);
      this.lamp4.State = Lamp.LampState.Off;
      this.lamp4.TabIndex = 8;
      this.lamp4.Click += new EventHandler(this.lamp_Click);
      this.lamp5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lamp5.BackColor = Color.Transparent;
      this.lamp5.BackgroundImage = (Image) componentResourceManager.GetObject("lamp5.BackgroundImage");
      this.lamp5.BackgroundImageLayout = ImageLayout.Zoom;
      this.lamp5.Location = new Point(161, 81);
      this.lamp5.Name = "lamp5";
      this.lamp5.Size = new Size(65, 65);
      this.lamp5.State = Lamp.LampState.Off;
      this.lamp5.TabIndex = 6;
      this.lamp5.Click += new EventHandler(this.lamp_Click);
      this.socketListener1.AddressFamily = AddressFamily.InterNetwork;
      this.socketListener1.Backlog = 100;
      this.socketListener1.BindAddress = (IPAddress) componentResourceManager.GetObject("socketListener1.BindAddress");
      this.socketListener1.LocalPort = 11000;
      this.socketListener1.ProtocolType = ProtocolType.Tcp;
      this.socketListener1.SocketType = SocketType.Stream;
      this.socketListener1.OnNewConnection += new SocketListener.ConnectionDataEvent(this.socketListener1_OnNewConnection);
      this.socketListener1.OnConnectionEnded += new SocketListener.ConnectionDataEvent(this.socketListener1_OnConnectionEnded);
      this.socketListener1.OnMessageReceived += new SocketListener.MessageReceivedEvent(this.socketListener1_OnMessageReceived);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(584, 564);
      this.Controls.Add((Control) this.splitContainer1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (frmHuis);
      this.Text = "Uw Remote Huis";
      this.FormClosing += new FormClosingEventHandler(this.frmHuis_FormClosing);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
    }

    private enum ObjectType
    {
      Lamp = 1,
      Window = 2,
      Heater = 3,
    }

    private enum Alignment
    {
      Left = 1,
      Right = 2,
      Centre = 3,
    }
  }
}
