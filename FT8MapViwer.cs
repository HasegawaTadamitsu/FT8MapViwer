namespace haselab
{
    public partial class FT8MapViwer : Form
    {

        private bool runningFlg = true;
        private UdpCommunicator udpCom;

        public FT8MapViwer()
        {
            InitializeComponent();
            udpCom = new UdpCommunicator();
            udpCom.onReceive += doReceive;
            udpCom.writeMsg += writeMsg;
            udpCom.toStop += toStop;
            changeStatus();
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
            Console.WriteLine("受信したデータ:{0}", str);
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
    }

}