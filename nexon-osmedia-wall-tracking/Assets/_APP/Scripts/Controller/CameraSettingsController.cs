using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSettingsController : MonoBehaviour
{
  public Transform depthCameraHolder;
  public Transform cameraSettingHodler;
  public GameObject cameraSettingPrefab;

  void Start() {
      
  }

  void Update() {
      
  }

  public void AddCameraSetting() {
    GameObject cameraSetting = Instantiate(cameraSettingPrefab);
    cameraSetting.transform.SetParent(cameraSettingHodler);

    // delete button onclick event
    cameraSetting.transform.GetChild(2).GetComponent<Button>().onClick.AddListener( () => { Destroy(cameraSetting); });

    // set default order
    cameraSetting.transform.GetChild(6).GetComponent<InputField>().text = (cameraSettingHodler.childCount-1).ToString();
  }

  public List<CameraData> GetCameraData() {
    List<CameraData> cameras = new List<CameraData>();
    for(int i=0; i<cameraSettingHodler.childCount; i++) {
      Transform cameraSetting = cameraSettingHodler.GetChild(i);
      CameraData camera = new CameraData();
      camera.serialNumber = cameraSetting.GetChild(4).GetComponent<InputField>().text;
      camera.order = int.Parse(cameraSetting.GetChild(6).GetComponent<InputField>().text);
      camera.width = int.Parse(cameraSetting.GetChild(8).GetComponent<InputField>().text);
      camera.height = int.Parse(cameraSetting.GetChild(10).GetComponent<InputField>().text);
      cameras.Add(camera);
    }
    return cameras;
  }

  public void LoadData(List<CameraData> cameras) {
    
    for(int i=0; i<cameras.Count; i++) {
      
      // create camera prefab
      if(i >= cameraSettingHodler.childCount) AddCameraSetting();
      
      // setup values
      Transform cameraSetting = cameraSettingHodler.GetChild(i);
      cameraSetting.GetChild(4).GetComponent<InputField>().text = cameras[i].serialNumber;
      cameraSetting.GetChild(6).GetComponent<InputField>().text = cameras[i].order.ToString();
      cameraSetting.GetChild(8).GetComponent<InputField>().text = cameras[i].width.ToString();
      cameraSetting.GetChild(10).GetComponent<InputField>().text = cameras[i].height.ToString();
      cameraSetting.GetChild(11).gameObject.SetActive(true);
      cameraSetting.GetChild(11).GetComponent<RawImage>().texture = depthCameraHolder.GetChild(i).GetComponentInChildren<RawImage>().texture;
    }
  }
}
