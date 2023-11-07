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
            pictureBox2 = new PictureBox();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
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
            // pictureBox2
            // 
            pictureBox2.BackColor = SystemColors.WindowFrame;
            pictureBox2.Location = new Point(335, 27);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(274, 244);
            pictureBox2.TabIndex = 7;
            pictureBox2.TabStop = false;
            // 
            // button1
            // 
            button1.BackColor = SystemColors.ActiveCaption;
            button1.FlatStyle = FlatStyle.Popup;
            button1.Font = new Font("Times New Roman", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(544, 318);
            button1.Name = "button1";
            button1.Size = new Size(65, 23);
            button1.TabIndex = 8;
            button1.Text = "Clear";
            button1.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ScrollBar;
            ClientSize = new Size(648, 505);
            Controls.Add(button1);
            Controls.Add(pictureBox2);
            Controls.Add(textBoxCmdLine);
            Controls.Add(pictureBox1);
            Controls.Add(button2);
            Controls.Add(btnRun);
            Controls.Add(textBox1);
            MaximizeBox = false;
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox textBox1;
        private Button btnRun;
        private Button button2;
        private PictureBox pictureBox1;
        private TextBox textBoxCmdLine;
        private PictureBox pictureBox2;
        private Button button1;
    }
}