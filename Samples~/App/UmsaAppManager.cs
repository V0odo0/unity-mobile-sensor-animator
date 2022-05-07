using System;
using System.Collections;
using UMSA.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace UMSA.App
{
    [AddComponentMenu("UMSA/App Manager")]
    public class UmsaAppManager : MonoBehaviour
    {
        const string NOT_SUPPORTED_OUTPUT = "Not supported";

        [SerializeField, Range(1, 120)] private int _dataSendFrequency;

        [Space]
        [SerializeField] private Text _accelerationDataText;
        [SerializeField] private Text _gyroAttitudeDataText;
        [SerializeField] private Text _gyroGravityDataText;
        [SerializeField] private Text _gyroRotationRateDataText;
        [SerializeField] private Text _gyroUserAccelerationDataText;
        [SerializeField] private Text _compassRawVectorDataText;

        private UmsaClient _client;


        void Awake()
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            _client = new UmsaClient();
            _client.Start();

            Input.gyro.enabled = true;
            Input.compass.enabled = true;

            if (!SystemInfo.supportsAccelerometer)
                _accelerationDataText.text = NOT_SUPPORTED_OUTPUT;
            if (!SystemInfo.supportsGyroscope)
            {
                _gyroAttitudeDataText.text = NOT_SUPPORTED_OUTPUT;
                _gyroGravityDataText.text = NOT_SUPPORTED_OUTPUT;
                _gyroRotationRateDataText.text = NOT_SUPPORTED_OUTPUT;
                _gyroUserAccelerationDataText.text = NOT_SUPPORTED_OUTPUT;
            }
#endif

            StartCoroutine(nameof(SendDataEnumerator));
        }

        void OnDestroy()
        {
            _client?.Stop();
        }


        void UpdateDataOutput(UmsaDeviceData data)
        {
            if (SystemInfo.supportsAccelerometer)
                _accelerationDataText.text = data.Acceleration.ToString();

            if (SystemInfo.supportsGyroscope)
            {
                _gyroAttitudeDataText.text = data.GyroAttitude.ToString();
                _gyroGravityDataText.text = data.GyroGravity.ToString();
                _gyroRotationRateDataText.text = data.GyroRotationRate.ToString();
                _gyroUserAccelerationDataText.text = data.GyroUserAcceleration.ToString();
            }

            _compassRawVectorDataText.text = data.CompassRawVector.ToString();
        }

        IEnumerator SendDataEnumerator()
        {
            while (_client != null && _client.IsRunning)
            {
                var data = new UmsaDeviceData
                {
                    DeviceName = SystemInfo.deviceName,
                    Acceleration = Input.acceleration,
                    GyroAttitude = Input.gyro.attitude,
                    GyroGravity = Input.gyro.gravity,
                    GyroRotationRate = Input.gyro.rotationRate,
                    GyroUserAcceleration = Input.gyro.userAcceleration,
                    CompassRawVector = Input.compass.rawVector,
                    Timestamp = Time.realtimeSinceStartup
                };

                _client.SendData(data);
                UpdateDataOutput(data);

                yield return new WaitForSecondsRealtime(1f / _dataSendFrequency);
            }
        }
    }
}
