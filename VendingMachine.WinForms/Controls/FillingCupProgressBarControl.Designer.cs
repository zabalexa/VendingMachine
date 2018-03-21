namespace VendingMachine.WinForms
{
    partial class FillingCupProgressBarControl
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
            _flashingCup.FillCupProgress -= FillCupProgress;
            _flashingCup.ShowCup -= ShowCup;
            _flashingCup.HideCup -= HideCup;
            _flashingCup.FillCupComplete -= FillCupComplete;
            _flashingCup.NextOperation -= NextOperation;
            _flashingCup.Dispose();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.taskQueue = new System.Windows.Forms.ListView();
            this.queueImages = new System.Windows.Forms.ImageList(this.components);
            this.fillingCupIndicator = new VendingMachine.WinForms.FillingCup();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.taskQueue);
            this.groupBox1.Controls.Add(this.fillingCupIndicator);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox1.Size = new System.Drawing.Size(400, 200);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // taskQueue
            // 
            this.taskQueue.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.taskQueue.AutoArrange = false;
            this.taskQueue.BackColor = System.Drawing.SystemColors.Window;
            this.taskQueue.Dock = System.Windows.Forms.DockStyle.Right;
            this.taskQueue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.taskQueue.LabelWrap = false;
            this.taskQueue.LargeImageList = this.queueImages;
            this.taskQueue.Location = new System.Drawing.Point(217, 16);
            this.taskQueue.MultiSelect = false;
            this.taskQueue.Name = "taskQueue";
            this.taskQueue.OwnerDraw = true;
            this.taskQueue.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.taskQueue.ShowGroups = false;
            this.taskQueue.Size = new System.Drawing.Size(180, 181);
            this.taskQueue.SmallImageList = this.queueImages;
            this.taskQueue.TabIndex = 1;
            this.taskQueue.TabStop = false;
            this.taskQueue.TileSize = new System.Drawing.Size(74, 72);
            this.taskQueue.UseCompatibleStateImageBehavior = false;
            this.taskQueue.View = System.Windows.Forms.View.Tile;
            this.taskQueue.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.taskQueue_DrawItem);
            // 
            // queueImages
            // 
            this.queueImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.queueImages.ImageSize = new System.Drawing.Size(66, 66);
            this.queueImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // fillingCupIndicator
            // 
            this.fillingCupIndicator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.fillingCupIndicator.ForeColor = System.Drawing.Color.Chocolate;
            this.fillingCupIndicator.Location = new System.Drawing.Point(22, 32);
            this.fillingCupIndicator.Name = "fillingCupIndicator";
            this.fillingCupIndicator.Size = new System.Drawing.Size(100, 150);
            this.fillingCupIndicator.SkinExceptRectangleForProgressBar = new System.Drawing.Rectangle(15, 36, 52, 105);
            this.fillingCupIndicator.SkinImage = global::VendingMachine.WinForms.Properties.Resources.ProgressCup;
            this.fillingCupIndicator.Step = 1;
            this.fillingCupIndicator.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.fillingCupIndicator.TabIndex = 0;
            this.fillingCupIndicator.Value = 50;
            this.fillingCupIndicator.Visible = false;
            this.fillingCupIndicator.Click += new System.EventHandler(this.fillingCupIndicator_Click);
            // 
            // FillingCupProgressBarControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "FillingCupProgressBarControl";
            this.Size = new System.Drawing.Size(400, 200);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private FillingCup fillingCupIndicator;
        private System.Windows.Forms.ListView taskQueue;
        private System.Windows.Forms.ImageList queueImages;
    }
}
