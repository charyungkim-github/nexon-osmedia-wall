using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackingManager : MonoBehaviour
{
  public Camera trackingCamera;
  public RawImage cameraFeed;
  // public RenderTexture cameraRenderTexture;
  public int width = 1920;
  public int height = 1080;
  public int cols = 48;
  public int rows = 16;
  public float depthThreshold = 0.7f;
  bool debugGUI = true;

  Manager manager;
  RenderTexture cameraRenderTexture;
  Texture2D trackingTexture;
  List<bool> resultData = new List<bool>();
  int pixelWidth, pixelHeight;

  // debug
  List<float> colorValues = new List<float>();
  GUIStyle style = new GUIStyle();

  void Start() {
    // manager
    manager = GetComponentInParent<Manager>();

    // tracking texture
    cameraRenderTexture = new RenderTexture(width, height, 24);
    trackingTexture = new Texture2D(width, height);

    // pixel size    
    pixelWidth = width / cols;
    pixelHeight = height / rows;

    // gui style
    style.alignment = TextAnchor.MiddleCenter;
    style.normal.textColor = Color.red;

    trackingCamera.targetTexture = cameraRenderTexture;
    cameraFeed.texture = cameraRenderTexture;
    cameraFeed.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
  }

  void Update() {
    // tracking
    UpdateTexture();
    GetTextureColors();

    // send result
    manager.SendTrackingData(resultData);
  }

  void UpdateTexture() {

    // copy render texture to texture2d
    RenderTexture.active = null;
    RenderTexture.active = cameraRenderTexture;
    trackingTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
    trackingTexture.Apply();
  }

  void GetTextureColors() {
    
    // reset
    resultData.Clear();    
    colorValues.Clear();

    for (int y = 0; y < height; y += pixelHeight) {
      for (int x = 0; x < width; x += pixelWidth) {
        
        // get color
        Color color = Utils.GetCenterColor((Texture2D)trackingTexture, x, y, pixelWidth, pixelHeight);

        // save result          
        resultData.Add(color.r > depthThreshold);
        colorValues.Add(color.r);
      }
    }
  }
  void OnGUI() {
    if(!debugGUI) return;
    if(resultData.Count < 1) return;

    int index = 0;
    for (int y = 0; y < height; y += pixelHeight) {
      for (int x = 0; x < width; x += pixelWidth) {
        
        // string val = string.Format("{0:N1}", colorValues[index]);
        style.normal.textColor = resultData[index] ? Color.red : Color.gray;
        GUI.Box(new Rect(x, y, pixelWidth, pixelHeight), index.ToString(), style);
        index ++;
      }
    }
  }
}
