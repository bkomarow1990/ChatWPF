using ChatLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string username = String.Empty;
        static AutoResetEvent mre = new AutoResetEvent(false);
        public MainWindow()
        {
            InitializeComponent();
            LoadColors();
        }
        void LoadColors()
        {

        }
        IPEndPoint endPoint = null;
        NetworkStream stream = null;
        TcpClient client = new TcpClient();
        private async Task<MessageInfo> GetData() {
            while (client.Connected)
            {
                MessageInfo info = null; 
                BinaryFormatter serializer = new BinaryFormatter();
                //mre.WaitOne();
                info = await (MessageInfo)serializer.Deserialize(client.GetStream());
                messagesListBox.Items.Add(info);
                return info;
                //Dispatcher.Invoke(() => { messagesListBox.Items.Add(info); });
            }
        }
        private async void joinBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // виконуємо підключення
                endPoint = new IPEndPoint(IPAddress.Parse(ipTxtBox.Text), Int32.Parse(portTxtBox.Text));
                //endPoint.Address = ;
                //endPoint.Port = ;
                client.Connect(endPoint);
                username = usernameTxtBox.Text;
                MessageTransferInfo info = new MessageTransferInfo { Action = Actions.JOIN, Username = username };
                stream = client.GetStream();
                await GetData();
                // створюємо клас, який містить інформацію про файл
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            client.Close();
        }

        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            //IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipTextBox.Text), int.Parse(portTextBox.Text));
            //TcpClient client = new TcpClient();

            try
            {
                // виконуємо підключення
                MessageTransferInfo info = new MessageTransferInfo { Message = messageTxtBox.Text, Username = usernameTxtBox.Text, Action = Actions.SEND };
                BinaryFormatter serializer = new BinaryFormatter();

                // серіалізуємо об'єкт класа
                // та відправляємо його на сервер
                mre.Reset();
                serializer.Serialize(client.GetStream(), info);
                mre.Set();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //finally
            //{
            //    client.Close();
            //}
        }
    }
}
