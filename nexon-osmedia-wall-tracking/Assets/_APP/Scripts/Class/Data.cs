using System.Collections.Generic;
public class CameraData {
  public string serialNumber;
  public int order;
  public int width;
  public int height;
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
  }

  public class Camera {
    public static int resolutionIndex; // 0:1280720, 1:640480
    public static List<CameraData> cameraData = new List<CameraData>();

  }
}