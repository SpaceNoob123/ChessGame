namespace ChessClient;

partial class FormUDPClient
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;


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
            this.pnlBoard = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlBoard
            // 
            this.pnlBoard.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBoard.Location = new System.Drawing.Point(0, 0);
            this.pnlBoard.Name = "pnlBoard";
            this.pnlBoard.Size = new System.Drawing.Size(452, 446);
            this.pnlBoard.TabIndex = 3;
            // 
            // FormUDPClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(452, 446);
            this.Controls.Add(this.pnlBoard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FormUDPClient";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Chess";
            this.ResumeLayout(false);

    }

    #endregion
    private Panel pnlBoard;
}