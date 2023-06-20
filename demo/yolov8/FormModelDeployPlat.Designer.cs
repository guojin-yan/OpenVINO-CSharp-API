namespace yolov8
{
    partial class FormModelDeployPlat
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
            pictureBox2 = new PictureBox();
            label2 = new Label();
            tb_model_path = new TextBox();
            btn_choose_model_path = new Button();
            groupBox2 = new GroupBox();
            rb_yolov8_cls = new RadioButton();
            rb_yolov8_pose = new RadioButton();
            rb_yolov8_seg = new RadioButton();
            rb_yolov8_det = new RadioButton();
            label3 = new Label();
            tb_clas_path = new TextBox();
            label4 = new Label();
            tb_test_image = new TextBox();
            btn_choose_claspath = new Button();
            btn_choose_testimage = new Button();
            btn_model_deploy = new Button();
            textBox1 = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = SystemColors.AppWorkspace;
            pictureBox2.Location = new Point(34, 217);
            pictureBox2.Margin = new Padding(4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(937, 569);
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("思源黑体 CN", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(26, 36);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(100, 29);
            label2.TabIndex = 3;
            label2.Text = "推理模型:";
            // 
            // tb_model_path
            // 
            tb_model_path.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            tb_model_path.Location = new Point(134, 34);
            tb_model_path.Margin = new Padding(4);
            tb_model_path.Name = "tb_model_path";
            tb_model_path.Size = new Size(433, 30);
            tb_model_path.TabIndex = 4;
            // 
            // btn_choose_model_path
            // 
            btn_choose_model_path.Location = new Point(575, 36);
            btn_choose_model_path.Margin = new Padding(4);
            btn_choose_model_path.Name = "btn_choose_model_path";
            btn_choose_model_path.Size = new Size(73, 31);
            btn_choose_model_path.TabIndex = 5;
            btn_choose_model_path.Text = "选 择";
            btn_choose_model_path.UseVisualStyleBackColor = true;
            btn_choose_model_path.Click += btn_choose_model_path_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(rb_yolov8_cls);
            groupBox2.Controls.Add(rb_yolov8_pose);
            groupBox2.Controls.Add(rb_yolov8_seg);
            groupBox2.Controls.Add(rb_yolov8_det);
            groupBox2.Font = new Font("思源黑体 CN", 12F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox2.Location = new Point(672, 18);
            groupBox2.Margin = new Padding(4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4);
            groupBox2.Size = new Size(327, 116);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "推理模型";
            // 
            // rb_yolov8_cls
            // 
            rb_yolov8_cls.AutoSize = true;
            rb_yolov8_cls.Location = new Point(8, 76);
            rb_yolov8_cls.Margin = new Padding(4);
            rb_yolov8_cls.Name = "rb_yolov8_cls";
            rb_yolov8_cls.Size = new Size(135, 33);
            rb_yolov8_cls.TabIndex = 6;
            rb_yolov8_cls.Text = "Yolov8-cls";
            rb_yolov8_cls.UseVisualStyleBackColor = true;
            // 
            // rb_yolov8_pose
            // 
            rb_yolov8_pose.AutoSize = true;
            rb_yolov8_pose.Checked = true;
            rb_yolov8_pose.Location = new Point(157, 76);
            rb_yolov8_pose.Margin = new Padding(4);
            rb_yolov8_pose.Name = "rb_yolov8_pose";
            rb_yolov8_pose.Size = new Size(156, 33);
            rb_yolov8_pose.TabIndex = 6;
            rb_yolov8_pose.TabStop = true;
            rb_yolov8_pose.Text = "Yolov8-pose";
            rb_yolov8_pose.UseVisualStyleBackColor = true;
            // 
            // rb_yolov8_seg
            // 
            rb_yolov8_seg.AutoSize = true;
            rb_yolov8_seg.Location = new Point(157, 35);
            rb_yolov8_seg.Margin = new Padding(4);
            rb_yolov8_seg.Name = "rb_yolov8_seg";
            rb_yolov8_seg.Size = new Size(142, 33);
            rb_yolov8_seg.TabIndex = 6;
            rb_yolov8_seg.Text = "Yolov8-seg";
            rb_yolov8_seg.UseVisualStyleBackColor = true;
            // 
            // rb_yolov8_det
            // 
            rb_yolov8_det.AutoSize = true;
            rb_yolov8_det.Location = new Point(8, 35);
            rb_yolov8_det.Margin = new Padding(4);
            rb_yolov8_det.Name = "rb_yolov8_det";
            rb_yolov8_det.Size = new Size(141, 33);
            rb_yolov8_det.TabIndex = 6;
            rb_yolov8_det.Text = "Yolov8-det";
            rb_yolov8_det.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("思源黑体 CN", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(26, 100);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(100, 29);
            label3.TabIndex = 3;
            label3.Text = "类别文件:";
            // 
            // tb_clas_path
            // 
            tb_clas_path.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            tb_clas_path.Location = new Point(134, 99);
            tb_clas_path.Margin = new Padding(4);
            tb_clas_path.Name = "tb_clas_path";
            tb_clas_path.Size = new Size(433, 30);
            tb_clas_path.TabIndex = 4;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("思源黑体 CN", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(26, 158);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(100, 29);
            label4.TabIndex = 3;
            label4.Text = "测试图片:";
            // 
            // tb_test_image
            // 
            tb_test_image.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            tb_test_image.Location = new Point(134, 161);
            tb_test_image.Margin = new Padding(4);
            tb_test_image.Name = "tb_test_image";
            tb_test_image.Size = new Size(433, 30);
            tb_test_image.TabIndex = 4;
            // 
            // btn_choose_claspath
            // 
            btn_choose_claspath.Location = new Point(575, 101);
            btn_choose_claspath.Margin = new Padding(4);
            btn_choose_claspath.Name = "btn_choose_claspath";
            btn_choose_claspath.Size = new Size(73, 31);
            btn_choose_claspath.TabIndex = 5;
            btn_choose_claspath.Text = "选 择";
            btn_choose_claspath.UseVisualStyleBackColor = true;
            btn_choose_claspath.Click += btn_choose_claspath_Click;
            // 
            // btn_choose_testimage
            // 
            btn_choose_testimage.Location = new Point(575, 161);
            btn_choose_testimage.Margin = new Padding(4);
            btn_choose_testimage.Name = "btn_choose_testimage";
            btn_choose_testimage.Size = new Size(73, 31);
            btn_choose_testimage.TabIndex = 5;
            btn_choose_testimage.Text = "选 择";
            btn_choose_testimage.UseVisualStyleBackColor = true;
            btn_choose_testimage.Click += btn_choose_testimage_Click;
            // 
            // btn_model_deploy
            // 
            btn_model_deploy.Font = new Font("思源黑体 CN", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btn_model_deploy.Location = new Point(752, 153);
            btn_model_deploy.Margin = new Padding(4);
            btn_model_deploy.Name = "btn_model_deploy";
            btn_model_deploy.Size = new Size(148, 45);
            btn_model_deploy.TabIndex = 5;
            btn_model_deploy.Text = "模 型 推 理";
            btn_model_deploy.UseVisualStyleBackColor = true;
            btn_model_deploy.Click += btn_model_deploy_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Microsoft YaHei UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(34, 793);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(937, 144);
            textBox1.TabIndex = 8;
            // 
            // FormModelDeployPlat
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(998, 960);
            Controls.Add(textBox1);
            Controls.Add(groupBox2);
            Controls.Add(btn_model_deploy);
            Controls.Add(btn_choose_testimage);
            Controls.Add(btn_choose_claspath);
            Controls.Add(btn_choose_model_path);
            Controls.Add(tb_test_image);
            Controls.Add(tb_clas_path);
            Controls.Add(tb_model_path);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(pictureBox2);
            Margin = new Padding(4);
            Name = "FormModelDeployPlat";
            Text = "Model deployment demonstration platform";
            Load += FormModelDeployPlat_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PictureBox pictureBox2;
        private Label label2;
        private TextBox tb_model_path;
        private Button btn_choose_model_path;
        private GroupBox groupBox2;
        private RadioButton rb_yolov8_pose;
        private RadioButton rb_yolov8_seg;
        private RadioButton rb_yolov8_det;
        private Label label3;
        private TextBox tb_clas_path;
        private Label label4;
        private TextBox tb_test_image;
        private Button btn_choose_claspath;
        private Button btn_choose_testimage;
        private Button btn_model_deploy;
        private RadioButton rb_yolov8_cls;
        private TextBox textBox1;
    }
}