using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UMSA.Runtime
{
    public static class UmsaManager
    {
        public static event Action<UmsaDeviceData> DeviceDataReceived;


        public static bool IsServerRunning => _server != null && _server.IsRunning;

        public static IReadOnlyList<string> Devices => _devices;
        private static List<string> _devices = new List<string>();

        private static UmsaClient _server { get; set; }
        
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#elif !UNITY_ANDROID && !UNITY_IOS
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
#endif
        static void Init()
        {
#if UNITY_EDITOR
            InitEditor();
#endif
        }

#if UNITY_EDITOR
        static void InitEditor()
        {
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;

            if (EditorPrefs.GetBool("UMSA.AutoStartServer", false))
                StartServer();
        }

        static void OnBeforeAssemblyReload()
        {
            try
            {
                StopServer();
            }
            catch 
            {
                //
            }
        }
#endif

        static void OnServerExceptionThrown(Exception obj)
        {
            Debug.LogException(obj);
        }

        static void OnServerDataReceived(UmsaDeviceData data)
        {
            if (data == null)
                return;

            if (!_devices.Contains(data.DeviceName))
                _devices.Add(data.DeviceName);

            //Convert raw sensor gyro data to unity quaternion
            data.GyroAttitude = Quaternion.Euler(90, 0, 0) * 
                new Quaternion(data.GyroAttitude.x, data.GyroAttitude.y, -data.GyroAttitude.z, -data.GyroAttitude.w);

            DeviceDataReceived?.Invoke(data);
        }

        internal static void Log(string message, LogType logType = LogType.Log, params object[] args)
        {
            Debug.LogFormat(logType, LogOption.None, null, $"UMSA: {message}", args);
        }

        public static void StartServer()
        {
            try
            {
                _server?.Dispose();
                _server = new UmsaClient();

                _server.ExceptionThrown += OnServerExceptionThrown;
                _server.DataReceived += OnServerDataReceived;

                _server.Start();
            }
            catch (Exception e)
            {
                Log("Failed to start UMSA server", LogType.Warning);
                Debug.LogException(e);
            }
        }

        public static void StopServer()
        {
            _server?.Dispose();
        }
    }
}