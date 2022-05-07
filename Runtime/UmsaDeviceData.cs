using System;
using UnityEngine;

namespace UMSA.Runtime
{
    [Serializable]
    public class UmsaDeviceData
    {
        public string DeviceName;

        public Vector3 Acceleration;
        public Quaternion GyroAttitude;
        public Vector3 GyroGravity;
        public Vector3 GyroRotationRate;
        public Vector3 GyroUserAcceleration;
        public Vector3 CompassRawVector;

        public float Timestamp;



    }
}
