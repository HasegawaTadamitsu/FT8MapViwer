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
            Form1_Resize(null, null);

            mat = new System.Drawing.Drawing2D.Matrix((float)0.44444445, (float)0, (float)0, (float)0.44444445, (float)-5232.223, (float)-716.41693);
            DrawImage();

            // this.pictureBox1.MouseHover += new System.EventHandler(this.pictureBox1_MouseHover);
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
            // PictureBox�Ɠ����傫����Bitmap�N���X���쐬����B  
            Bitmap bmpPicBox = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            // ���Bitmap��PictureBox��Image�Ɏw�肷��B  
            pictureBox1.Image = bmpPicBox;
            // Graphics�I�u�W�F�N�g�̍쐬(FromImage���g��)  
            Graphics gp = Graphics.FromImage(pictureBox1.Image);
            // �A�t�B���ϊ��s��̐ݒ�  
            if (mat != null)
            {
                gp.Transform = mat;
            }

            // ��ԃ��[�h�̐ݒ�
            gp.InterpolationMode
                = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            // �A�t�B���ϊ��s��̐ݒ�  
            if ((mat != null))
            {
                gp.Transform = mat;
            }
            // �s�N�`���{�b�N�X�̃N���A  
            gp.Clear(pictureBox1.BackColor);

            var img = imgMgr.getImageOrg();
            Graphics go = Graphics.FromImage(img);
            //Pen p = new Pen(Color.Black, 10);
            //go.DrawRectangle(p, img.Width / 3, img.Height / 3,
            //    img.Width / 2, img.Height / 2);
            var po = imgMgr.getMyPicBoxPo(new Point(img.Width, img.Height));
            int zz = (img.Width - img.Height) / 100;
            go.FillEllipse(Brushes.Black, po.X, po.Y, zz, zz);

            // debug for initial mat
            /*String msg = "";
            foreach(var data in mat.Elements)
            {
                msg += " " + data;
            }
            writeMsg("mat = " + msg);
            */

            // �`��  
            gp.DrawImage(img, 0, 0);
            go.Dispose();
            gp.Dispose();
            // �ĕ`��  
            pictureBox1.Refresh();
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
    }
}


