using System.Diagnostics;

namespace haselab
{
    public partial class FT8MapViwer : Form
    {

        private bool runningFlg = true;
        private UdpCommunicator udpCom;
        private ImageMgr imgMgr;
        private List<ReceiveAnime> recieveAnimes = new List<ReceiveAnime>();
        private DataReceiveLED receiveLed;

        public FT8MapViwer()
        {
            InitializeComponent();
            udpCom = new UdpCommunicator();
            udpCom.onReceive += doReceive;
            udpCom.writeMsg += writeMsg;
            receiveLed = new DataReceiveLED();
            receiveLed.setLEDLabel += setReceivingLED;

            imgMgr = new ImageMgr(pictureBox1.Width, pictureBox1.Height);
            changeStatus();

            // this.pictureBox1.MouseHover += new System.EventHandler(this.pictureBox1_MouseHover);
            receiveLed.doStop();
        }

        private void DrawImage()
        {
            debugWrite("Start Draw");

            var img = imgMgr.getImageOrg();
            var po = imgMgr.getMyPicBoxPo();
            Graphics go = Graphics.FromImage(img);


            debugWrite("Start step2");

            var pen = new Pen(Color.Black, 1);
            foreach (var data in recieveAnimes)
            {
                var toLongLat = data.getToLongLat();
                var toPoint = imgMgr.toChangePoint(toLongLat);

                var fromLongLat = data.getStartLongLat();
                var fromPoint = imgMgr.toChangePoint(fromLongLat);

                //go.FillEllipse(Brushes.Black, po2.X, po2.Y ,10, 10);
                go.DrawLine(pen, fromPoint.X, fromPoint.Y, toPoint.X, toPoint.Y);
            }

            debugWrite("Start step3");

            // •`‰æ  
            go.DrawImage(img, 0, 0);
            go.Dispose();

            debugWrite("Start step4");

            pictureBox1.Image= img;
            debugWrite("Start end");

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
        public void debugWrite(String msg)
        {
            Debug.WriteLine(getNow() + " " + msg);
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
            btnStartEnd.Text = "’âŽ~‚·‚é";
            writeMsg("start");
            udpCom.start();
        }

        private void toStop()
        {
            // to stop statatus
            runningFlg = false;
            stbStatus.Text = "stop";
            btnStartEnd.Text = "ŠJŽn‚·‚é";
            writeMsg("stop.");
            udpCom.stop();
        }
        private void doReceive(byte[] bytes)
        {
            string str = System.Text.Encoding.UTF8.GetString(bytes);
            WJSTAnalizer an = new WJSTAnalizer(bytes);
            int mode = an.getHeader();

            // writeMsg("size=" + bytes.Length + " header mode=" + mode);
            receiveLed.doReceive();

            if (mode != 2) return;
            var gr = GridLocator.getLastGridLocator(an.getMsg());
            if (gr == null) return;
            writeMsg("dt:" + an.getDT() + " freq: "
                + an.getFreq() + " msg:[" + an.getMsg() + "] GL:" + gr.getGridLocator());
            var ra = new ReceiveAnime(imgMgr.getMyGridLocator(), gr);
            recieveAnimes.Add(ra);
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

        delegate void SetReceivingLEDCallback(Boolean text);
        public void setReceivingLED(Boolean receivingFlg)
        {
            if (this.IsDisposed) return;
            if (this.InvokeRequired)
            {
                SetReceivingLEDCallback delegateMethod = new SetReceivingLEDCallback(setReceivingLED);
                this.Invoke(delegateMethod, new object[] { receivingFlg });
            }
            else
            {

                if (receivingFlg)
                {
                    this.lblDataRecive.Text = "receiving.";
                    this.lblDataRecive.ForeColor = Color.Red;
                }
                else
                {
                    this.lblDataRecive.Text = "wait.";
                    this.lblDataRecive.ForeColor = Color.Black;
                }
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

        private void timer1_Tick(object sender, EventArgs e)
        {

            receiveLed.doExec();
            if (recieveAnimes.Count == 0) return;
            List<int> deleteList = new List<int>();
            int i = 0;
            foreach (var data in recieveAnimes)
            {
                var co = data.doExec();
                if (co <= 0)
                {
                    deleteList.Add(i);
                }
                i++;
            }
            deleteList.Reverse();
            foreach (var index in deleteList)
            {
                recieveAnimes.RemoveAt(index);
            }

            this.DrawImage();

        }
    }
}


