/// <summary>
/// Summary description for Class1
/// </summary>
public class dlIsExit : Form
{

    private Button btnNo;
    private Button btnYes;
    private Label lblIsExit;

    public dlIsExit()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.btnNo = new System.Windows.Forms.Button();
        this.btnYes = new System.Windows.Forms.Button();
        this.lblIsExit = new System.Windows.Forms.Label();
        this.SuspendLayout();
        // 
        // btnNo
        // 
        this.btnNo.Location = new System.Drawing.Point(137, 41);
        this.btnNo.Name = "btnNo";
        this.btnNo.Size = new System.Drawing.Size(75, 23);
        this.btnNo.TabIndex = 0;
        this.btnNo.Text = "いいえ(&N)";
        this.btnNo.UseVisualStyleBackColor = true;
        this.btnYes.DialogResult = DialogResult.No;

        // 
        // btnYes
        // 
        this.btnYes.Location = new System.Drawing.Point(35, 41);
        this.btnYes.Name = "btnYes";
        this.btnYes.Size = new System.Drawing.Size(75, 23);
        this.btnYes.TabIndex = 1;
        this.btnYes.Text = "はい(&Y)";
        this.btnYes.UseVisualStyleBackColor = true;
        this.btnYes.DialogResult = DialogResult.Yes;

        // 
        // lblIsExit
        // 
        this.lblIsExit.AutoSize = true;
        this.lblIsExit.Location = new System.Drawing.Point(35, 9);
        this.lblIsExit.Name = "lblIsExit";
        this.lblIsExit.Size = new System.Drawing.Size(110, 15);
        this.lblIsExit.TabIndex = 2;
        this.lblIsExit.Text = "終了していいですか？";
        // 
        // dlIsExit
        // 
        this.ClientSize = new System.Drawing.Size(227, 75);
        this.Controls.Add(this.lblIsExit);
        this.Controls.Add(this.btnYes);
        this.Controls.Add(this.btnNo);
        this.Name = "dlIsExit";
        this.Text = "確認";
        this.ResumeLayout(false);
        this.PerformLayout();

        this.AcceptButton = this.btnYes;  // Enter キーで選択できるボタン
        this.CancelButton = this.btnNo;   // Esc キーで選択できるボタン
    }
}
