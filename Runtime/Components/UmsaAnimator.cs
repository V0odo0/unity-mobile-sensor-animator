using System;
using UnityEngine;
using UnityEngine.Events;

namespace UMSA.Runtime
{
    [AddComponentMenu("UMSA/UMSA Animator")]
    public class UmsaAnimator : MonoBehaviour
    {
        public string SourceDevice
        {
            get => _sourceDevice;
            set => _sourceDevice = value;
        }
        [SerializeField, UmsaDeviceField] private string _sourceDevice;

        public bool UseInterpolator
        {
            get => _useInterpolator;
            set => _useInterpolator = value;
        }
        [SerializeField] private bool _useInterpolator = true;

        public SensorDataOutputEvents OutputEvents => _outputEvents ?? (_outputEvents = new SensorDataOutputEvents());
        [SerializeField] private SensorDataOutputEvents _outputEvents = new SensorDataOutputEvents();
        

        private UmsaDeviceDataInterpolator _interpolator;


        protected virtual void OnEnable()
        {
            UmsaManager.DeviceDataReceived += OnDataReceived;
        }

        protected virtual void OnDisable()
        {
            UmsaManager.DeviceDataReceived -= OnDataReceived;
        }

        protected virtual void Update()
        {
            if (UseInterpolator)
            {
                _interpolator ??= new UmsaDeviceDataInterpolator();
                var interpolatedData = _interpolator.Sample(Time.deltaTime);
                OutputEvents.InvokeAll(interpolatedData);
            }
        }

        protected virtual void OnDataReceived(UmsaDeviceData data)
        {
            if (!string.IsNullOrEmpty(_sourceDevice) && data.DeviceName != SourceDevice)
                return;

            if (UseInterpolator)
            {
                _interpolator ??= new UmsaDeviceDataInterpolator();
                _interpolator.Set(data);
            }
            else OutputEvents.InvokeAll(data);
        }

        [Serializable]
        public class SensorDataOutputEvents
        {
            public UnityEvent<Vector3> Acceleration;
            public UnityEvent<Quaternion> GyroAttitude;
            public UnityEvent<Vector3> GyroGravity;
            public UnityEvent<Vector3> GyroRotationRate;
            public UnityEvent<Vector3> GyroUserAcceleration;
            public UnityEvent<Vector3> CompassRawVector;


            public void InvokeAll(UmsaDeviceData data)
            {
                if (data == null)
                    return;

                Acceleration.Invoke(data.Acceleration);
                GyroAttitude.Invoke(data.GyroAttitude);
                GyroGravity.Invoke(data.GyroGravity);
                GyroRotationRate.Invoke(data.GyroRotationRate);
                GyroUserAcceleration.Invoke(data.GyroUserAcceleration);
                CompassRawVector.Invoke(data.CompassRawVector);

            }
        }
    }
}
