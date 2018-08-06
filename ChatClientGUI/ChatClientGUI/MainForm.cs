//AUTHOR: HABIB ADO SABIU

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClientGUI
{
    public partial class MainForm : Form

    {
        private static string connectedRoom = "";
        private static string currentRoom = "newuser";

        // flag to indicate if a user is already connected to a chat room or is a new user
        private static bool flag = false;

        private static string messageToSend;

        public MainForm()
        {

            // initialized all GUI components 
            InitializeComponent();

            // create a thread to receive messages from the server
            Thread receiveMessageThread = new Thread(new ThreadStart(ReceivingMessages));
            receiveMessageThread.Start();
        }

        private void txtSendMessages_TextChanged(object sender, EventArgs e)
        {
        }

        // the action to perform when send button is clicked
        private void btnSend_Click(object sender, EventArgs e)
        {
            // if the message typed into 'txtSendMessages' starts with the keyword '/join'
            // check to make sure the user is not in any other room
            // set 'messageToSend' variable with the typed message
            if (txtSendMessages.Text.StartsWith("/join"))
            {
                if (flag)
                {
                    txtReceivedMessages.Text += "server>> You can't join two rooms at the same time.\r\n";
                }
                else
                {
                    String[] inputParts = txtSendMessages.Text.Split(null);
                    connectedRoom = inputParts[1];
                    messageToSend = txtSendMessages.Text;
                    txtSendMessages.Text = "";
                }
            }

            // else if the message typed into 'txtSendMessages' starts with the keyword '/users'
            // set 'messageToSend' variable with the typed message
            else if (txtSendMessages.Text.StartsWith("/users"))
            {
                messageToSend = txtSendMessages.Text + " " + connectedRoom;
                txtSendMessages.Text = "";
            }

            // else if the message typed into 'txtSendMessages' starts with the keyword '/rooms'
            // set 'messageToSend' variable with the typed message
            else if (txtSendMessages.Text.StartsWith("/rooms"))
            {
                messageToSend = txtSendMessages.Text;
                txtSendMessages.Text = "";
            }

            // else if the message typed into 'txtSendMessages' starts with the keyword '/create'
            // set 'messageToSend' variable with the typed message
            else if (txtSendMessages.Text.StartsWith("/create"))
            {
                messageToSend = txtSendMessages.Text;
                txtSendMessages.Text = "";
            }

            // else if the message typed into 'txtSendMessages' starts with the keyword '/leave'
            // set 'messageToSend' variable with the typed message
            else if (txtSendMessages.Text.StartsWith("/leave"))
            {
                messageToSend = txtSendMessages.Text;
                txtSendMessages.Text = "";
            }

            // else send the message typed into 'txtSendMessages' to the server
            // the message contain message room (currentRoom) and the raw message (txtSendMessages)
            else
            {
                messageToSend = currentRoom + " " + txtSendMessages.Text;
                txtSendMessages.Text = "";
            }

            // Send message to the server
            int toSendLen = System.Text.Encoding.ASCII.GetByteCount(messageToSend);
            byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(messageToSend);
            byte[] toSendLenBytes = System.BitConverter.GetBytes(toSendLen);
            ConnectionGUI.clientSocket.Send(toSendLenBytes);
            ConnectionGUI.clientSocket.Send(toSendBytes);
        }

        private void txtReceivedMessages_TextChanged(object sender, EventArgs e)
        {

        }

        delegate void SetTextCallback(string text);

        // this method is called any time 'txtReceivedMessages' need to be updated with a new message
        private void SetText(string text)
        {

            if (this.txtReceivedMessages.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtReceivedMessages.Text += text;
            }
        }

        

        private void ReceivingMessages()
        {
            while (true)
            {
                // Receiving message from the server
                byte[] rcvLenBytes = new byte[4];
                ConnectionGUI.clientSocket.Receive(rcvLenBytes);
                int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
                byte[] rcvBytes = new byte[rcvLen];
                ConnectionGUI.clientSocket.Receive(rcvBytes);
                string line = System.Text.Encoding.ASCII.GetString(rcvBytes);

                // if the response is 'JOINSUCCESS' then,
                // change title of the client GUI
                // set flag to true (indicating the user is now connected to a room)
                if (line.StartsWith("JOINSUCCESS"))
                {
                    SetText(line.Remove(0, 12) + "\r\n");
                    currentRoom = connectedRoom;
                    SetTitle(ConnectionGUI.name + " @ " + currentRoom);
                    flag = true;
                }

                // if the response is 'LEAVESUCCESS' then,
                // change the user room to 'newuser' (not connected to any room)
                // change the title of the cllient GUI
                // set flag to false (indicating the user is not connected to any room)
                else if (line.StartsWith("LEAVESUCCESS"))
                {
                    SetText(line.Remove(0, 13) + "\r\n");
                    currentRoom = "newuser";
                    SetTitle(ConnectionGUI.name);
                    flag = false;
                }

                // if the response is 'MESSAGE' then,
                // append the message to 'txtReceivedMessages'
                else if (line.StartsWith("MESSAGE"))
                {
                    SetText(line.Remove(0, 8) + "\r\n");
                }
            }
        }

        // this method is use to update the title of the client GUI
        public void SetTitle(string title)
        {
            this.BeginInvoke((Action)delegate ()
            {
                this.Text = title;
            });
        }

        private void lblMessage_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // action to perform when disconnect button is clicked
        public void btnDisconnect_Click(object sender, EventArgs e)
        {
            // Send a diconnect message to the server
            int toSendLen = System.Text.Encoding.ASCII.GetByteCount("/disconnect " + connectedRoom);
            byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes("/disconnect " + connectedRoom);
            byte[] toSendLenBytes = System.BitConverter.GetBytes(toSendLen);
            ConnectionGUI.clientSocket.Send(toSendLenBytes);
            ConnectionGUI.clientSocket.Send(toSendBytes);

            // terminate the application
            Environment.Exit(0);
        }
    }
}
