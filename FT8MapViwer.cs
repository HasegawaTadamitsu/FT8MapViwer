using System.Diagnostics;

namespace haselab
{
    public partial class FT8MapViwer : Form
    {

        private bool runningFlg = true;
        private UdpCommunicator udpCom;
        private ImageMgr imgMgr;
        // マウスをクリックした位置の保持用  
        private PointF OldPoint;
        // アフィン変換行列  
        private System.Drawing.Drawing2D.Matrix mat;
        // マウスダウンフラグ  
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
            // 画像の描画  
            DrawImage();

        }
        // ビットマップの描画  
        private void DrawImage()
        {
            debugWrite("Start Draw");

            // PictureBoxと同じ大きさのBitmapクラスを作成する。  
            Bitmap bmpPicBox = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            // 空のBitmapをPictureBoxのImageに指定する。  
            pictureBox1.Image = bmpPicBox;
            // Graphicsオブジェクトの作成(FromImageを使う)  
            Graphics gp = Graphics.FromImage(pictureBox1.Image);
            // アフィン変換行列の設定  
            gp.Transform = mat;



            debugWrite("Start step1");

            // 補間モードの設定
            gp.InterpolationMode
                = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            // ピクチャボックスのクリア  
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

            // 描画  
            gp.DrawImage(img, 0, 0);
            go.Dispose();
            gp.Dispose();

            debugWrite("Start step4");

            // 再描画  
            pictureBox1.Refresh();
            debugWrite("Start end");

        }

        // マウスホイールイベント  
        private void pictureBox1_MouseWheel(object? sender, MouseEventArgs e)
        {
            // ポインタの位置→原点へ移動  
            mat.Translate(-e.X, -e.Y,
                System.Drawing.Drawing2D.MatrixOrder.Append);
            if (e.Delta > 0)
            {
                // 拡大  
                if (mat.Elements[0] < 100)  // X方向の倍率を代表してチェック  
                {
                    mat.Scale(1.5f, 1.5f,
                        System.Drawing.Drawing2D.MatrixOrder.Append);
                }
            }
            else
            {
                // 縮小  
                if (mat.Elements[0] > 0.01)  // X方向の倍率を代表してチェック  
                {
                    mat.Scale(1.0f / 1.5f, 1.0f / 1.5f,
                        System.Drawing.Drawing2D.MatrixOrder.Append);
                }
            }
            // 原点→ポインタの位置へ移動(元の位置へ戻す)  
            mat.Translate(e.X, e.Y,
                System.Drawing.Drawing2D.MatrixOrder.Append);
            // 画像の描画  
            DrawImage();
        }
        private void pictureBox1_MouseDown(object? sender, MouseEventArgs e)
        {
            // 右ボタンがクリックされたとき  
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // アフィン変換行列に単位行列を設定する  
                mat.Reset();
                // 画像の描画  
                DrawImage();

                return;
            }
            // フォーカスの設定  
            //（クリックしただけではMouseWheelイベントが有効にならない）  
            pictureBox1.Focus();
            // マウスをクリックした位置の記録  
            OldPoint.X = e.X;
            OldPoint.Y = e.Y;
            // マウスダウンフラグ  
            MouseDownFlg = true;
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            // マウスをクリックしながら移動中のとき  
            if (MouseDownFlg == true)
            {
                // 画像の移動  
                mat.Translate(e.X - OldPoint.X, e.Y - OldPoint.Y,
                    System.Drawing.Drawing2D.MatrixOrder.Append);
                // 画像の描画  
                DrawImage();

                // ポインタ位置の保持  
                OldPoint.X = e.X;
                OldPoint.Y = e.Y;
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            // マウスダウンフラグ  
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


