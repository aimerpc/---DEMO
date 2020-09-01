using System;
using System.Text;
using System.Windows.Forms;
using DtuConfig.Properties;
using System.IO.Ports;
using System.Threading;
using System.Reflection;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;


namespace DtuConfig
{
    public partial class Form1 : Form
    {
        public StringBuilder strbRx = new StringBuilder();

        public string strComRecv = "";

        public string strError = "";

        //public static string strApnName = "";
        //public static string strApnAccount = "";
        //public static string strApnKey = "";
        public static string strApnData = "";

        private Thread threadGetParam;
        private Thread threadSetParam;

        private bool isThreadAlive = false;

        public bool IS_LITE_VERSION = true;

        public Form1()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
            if (IS_LITE_VERSION)
            {
                this.Text = "TCP-Test V" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            else
            {
                this.Text = "TCP-Test V" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            parameterLoad();
        }

        private void parameterLoad()
        {
            try
            {
                comboBox6.SelectedIndex = 0;
                //comboBox6.SelectedIndex = Settings.Default.comboBox6;
            }
            catch (Exception ex)
            {
                
            }
        }

        private void parameterSave()
        {
            try
            {
                
                Settings.Default.GlobalApnData = strApnData;

                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                
            }
        }
   

        private bool atCmdIsOK(string cmpStr, int millisecond)
        {
            try
            {
                while (millisecond > 0)
                {
                    Thread.Sleep(100);
                    millisecond -= 100;
                    if (strComRecv.IndexOf(cmpStr) >= 0)
                    {
                        Thread.Sleep(50);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
               
                return false;
            }
        }

        private void buttonCmdSaveParam_Click(object sender, EventArgs e)
        {
            if (isThreadAlive)
            {
                return;
            }
            parameterSave();
            //serialPortWrite("AT&W", true);
        }

        private void buttonCmdHellp_Click(object sender, EventArgs e)
        {
            if (isThreadAlive)
            {
                return;
            }

         
        }

        private void 关于ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //CreateFrmChild();
            AboutBox1 myAboutBox1 = new AboutBox1();
            myAboutBox1.ShowDialog();
        }


        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
            }
            System.Environment.Exit(0);
        }


        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox4.Clear();
        }


        private void buttonCmdGetParam_Click()
        {
            try
            {
                threadGetParam = new Thread(put_Delay);
                threadGetParam.Start();
                isThreadAlive = true;
            }
            catch (Exception ex)
            { }
        }
        private void put_Delay()
        {
            
            //Thread.Sleep(int.Parse(textBox3.Text));

        }
        //发送网络数据
        private void button2_Click(object sender, EventArgs e)
        {
            
                try
                {
                if (button3.Text == "连接")
                {
                    richTextBox4.AppendText("未连接到服务器" + "\r\n");
                    return;
                }
                else if (ConnectedType.Text == "未连接")
                {
                    richTextBox4.AppendText("未连接到客户端" + "\r\n");
                    return;
                }
                if (!checkBox4.Checked)
                {
                    if (comboBox6.SelectedIndex == 1)
                    {
                        string msg = richTextBox2.Text;
                        NetworkStream networkStream = new NetworkStream(m_clientSocket);
                        StreamWriter streamWriter = new StreamWriter(networkStream);
                        streamWriter.WriteLine(msg);
                        streamWriter.Flush();
                        streamWriter.Close();
                        networkStream.Close();
                    }
                    else if (comboBox6.SelectedIndex == 0)
                    {
                        for (int i = 0; i < m_workerSocketList.Count; i++)
                        {
                            if (checkBox1.Checked)
                            {
                                Socket workerSocket = (Socket)m_workerSocketList[i];
                                byte[] byData = System.Text.Encoding.ASCII.GetBytes(richTextBox2.Text + "\r\n");
                                workerSocket.Send(byData);
                            }
                            else
                            {
                                Socket workerSocket = (Socket)m_workerSocketList[i];
                                byte[] byData = System.Text.Encoding.ASCII.GetBytes(richTextBox2.Text);
                                workerSocket.Send(byData);
                            }
                        }
                    }
                }
                else
                {
                    if (button2.Text == "自动发送")
                    {
                        button2.Text = "停止发送";
                        buttonCmdGetParam_Click1();
                        if (textBox4.Text.Length == 0)
                        {
                            button2.Text = "自动发送";
                            richTextBox4.AppendText("定时发送时间不能为0" + "\r\n");
                            return;
                        }
                        if (int.Parse(textBox4.Text) < 0)
                        {
                            button2.Text = "自动发送";
                            richTextBox4.AppendText("定时发送时间需大于0" + "\r\n");
                            return;
                        }
                        checkBox4.Enabled = false;
                        textBox4.ReadOnly = true;
                    }
                    else if (button2.Text == "停止发送")
                    {
                        button2.Text = "自动发送";
                    }
                }
                }
                catch (System.Exception se)
                {
                    MessageBox.Show(se.Message);
                }
        }
        private void buttonCmdGetParam_Click1()
        {
            try
            {
                if (button2.Text != "停止发送")
                {
                    return;
                }
                threadGetParam = new Thread(put_Delay1);
                threadGetParam.Start();
                isThreadAlive = true;
            }
            catch (Exception ex)
            { }
        }
        private void put_Delay1()
        {
            if (button2.Text != "停止发送")
            {
                return;
            }

            if (comboBox6.SelectedIndex == 1)
            {
                string msg = richTextBox2.Text;
                NetworkStream networkStream = new NetworkStream(m_clientSocket);
                StreamWriter streamWriter = new StreamWriter(networkStream);
                streamWriter.WriteLine(msg);
                streamWriter.Flush();
                streamWriter.Close();
                networkStream.Close();
            }
            else if (comboBox6.SelectedIndex == 0)
            {
                for (int i = 0; i < m_workerSocketList.Count; i++)
                {
                    if (checkBox1.Checked)
                    {
                        Socket workerSocket = (Socket)m_workerSocketList[i];
                        byte[] byData = System.Text.Encoding.ASCII.GetBytes(richTextBox2.Text + "\r\n");
                        workerSocket.Send(byData);
                    }
                    else
                    {
                        Socket workerSocket = (Socket)m_workerSocketList[i];
                        byte[] byData = System.Text.Encoding.ASCII.GetBytes(richTextBox2.Text);
                        workerSocket.Send(byData);
                    }
                }
            }
            buttonCmdGetParam_Click1();
        }

       
        byte[] m_dataBuffer = new byte[10];
        IAsyncResult m_result;
        public AsyncCallback m_pfnCallBack;
        public Socket m_clientSocket = null;
        public class SocketPacket2
        {
            public SocketPacket2(Socket socket, int clientNumber)
            {
                m_currentSocket = socket;
                m_clientNumber = clientNumber;
                string_RemoteEndPoint = socket.RemoteEndPoint;
            }
            public Socket m_currentSocket;
            public int m_clientNumber;
            public byte[] dataBuffer = new byte[1024];
            public EndPoint string_RemoteEndPoint;
        }
        public class SocketPacket
        {
            public SocketPacket(Socket socket)
            {
                string_RemoteEndPoint = socket.RemoteEndPoint;
            }
            public System.Net.Sockets.Socket thisSocket;
            public byte[] dataBuffer = new byte[1024];
            public EndPoint string_RemoteEndPoint;
        }
        private delegate void UpdateText(string text);
        private void updateText(string text)
        {
            richTextBox4.AppendText(text + "\r\n");
        }
        public void OnDataReceived2(IAsyncResult asyn)
        {
            SocketPacket2 socketData = (SocketPacket2)asyn.AsyncState;
            try
            {
                int iRx = socketData.m_currentSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];
                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(socketData.dataBuffer, 0, iRx, chars, 0);
                System.String szData = new System.String(chars);
                AppendToRichEditControl(socketData.string_RemoteEndPoint.ToString() + ":" + szData);
                richTextBox4.AppendText(socketData.string_RemoteEndPoint.ToString() + ":" + szData + "\r\n");
                Socket workerSocket = socketData.m_currentSocket;
                byte[] byData = Encoding.ASCII.GetBytes(szData);
                //workerSocket.Send(byData);
                WaitForData2(socketData.m_currentSocket, socketData.m_clientNumber);
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                if (se.ErrorCode == 10054)
                {
                    string msg = socketData.string_RemoteEndPoint.ToString() + " Disconnected";
                    AppendToRichEditControl(msg);
                    m_workerSocketList[socketData.m_clientNumber - 1] = null;
                    UpdateClientListControl();
                }
                else
                {
                    MessageBox.Show(se.Message);
                }
            }
        }
        public void OnDataReceived(IAsyncResult asyn)
        {
            try
            {
                SocketPacket theSockId = (SocketPacket)asyn.AsyncState;
                int iRx = theSockId.thisSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];
                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);
                System.String szData = theSockId.string_RemoteEndPoint + ":" + (new System.String(chars));
                richTextBox4.Invoke(new UpdateText(updateText), szData);
                richTextBox4.AppendText("\r\n");
                WaitForData();
            }

            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
            }
            catch (System.Exception se)
            {
                //MessageBox.Show("服务器断开连接,请检查服务器然后重新连接！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.richTextBox4.AppendText(se.Message + "\r\n");
                button2.PerformClick();
            }
        }
        public void WaitForData()
        {
            try
            {
                if (m_pfnCallBack == null)
                {
                    m_pfnCallBack = new AsyncCallback(OnDataReceived);
                }
                SocketPacket theSocPkt = new SocketPacket(m_clientSocket);
                theSocPkt.thisSocket = m_clientSocket;
                m_result = m_clientSocket.BeginReceive(theSocPkt.dataBuffer, 0, theSocPkt.dataBuffer.Length, SocketFlags.None, m_pfnCallBack, theSocPkt);
            }
            catch (System.Exception se)
            {
                MessageBox.Show(se.Message);
            }
        }
        private Socket m_mainSocket;
        public delegate void UpdateRichEditCallback(string text);
        public delegate void UpdateClientListCallback();
        private AsyncCallback pfnWorkerCallBack;
        private ArrayList m_workerSocketList = ArrayList.Synchronized(new System.Collections.ArrayList());
        private int m_clientCount = 0;
        public void WaitForData2(Socket soc, int clientNumber)
        {
            SocketPacket2 theSocPkt = new SocketPacket2(soc, clientNumber);
            try
            {
                if (pfnWorkerCallBack == null)
                {
                    pfnWorkerCallBack = new AsyncCallback(OnDataReceived2);
                }
                soc.BeginReceive(theSocPkt.dataBuffer, 0, theSocPkt.dataBuffer.Length, SocketFlags.None, pfnWorkerCallBack, theSocPkt);
            }
            catch (SocketException se)
            {
                if (se.ErrorCode == 10053)
                {
                    string msg = theSocPkt.string_RemoteEndPoint.ToString() + " Disconnected";
                    AppendToRichEditControl(msg);
                    m_workerSocketList[theSocPkt.m_clientNumber - 1] = null;
                    UpdateClientListControl();
                }
                else
                {
                    MessageBox.Show(se.Message);
                }
            }
        }
        public void WaitForData1()
        {
            try
            {
                if (m_pfnCallBack == null)
                {
                    m_pfnCallBack = new AsyncCallback(OnDataReceived);
                }
                SocketPacket theSocPkt = new SocketPacket(m_clientSocket);
                theSocPkt.thisSocket = m_clientSocket;
                m_result = m_clientSocket.BeginReceive(theSocPkt.dataBuffer, 0, theSocPkt.dataBuffer.Length, SocketFlags.None, m_pfnCallBack, theSocPkt);
            }
            catch (System.Exception se)
            {
                MessageBox.Show(se.Message);
            }
        }
        private void AppendToRichEditControl(string msg)
        {

            if (InvokeRequired)
            {
                object[] pList = { msg };
                richTextBox4.BeginInvoke(new UpdateRichEditCallback(OnUpdateRichEdit), pList);//回包
            }
            else
            {
                OnUpdateRichEdit(msg);
            }
        }
        private void OnUpdateRichEdit(string msg)
        {
            if (msg.Length > 17)
            {
                this.richTextBox4.AppendText("\r\n");
            }
        }
        void SendMsgToClient(string msg, int clientNumber)
        {
            byte[] byData = System.Text.Encoding.ASCII.GetBytes(msg);
            Socket workerSocket = (Socket)m_workerSocketList[clientNumber - 1];
            workerSocket.Send(byData);
        }
        void UpdateClientList()
        {
            try
            {
                ConnectedType.Text = "未连接";
                for (int i = 0; i < m_workerSocketList.Count; i++)
                {
                    Socket workerSocket = (Socket)m_workerSocketList[i];
                    if (workerSocket != null)
                    {
                        if (workerSocket.Connected)
                        {
                            ConnectedType.Text = "已连接";
                        }
                        else
                        {
                            workerSocket.Close();
                            workerSocket = null;
                        }
                    }
                }
            }
            catch (System.Exception se)
            {
                MessageBox.Show(se.Message);
            }
        }
        private void UpdateClientListControl()
        {
            if (InvokeRequired)
            {
                ConnectedType.BeginInvoke(new UpdateClientListCallback(UpdateClientList), null);
            }
            else
            {
                UpdateClientList();
            }
        }
        private void Server_Load(object sender, EventArgs e)
        {
            if (!Socket.OSSupportsIPv4) MessageBox.Show("系统不支持IPv4地址或IPv4地址未启用！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            for (int i = 0; i < Dns.GetHostEntry(Dns.GetHostName()).AddressList.Length; i++)
            {
                if (Dns.GetHostEntry(Dns.GetHostName()).AddressList[i].ToString().Contains("."))
                {
                    this.comboBox1.Items.Add(Dns.GetHostEntry(Dns.GetHostName()).AddressList[i].ToString());
                }
            }
            comboBox1.SelectedIndex = 2;
        }
        public void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                Socket workerSocket = m_mainSocket.EndAccept(asyn);
                Interlocked.Increment(ref m_clientCount);
                m_workerSocketList.Add(workerSocket);
                //string msg = "Welcome " + workerSocket.RemoteEndPoint.ToString() + "\r\n";
               // richTextBox4.AppendText(workerSocket.RemoteEndPoint.ToString() + " Connected" + "\r\n");
                //SendMsgToClient(msg, m_clientCount);
                UpdateClientListControl();
                WaitForData2(workerSocket, m_clientCount);
                m_mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), m_mainSocket);
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\n OnClientConnection: Socket has been closed\n");
            }
            catch (System.Exception se)
            {
                //MessageBox.Show(se.Message);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {      
            try        
            {
                if (comboBox6.SelectedIndex == 1)
                {
                    switch (button3.Text)
                    {
                        case "连接":
                            {
                                m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                m_clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                                EndPoint ipeh_Server = new IPEndPoint(IPAddress.Parse(textBox1.Text), Convert.ToInt32(textBox2.Text));
                                if (!m_clientSocket.IsBound)
                                    m_clientSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
                                m_clientSocket.Connect(ipeh_Server);
                                WaitForData();
                                button3.Text = "断开";
                                textBox1.ReadOnly = true;
                                textBox2.ReadOnly = true;
                                comboBox6.Enabled = false;
                                break;
                            }

                        case "断开":
                            {
                                if (m_clientSocket != null)
                                {
                                    //m_clientSocket.Shutdown(SocketShutdown.Both);
                                    m_clientSocket.Close();
                                    m_clientSocket = null;
                                }
                                button3.Text = "连接";
                                textBox1.ReadOnly = false;
                                textBox2.ReadOnly = false;
                                comboBox6.Enabled = true;
                                break;
                            }
                    }
                }
                else if (comboBox6.SelectedIndex == 0)
                {
                    switch (button3.Text)
                    {
                        case "监听":
                            {
                                try
                                {
                                    button3.Text = "停止";
                                    comboBox6.Enabled = false;
                                    textBox2.ReadOnly = true;
                                    comboBox1.Enabled = false;
                                    m_mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                    m_mainSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                                    if (comboBox1.SelectedItem.ToString() == "Any")
                                    {
                                        m_mainSocket.Bind(new IPEndPoint(IPAddress.Any, Convert.ToInt32(textBox2.Text)));
                                    }
                                    else
                                    {
                                        m_mainSocket.Bind(new IPEndPoint(IPAddress.Parse(comboBox1.SelectedItem.ToString()), Convert.ToInt32(textBox2.Text)));
                                    }
                                    m_mainSocket.Listen(10);
                                    m_mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), m_mainSocket);
                                    break;
                                }
                                catch (System.Exception ex)
                                {
                                    MessageBox.Show(ex.Message + "1", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    break;
                                }

                            }
                        case "停止":
                            {
                                button3.Text = "监听";
                                textBox2.ReadOnly = false;
                                comboBox6.Enabled = true;
                                comboBox1.Enabled = true;
                                CloseSockets();
                                break;
                            }
                    }
                }
            }        
            catch (System.Exception ex)
            {        
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }               
        }
        private void CloseSockets()
        {
            try
            {
                ConnectedType.Text = "未连接";
                if (m_mainSocket != null)
                {
                    m_mainSocket.Close();
                    m_mainSocket = null;
                }
                for (int i = 0; i < m_workerSocketList.Count; i++)
                {
                    Socket workerSocket = (Socket)m_workerSocketList[i];
                    if (workerSocket != null)
                    {
                        workerSocket.Close();
                        workerSocket = null;
                    }

                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "关闭套接字错误");
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (button3.Text == "断开" || button3.Text == "停止")
                    return;
                if (comboBox6.SelectedIndex == 0)//TCP server
                {
                    
                    comboBox1.Visible = true;
                    textBox1.Visible = false;
                    button3.Text = "监听";
                    label6.Text = "本地IP地址";
                    label7.Text = "端口号";
                    label14.Visible = true;
                    ConnectedType.Visible = true;
                }
                else if (comboBox6.SelectedIndex == 1)//TCP client
                {
                    comboBox1.Visible = false;
                    textBox1.Visible = true;
                    button3.Text = "连接";
                    label6.Text = "服务器IP";
                    label7.Text = "目标端口号";
                    label14.Visible = false;
                    ConnectedType.Visible = false;
                }


            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "关闭套接字错误");
            }
        
        }


        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (Char)8)
            {
                e.Handled = true;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
                button2.Text = "自动发送";
            else
                button2.Text = "发送";
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (Char)8)
            {
                e.Handled = true;
            }
        }
    }
}
