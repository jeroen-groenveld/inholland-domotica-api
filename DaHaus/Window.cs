// Decompiled with JetBrains decompiler
// Type: DaHaus.Window
// Assembly: DaHaus, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F5293888-287F-47C1-B6A9-B2BD840DD288
// Assembly location: C:\Users\jeroe\AppData\Local\Apps\2.0\D9T6E6NY.OPP\BECXCBMJ.759\daha..tion_47687aec7c164033_0001.0000_61d211aba69693e5\DaHaus.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DaHaus
{
  public class Window : UserControl
  {
    private int imageIndex;
    private bool opening;
    private IContainer components;
    private ImageList ilWindows;
    private Timer tmrAnimation;

    public Window.WindowState State
    {
      get
      {
        if (this.imageIndex == 0)
          return Window.WindowState.Open;
        return this.imageIndex == this.ilWindows.Images.Count - 1 ? Window.WindowState.Closed : Window.WindowState.Half;
      }
      set
      {
        int num;
        if (value == Window.WindowState.Open)
        {
          num = 0;
        }
        else
        {
          if (value != Window.WindowState.Closed)
            throw new Exception("Value out of range");
          num = this.ilWindows.Images.Count - 1;
        }
        if (this.imageIndex == num)
          return;
        this.opening = num < this.imageIndex;
        this.tmrAnimation.Enabled = true;
      }
    }

    public Window()
    {
      this.imageIndex = 0;
      this.opening = true;
      this.InitializeComponent();
      this.BackgroundImage = this.ilWindows.Images[this.imageIndex];
    }

    private void tmrAnimation_Tick(object sender, EventArgs e)
    {
      this.imageIndex += this.opening ? -1 : 1;
      this.tmrAnimation.Enabled = this.imageIndex != 0 && this.imageIndex != this.ilWindows.Images.Count - 1;
      this.BackgroundImage = this.ilWindows.Images[this.imageIndex];
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Window));
      this.ilWindows = new ImageList(this.components);
      this.tmrAnimation = new Timer(this.components);
      this.SuspendLayout();
      this.ilWindows.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilWindows.ImageStream");
      this.ilWindows.TransparentColor = Color.Transparent;
      this.ilWindows.Images.SetKeyName(0, "Window00.png");
      this.ilWindows.Images.SetKeyName(1, "Window01.png");
      this.ilWindows.Images.SetKeyName(2, "Window02.png");
      this.ilWindows.Images.SetKeyName(3, "Window03.png");
      this.ilWindows.Images.SetKeyName(4, "Window04.png");
      this.ilWindows.Images.SetKeyName(5, "Window05.png");
      this.ilWindows.Images.SetKeyName(6, "Window06.png");
      this.ilWindows.Images.SetKeyName(7, "Window07.png");
      this.ilWindows.Images.SetKeyName(8, "Window08.png");
      this.ilWindows.Images.SetKeyName(9, "Window09.png");
      this.ilWindows.Images.SetKeyName(10, "Window10.png");
      this.ilWindows.Images.SetKeyName(11, "Window11.png");
      this.ilWindows.Images.SetKeyName(12, "Window12.png");
      this.ilWindows.Images.SetKeyName(13, "Window13.png");
      this.ilWindows.Images.SetKeyName(14, "Window14.png");
      this.ilWindows.Images.SetKeyName(15, "Window15.png");
      this.ilWindows.Images.SetKeyName(16, "Window16.png");
      this.ilWindows.Images.SetKeyName(17, "Window17.png");
      this.ilWindows.Images.SetKeyName(18, "Window18.png");
      this.ilWindows.Images.SetKeyName(19, "Window19.png");
      this.ilWindows.Images.SetKeyName(20, "Window20.png");
      this.ilWindows.Images.SetKeyName(21, "Window21.png");
      this.tmrAnimation.Tick += new EventHandler(this.tmrAnimation_Tick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Control;
      this.BackgroundImageLayout = ImageLayout.Zoom;
      this.Name = nameof (Window);
      this.ResumeLayout(false);
    }

    public enum WindowState
    {
      Open,
      Half,
      Closed,
    }
  }
}
