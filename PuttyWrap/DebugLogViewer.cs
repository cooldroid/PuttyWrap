using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PuttyWrap
{
    public partial class DebugLogViewer : ToolWindow
    {
        public DebugLogViewer()
        {
            InitializeComponent();
            Logger.OnLog += new LogCallback(Logger_OnLog);
        }

        void Logger_OnLog(string logLine)
        {
            if (this.InvokeRequired)
                this.BeginInvoke((MethodInvoker)delegate() { Logger_OnLog(logLine); });
            else
                richTextBox1.AppendText(logLine + System.Environment.NewLine);
        }
    }
}
