namespace TypingGame
{
    partial class Game
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
            toCopyTextBox = new TextBox();
            inputTextBox = new TextBox();
            gameSelectorComboBox = new ComboBox();
            SuspendLayout();
            // 
            // toCopyTextBox
            // 
            toCopyTextBox.Location = new Point(249, 289);
            toCopyTextBox.Name = "toCopyTextBox";
            toCopyTextBox.ReadOnly = true;
            toCopyTextBox.Size = new Size(777, 39);
            toCopyTextBox.TabIndex = 1;
            // 
            // inputTextBox
            // 
            inputTextBox.Location = new Point(249, 446);
            inputTextBox.Name = "inputTextBox";
            inputTextBox.ReadOnly = true;
            inputTextBox.Size = new Size(777, 39);
            inputTextBox.TabIndex = 0;
            inputTextBox.KeyPress += InputTextBox_KeyPress;
            // 
            // gameSelectorComboBox
            // 
            gameSelectorComboBox.FormattingEnabled = true;
            gameSelectorComboBox.Location = new Point(37, 35);
            gameSelectorComboBox.Name = "comboBox1";
            gameSelectorComboBox.Size = new Size(242, 40);
            gameSelectorComboBox.TabIndex = 2;
            gameSelectorComboBox.DataSource = Enum.GetValues(typeof(SelectedGame));
            gameSelectorComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            gameSelectorComboBox.SelectedValueChanged += GameSelectorComboBox_ValueChanged;
            // 
            // Game
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(gameSelectorComboBox);
            Controls.Add(inputTextBox);
            Controls.Add(toCopyTextBox);
            Name = "Game";
            Text = "Typing Game";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox toCopyTextBox;
        private TextBox inputTextBox;
        private ComboBox gameSelectorComboBox;
    }
}
