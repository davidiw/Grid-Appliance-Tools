namespace gridtool
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.showTextToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.showTextToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.fontDialog1 = new System.Windows.Forms.FontDialog();
      this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // timer1
      // 
      this.timer1.Interval = 1;
      this.timer1.Tick += new System.EventHandler(this.tickClosing);
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
      this.showTextToolStripMenuItem1,this.showTextToolStripMenuItem2,
      this.exitToolStripMenuItem});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(70, 70);
      // 
      // showTextToolStripMenuItem
      // 
      this.showTextToolStripMenuItem1.Name = "showTextToolStripMenuItem";
      this.showTextToolStripMenuItem1.Size = new System.Drawing.Size(156, 22);
      this.showTextToolStripMenuItem1.Text = "Enable monitor";
      this.showTextToolStripMenuItem1.Click += new System.EventHandler(this.showTextToolStripMenuItem_Click1);
      // 
      // showTextToolStripMenuItem
      // 
      this.showTextToolStripMenuItem2.Name = "showTextToolStripMenuItem";
      this.showTextToolStripMenuItem2.Size = new System.Drawing.Size(156, 22);
      this.showTextToolStripMenuItem2.Text = "Disable monitor";
      this.showTextToolStripMenuItem2.Click += new System.EventHandler(this.showTextToolStripMenuItem_Click2);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
      this.exitToolStripMenuItem.Text = "Exit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // fontDialog1
      // 
      this.fontDialog1.Color = System.Drawing.Color.Blue;
      this.fontDialog1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.fontDialog1.ShowColor = true;
      // 
      // notifyIcon1
      // 
      this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
      this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
      this.notifyIcon1.Text = "Gridtool";
      this.notifyIcon1.Visible = true;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.ClientSize = new System.Drawing.Size(204, 22);
      this.ForeColor = System.Drawing.SystemColors.Desktop;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Form1";
      this.ShowInTaskbar = false;
      this.Text = "Gridtool V1.0";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
      this.contextMenuStrip1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem showTextToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem showTextToolStripMenuItem2;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.FontDialog fontDialog1;
    private System.Windows.Forms.NotifyIcon notifyIcon1;
  }
}

