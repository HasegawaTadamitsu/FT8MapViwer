using System.Diagnostics;

namespace haselab
{
    public partial class FT8MapViwer : Form
    {

        private bool runningFlg = true;
        private UdpCommunicator udpCom;
        private ImageMgr imgMgr;
        // �}�E�X���N���b�N�����ʒu�̕ێ��p  
        private PointF OldPoint;
        // �A�t�B���ϊ��s��  
        private System.Drawing.Drawing2D.Matrix mat;
        // �}�E�X�_�E���t���O  
        private bool MouseDownFlg = false;
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
            pictureBox1.Image = imgMgr.getImageOrg();
            Form1_Resize(null, null);

            mat = new System.Drawing.Drawing2D.Matrix((float)0.44444445, (float)0, (float)0, (float)0.44444445, (float)-5232.223, (float)-716.41693);
            DrawImage();

            // this.pictureBox1.MouseHover += new System.EventHandler(this.pictureBox1_MouseHover);
            receiveLed.doStop();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (mat == null)
            {
                Bitmap backImage = imgMgr.getImageOrg();
                Graphics g = Graphics.FromImage(backImage);
                mat = g.Transform;
                g.Dispose();
                g = null;
            }
            // �摜�̕`��  
            DrawImage();

        }
        // �r�b�g�}�b�v�̕`��  
        private void DrawImage()
        {
            debugWrite("Start Draw");

            // PictureBox�Ɠ����傫����Bitmap�N���X���쐬����B  
            Bitmap bmpPicBox = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            // ���Bitmap��PictureBox��Image�Ɏw�肷��B  
            pictureBox1.Image = bmpPicBox;
            // Graphics�I�u�W�F�N�g�̍쐬(FromImage���g��)  
            Graphics gp = Graphics.FromImage(pictureBox1.Image);
            // �A�t�B���ϊ��s��̐ݒ�  
            gp.Transform = mat;



            debugWrite("Start step1");

            // ��ԃ��[�h�̐ݒ�
            gp.InterpolationMode
                = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            // �s�N�`���{�b�N�X�̃N���A  
            gp.Clear(pictureBox1.BackColor);

            var img = imgMgr.getImageOrg();
            Graphics go = Graphics.FromImage(img);
            //Pen p = new Pen(Color.Black, 10);
            //go.DrawRectangle(p, img.Width / 3, img.Height / 3,
            //    img.Width/ 2, img.Height / 2);
            var po = imgMgr.getMyPicBoxPo();
            // var el = mat.Elements;
            // int zz = (int)Math.Round((float)img.Width / el[0] * 10000.0);
            // int zz = (int)Math.Round(1 / el[0] * 10.0);
            //writeMsg("mat = " + zz + " el[0]" + el[0] + " img.Width" + img.Width);
            // go.FillEllipse(Brushes.Black, po.X, po.Y, zz, zz);

            debugWrite("Start step2");

            var pen = new Pen(Color.Black, 1);
            foreach (var data in recieveAnimes)
            {
                var longLat = data.getLongLat();
                var po2 = imgMgr.toChangePoint(longLat);

                //go.FillEllipse(Brushes.Black, po2.X, po2.Y ,10, 10);
                go.DrawLine(pen, po.X, po.Y, po2.X, po2.Y);
            }

            debugWrite("Start step3");

            // �`��  
            gp.DrawImage(img, 0, 0);
            go.Dispose();
            gp.Dispose();

            debugWrite("Start step4");

            // �ĕ`��  
            pictureBox1.Refresh();
            debugWrite("Start end");

        }

        // �}�E�X�z�C�[���C�x���g  
        private void pictureBox1_MouseWheel(object? sender, MouseEventArgs e)
        {
            // �|�C���^�̈ʒu�����_�ֈړ�  
            mat.Translate(-e.X, -e.Y,
                System.Drawing.Drawing2D.MatrixOrder.Append);
            if (e.Delta > 0)
            {
                // �g��  
                if (mat.Elements[0] < 100)  // X�����̔{�����\���ă`�F�b�N  
                {
                    mat.Scale(1.5f, 1.5f,
                        System.Drawing.Drawing2D.MatrixOrder.Append);
                }
            }
            else
            {
                // �k��  
                if (mat.Elements[0] > 0.01)  // X�����̔{�����\���ă`�F�b�N  
                {
                    mat.Scale(1.0f / 1.5f, 1.0f / 1.5f,
                        System.Drawing.Drawing2D.MatrixOrder.Append);
                }
            }
            // ���_���|�C���^�̈ʒu�ֈړ�(���̈ʒu�֖߂�)  
            mat.Translate(e.X, e.Y,
                System.Drawing.Drawing2D.MatrixOrder.Append);
            // �摜�̕`��  
            DrawImage();
        }
        private void pictureBox1_MouseDown(object? sender, MouseEventArgs e)
        {
            // �E�{�^�����N���b�N���ꂽ�Ƃ�  
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // �A�t�B���ϊ��s��ɒP�ʍs���ݒ肷��  
                mat.Reset();
                // �摜�̕`��  
                DrawImage();

                return;
            }
            // �t�H�[�J�X�̐ݒ�  
            //�i�N���b�N���������ł�MouseWheel�C�x���g���L���ɂȂ�Ȃ��j  
            pictureBox1.Focus();
            // �}�E�X���N���b�N�����ʒu�̋L�^  
            OldPoint.X = e.X;
            OldPoint.Y = e.Y;
            // �}�E�X�_�E���t���O  
            MouseDownFlg = true;
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            // �}�E�X���N���b�N���Ȃ���ړ����̂Ƃ�  
            if (MouseDownFlg == true)
            {
                // �摜�̈ړ�  
                mat.Translate(e.X - OldPoint.X, e.Y - OldPoint.Y,
                    System.Drawing.Drawing2D.MatrixOrder.Append);
                // �摜�̕`��  
                DrawImage();

                // �|�C���^�ʒu�̕ێ�  
                OldPoint.X = e.X;
                OldPoint.Y = e.Y;
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            // �}�E�X�_�E���t���O  
            MouseDownFlg = false;
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
            btnStartEnd.Text = "��~����";
            writeMsg("start");
            udpCom.start();
        }

        private void toStop()
        {
            // to stop statatus
            runningFlg = false;
            stbStatus.Text = "stop";
            btnStartEnd.Text = "�J�n����";
            writeMsg("stop.");
            udpCom.stop();
        }
        private void doReceive(byte[] bytes)
        {
            string str = System.Text.Encoding.UTF8.GetString(bytes);
            WJSTAnalizer an = new WJSTAnalizer(bytes);
            int mode = an.getHeader();

            writeMsg("size=" + bytes.Length + " header mode=" + mode);
            receiveLed.doReceive();

            if (mode != 2) return;
            var gr = GridLocator.getLastGridLocator(an.getMsg());
            if (gr == null) return;
            writeMsg("dt:" + an.getDT() + " freq: "
                + an.getFreq() + " msg: " + an.getMsg() + " GR:" + gr.getGridLocator());
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


