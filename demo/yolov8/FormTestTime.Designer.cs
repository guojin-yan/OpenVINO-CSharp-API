namespace yolov8
{
    partial class FormTestTime
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
            btn_choose_model_path = new Button();
            tb_model_path = new TextBox();
            label2 = new Label();
            btn_choose_testimage = new Button();
            btn_choose_claspath = new Button();
            tb_test_image = new TextBox();
            tb_clas_path = new TextBox();
            label4 = new Label();
            label3 = new Label();
            groupBox2 = new GroupBox();
            rb_yolov8_cls = new RadioButton();
            rb_yolov8_pose = new RadioButton();
            rb_yolov8_seg = new RadioButton();
            rb_yolov8_det = new RadioButton();
            btn_model_deploy = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            tb_count = new TextBox();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // btn_choose_model_path
            // 
            btn_choose_model_path.Location = new Point(608, 62);
            btn_choose_model_path.Margin = new Padding(4);
            btn_choose_model_path.Name = "btn_choose_model_path";
            btn_choose_model_path.Size = new Size(73, 31);
            btn_choose_model_path.TabIndex = 8;
            btn_choose_model_path.Text = "选 择";
            btn_choose_model_path.UseVisualStyleBackColor = true;
            btn_choose_model_path.Click += btn_choose_model_path_Click;
            // 
            // tb_model_path
            // 
            tb_model_path.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            tb_model_path.Location = new Point(167, 60);
            tb_model_path.Margin = new Padding(4);
            tb_model_path.Name = "tb_model_path";
            tb_model_path.Size = new Size(433, 30);
            tb_model_path.TabIndex = 7;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("思源黑体 CN", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(59, 62);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(100, 29);
            label2.TabIndex = 6;
            label2.Text = "推理模型:";
            // 
            // btn_choose_testimage
            // 
            btn_choose_testimage.Location = new Point(608, 186);
            btn_choose_testimage.Margin = new Padding(4);
            btn_choose_testimage.Name = "btn_choose_testimage";
            btn_choose_testimage.Size = new Size(73, 31);
            btn_choose_testimage.TabIndex = 13;
            btn_choose_testimage.Text = "选 择";
            btn_choose_testimage.UseVisualStyleBackColor = true;
            btn_choose_testimage.Click += btn_choose_testimage_Click;
            // 
            // btn_choose_claspath
            // 
            btn_choose_claspath.Location = new Point(608, 126);
            btn_choose_claspath.Margin = new Padding(4);
            btn_choose_claspath.Name = "btn_choose_claspath";
            btn_choose_claspath.Size = new Size(73, 31);
            btn_choose_claspath.TabIndex = 14;
            btn_choose_claspath.Text = "选 择";
            btn_choose_claspath.UseVisualStyleBackColor = true;
            btn_choose_claspath.Click += btn_choose_claspath_Click;
            // 
            // tb_test_image
            // 
            tb_test_image.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            tb_test_image.Location = new Point(167, 186);
            tb_test_image.Margin = new Padding(4);
            tb_test_image.Name = "tb_test_image";
            tb_test_image.Size = new Size(433, 30);
            tb_test_image.TabIndex = 11;
            // 
            // tb_clas_path
            // 
            tb_clas_path.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            tb_clas_path.Location = new Point(167, 124);
            tb_clas_path.Margin = new Padding(4);
            tb_clas_path.Name = "tb_clas_path";
            tb_clas_path.Size = new Size(433, 30);
            tb_clas_path.TabIndex = 12;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("思源黑体 CN", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(59, 183);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(100, 29);
            label4.TabIndex = 9;
            label4.Text = "测试图片:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("思源黑体 CN", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(59, 125);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(100, 29);
            label3.TabIndex = 10;
            label3.Text = "类别文件:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(rb_yolov8_cls);
            groupBox2.Controls.Add(rb_yolov8_pose);
            groupBox2.Controls.Add(rb_yolov8_seg);
            groupBox2.Controls.Add(rb_yolov8_det);
            groupBox2.Font = new Font("思源黑体 CN", 12F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox2.Location = new Point(716, 62);
            groupBox2.Margin = new Padding(4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4);
            groupBox2.Size = new Size(327, 116);
            groupBox2.TabIndex = 16;
            groupBox2.TabStop = false;
            groupBox2.Text = "推理模型";
            // 
            // rb_yolov8_cls
            // 
            rb_yolov8_cls.AutoSize = true;
            rb_yolov8_cls.Location = new Point(9, 77);
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
            rb_yolov8_pose.Location = new Point(158, 77);
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
            rb_yolov8_seg.Location = new Point(158, 36);
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
            rb_yolov8_det.Location = new Point(9, 36);
            rb_yolov8_det.Margin = new Padding(4);
            rb_yolov8_det.Name = "rb_yolov8_det";
            rb_yolov8_det.Size = new Size(141, 33);
            rb_yolov8_det.TabIndex = 6;
            rb_yolov8_det.Text = "Yolov8-det";
            rb_yolov8_det.UseVisualStyleBackColor = true;
            // 
            // btn_model_deploy
            // 
            btn_model_deploy.Font = new Font("思源黑体 CN", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btn_model_deploy.Location = new Point(868, 186);
            btn_model_deploy.Margin = new Padding(4);
            btn_model_deploy.Name = "btn_model_deploy";
            btn_model_deploy.Size = new Size(148, 45);
            btn_model_deploy.TabIndex = 15;
            btn_model_deploy.Text = "模 型 推 理";
            btn_model_deploy.UseVisualStyleBackColor = true;
            btn_model_deploy.Click += btn_model_deploy_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(59, 308);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(890, 265);
            textBox1.TabIndex = 17;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("思源黑体 CN", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(59, 250);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(100, 29);
            label1.TabIndex = 9;
            label1.Text = "测试次数:";
            // 
            // tb_count
            // 
            tb_count.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point);
            tb_count.Location = new Point(167, 249);
            tb_count.Margin = new Padding(4);
            tb_count.Name = "tb_count";
            tb_count.Size = new Size(112, 30);
            tb_count.TabIndex = 11;
            // 
            // FormTestTime
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1049, 585);
            Controls.Add(textBox1);
            Controls.Add(groupBox2);
            Controls.Add(btn_model_deploy);
            Controls.Add(btn_choose_testimage);
            Controls.Add(btn_choose_claspath);
            Controls.Add(tb_count);
            Controls.Add(tb_test_image);
            Controls.Add(tb_clas_path);
            Controls.Add(label1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(btn_choose_model_path);
            Controls.Add(tb_model_path);
            Controls.Add(label2);
            Name = "FormTestTime";
            Text = "FormTestTime";
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_choose_model_path;
        private TextBox tb_model_path;
        private Label label2;
        private Button btn_choose_testimage;
        private Button btn_choose_claspath;
        private TextBox tb_test_image;
        private TextBox tb_clas_path;
        private Label label4;
        private Label label3;
        private GroupBox groupBox2;
        private RadioButton rb_yolov8_cls;
        private RadioButton rb_yolov8_pose;
        private RadioButton rb_yolov8_seg;
        private RadioButton rb_yolov8_det;
        private Button btn_model_deploy;
        private TextBox textBox1;
        private Label label1;
        private TextBox tb_count;
    }
}