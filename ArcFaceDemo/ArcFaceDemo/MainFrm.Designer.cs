namespace ArcFaceDemo
{
    partial class MainFrm
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
            this.logBox = new System.Windows.Forms.TextBox();
            this.btnRegisterFace = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelectImageToRegister = new System.Windows.Forms.Button();
            this.pictureBoxSelected = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSelectImageToRecognize = new System.Windows.Forms.Button();
            this.pictureBoxToRecognize = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSelected)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxToRecognize)).BeginInit();
            this.SuspendLayout();
            // 
            // logBox
            // 
            this.logBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logBox.Location = new System.Drawing.Point(12, 252);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.Size = new System.Drawing.Size(687, 186);
            this.logBox.TabIndex = 0;
            // 
            // btnRegisterFace
            // 
            this.btnRegisterFace.Location = new System.Drawing.Point(6, 194);
            this.btnRegisterFace.Name = "btnRegisterFace";
            this.btnRegisterFace.Size = new System.Drawing.Size(132, 23);
            this.btnRegisterFace.TabIndex = 1;
            this.btnRegisterFace.Text = "Register Face";
            this.btnRegisterFace.UseVisualStyleBackColor = true;
            this.btnRegisterFace.Click += new System.EventHandler(this.btnRegisterFace_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSelectImageToRegister);
            this.groupBox1.Controls.Add(this.pictureBoxSelected);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxName);
            this.groupBox1.Controls.Add(this.btnRegisterFace);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(386, 234);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Face Manage";
            // 
            // btnSelectImageToRegister
            // 
            this.btnSelectImageToRegister.Location = new System.Drawing.Point(6, 74);
            this.btnSelectImageToRegister.Name = "btnSelectImageToRegister";
            this.btnSelectImageToRegister.Size = new System.Drawing.Size(132, 23);
            this.btnSelectImageToRegister.TabIndex = 5;
            this.btnSelectImageToRegister.Text = "Select Image";
            this.btnSelectImageToRegister.UseVisualStyleBackColor = true;
            this.btnSelectImageToRegister.Click += new System.EventHandler(this.btnSelectImageToRegister_Click);
            // 
            // pictureBoxSelected
            // 
            this.pictureBoxSelected.Location = new System.Drawing.Point(144, 74);
            this.pictureBoxSelected.Name = "pictureBoxSelected";
            this.pictureBoxSelected.Size = new System.Drawing.Size(220, 143);
            this.pictureBoxSelected.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxSelected.TabIndex = 4;
            this.pictureBoxSelected.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Name";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(144, 28);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(220, 21);
            this.textBoxName.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBoxToRecognize);
            this.groupBox2.Controls.Add(this.btnSelectImageToRecognize);
            this.groupBox2.Location = new System.Drawing.Point(415, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(282, 234);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Face Recognition";
            // 
            // btnSelectImageToRecognize
            // 
            this.btnSelectImageToRecognize.Location = new System.Drawing.Point(6, 26);
            this.btnSelectImageToRecognize.Name = "btnSelectImageToRecognize";
            this.btnSelectImageToRecognize.Size = new System.Drawing.Size(248, 23);
            this.btnSelectImageToRecognize.TabIndex = 1;
            this.btnSelectImageToRecognize.Text = "Select Image And Recognize";
            this.btnSelectImageToRecognize.UseVisualStyleBackColor = true;
            this.btnSelectImageToRecognize.Click += new System.EventHandler(this.btnSelectImageToRecognize_Click);
            // 
            // pictureBoxToRecognize
            // 
            this.pictureBoxToRecognize.Location = new System.Drawing.Point(6, 74);
            this.pictureBoxToRecognize.Name = "pictureBoxToRecognize";
            this.pictureBoxToRecognize.Size = new System.Drawing.Size(248, 143);
            this.pictureBoxToRecognize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxToRecognize.TabIndex = 2;
            this.pictureBoxToRecognize.TabStop = false;
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 450);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.logBox);
            this.Name = "MainFrm";
            this.Text = "ArcFace Demo";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSelected)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxToRecognize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Button btnRegisterFace;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSelectImageToRegister;
        private System.Windows.Forms.PictureBox pictureBoxSelected;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSelectImageToRecognize;
        private System.Windows.Forms.PictureBox pictureBoxToRecognize;
    }
}

