![GitHub](https://img.shields.io/github/license/V0odo0/KNOT-Localization?label=license)
![Dependencies](https://img.shields.io/badge/dependencies-none-green)
![Unity](https://img.shields.io/badge/Unity-2020.3%2B-green)

## Overview

![ezgif-1-1c86a00656](https://user-images.githubusercontent.com/10213769/167265173-73ed53c6-0a4d-4b02-a709-2864a473693c.gif)

YouTube: https://youtu.be/xx_mwjVujLc


This package allows you to use your mobile device sensor data as animation source.

* Accelerometer
* Gyroscope Attitude
* Gyroscope Gravity
* Gyroscope Rotation Rate
* Gyroscope Acceleration
* Compass


UMSA consists of two parts:
1. `Client` - Mobile application that broadcasts sesnor data trough UDP protocol to all local network devices
2. `Server` - Editor or Standalone application that is listening for incoming sensor data

Default server and client port: `14960`

## Installation

Install via Package Manager

![Screenshot_3](https://user-images.githubusercontent.com/10213769/162617479-51c3d2d5-8573-44a2-bc56-8c68d09183f1.png)

```
https://github.com/V0odo0/unity-mobile-sensor-animator.git
```

*or*

Add dependency to your /YourProjectName/Packages/manifest.json

```
"com.knot.localization": "https://github.com/V0odo0/unity-mobile-sensor-animator.git",
```

## Usage 

1. Run UMSA App on your mobile device connected to the same local network as your Unity Editor application. Download and install latests Android APK here: https://github.com/V0odo0/unity-mobile-sensor-animator/releases. Or import `App` sample from package manager and compile the app yourself.

2. Open `Tools/UMSA Debugger` window and check `Auto Start Server` to start UMSA server. You will see your device in `Devices` list.

![image](https://user-images.githubusercontent.com/10213769/167265505-f44d85ef-b893-4812-9697-2b5f5154b24e.png)

3. Attach `UMSA Animator` component to game object on the scene. Select `Source Device` from the list or leave it as `Any`. Add sensor data receivers in `Output Events` and hit Play. 

![image](https://user-images.githubusercontent.com/10213769/167265812-6b9cb984-a5ed-4b29-8da8-a4494a54fb22.png)

## Recording animations

You can use Unity's `Recorder` package to convert the motion to Animation Clip in Play Mode.

![image](https://user-images.githubusercontent.com/10213769/167265907-25a6e0c8-f348-4ae0-b1db-ccce07d8b90b.png)

![image](https://user-images.githubusercontent.com/10213769/167265958-df9bdf79-408d-49d1-b9ba-084e1b83c55c.png)
