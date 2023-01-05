/// <summary>
/// Summary description for Class1
/// </summary>
public class Config : Form
{
    private Label label1;
    private TextBox textBox1;
    private TextBox tbReciveIP;
    private TextBox tbPort;
    private Button btnHozon;
    private Button btnCancel;
    private Label label2;

    public Config()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.label1 = new System.Windows.Forms.Label();
        this.textBox1 = new System.Windows.Forms.TextBox();
        this.tbReciveIP = new System.Windows.Forms.TextBox();
        this.label2 = new System.Windows.Forms.Label();
        this.tbPort = new System.Windows.Forms.TextBox();
        this.btnHozon = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(23, 52);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(69, 15);
        this.label1.TabIndex = 0;
        this.label1.Text = "GridLocator";
        // 
        // textBox1
        // 
        this.textBox1.Location = new System.Drawing.Point(58, 70);
        this.textBox1.Name = "textBox1";
        this.textBox1.Size = new System.Drawing.Size(100, 23);
        this.textBox1.TabIndex = 1;
        // 
        // tbReciveIP
        // 
        this.tbReciveIP.Location = new System.Drawing.Point(58, 161);
        this.tbReciveIP.Name = "tbReciveIP";
        this.tbReciveIP.Size = new System.Drawing.Size(100, 23);
        this.tbReciveIP.TabIndex = 2;
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(23, 143);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(103, 15);
        this.label2.TabIndex = 3;
        this.label2.Text = "Receive　IP/PORT";
        // 
        // tbPort
        // 
        this.tbPort.Location = new System.Drawing.Point(180, 161);
        this.tbPort.Name = "tbPort";
        this.tbPort.Size = new System.Drawing.Size(100, 23);
        this.tbPort.TabIndex = 4;
        this.tbPort.Text = "2233";
        // 
        // btnHozon
        // 
        this.btnHozon.Location = new System.Drawing.Point(373, 362);
        this.btnHozon.Name = "btnHozon";
        this.btnHozon.Size = new System.Drawing.Size(75, 23);
        this.btnHozon.TabIndex = 5;
        this.btnHozon.Text = "保存";
        this.btnHozon.UseVisualStyleBackColor = true;
        // 
        // btnCancel
        // 
        this.btnCancel.Location = new System.Drawing.Point(454, 362);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(75, 23);
        this.btnCancel.TabIndex = 6;
        this.btnCancel.Text = "キャンセル";
        this.btnCancel.UseVisualStyleBackColor = true;
        // 
        // Config
        // 
        this.ClientSize = new System.Drawing.Size(579, 397);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnHozon);
        this.Controls.Add(this.tbPort);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.tbReciveIP);
        this.Controls.Add(this.textBox1);
        this.Controls.Add(this.label1);
        this.Name = "Config";
        this.Text = "Config(設定)";
        this.ResumeLayout(false);
        this.PerformLayout();

        this.AcceptButton = this.btnHozon;  // Enter キーで選択できるボタン
        this.CancelButton = this.btnCancel;   // Esc キーで選択できるボタン
    }
}
