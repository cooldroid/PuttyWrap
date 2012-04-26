using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Web;

namespace PuttyWrap
{
    public partial class dlgEditSession : Form
    {
        private SessionData Session;
        public dlgEditSession(SessionData session)
        {
            Session = session;
            InitializeComponent();

            // get putty saved settings from the registry to populate
            // the dropdown
            PopulatePuttySettings();

            if (!String.IsNullOrEmpty(Session.SessionName))
            {
                this.Text = "Edit session: " + session.SessionName;
                this.textBoxSessionName.Text = Session.SessionName;
                this.textBoxHostname.Text = Session.Host;
                this.textBoxPort.Text = Session.Port.ToString();
                this.textBoxUsername.Text = Session.Username;

                if (this.comboBoxPuttyProfile.Items.Contains(session.SessionName))
                    this.comboBoxPuttyProfile.SelectedItem = session.SessionName;

                switch (Session.Proto)
                {
                    case ConnectionProtocol.Raw:
                        radioButtonRaw.Checked = true;
                        break;
                    case ConnectionProtocol.Rlogin:
                        radioButtonRlogin.Checked = true;
                        break;
                    case ConnectionProtocol.Serial:
                        radioButtonSerial.Checked = true;
                        break;
                    case ConnectionProtocol.SSH:
                        radioButtonSSH.Checked = true;
                        break;
                    case ConnectionProtocol.Telnet:
                        radioButtonTelnet.Checked = true;
                        break;
                    default:
                        radioButtonSSH.Checked = true;
                        break;
                }
            }
            else
            {
                this.Text = "Create new session";
                radioButtonSSH.Checked = true;
            }
        }

        private void PopulatePuttySettings()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SimonTatham\PuTTY\Sessions");
            if (key != null)
            {
                string[] savedSessionNames = key.GetSubKeyNames();

                for (int i = 0; i < savedSessionNames.Length; i++)
                    comboBoxPuttyProfile.Items.Add(HttpUtility.UrlDecode(savedSessionNames[i]));
            }
        }

        private void sessionForm_TextChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = (textBoxSessionName.Text.Length > 0
                && textBoxHostname.Text.Length > 0
                && textBoxPort.Text.Length > 0
                && comboBoxPuttyProfile.Text.Length > 0);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Session.SessionName = textBoxSessionName.Text.Trim();
            Session.PuttySession = comboBoxPuttyProfile.Text.Trim();
            Session.Host = textBoxHostname.Text.Trim();
            Session.Port = int.Parse(textBoxPort.Text.Trim());
            Session.Username = textBoxUsername.Text.Trim();

            for (int i = 0; i < groupBox1.Controls.Count; i++)
            {
                RadioButton rb = (RadioButton)groupBox1.Controls[i];
                if (rb.Checked)
                {
                    Session.Proto = (ConnectionProtocol)rb.Tag;
                }
            }
            
            Session.SaveToRegistry();

            DialogResult = DialogResult.OK;
        }
    }
}
