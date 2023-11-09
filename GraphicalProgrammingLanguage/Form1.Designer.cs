namespace GraphicalProgrammingLanguage
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            textBox1 = new TextBox();
            btnRun = new Button();
            button2 = new Button();
            pictureBox1 = new PictureBox();
            textBoxCmdLine = new TextBox();
            pictureBoxDraw = new PictureBox();
            btnClear = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDraw).BeginInit();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(24, 289);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(345, 23);
            textBox1.TabIndex = 1;
            // 
            // btnRun
            // 
            btnRun.BackColor = SystemColors.ActiveCaption;
            btnRun.FlatStyle = FlatStyle.Popup;
            btnRun.Font = new Font("Times New Roman", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnRun.Location = new Point(38, 318);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(65, 23);
            btnRun.TabIndex = 2;
            btnRun.Text = "Run";
            btnRun.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = SystemColors.ActiveCaption;
            button2.FlatStyle = FlatStyle.Popup;
            button2.Font = new Font("Times New Roman", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button2.Location = new Point(109, 318);
            button2.Name = "button2";
            button2.Size = new Size(65, 23);
            button2.TabIndex = 3;
            button2.Text = "Syntax";
            button2.UseVisualStyleBackColor = false;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = SystemColors.ControlDarkDark;
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Location = new Point(38, 356);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(571, 137);
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // textBoxCmdLine
            // 
            textBoxCmdLine.Location = new Point(24, 27);
            textBoxCmdLine.Multiline = true;
            textBoxCmdLine.Name = "textBoxCmdLine";
            textBoxCmdLine.Size = new Size(285, 244);
            textBoxCmdLine.TabIndex = 6;
            // 
            // pictureBoxDraw
            // 
            pictureBoxDraw.BackColor = SystemColors.WindowFrame;
            pictureBoxDraw.Location = new Point(335, 27);
            pictureBoxDraw.Name = "pictureBoxDraw";
            pictureBoxDraw.Size = new Size(274, 244);
            pictureBoxDraw.TabIndex = 7;
            pictureBoxDraw.TabStop = false;
            pictureBoxDraw.Paint += pictureBoxDraw_Paint;
            // 
            // btnClear
            // 
            btnClear.BackColor = SystemColors.ActiveCaption;
            btnClear.FlatStyle = FlatStyle.Popup;
            btnClear.Font = new Font("Times New Roman", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnClear.Location = new Point(544, 318);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(65, 23);
            btnClear.TabIndex = 8;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ScrollBar;
            ClientSize = new Size(648, 505);
            Controls.Add(btnClear);
            Controls.Add(pictureBoxDraw);
            Controls.Add(textBoxCmdLine);
            Controls.Add(pictureBox1);
            Controls.Add(button2);
            Controls.Add(btnRun);
            Controls.Add(textBox1);
            MaximizeBox = false;
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDraw).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox textBox1;
        private Button btnRun;
        private Button button2;
        private PictureBox pictureBox1;
        private TextBox textBoxCmdLine;
        private PictureBox pictureBoxDraw;
        private Button btnClear;
    }
}