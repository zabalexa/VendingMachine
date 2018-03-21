namespace VendingMachine.WinForms
{
    partial class CashButtonControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.coinsInfo1 = new System.Windows.Forms.TextBox();
            this.coinsInfo2 = new System.Windows.Forms.TextBox();
            this.coinsInfo5 = new System.Windows.Forms.TextBox();
            this.coinsInfo10 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.putCoins = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.payBalance = new System.Windows.Forms.TextBox();
            this.coinsInfoVM1 = new System.Windows.Forms.TextBox();
            this.coinsInfoVM2 = new System.Windows.Forms.TextBox();
            this.coinsInfoVM5 = new System.Windows.Forms.TextBox();
            this.coinsInfoVM10 = new System.Windows.Forms.TextBox();
            this.changeButton = new System.Windows.Forms.Button();
            this.descriptionToolTips = new System.Windows.Forms.ToolTip(this.components);
            this.coinsVM10 = new VendingMachine.WinForms.CoinsBar();
            this.coinsVM5 = new VendingMachine.WinForms.CoinsBar();
            this.coinsVM2 = new VendingMachine.WinForms.CoinsBar();
            this.coinsVM1 = new VendingMachine.WinForms.CoinsBar();
            this.coins10 = new VendingMachine.WinForms.CoinsBar();
            this.coins5 = new VendingMachine.WinForms.CoinsBar();
            this.coins2 = new VendingMachine.WinForms.CoinsBar();
            this.coins1 = new VendingMachine.WinForms.CoinsBar();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // coinsInfo1
            // 
            this.coinsInfo1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.coinsInfo1.Cursor = System.Windows.Forms.Cursors.Default;
            this.coinsInfo1.Location = new System.Drawing.Point(3, 8);
            this.coinsInfo1.Name = "coinsInfo1";
            this.coinsInfo1.ReadOnly = true;
            this.coinsInfo1.Size = new System.Drawing.Size(25, 20);
            this.coinsInfo1.TabIndex = 4;
            this.coinsInfo1.Text = "999";
            this.coinsInfo1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.coinsInfo1.Visible = false;
            this.coinsInfo1.Enter += new System.EventHandler(this.noFocus);
            // 
            // coinsInfo2
            // 
            this.coinsInfo2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.coinsInfo2.Cursor = System.Windows.Forms.Cursors.Default;
            this.coinsInfo2.Location = new System.Drawing.Point(3, 44);
            this.coinsInfo2.Name = "coinsInfo2";
            this.coinsInfo2.ReadOnly = true;
            this.coinsInfo2.Size = new System.Drawing.Size(25, 20);
            this.coinsInfo2.TabIndex = 5;
            this.coinsInfo2.Text = "999";
            this.coinsInfo2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.coinsInfo2.Visible = false;
            this.coinsInfo2.Enter += new System.EventHandler(this.noFocus);
            // 
            // coinsInfo5
            // 
            this.coinsInfo5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.coinsInfo5.Cursor = System.Windows.Forms.Cursors.Default;
            this.coinsInfo5.Location = new System.Drawing.Point(3, 80);
            this.coinsInfo5.Name = "coinsInfo5";
            this.coinsInfo5.ReadOnly = true;
            this.coinsInfo5.Size = new System.Drawing.Size(25, 20);
            this.coinsInfo5.TabIndex = 6;
            this.coinsInfo5.Text = "999";
            this.coinsInfo5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.coinsInfo5.Visible = false;
            this.coinsInfo5.Enter += new System.EventHandler(this.noFocus);
            // 
            // coinsInfo10
            // 
            this.coinsInfo10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.coinsInfo10.Cursor = System.Windows.Forms.Cursors.Default;
            this.coinsInfo10.Location = new System.Drawing.Point(3, 116);
            this.coinsInfo10.Name = "coinsInfo10";
            this.coinsInfo10.ReadOnly = true;
            this.coinsInfo10.Size = new System.Drawing.Size(25, 20);
            this.coinsInfo10.TabIndex = 7;
            this.coinsInfo10.Text = "999";
            this.coinsInfo10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.coinsInfo10.Visible = false;
            this.coinsInfo10.Enter += new System.EventHandler(this.noFocus);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.putCoins);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.payBalance);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox1.Location = new System.Drawing.Point(187, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(73, 128);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // putCoins
            // 
            this.putCoins.AllowDrop = true;
            this.putCoins.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.putCoins.Cursor = System.Windows.Forms.Cursors.Default;
            this.putCoins.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.putCoins.Location = new System.Drawing.Point(27, 63);
            this.putCoins.Name = "putCoins";
            this.putCoins.ReadOnly = true;
            this.putCoins.Size = new System.Drawing.Size(20, 40);
            this.putCoins.TabIndex = 12;
            this.putCoins.TabStop = false;
            this.putCoins.WordWrap = false;
            this.putCoins.DragDrop += new System.Windows.Forms.DragEventHandler(this.putCoins_DragDrop);
            this.putCoins.DragEnter += new System.Windows.Forms.DragEventHandler(this.putCoins_DragEnter);
            this.putCoins.DragLeave += new System.EventHandler(this.putCoins_DragLeave);
            this.putCoins.Enter += new System.EventHandler(this.noFocus);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label1.Location = new System.Drawing.Point(40, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 10;
            // 
            // payBalance
            // 
            this.payBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.payBalance.Cursor = System.Windows.Forms.Cursors.Default;
            this.payBalance.Location = new System.Drawing.Point(6, 19);
            this.payBalance.Name = "payBalance";
            this.payBalance.ReadOnly = true;
            this.payBalance.Size = new System.Drawing.Size(30, 20);
            this.payBalance.TabIndex = 9;
            this.payBalance.Text = "9999";
            this.payBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.payBalance.Enter += new System.EventHandler(this.noFocus);
            // 
            // coinsInfoVM1
            // 
            this.coinsInfoVM1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.coinsInfoVM1.Cursor = System.Windows.Forms.Cursors.Default;
            this.coinsInfoVM1.Location = new System.Drawing.Point(5, 3);
            this.coinsInfoVM1.Name = "coinsInfoVM1";
            this.coinsInfoVM1.ReadOnly = true;
            this.coinsInfoVM1.Size = new System.Drawing.Size(25, 20);
            this.coinsInfoVM1.TabIndex = 9;
            this.coinsInfoVM1.Text = "999";
            this.coinsInfoVM1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.coinsInfoVM1.Visible = false;
            this.coinsInfoVM1.Enter += new System.EventHandler(this.noFocus);
            // 
            // coinsInfoVM2
            // 
            this.coinsInfoVM2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.coinsInfoVM2.Cursor = System.Windows.Forms.Cursors.Default;
            this.coinsInfoVM2.Location = new System.Drawing.Point(41, 3);
            this.coinsInfoVM2.Name = "coinsInfoVM2";
            this.coinsInfoVM2.ReadOnly = true;
            this.coinsInfoVM2.Size = new System.Drawing.Size(25, 20);
            this.coinsInfoVM2.TabIndex = 14;
            this.coinsInfoVM2.Text = "999";
            this.coinsInfoVM2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.coinsInfoVM2.Visible = false;
            this.coinsInfoVM2.Enter += new System.EventHandler(this.noFocus);
            // 
            // coinsInfoVM5
            // 
            this.coinsInfoVM5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.coinsInfoVM5.Cursor = System.Windows.Forms.Cursors.Default;
            this.coinsInfoVM5.Location = new System.Drawing.Point(77, 3);
            this.coinsInfoVM5.Name = "coinsInfoVM5";
            this.coinsInfoVM5.ReadOnly = true;
            this.coinsInfoVM5.Size = new System.Drawing.Size(25, 20);
            this.coinsInfoVM5.TabIndex = 15;
            this.coinsInfoVM5.Text = "999";
            this.coinsInfoVM5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.coinsInfoVM5.Visible = false;
            this.coinsInfoVM5.Enter += new System.EventHandler(this.noFocus);
            // 
            // coinsInfoVM10
            // 
            this.coinsInfoVM10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.coinsInfoVM10.Cursor = System.Windows.Forms.Cursors.Default;
            this.coinsInfoVM10.Location = new System.Drawing.Point(113, 3);
            this.coinsInfoVM10.Name = "coinsInfoVM10";
            this.coinsInfoVM10.ReadOnly = true;
            this.coinsInfoVM10.Size = new System.Drawing.Size(25, 20);
            this.coinsInfoVM10.TabIndex = 16;
            this.coinsInfoVM10.Text = "999";
            this.coinsInfoVM10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.coinsInfoVM10.Visible = false;
            this.coinsInfoVM10.Enter += new System.EventHandler(this.noFocus);
            // 
            // changeButton
            // 
            this.changeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.changeButton.Location = new System.Drawing.Point(151, 71);
            this.changeButton.Name = "changeButton";
            this.changeButton.Size = new System.Drawing.Size(109, 44);
            this.changeButton.TabIndex = 17;
            this.changeButton.Text = "Выдать сдачу";
            this.changeButton.UseVisualStyleBackColor = true;
            this.changeButton.Visible = false;
            this.changeButton.Click += new System.EventHandler(this.changeButton_Click);
            this.changeButton.MouseEnter += new System.EventHandler(this.changeButton_MouseEnter);
            // 
            // descriptionToolTips
            // 
            this.descriptionToolTips.Active = false;
            this.descriptionToolTips.OwnerDraw = true;
            this.descriptionToolTips.ShowAlways = true;
            this.descriptionToolTips.Draw += new System.Windows.Forms.DrawToolTipEventHandler(this.descriptionToolTips_Draw);
            this.descriptionToolTips.Popup += new System.Windows.Forms.PopupEventHandler(this.descriptionToolTips_Popup);
            // 
            // coinsVM10
            // 
            this.coinsVM10.BackColor = System.Drawing.Color.Gold;
            this.coinsVM10.CoinValue = 10;
            this.coinsVM10.ForeColor = System.Drawing.Color.Chocolate;
            this.coinsVM10.Location = new System.Drawing.Point(111, 15);
            this.coinsVM10.Maximum = 10;
            this.coinsVM10.Name = "coinsVM10";
            this.coinsVM10.Size = new System.Drawing.Size(30, 100);
            this.coinsVM10.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.coinsVM10.TabIndex = 13;
            this.coinsVM10.Value = 5;
            this.coinsVM10.Visible = false;
            // 
            // coinsVM5
            // 
            this.coinsVM5.BackColor = System.Drawing.Color.Gold;
            this.coinsVM5.CoinValue = 5;
            this.coinsVM5.ForeColor = System.Drawing.Color.Chocolate;
            this.coinsVM5.Location = new System.Drawing.Point(75, 15);
            this.coinsVM5.Maximum = 10;
            this.coinsVM5.Name = "coinsVM5";
            this.coinsVM5.Size = new System.Drawing.Size(30, 100);
            this.coinsVM5.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.coinsVM5.TabIndex = 12;
            this.coinsVM5.Value = 5;
            this.coinsVM5.Visible = false;
            // 
            // coinsVM2
            // 
            this.coinsVM2.BackColor = System.Drawing.Color.Gold;
            this.coinsVM2.CoinValue = 2;
            this.coinsVM2.ForeColor = System.Drawing.Color.Chocolate;
            this.coinsVM2.Location = new System.Drawing.Point(39, 15);
            this.coinsVM2.Maximum = 10;
            this.coinsVM2.Name = "coinsVM2";
            this.coinsVM2.Size = new System.Drawing.Size(30, 100);
            this.coinsVM2.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.coinsVM2.TabIndex = 11;
            this.coinsVM2.Value = 5;
            this.coinsVM2.Visible = false;
            // 
            // coinsVM1
            // 
            this.coinsVM1.BackColor = System.Drawing.Color.Gold;
            this.coinsVM1.CoinValue = 1;
            this.coinsVM1.ForeColor = System.Drawing.Color.Chocolate;
            this.coinsVM1.Location = new System.Drawing.Point(3, 15);
            this.coinsVM1.Maximum = 10;
            this.coinsVM1.Name = "coinsVM1";
            this.coinsVM1.Size = new System.Drawing.Size(30, 100);
            this.coinsVM1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.coinsVM1.TabIndex = 10;
            this.coinsVM1.Value = 5;
            this.coinsVM1.Visible = false;
            // 
            // coins10
            // 
            this.coins10.BackColor = System.Drawing.Color.Gold;
            this.coins10.CoinValue = 10;
            this.coins10.ForeColor = System.Drawing.Color.Chocolate;
            this.coins10.Location = new System.Drawing.Point(20, 111);
            this.coins10.Maximum = 10;
            this.coins10.Name = "coins10";
            this.coins10.Size = new System.Drawing.Size(100, 30);
            this.coins10.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.coins10.TabIndex = 3;
            this.coins10.Value = 5;
            this.coins10.Visible = false;
            this.coins10.Click += new System.EventHandler(this.coins_Click);
            this.coins10.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.coins_GiveFeedback);
            this.coins10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.coins_MouseDown);
            this.coins10.MouseMove += new System.Windows.Forms.MouseEventHandler(this.coins_MouseMove);
            this.coins10.MouseUp += new System.Windows.Forms.MouseEventHandler(this.coins_MouseUp);
            // 
            // coins5
            // 
            this.coins5.BackColor = System.Drawing.Color.Gold;
            this.coins5.CoinValue = 5;
            this.coins5.ForeColor = System.Drawing.Color.Chocolate;
            this.coins5.Location = new System.Drawing.Point(20, 75);
            this.coins5.Maximum = 10;
            this.coins5.Name = "coins5";
            this.coins5.Size = new System.Drawing.Size(100, 30);
            this.coins5.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.coins5.TabIndex = 2;
            this.coins5.Value = 5;
            this.coins5.Visible = false;
            this.coins5.Click += new System.EventHandler(this.coins_Click);
            this.coins5.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.coins_GiveFeedback);
            this.coins5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.coins_MouseDown);
            this.coins5.MouseMove += new System.Windows.Forms.MouseEventHandler(this.coins_MouseMove);
            this.coins5.MouseUp += new System.Windows.Forms.MouseEventHandler(this.coins_MouseUp);
            // 
            // coins2
            // 
            this.coins2.BackColor = System.Drawing.Color.Gold;
            this.coins2.CoinValue = 2;
            this.coins2.ForeColor = System.Drawing.Color.Chocolate;
            this.coins2.Location = new System.Drawing.Point(20, 39);
            this.coins2.Maximum = 10;
            this.coins2.Name = "coins2";
            this.coins2.Size = new System.Drawing.Size(100, 30);
            this.coins2.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.coins2.TabIndex = 1;
            this.coins2.Value = 5;
            this.coins2.Visible = false;
            this.coins2.Click += new System.EventHandler(this.coins_Click);
            this.coins2.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.coins_GiveFeedback);
            this.coins2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.coins_MouseDown);
            this.coins2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.coins_MouseMove);
            this.coins2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.coins_MouseUp);
            // 
            // coins1
            // 
            this.coins1.BackColor = System.Drawing.Color.Gold;
            this.coins1.CoinValue = 1;
            this.coins1.ForeColor = System.Drawing.Color.Chocolate;
            this.coins1.Location = new System.Drawing.Point(20, 3);
            this.coins1.Maximum = 10;
            this.coins1.Name = "coins1";
            this.coins1.Size = new System.Drawing.Size(100, 30);
            this.coins1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.coins1.TabIndex = 0;
            this.coins1.Value = 5;
            this.coins1.Visible = false;
            this.coins1.Click += new System.EventHandler(this.coins_Click);
            this.coins1.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.coins_GiveFeedback);
            this.coins1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.coins_MouseDown);
            this.coins1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.coins_MouseMove);
            this.coins1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.coins_MouseUp);
            // 
            // CashButtonControl
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.changeButton);
            this.Controls.Add(this.coinsInfoVM10);
            this.Controls.Add(this.coinsInfoVM5);
            this.Controls.Add(this.coinsInfoVM2);
            this.Controls.Add(this.coinsVM10);
            this.Controls.Add(this.coinsVM5);
            this.Controls.Add(this.coinsVM2);
            this.Controls.Add(this.coinsInfoVM1);
            this.Controls.Add(this.coinsVM1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.coinsInfo10);
            this.Controls.Add(this.coinsInfo5);
            this.Controls.Add(this.coinsInfo2);
            this.Controls.Add(this.coinsInfo1);
            this.Controls.Add(this.coins10);
            this.Controls.Add(this.coins5);
            this.Controls.Add(this.coins2);
            this.Controls.Add(this.coins1);
            this.Name = "CashButtonControl";
            this.Size = new System.Drawing.Size(300, 300);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.CashButtonControl_DragEnter);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CoinsBar coins1;
        private CoinsBar coins2;
        private CoinsBar coins5;
        private CoinsBar coins10;
        private System.Windows.Forms.TextBox coinsInfo1;
        private System.Windows.Forms.TextBox coinsInfo2;
        private System.Windows.Forms.TextBox coinsInfo5;
        private System.Windows.Forms.TextBox coinsInfo10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox payBalance;
        private System.Windows.Forms.TextBox putCoins;
        private System.Windows.Forms.TextBox coinsInfoVM1;
        private CoinsBar coinsVM1;
        private CoinsBar coinsVM2;
        private CoinsBar coinsVM5;
        private CoinsBar coinsVM10;
        private System.Windows.Forms.TextBox coinsInfoVM2;
        private System.Windows.Forms.TextBox coinsInfoVM5;
        private System.Windows.Forms.TextBox coinsInfoVM10;
        private System.Windows.Forms.Button changeButton;
        private System.Windows.Forms.ToolTip descriptionToolTips;
    }
}
