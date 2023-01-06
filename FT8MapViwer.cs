namespace haselab
{
    public partial class FT8MapViwer : Form
    {

        private bool runningFlg = true;
        private UdpCommunicator udpCom;
        private ImageMgr imgMgr;

        public FT8MapViwer()
        {
            InitializeComponent();
            udpCom = new UdpCommunicator();
            udpCom.onReceive += doReceive;
            udpCom.writeMsg += writeMsg;
            udpCom.toStop += toStop;

            imgMgr = new ImageMgr(pictureBox1.Width, pictureBox1.Height);
            changeStatus();
            pictureBox1.Image = imgMgr.getImageOrg();

        }

        private void btnStartEnd_Click(object sender, EventArgs e)
        {
            changeStatus();
        }

        private String getNow()
        {
            DateTime dt = DateTime.Now;
            return (dt.ToString("yyyy/MM/dd HH:mm:ss"));

        }

        private void changeStatus()
        {
            if (runningFlg)
            {
                toStop();
            }
            else
            {
                toStart();
            }
        }

        private void toStart()
        {
            runningFlg = true;
            stbStatus.Text = "running";
            btnStartEnd.Text = "停止する";
            writeMsg("start");
            udpCom.start();
        }

        private void toStop()
        {
            // to stop statatus
            runningFlg = false;
            stbStatus.Text = "stop";
            btnStartEnd.Text = "開始する";
            writeMsg("stop.");
            udpCom.stop();
        }
        private void doReceive(byte[] bytes)
        {
            string str = System.Text.Encoding.UTF8.GetString(bytes);
            WJSTAnalizer an = new WJSTAnalizer(bytes);
            int mode = an.getHeader();

            writeMsg("size=" + bytes.Length + " header mode=" + mode);
            if (mode != 2) return;
            //            writeMsg("dump:" + an.dump());
            writeMsg("dt:" + an.getDT());
            writeMsg("freq:" + an.getFreq());
            writeMsg("msg:" + an.getMsg());
        }
        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form dlg = new dlIsExit();
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        delegate void SetTextCallback(string text);

        public void writeMsg(String msg)
        {
            if (this.IsDisposed) return;
            if (this.InvokeRequired)
            {
                SetTextCallback delegateMethod = new SetTextCallback(writeMsg);
                this.Invoke(delegateMethod, new object[] { msg });
            }
            else
            {
                string str = getNow() + " : " + msg + "\r\n";
                Console.WriteLine(str);
                tbMessage.AppendText(str);
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form dlg = new dlIsExit();
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.Yes)
            {
                this.Close();
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var p = ConvertCoordinates(((MouseEventArgs)e).Location);
            labPos.Text = "long" + p.X + " : lot" + p.Y;
        }

        private Point ConvertCoordinates(Point location)
        {
            var x = location.X;
            var y = location.Y;
            var picH = pictureBox1.ClientSize.Height;
            var picW = pictureBox1.ClientSize.Width;
            var imgH = pictureBox1.Image.Height;
            var imgW = pictureBox1.Image.Width;

            int X0;
            int Y0;
            if (picW / (float)picH > imgW / (float)imgH)
            {
                var scaledW = imgW * picH / (float)imgH;
                var dx = (picW - scaledW) / 2;
                X0 = (int)((x - dx) * imgH / picH);

                Y0 = (int)(imgH * y / (float)picH);
            }
            else
            {
                X0 = (int)(imgW * x / (float)picW);

                var scaledH = imgH * picW / (float)imgW;
                var dy = (picH - scaledH) / 2;
                Y0 = (int)((y - dy) * imgW / picW);
            }

            if (X0 < 0 || imgW < X0 || Y0 < 0 || imgH < Y0)
            {
                return new Point(-1, -1); // 範囲外をどう表すのがいいか
            }

            return new Point(X0, Y0);
        }
    }
}

