// Decompiled with JetBrains decompiler
// Type: DaHaus.Lamp
// Assembly: DaHaus, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F5293888-287F-47C1-B6A9-B2BD840DD288
// Assembly location: C:\Users\jeroe\AppData\Local\Apps\2.0\D9T6E6NY.OPP\BECXCBMJ.759\daha..tion_47687aec7c164033_0001.0000_61d211aba69693e5\DaHaus.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DaHaus
{
  public class Lamp : UserControl
  {
    private Lamp.LampState imageIndex;
    private IContainer components;
    private ImageList ilLampen;

    public Lamp()
    {
      this.InitializeComponent();
      this.imageIndex = Lamp.LampState.On;
      this.BackgroundImage = this.ilLampen.Images[(int) this.imageIndex];
    }

    public Lamp.LampState State
    {
      get
      {
        return this.imageIndex;
      }
      set
      {
        this.imageIndex = value;
        this.BackgroundImage = this.ilLampen.Images[(int) this.imageIndex];
      }
    }

    public void Toggle()
    {
      this.State = this.State == Lamp.LampState.On ? Lamp.LampState.Off : Lamp.LampState.On;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Lamp));
      this.ilLampen = new ImageList(this.components);
      this.SuspendLayout();
      this.ilLampen.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilLampen.ImageStream");
      this.ilLampen.TransparentColor = Color.Transparent;
      this.ilLampen.Images.SetKeyName(0, "lamp_aan.png");
      this.ilLampen.Images.SetKeyName(1, "lamp_uit.png");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Transparent;
      this.BackgroundImageLayout = ImageLayout.Zoom;
      this.Name = nameof (Lamp);
      this.ResumeLayout(false);
    }

    public enum LampState
    {
      On,
      Off,
    }
  }
}
