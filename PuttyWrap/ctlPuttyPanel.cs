using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace PuttyWrap
{
    public partial class ctlPuttyPanel : ToolWindow
    {        
        private string ApplicationName = String.Empty;
        private string ApplicationParameters = String.Empty;

        private ApplicationPanel applicationwrapper1;
        private SessionData m_Session;
        private PuttyClosedCallback m_ApplicationExit;
        private frmPuttyWrap m_PuttyWrap;

        public string ApplicationTitle
        {
        	get { return this.applicationwrapper1.ApplicationWindowTitle; }
        }

        public ctlPuttyPanel(frmPuttyWrap PuttyWrap, SessionData session, PuttyClosedCallback callback)
        {
            m_PuttyWrap = PuttyWrap;
            m_Session = session;
            m_ApplicationExit = callback;

            string args = "-" + session.Proto.ToString().ToLower() + " ";            
            args += (!String.IsNullOrEmpty(m_Session.Password) && m_Session.Password.Length > 0) ? "-pw " + m_Session.Password + " " : "";
            args += "-P " + m_Session.Port + " ";
            args += (!String.IsNullOrEmpty(m_Session.PuttySession)) ? "-load \"" + m_Session.PuttySession + "\" " : "";
            args += (!String.IsNullOrEmpty(m_Session.Username) && m_Session.Username.Length > 0) ? m_Session.Username + "@" : "";
            args += m_Session.Host;
            Logger.Log("Args: '{0}'", args);
            this.ApplicationParameters = args;

            InitializeComponent();

            this.Text = session.SessionName;

            CreatePanel();
        }

        private void CreatePanel()
        {
            this.applicationwrapper1 = new ApplicationPanel();
            this.SuspendLayout();            
            this.applicationwrapper1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.applicationwrapper1.ApplicationName = frmPuttyWrap.PuttyExe;
            this.applicationwrapper1.ApplicationParameters = this.ApplicationParameters;
            this.applicationwrapper1.Location = new System.Drawing.Point(0, 0);
            this.applicationwrapper1.Name = "applicationControl1";
            this.applicationwrapper1.Size = new System.Drawing.Size(284, 264);
            this.applicationwrapper1.TabIndex = 0;            
            this.applicationwrapper1.m_CloseCallback = this.m_ApplicationExit;
            this.Controls.Add(this.applicationwrapper1);

            this.ResumeLayout();
        }

        private void closeSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void duplicateSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionData sessionData = new SessionData(m_Session);
            m_PuttyWrap.CreatePuttyPanel(sessionData);
        }

        private void renameSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgRename form = new dlgRename(m_Session);
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.TabText = form.textBox1.Text;
            }
        }

        /// <summary>
        /// Reset the focus to the child application window
        /// </summary>
        internal void SetFocusToChildApplication()
        {
            this.applicationwrapper1.ReFocusPuTTY();         
        }

        void RestartSessionToolStripMenuItemClick(object sender, EventArgs e)
        {
        	SessionData sessionData = new SessionData(m_Session);
            m_PuttyWrap.CreatePuttyPanel(sessionData);
            this.Close();
        }
    }
}
