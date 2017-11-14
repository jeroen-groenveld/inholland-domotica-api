// Decompiled with JetBrains decompiler
// Type: DaHaus.Heater
// Assembly: DaHaus, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F5293888-287F-47C1-B6A9-B2BD840DD288
// Assembly location: C:\Users\jeroe\AppData\Local\Apps\2.0\D9T6E6NY.OPP\BECXCBMJ.759\daha..tion_47687aec7c164033_0001.0000_61d211aba69693e5\DaHaus.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DaHaus
{
  public class Heater : UserControl
  {
    private Heater.HeaterState imageIndex;
    private IContainer components;
    private ImageList ilHeaterstates;
    private NumericUpDown nudTemperature;

    public Heater()
    {
      this.InitializeComponent();
      this.imageIndex = Heater.HeaterState.On;
      this.BackgroundImage = this.ilHeaterstates.Images[(int) this.imageIndex];
    }

    public Heater.HeaterState State
    {
      get
      {
        return this.imageIndex;
      }
      set
      {
        this.imageIndex = value;
        this.BackgroundImage = this.ilHeaterstates.Images[(int) this.imageIndex];
      }
    }

    public Decimal Temperature
    {
      get
      {
        return this.nudTemperature.Value;
      }
      set
      {
        try
        {
          this.nudTemperature.Value = value;
        }
        catch
        {
        }
        this.State = this.nudTemperature.Value > new Decimal(1900, 0, 0, false, (byte) 2) ? Heater.HeaterState.On : Heater.HeaterState.Off;
      }
    }

    public void Toggle()
    {
      this.State = this.State == Heater.HeaterState.On ? Heater.HeaterState.Off : Heater.HeaterState.On;
    }

    private void nudTemperature_ValueChanged(object sender, EventArgs e)
    {
      this.State = this.nudTemperature.Value > new Decimal(1900, 0, 0, false, (byte) 2) ? Heater.HeaterState.On : Heater.HeaterState.Off;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Heater));
      this.ilHeaterstates = new ImageList(this.components);
      this.nudTemperature = new NumericUpDown();
      this.nudTemperature.BeginInit();
      this.SuspendLayout();
      this.ilHeaterstates.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilHeaterstates.ImageStream");
      this.ilHeaterstates.TransparentColor = Color.Transparent;
      this.ilHeaterstates.Images.SetKeyName(0, "woodstove_off.png");
      this.ilHeaterstates.Images.SetKeyName(1, "woodstove_on.png");
      this.nudTemperature.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.nudTemperature.DecimalPlaces = 1;
      this.nudTemperature.Increment = new Decimal(new int[4]
      {
        10,
        0,
        0,
        131072
      });
      this.nudTemperature.Location = new Point(37, 103);
      this.nudTemperature.Maximum = new Decimal(new int[4]
      {
        35,
        0,
        0,
        0
      });
      this.nudTemperature.Minimum = new Decimal(new int[4]
      {
        12,
        0,
        0,
        0
      });
      this.nudTemperature.Name = "nudTemperature";
      this.nudTemperature.Size = new Size(74, 20);
      this.nudTemperature.TabIndex = 0;
      this.nudTemperature.TextAlign = HorizontalAlignment.Center;
      this.nudTemperature.Value = new Decimal(new int[4]
      {
        21,
        0,
        0,
        0
      });
      this.nudTemperature.ValueChanged += new EventHandler(this.nudTemperature_ValueChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Transparent;
      this.BackgroundImage = (Image) componentResourceManager.GetObject("$this.BackgroundImage");
      this.BackgroundImageLayout = ImageLayout.Zoom;
      this.Controls.Add((Control) this.nudTemperature);
      this.DoubleBuffered = true;
      this.Name = nameof (Heater);
      this.nudTemperature.EndInit();
      this.ResumeLayout(false);
    }

    public enum HeaterState
    {
      Off,
      On,
    }
  }
}
