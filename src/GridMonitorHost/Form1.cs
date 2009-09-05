using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace gridtool
{
  public partial class Form1 : Form
  {
    GuestConnectivity _gc;

    public Form1(GuestConnectivity GC)
    {
      _gc = GC;
      InitializeComponent();
    }

    //Pass in the number
    protected void UpdateTaskBar(int i)
    {
      String TaskBarLetter;
      
      // Create a graphics instance that draws to a bitmap
      Bitmap bitmap = new Bitmap(16, 16);
      SolidBrush brush = new SolidBrush(fontDialog1.Color);
      Graphics graphics = Graphics.FromImage(bitmap);

      // Draw the number of Messages to the bitmap using the user selected font
      if (i == 0) {
        TaskBarLetter = "D";
      } else if ( i == 1) {
        TaskBarLetter = "E";
      } else {
        TaskBarLetter = "G";
      }

      graphics.DrawString(TaskBarLetter, fontDialog1.Font, brush, 0, 0);

      // Convert the bitmap into an icon and use it for the system tray icon
      IntPtr hIcon = bitmap.GetHicon();
      Icon icon = Icon.FromHandle(hIcon);
      notifyIcon1.Icon = icon;

      //GetHicon creates an unmanaged handle which must be manually destroyed
     // DestroyIcon(hIcon);
    }

    // Hide the window if we're being minimized so it doesn't show in the
    // taskbar (will still show in the system tray).
    protected override void OnResize(EventArgs e)
    {
      this.Hide();
    }

    protected override void OnLoad(EventArgs e)
    {
      timer1.Enabled = true;
      timer1.Start();

      _gc.Enable = true;
      UpdateTaskBar(1);
    }

    protected void tickClosing(object sender, EventArgs e)
    {
      this.Hide();
      timer1.Stop();
    }

    protected void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.Hide();

      // Cancel the close...
      e.Cancel = true;
    }

    protected void showTextToolStripMenuItem_Click1(object sender, EventArgs e)
    {
      _gc.Enable = true;
      UpdateTaskBar(1);     
    }

    protected void showTextToolStripMenuItem_Click2(object sender, EventArgs e)
    {
      _gc.Enable = false;
      UpdateTaskBar(0);
    }

    protected void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      _gc.Enable = false;
      Application.Exit();
    }
  }
}
