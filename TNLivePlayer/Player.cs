using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Net;
using System.IO;
using System.Threading;

namespace TNLivePlayer
{
    public partial class Player : Form
    {
        string _streamUrl;
        object _playLock;
        string _streamUpdateUrlFileName;

        public Player()
        {
            InitializeComponent();

            this.Load += new EventHandler(Player_Load);
            this.Resize += new EventHandler(Player_Resize);
            this.FormClosing += new FormClosingEventHandler(Player_FormClosing);

            _streamUrl = "http://www.tnlrocks.com/listen_live/player_flash.php";
            _playLock = new object();
                        
            _streamUpdateUrlFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TNLivePlayer\\playerurl.txt");
            if (File.Exists(_streamUpdateUrlFileName))
            {
                string fileContent = File.ReadAllText(_streamUpdateUrlFileName);
                fileContent = fileContent.Trim();
                if (!String.IsNullOrEmpty(fileContent))
                    _streamUrl = fileContent;
            }
        }

        private void Play()
        {
            lock (_playLock)
            {                
                wbPlayer.AllowNavigation = true;
                wbPlayer.Navigate(_streamUrl);
            }
        }

        private void UpdateStreamUrl()
        {
            try
            {
                string streamUpdateUrl = "http://dl.dropbox.com/u/58297906/Projects/TNLivePlayer/playerurl.txt";

                WebClient wc = new WebClient();
                string newUrl = wc.DownloadString(streamUpdateUrl);

                if (!String.IsNullOrEmpty(newUrl) && !newUrl.Equals(_streamUrl))
                {
                    _streamUrl = newUrl;
                    string dir = Path.GetDirectoryName(_streamUpdateUrlFileName);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    File.WriteAllText(_streamUpdateUrlFileName, newUrl);

                    this.RefreshPlayer();
                }
            }
            catch (Exception) { }
        }

        private void Pause()
        {
            wbPlayer.AllowNavigation = true;
            wbPlayer.Navigate("about:blank");            
        }

        private void RefreshPlayer()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(this.RefreshPlayer));
            else
            {
                wbPlayer.Stop();
                this.Play();
                wbPlayer.Refresh(WebBrowserRefreshOption.Completely);
            }
        }

        void Player_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        void Player_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.Visible = false;                
        }

        void Player_Load(object sender, EventArgs e)
        {
            this.Play();

            Thread updateStreamUrlThread = new Thread(new ThreadStart(UpdateStreamUrl));
            updateStreamUrlThread.IsBackground = true;
            updateStreamUrlThread.Start();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.RefreshPlayer();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        private void niPlayer_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Play();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Pause();
        }

        private void wbPlayer_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            wbPlayer.AllowNavigation = false;
        }

    }
}
