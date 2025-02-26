namespace Server.Systems
{
    partial class PositionMoveInfoForm
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
            label1 = new Label();
            PositionMoveCost = new TextBox();
            SaveButton = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 71);
            label1.Name = "label1";
            label1.Size = new Size(110, 15);
            label1.TabIndex = 0;
            label1.Text = "PositionMove Cost:";
            // 
            // PositionMoveCost
            // 
            PositionMoveCost.Location = new Point(128, 68);
            PositionMoveCost.Name = "PositionMoveCost";
            PositionMoveCost.Size = new Size(100, 23);
            PositionMoveCost.TabIndex = 1;
            // 
            // SaveButton
            // 
            SaveButton.Location = new Point(92, 124);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(75, 23);
            SaveButton.TabIndex = 2;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // PositionMoveInfoForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(259, 159);
            Controls.Add(SaveButton);
            Controls.Add(PositionMoveCost);
            Controls.Add(label1);
            Name = "PositionMoveInfoForm";
            Text = "PositionMoveInfoForm";
            Load += PositionMoveInfoForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox PositionMoveCost;
        private Button SaveButton;
    }
}