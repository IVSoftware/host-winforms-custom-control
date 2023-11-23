using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace host_winforms_custom_control
{
    /// <summary>
    /// This represents the "Black Box" third-party control.
    /// </summary>
    public partial class AlertViewer : UserControl
    {
        public AlertViewer()
        {
            InitializeComponent();
            Disposed += (sender, e) =>FadeForm.Dispose();
        }
        public void DisplayAlert(string alert="Alert")
        {
            FadeForm.ShowAlert(this, alert);
        }
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            FadeForm.Font = Font;
        }
        FadeForm FadeForm { get; } = new FadeForm();
        public double AlertOpacity
        {
            get => Convert.ToSingle(FadeForm.AlertOpacity);
            set
            {
                if (!Equals(FadeForm.AlertOpacity, value))
                {
                    FadeForm.AlertOpacity = value;
                }
            }
        }

        public double Step
        {
            get => Convert.ToSingle(FadeForm.Step);
            set
            {
                if (!Equals(FadeForm.Step, value))
                {
                    FadeForm.Step = value;
                }
            }
        }
    }
    class FadeForm : Form
    {
        public double AlertOpacity { get; set; } = 0.85F;
        public double Step { get; set; } = 0.02F;
        public FadeForm()
        {
            BackColor = Color.DarkGray;
            FormBorderStyle = FormBorderStyle.None;
            _alertLabel = new Label
            {
                Name = "labelAlert",
                Margin = new Padding(20),
                BackColor = Color.LightBlue,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            Controls.Add(_alertLabel);
        }
        readonly Label _alertLabel;
        public TimeSpan AnimationTimeSpan { get; set; } = TimeSpan.FromSeconds(2);
        public async void ShowAlert(IWin32Window owner, string msg)
        {
            if (!DesignMode)
            {
                if (owner is Control parent)
                {
                    localTrackParent();
                    base.Show(owner);
                    _alertLabel.Text = msg;
                    // Animate
                    for (double d = 0; d <= AlertOpacity; d += Step)
                    {
                        localTrackParent();
                        base.Opacity = Math.Pow(d, 2);
                        await Task.Delay(TimeSpan.FromSeconds(0.01));
                        localTrackParent();
                        if (!Visible) break; // If form hides during animation.
                    }

                    while (Visible)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(0.01));
                        localTrackParent();
                    }
                    void localTrackParent()
                    {
                        Bounds = parent.RectangleToScreen(parent.ClientRectangle);
                        BringToFront();
                    }
                }
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            e.Cancel = true;
            Hide(); 
        }
    }
}
