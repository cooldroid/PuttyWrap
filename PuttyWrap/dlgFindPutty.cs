using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PuttyWrap
{
    public partial class dlgFindPutty : Form
    {
        private string m_PuttyLocation;

        public string PuttyLocation
        {
            get { return m_PuttyLocation; }
            private set { m_PuttyLocation = value; }
        }
        private string m_PscpLocation;

        public string PscpLocation
        {
            get { return m_PscpLocation; }
            private set { m_PscpLocation = value; }
        }

        public dlgFindPutty()
        {
            InitializeComponent();


            // check for location of putty/pscp
            if (!String.IsNullOrEmpty(frmPuttyWrap.PuttyExe) && File.Exists(frmPuttyWrap.PuttyExe))
            {
                textBoxPuttyLocation.Text = frmPuttyWrap.PuttyExe;
                if (!String.IsNullOrEmpty(frmPuttyWrap.PscpExe) && File.Exists(frmPuttyWrap.PscpExe))
                {
                    textBoxPscpLocation.Text = frmPuttyWrap.PscpExe;
                }
            }
            else if(!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("ProgramFiles(x86)")))
            {
                if (File.Exists(Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\Common Files\putty SSH\putty.exe"))
                {
                    textBoxPuttyLocation.Text = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\Common Files\putty SSH\putty.exe";
                    openFileDialog1.InitialDirectory = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
                }

                if (File.Exists(Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\Common Files\putty SSH\pscp.exe"))
                {

                    textBoxPscpLocation.Text = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\Common Files\putty SSH\pscp.exe";
                }
            }
            else if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("ProgramFiles")))
            {
                if (File.Exists(Environment.GetEnvironmentVariable("ProgramFiles") + @"\Common Files\putty SSH\putty.exe"))
                {
                    textBoxPuttyLocation.Text = Environment.GetEnvironmentVariable("ProgramFiles") + @"\Common Files\putty SSH\putty.exe";
                    openFileDialog1.InitialDirectory = Environment.GetEnvironmentVariable("ProgramFiles");
                }

                if (File.Exists(Environment.GetEnvironmentVariable("ProgramFiles") + @"\Common Files\putty SSH\pscp.exe"))
                {
                    textBoxPscpLocation.Text = Environment.GetEnvironmentVariable("ProgramFiles") + @"\Common Files\putty SSH\pscp.exe";
                }
            }            
            else
            {
                openFileDialog1.InitialDirectory = Application.StartupPath;
            }                
        }
       
        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxPscpLocation.Text) && File.Exists(textBoxPscpLocation.Text))
            {
                PscpLocation = textBoxPscpLocation.Text;
            }

            if (!String.IsNullOrEmpty(textBoxPuttyLocation.Text) && File.Exists(textBoxPuttyLocation.Text))
            {
                PuttyLocation = textBoxPuttyLocation.Text;
                DialogResult = DialogResult.OK;
            }
            else
            {
                if (MessageBox.Show("PuTTY is required to properly use this application.", "PuTTY Required", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                    System.Environment.Exit(1);
                }
            }                        
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "PuTTY|putty.exe";
            openFileDialog1.FileName = "putty.exe";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!String.IsNullOrEmpty(openFileDialog1.FileName))
                    textBoxPuttyLocation.Text = openFileDialog1.FileName;
            }            
        }

        private void buttonBrowsePscp_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "PScp|pscp.exe";
            openFileDialog1.FileName = "pscp.exe";
            openFileDialog1.ShowDialog();
            if (!String.IsNullOrEmpty(openFileDialog1.FileName))
                textBoxPscpLocation.Text = openFileDialog1.FileName;
        }
    }
}
