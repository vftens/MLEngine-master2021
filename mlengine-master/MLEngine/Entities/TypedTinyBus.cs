using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace MLEngine.Entities
{
    class TypedMessageBus
    {
        public ResponseSocket MessageBusServer;
        public RequestSocket MessageBusClient;
        public AutoResetEvent reset_resieve_event;
        public AutoResetEvent reset_send_event;

        public string channelName;

        public TypedMessageBus(string channelName)
        {
            this.channelName = channelName;
            MessageBusServer = new ResponseSocket("@tcp://localhost:" + StringToUniqInt(channelName).ToString());
            MessageBusClient = new RequestSocket(">tcp://localhost:" + StringToUniqInt(channelName).ToString());
            reset_resieve_event = new AutoResetEvent(true);

            MessageBusServer.ReceiveReady += MessageBusServer_ReceiveReady;
            MessageBusClient.SendReady += MessageBusClient_SendReady;
        }

        private int StringToUniqInt(string s)
        {
            int r = 0;
            for (int i = 0; i < s.Length; i++)
            {
                r = r + (byte)s[i];
            }

            return r + 10000;
        }

        private void MessageBusClient_SendReady(object sender, NetMQSocketEventArgs e)
        {
            reset_send_event.Set();
        }

        private void MessageBusServer_ReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            reset_resieve_event.Set();
        }

        public void Start()
        {
            Task task = new Task(() =>
            {
                while (true)
                {
                    reset_resieve_event.WaitOne(300);
                    reset_resieve_event.Reset();

                    byte[] request = MessageBusServer.ReceiveFrameBytes();

                    MessageBusServer_MessageReceived(request);
                }
            });

            task.Start();
        }

        public void Stop()
        {
            MessageBusServer.Dispose();
            MessageBusClient.Dispose();
        }

        public void SendMessage(string message)
        {
            reset_send_event.WaitOne(300);
            reset_send_event.Reset();

            MessageBusClient.SendFrame(Encoding.UTF8.GetBytes(message));
        }

        private void MessageBusServer_MessageReceived(byte[] request)
        {
            NewMessageRecived(channelName, Encoding.UTF8.GetString(request));
        }

        public event Action<string, string> NewMessageRecived;
    }
}
