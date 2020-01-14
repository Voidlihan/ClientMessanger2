using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

namespace messanger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Message> MESSAGES = new List<Message>();
        public MainWindow()
        {
            InitializeComponent();
            SendingMessage();
        }

        private void SendButton(object sender, RoutedEventArgs e)
        {
            string text = "От " + textBoxUsername.Text + ":" + textBoxMessage.Text;
            Send(text);
        }
        private async Task Send(string text)
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                var localIp = IPAddress.Parse("192.168.1.19");
                var port = 3231;
                var endPoint = new IPEndPoint(localIp, port);
                await socket.ConnectAsync(endPoint);
                var buffer = Encoding.UTF8.GetBytes(text);
                socket.Send(buffer);
                socket.Shutdown(SocketShutdown.Both);
            }
        }
        private async Task SendingMessage()
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            using (var context = new Context())
            {
                var set = context.Set<Message>();
                var localIp = IPAddress.Parse("192.168.1.19");
                var port = 3231;
                var endPoint = new IPEndPoint(localIp, port);
                socket.Bind(endPoint);
                socket.Listen(5);
                while (true)
                {
                    var incomnigSocket = await socket.AcceptAsync();
                    while (incomnigSocket.Available > 0)
                    {
                        var buffer = new byte[incomnigSocket.Available];
                        incomnigSocket.Receive(buffer);

                        string sendMessage = System.Text.Encoding.UTF8.GetString(buffer);
                        set.Add(
                            new Message 
                            { 
                                Text = sendMessage
                            });
                        await context.SaveChangesAsync();
                    }
                    foreach (var message in context.Messages)
                    {                        
                        textBlockMessage.Text += message.Text + '\n';
                        textBlockMessage.Text += message.Text + '\n';
                        MESSAGES.Add(message);
                    }
                    incomnigSocket.Close();
                }
            }
        }
    }
}
