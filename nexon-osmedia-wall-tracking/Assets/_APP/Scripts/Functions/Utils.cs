using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour{
  
  public static Color GetAvrColor(Texture2D texture, int x, int y, float width, float height) {
    width = 10;
    height = 10;
    Color color = new Color(0,0,0);
    for(int i = x; i < x + width; i++) {
      for(int j = y; j < y + height; j++) {
        Texture2D sourceTexture = texture;
        Color pixelColor = sourceTexture.GetPixel(i, j);
        color.r += pixelColor.r;
        color.g += pixelColor.g;
        color.b += pixelColor.b;
      }
    }
    color.r /= (width * height);
    color.g /= (width * height);
    color.b /= (width * height);

    return color;
  }

  public static Color GetCenterColor(Texture2D texture, int x, int y, float width, float height) {
    int centerX = (int) (x + (width/2));
    int centerY = (int) (y + (height/2));
    return texture.GetPixel(centerX, centerY);
  }

  public static Vector2 GetLeftTopPosition(RectTransform target) {
    return new Vector2(target.position.x + target.rect.xMin, target.position.y + target.rect.yMin);
  }

  public static Vector2Int GetPixelSize(RectTransform target, int cols, int  rows) {
    return new Vector2Int( (int) (target.sizeDelta.x / cols), (int) (target.sizeDelta.y / rows));
  }

  public static double Map(double x, double in_min, double in_max, double out_min, double out_max) {
    return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
  }
  
  public static Vector2 GetAnchoredPosition(CameraData cameraData) {
    return new Vector2(-(cameraData.width/2) + cameraData.positionX, -cameraData.positionY);
  }
}