namespace ChessClient;

partial class FormLauncher
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnJoinServer = new System.Windows.Forms.Button();
            this.btnHostServer = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::ChessClient.Resources.WhiteRook;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnJoinServer
            // 
            this.btnJoinServer.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnJoinServer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnJoinServer.FlatAppearance.BorderSize = 0;
            this.btnJoinServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJoinServer.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnJoinServer.ForeColor = System.Drawing.Color.White;
            this.btnJoinServer.Location = new System.Drawing.Point(144, 275);
            this.btnJoinServer.Name = "btnJoinServer";
            this.btnJoinServer.Size = new System.Drawing.Size(99, 23);
            this.btnJoinServer.TabIndex = 1;
            this.btnJoinServer.Text = "Join Server";
            this.btnJoinServer.UseVisualStyleBackColor = false;
            this.btnJoinServer.Click += new System.EventHandler(this.btnJoinServer_Click);
            // 
            // btnHostServer
            // 
            this.btnHostServer.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnHostServer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnHostServer.FlatAppearance.BorderSize = 0;
            this.btnHostServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHostServer.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnHostServer.ForeColor = System.Drawing.Color.White;
            this.btnHostServer.Location = new System.Drawing.Point(249, 275);
            this.btnHostServer.Name = "btnHostServer";
            this.btnHostServer.Size = new System.Drawing.Size(99, 23);
            this.btnHostServer.TabIndex = 1;
            this.btnHostServer.Text = "Host Server";
            this.btnHostServer.UseVisualStyleBackColor = false;
            this.btnHostServer.Click += new System.EventHandler(this.btnHostServer_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::ChessClient.Resources.BlackQueen;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.InitialImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(383, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(100, 100);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(118, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(259, 100);
            this.label1.TabIndex = 2;
            this.label1.Text = "Chess";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(495, 315);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnHostServer);
            this.Controls.Add(this.btnJoinServer);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "FormLauncher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chess";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private PictureBox pictureBox1;
    private Button btnJoinServer;
    private Button btnHostServer;
    private PictureBox pictureBox2;
    private Label label1;
}