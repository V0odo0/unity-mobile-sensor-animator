using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UMSA.Runtime
{
    public class UmsaDeviceDataInterpolator
    {
        private UmsaDeviceData _lastDeviceData;
        private UmsaDeviceData _curDeviceData;


        private readonly UmsaDeviceData _sample;
        private float _interpolatorTime;

        
        public UmsaDeviceDataInterpolator()
        {
            _sample = new UmsaDeviceData();
        }

        public UmsaDeviceData Sample(float t)
        {
            if (_lastDeviceData == null)
                return _curDeviceData ?? _sample;
            if (_curDeviceData == null)
                return _sample;

            float tDiff = _curDeviceData.Timestamp - _lastDeviceData.Timestamp;
            _interpolatorTime += t * (1f / tDiff);
            
            _sample.Acceleration = Vector3.Lerp(_lastDeviceData.Acceleration, _curDeviceData.Acceleration, _interpolatorTime);
            _sample.GyroAttitude = Quaternion.Lerp(_lastDeviceData.GyroAttitude, _curDeviceData.GyroAttitude, _interpolatorTime);
            _sample.GyroGravity = Vector3.Lerp(_lastDeviceData.GyroGravity, _curDeviceData.GyroGravity, _interpolatorTime);
            _sample.GyroRotationRate = Vector3.Lerp(_lastDeviceData.GyroRotationRate, _curDeviceData.GyroRotationRate, _interpolatorTime);
            _sample.GyroUserAcceleration = Vector3.Lerp(_lastDeviceData.GyroUserAcceleration, _curDeviceData.GyroUserAcceleration, _interpolatorTime);
            _sample.CompassRawVector = Vector3.Lerp(_lastDeviceData.CompassRawVector, _curDeviceData.CompassRawVector, _interpolatorTime);
            
            return _sample;
        }

        public void Set(UmsaDeviceData d)
        {
            _lastDeviceData = _curDeviceData;
            _curDeviceData = d;
            _interpolatorTime = 0f;
        }
    }
}
