using System.Collections.Generic;

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
    public static int count;
    public static List<string> serialNumbers;
    public static List<int> orders;
    public static List<int> widths;
    public static List<int> heights;
  }
}