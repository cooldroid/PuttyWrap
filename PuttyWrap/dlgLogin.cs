using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Principal;

namespace PuttyWrap
{
    public partial class dlgLogin : Form
    {
        private string _Username = WindowsIdentity.GetCurrent().Name.Split('\\')[1];

        public string Username
        {
            get { return _Username; }
            private set { _Username = value; }
        }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            private set { _Password = value; }
        }

        private bool _Remember = false;
        public bool Remember
        {
            get { return _Remember; }
            private set { _Remember = value; }
        }

        private SessionData m_Session;

        public dlgLogin(SessionData session)
        {
            m_Session = session;
            InitializeComponent();

            if (!String.IsNullOrEmpty(m_Session.Username))
                this.Username = m_Session.Username;

            textBoxUsername.Text = this.Username;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Username = textBoxUsername.Text;
            Password = textBoxPasssword.Text;
            DialogResult = DialogResult.OK;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // a Password is not required since user could be using keys
            button1.Enabled = textBoxUsername.Text.Length > 0;
        }

        private void checkBoxRemember_CheckedChanged(object sender, EventArgs e)
        {
            Remember = checkBoxRemember.Checked;
        }

        private void dlgLogin_Shown(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(textBoxUsername.Text))
                textBoxPasssword.Focus();
        }
    }
}
