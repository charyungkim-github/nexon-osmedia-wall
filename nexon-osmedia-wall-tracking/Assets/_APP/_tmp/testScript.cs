using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
  public RsProcessingProfile profile;
  public RsProcessingBlock processingBlock;

  public RsColorizer colorizer;

  void Start() {
    // Debug.Log("change processing profile :" + profile._processingBlocks.Count);
    
    // colorizer.colorScheme = RsColorizer.ColorScheme.WhiteToBlack;
    // colorizer.minDist = 0;
  }

  void Update() {

  }
}
