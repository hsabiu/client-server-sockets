using System;

namespace ChatClientGUI
{
    partial class MainForm
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
            this.txtReceivedMessages = new System.Windows.Forms.TextBox();
            this.txtSendMessages = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtReceivedMessages
            // 
            this.txtReceivedMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReceivedMessages.Location = new System.Drawing.Point(5, 12);
            this.txtReceivedMessages.Multiline = true;
            this.txtReceivedMessages.Name = "txtReceivedMessages";
            this.txtReceivedMessages.ReadOnly = true;
            this.txtReceivedMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtReceivedMessages.Size = new System.Drawing.Size(357, 381);
            this.txtReceivedMessages.TabIndex = 0;
            this.txtReceivedMessages.TextChanged += new System.EventHandler(this.txtReceivedMessages_TextChanged);
            // 
            // txtSendMessages
            // 
            this.txtSendMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSendMessages.Location = new System.Drawing.Point(73, 414);
            this.txtSendMessages.Name = "txtSendMessages";
            this.txtSendMessages.Size = new System.Drawing.Size(289, 21);
            this.txtSendMessages.TabIndex = 1;
            this.txtSendMessages.TextChanged += new System.EventHandler(this.txtSendMessages_TextChanged);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(2, 417);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(59, 13);
            this.lblMessage.TabIndex = 2;
            this.lblMessage.Text = "MASSAGE";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(187, 440);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "SEND";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(282, 440);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(78, 23);
            this.btnDisconnect.TabIndex = 4;
            this.btnDisconnect.Text = "EXIT";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 472);
            this.ControlBox = false;
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.txtSendMessages);
            this.Controls.Add(this.txtReceivedMessages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.ActiveControl = this.txtSendMessages;
        }

        #endregion

        private System.Windows.Forms.TextBox txtReceivedMessages;
        private System.Windows.Forms.TextBox txtSendMessages;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnDisconnect;
    }
}

