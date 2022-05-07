using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace UMSA.Runtime
{
    public class UmsaClient : IDisposable
    {
        internal const int DefaultUdpClientPort = 14960;

        public event Action<Exception> ExceptionThrown; 
        public event Action<UmsaDeviceData> DataReceived;
        
        public bool IsRunning { get; private set; }
        
        private UdpClient _udpClient;
        private readonly int _port;
        private readonly IPEndPoint _sendDataEdnPoint;


        public UmsaClient(int port = DefaultUdpClientPort)
        {
            _port = port;
            _sendDataEdnPoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), _port);
        }

        ~UmsaClient()
        {
            Dispose();
        }


        async void ReceiveDataLoop()
        {
            while (IsRunning)
            {
                try
                {
                    var receiveResult = await _udpClient.ReceiveAsync();
                    
                    string json = Encoding.UTF8.GetString(receiveResult.Buffer);

                    var deviceData = JsonUtility.FromJson<UmsaDeviceData>(json);
                    if (deviceData != null)
                        DataReceived?.Invoke(deviceData);
                }
                catch (Exception e)
                {
                    ExceptionThrown?.Invoke(e);
                    Stop();
                }
            }
        }


        public void Start()
        {
            if (IsRunning)
                return;

            _udpClient = new UdpClient(_port);

            IsRunning = true;

            ReceiveDataLoop();
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            _udpClient?.Close();

            IsRunning = false;
        }

        public void SendData(UmsaDeviceData data)
        {
            if (!IsRunning)
                return;
            
            try
            {
                var json = JsonUtility.ToJson(data);
                var bytes = Encoding.UTF8.GetBytes(json);

                _udpClient.Send(bytes, bytes.Length, _sendDataEdnPoint);
            }
            catch (Exception e)
            {
                ExceptionThrown?.Invoke(e);
                Stop();
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
