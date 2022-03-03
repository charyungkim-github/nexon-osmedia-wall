using System.Collections.Generic;
using UnityEngine;

public class CameraData {
  public string serialNumber;
  public Vector3 position;
  public Vector3 rotation;
  public Vector3 scale;
}

public class Data {
  public class Network {
    public static string ip ;
    public static string port;
    public static bool isConnected;
  }

  public class Tracking {
    public static int width;
    public static int height;
    public static int rows;
    public static int cols;
    public static float depthThreshold;
    public static bool debug;
    public static float profileNear;
    public static float profileFar;
  }

  public class Camera {
    public static Vector3 position;
    public static float size;
    public static float near;
    public static float far;
    public static int resolutionIndex; // 0:1280720, 1:640480
    public static List<CameraData> cameraData = new List<CameraData>();
  }
}