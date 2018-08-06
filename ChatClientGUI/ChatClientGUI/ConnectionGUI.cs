//NAME: HABIB ADO SABIU
//NSID: has956
//STUDENT NUMBER: 11198210
//ASSIGNMENT 1: CLIENT CONNECTION

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClientGUI
{
    public partial class ConnectionGUI : Form
    {
        public static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public static string name = "";

        public ConnectionGUI()
        {
            // initialize the connection GUI
            InitializeComponent();
        }

        // action to perform when connect button is clicked
        private void btnConnect_Click(object sender, EventArgs e)
        {

            // check to make sure all fields are not empty before trying to connect
            if ((txtIPAddress.Text.Equals("")) || (txtName.Text.Equals("")))
            {
                MessageBox.Show("Fields cant be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // get the server ip address to connect to 
                string IPtoConnect = txtIPAddress.Text;

                // set the connection port to 500
                int PortToConnect = 500;

                try
                {
                    // try connecting to the server using it's ip address and port
                    IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(IPtoConnect), PortToConnect);
                    clientSocket.Connect(serverAddress);

                }
                // catch errors related to unsuccessiful connection
                catch (SocketException)
                {
                    MessageBox.Show("Connection failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (FormatException)
                {
                    MessageBox.Show("invalid IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // if connection is successiful
                if (clientSocket.Connected)
                {
                    while (true)
                    {
                        // Receiving the response from server
                        byte[] rcvLenBytes = new byte[4];
                        clientSocket.Receive(rcvLenBytes);
                        int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
                        byte[] rcvBytes = new byte[rcvLen];
                        clientSocket.Receive(rcvBytes);
                        string line = System.Text.Encoding.ASCII.GetString(rcvBytes);

                        // if the response is 'SUBMITNAME' then send the user name to the server
                        if (line.StartsWith("SUBMITNAME"))
                        {
                            // Sending
                            int toSendLen = System.Text.Encoding.ASCII.GetByteCount(txtName.Text);
                            byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(txtName.Text);
                            byte[] toSendLenBytes = System.BitConverter.GetBytes(toSendLen);
                            clientSocket.Send(toSendLenBytes);
                            clientSocket.Send(toSendBytes);
                        }

                        // else if the response is 'NAMEEXIST' then display an error message indication the name is 
                        //         already in use by another user
                        else if (line.StartsWith("NAMEEXIST"))
                        {
                            MessageBox.Show("Name exist, please choose a different name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(0);
                        }

                        // else if the response is 'NAMEACCEPTED', hide the connection GUI
                        //         create and object of 'MainForm', set the appropriate
                        //         variables, and show the main client (MainForm)
                        else if (line.StartsWith("NAMEACCEPTED"))
                        {
                            name = txtName.Text;
                            this.Hide();
                            MainForm form = new MainForm();
                            form.Text = name;
                            form.Show();
                            return;
                        }
                    }
                }
            }
        }

        private void txtIPAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        // action to perform when cancel button is clicked
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            // close the connection GUI
            Environment.Exit(0);
        }

        private void txtName_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void ConnectionGUI_Load(object sender, EventArgs e)
        {

        }
    }
}
