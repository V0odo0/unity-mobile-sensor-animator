using System.Collections.Generic;
using UMSA.Runtime;
using UnityEditor;
using UnityEngine;

namespace UMSA.Editor
{
    public sealed class UmsaServerEditorWindow : EditorWindow
    {
        private Dictionary<string, UmsaDeviceData> _deviceData;

        private bool _autoStartServer;
        

        void OnEnable()
        {
            titleContent = new GUIContent("UMSA Server");
            minSize = new Vector2(320, 320);

            _deviceData = new Dictionary<string, UmsaDeviceData>();

            _autoStartServer = EditorPrefs.GetBool("UMSA.AutoStartServer", false);

            UmsaManager.DeviceDataReceived += UmsaManagerOnDeviceDataReceived;
        }

        void OnDisable()
        {
            UmsaManager.DeviceDataReceived -= UmsaManagerOnDeviceDataReceived;

            EditorPrefs.SetBool("UMSA.AutoStartServer", _autoStartServer);
        }

        private void UmsaManagerOnDeviceDataReceived(UmsaDeviceData obj)
        {
            if (!_deviceData.ContainsKey(obj.DeviceName))
                _deviceData.Add(obj.DeviceName, obj);
            else _deviceData[obj.DeviceName] = obj;


        }


        void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            _autoStartServer = GUILayout.Toggle(_autoStartServer, "Auto Start Server [Editor only]");
            if (EditorGUI.EndChangeCheck())
            {
                if (_autoStartServer)
                    UmsaManager.StartServer();
                else UmsaManager.StopServer();
            }

            EditorGUILayout.LabelField($"Server: {(UmsaManager.IsServerRunning ? "Running" : "Stopped")}");

            if (UmsaManager.IsServerRunning)
            {
                EditorGUILayout.LabelField($"{(UmsaManager.Devices.Count == 0 ? "No Devices" : "Devices")}", EditorStyles.boldLabel);

                if (UmsaManager.Devices.Count == 0)
                {
                    EditorGUILayout.HelpBox("Connect your mobile device to local network and open UMSA App.", MessageType.Info);
                }
                else
                {
                    int deviceId = 1;
                    foreach (var c in _deviceData)
                    {
                        GUILayout.Label($"{deviceId}. {c.Key} | Timestamp: {c.Value.Timestamp}");

                        deviceId++;
                    }
                }
            }

            
            
            Repaint();
        }


        [MenuItem("Tools/UMSA Debugger")]
        public static UmsaServerEditorWindow Open()
        {
            return GetWindow<UmsaServerEditorWindow>();
        }
    }
}
