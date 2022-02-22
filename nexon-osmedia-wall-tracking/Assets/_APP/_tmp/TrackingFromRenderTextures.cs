using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackingFromRenderTextures : MonoBehaviour
{
  public int cols;
  public int rows;
  public float threshold = 0.5f;
  public RawImage source;
  public RawImage result;
  public bool debugGUI = true;
  public bool debugTexture = false;

  // manager
  Manager manager;

  // size
  int width, height, pixelWidth, pixelHeight; 

  // results
  Texture2D resultTexture;
  List<bool> resultData = new List<bool>();

  // debug
  RectTransform resultRectTransform;
  Vector2 debugOffset;
  Vector2Int debugPixelSize;
  GUIStyle style = new GUIStyle();

  void Start() {
    
    // need to modify, calibrate resolution
    cols--;

    // manager
    manager = GetComponentInParent<Manager>();
    
    // webcam size
    width = 1280; 
    height = 720; 
    pixelWidth = width / cols;
    pixelHeight = height / rows;

    // result texture
    resultTexture = new Texture2D(width, height);
    result.texture = (Texture) resultTexture;

    // debugs
    resultRectTransform = result.GetComponent<RectTransform>();
    debugOffset = Utils.GetLeftTopPosition(resultRectTransform);
    debugPixelSize = Utils.GetPixelSize(resultRectTransform, cols, rows);
    style.alignment = TextAnchor.MiddleCenter;
    style.normal.textColor = Color.red;
  }
  
  void Update() {
    if(source.texture) {
      
      // set variables on update for DEBUG
      pixelWidth = width / cols ;
      pixelHeight = height / rows;

      // convert image
      ConvertImage();

      // send result
      manager.SendTrackingData(resultData);
    }
  }

  void ConvertImage() {
    
    // reset
    resultData.Clear();

    for (int y = 0; y < height; y += pixelHeight) {
      for (int x = 0; x < width; x += pixelWidth) {

        // if(x + pixelWidth > width) continue;
        
        // get color
        Color color = Utils.GetCenterColor((Texture2D)source.texture, x, y, pixelWidth, pixelHeight);

        // pixelate image
        if(debugTexture) {
          PixelateImage(x, y, pixelWidth, pixelHeight, color);
          resultTexture.Apply();
        }

        // save result          
        double convertedThreshold = 0.04 - Utils.Map(threshold, 0, 1, 0, 0.04); // 0 -> white, 0.04 -> black
        bool result = color.r < convertedThreshold;
        resultData.Add(result);
      }
    }
  }


  // Debug Drawing
  void PixelateImage(int x, int y, float width, float height, Color color) {
    
    for(int i = x; i < x + width; i++) {
      for(int j = y; j < y + height; j++) {
        resultTexture.SetPixel(i, j, color);
      }
    }
  }

  void OnGUI() {

    if(!debugGUI) return;
    if(resultData.Count < 1) return;
    
    // set variables on update for DEBUG
    debugOffset = Utils.GetLeftTopPosition(resultRectTransform);
    debugPixelSize = Utils.GetPixelSize(resultRectTransform, cols, rows);
    
    int index = 0;
    for (int y=0; y<resultRectTransform.sizeDelta.y; y+=debugPixelSize.y) {
      for (int x=0; x<resultRectTransform.sizeDelta.x; x+=debugPixelSize.x) {

        // if(x + debugPixelSize.x > resultRectTransform.sizeDelta.x) continue;
        // if(index >= resultData.Count) continue;

        style.normal.textColor = resultData[index] ? Color.red : Color.gray;
        GUI.Box(new Rect(x + debugOffset.x, y + debugOffset.y, debugPixelSize.x, debugPixelSize.y), index.ToString(), style);
        index ++;
      }
    }
  }
}