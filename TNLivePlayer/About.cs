using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace TNLivePlayer
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();

            lblVersion.Text = AssemblyInfo.Current.AssemblyVersion;
            lblName.Text = AssemblyInfo.Current.AssemblyProduct;
        }

        private void LaunchLink(string url, bool mailTo)
        {
            try
            {
                Process p = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();

                if(mailTo)
                    url = "mailto:" + lblEmail.Text + "?subject=" + AssemblyInfo.Current.AssemblyProduct;

                startInfo.FileName = url;
                p.StartInfo = startInfo;
                p.Start();
            }
            catch (Exception) { }
        }

        private void lblBlog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.LaunchLink(lblBlog.Text, false);
        }

        private void lblEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.LaunchLink(lblEmail.Text, true);
        }

        private void lblFacebook_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.LaunchLink(lblFacebook.Text, false);
        }
    }
}
