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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(scalingFactorTextBox);
            Controls.Add(downsizebtn);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button downsizebtn;
        private TextBox scalingFactorTextBox;
    }
}
