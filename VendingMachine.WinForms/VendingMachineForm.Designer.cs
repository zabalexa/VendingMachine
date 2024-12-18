using System.Windows.Forms;

namespace VendingMachine.WinForms
{
    partial class VendingMachineForm
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
            this.AutoScaleMode = AutoScaleMode.None;
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VendingMachineForm));
            this.fillingCupProgressBarControl = new VendingMachine.WinForms.FillingCupProgressBarControl();
            this.goodsButtonControl = new VendingMachine.WinForms.GoodsButtonControl();
            this.vendingMachineChangeCashButtonControl = new VendingMachine.WinForms.CashButtonControl();
            this.customerCashButtonControl = new VendingMachine.WinForms.CashButtonControl();
            this.display = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // fillingCupProgressBarControl
            // 
            this.fillingCupProgressBarControl.Location = new System.Drawing.Point(18, 368);
            this.fillingCupProgressBarControl.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.fillingCupProgressBarControl.Name = "fillingCupProgressBarControl";
            this.fillingCupProgressBarControl.Size = new System.Drawing.Size(488, 292);
            this.fillingCupProgressBarControl.TabIndex = 0;
            // 
            // goodsButtonControl
            // 
            this.goodsButtonControl.Location = new System.Drawing.Point(430, 18);
            this.goodsButtonControl.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.goodsButtonControl.Name = "goodsButtonControl";
            this.goodsButtonControl.Size = new System.Drawing.Size(488, 340);
            this.goodsButtonControl.TabIndex = 1;
            // 
            // vendingMachineChangeCashButtonControl
            // 
            this.vendingMachineChangeCashButtonControl.AllowDrop = true;
            this.vendingMachineChangeCashButtonControl.Location = new System.Drawing.Point(513, 368);
            this.vendingMachineChangeCashButtonControl.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.vendingMachineChangeCashButtonControl.Name = "vendingMachineChangeCashButtonControl";
            this.vendingMachineChangeCashButtonControl.Size = new System.Drawing.Size(405, 292);
            this.vendingMachineChangeCashButtonControl.TabIndex = 2;
            // 
            // customerCashButtonControl
            // 
            this.customerCashButtonControl.AllowDrop = true;
            this.customerCashButtonControl.Location = new System.Drawing.Point(18, 18);
            this.customerCashButtonControl.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.customerCashButtonControl.Name = "customerCashButtonControl";
            this.customerCashButtonControl.Size = new System.Drawing.Size(405, 292);
            this.customerCashButtonControl.TabIndex = 3;
            // 
            // display
            // 
            this.display.AutoSize = true;
            this.display.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.display.Location = new System.Drawing.Point(18, 323);
            this.display.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.display.Name = "display";
            this.display.Size = new System.Drawing.Size(0, 37);
            this.display.TabIndex = 4;
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // VendingMachineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 678);
            this.Controls.Add(this.display);
            this.Controls.Add(this.customerCashButtonControl);
            this.Controls.Add(this.vendingMachineChangeCashButtonControl);
            this.Controls.Add(this.goodsButtonControl);
            this.Controls.Add(this.fillingCupProgressBarControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VendingMachineForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vending Machine";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FillingCupProgressBarControl fillingCupProgressBarControl;
        private GoodsButtonControl goodsButtonControl;
        private CashButtonControl vendingMachineChangeCashButtonControl;
        private CashButtonControl customerCashButtonControl;
        private Label display;
        private Timer timer1;
    }
}

