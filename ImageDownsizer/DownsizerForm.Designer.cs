namespace ImageDownsizer
{
    partial class DownsizerForm
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
            downsizebtn = new Button();
            scalingFactorTextBox = new TextBox();
            imageBox = new PictureBox();
            consequentialTB = new TextBox();
            parallelTB = new TextBox();
            label1 = new Label();
            label2 = new Label();
            addImageBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)imageBox).BeginInit();
            SuspendLayout();
            // 
            // downsizebtn
            // 
            downsizebtn.Location = new Point(581, 47);
            downsizebtn.Name = "downsizebtn";
            downsizebtn.Size = new Size(137, 30);
            downsizebtn.TabIndex = 1;
            downsizebtn.Text = "downsize";
            downsizebtn.UseVisualStyleBackColor = true;
            downsizebtn.Click += downsizebtn_Click;
            // 
            // scalingFactorTextBox
            // 
            scalingFactorTextBox.Location = new Point(581, 92);
            scalingFactorTextBox.Name = "scalingFactorTextBox";
            scalingFactorTextBox.Size = new Size(137, 27);
            scalingFactorTextBox.TabIndex = 2;
            // 
            // imageBox
            // 
            imageBox.Location = new Point(46, 92);
            imageBox.Name = "imageBox";
            imageBox.Size = new Size(392, 305);
            imageBox.SizeMode = PictureBoxSizeMode.StretchImage;
            imageBox.TabIndex = 4;
            imageBox.TabStop = false;
            // 
            // consequentialTB
            // 
            consequentialTB.Location = new Point(566, 195);
            consequentialTB.Name = "consequentialTB";
            consequentialTB.ReadOnly = true;
            consequentialTB.Size = new Size(215, 27);
            consequentialTB.TabIndex = 5;
            // 
            // parallelTB
            // 
            parallelTB.Location = new Point(566, 250);
            parallelTB.Name = "parallelTB";
            parallelTB.ReadOnly = true;
            parallelTB.Size = new Size(215, 27);
            parallelTB.TabIndex = 6;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(457, 202);
            label1.Name = "label1";
            label1.Size = new Size(103, 20);
            label1.TabIndex = 7;
            label1.Text = "Consequential";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(503, 253);
            label2.Name = "label2";
            label2.Size = new Size(57, 20);
            label2.TabIndex = 8;
            label2.Text = "Parallel";
            // 
            // addImageBtn
            // 
            addImageBtn.Location = new Point(46, 48);
            addImageBtn.Name = "addImageBtn";
            addImageBtn.Size = new Size(135, 29);
            addImageBtn.TabIndex = 9;
            addImageBtn.Text = "Choose Image";
            addImageBtn.UseVisualStyleBackColor = true;
            addImageBtn.Click += addImageBtn_Click;
            // 
            // DownsizerForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(addImageBtn);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(parallelTB);
            Controls.Add(consequentialTB);
            Controls.Add(imageBox);
            Controls.Add(scalingFactorTextBox);
            Controls.Add(downsizebtn);
            Name = "DownsizerForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)imageBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button downsizebtn;
        private TextBox scalingFactorTextBox;
        private PictureBox imageBox;
        private TextBox consequentialTB;
        private TextBox parallelTB;
        private Label label1;
        private Label label2;
        private Button addImageBtn;
    }
}
