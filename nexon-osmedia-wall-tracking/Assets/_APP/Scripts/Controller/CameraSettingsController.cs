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
    cameraSetting.transform.GetChild(3).GetComponent<Button>().onClick.AddListener( () => { Destroy(cameraSetting); });
  }

  public List<CameraData> GetCameraData() {
    List<CameraData> cameras = new List<CameraData>();
    for(int i=0; i<cameraSettingHodler.childCount; i++) {
      Transform cameraSetting = cameraSettingHodler.GetChild(i);
      CameraData camera = new CameraData();
      camera.serialNumber = cameraSetting.GetChild(5).GetComponent<InputField>().text;
      camera.width = int.Parse(cameraSetting.GetChild(7).GetComponent<InputField>().text);
      camera.height = int.Parse(cameraSetting.GetChild(9).GetComponent<InputField>().text);
      camera.positionX = int.Parse(cameraSetting.GetChild(11).GetComponent<InputField>().text);
      camera.positionY = int.Parse(cameraSetting.GetChild(13).GetComponent<InputField>().text);
      camera.rotationZ = int.Parse(cameraSetting.GetChild(15).GetComponent<InputField>().text);
      cameras.Add(camera);
    }
    return cameras;
  }

  public void LoadData(List<CameraData> cameras) {
    Debug.Log("load data");
    for(int i=0; i<cameras.Count; i++) {
      
      // create camera prefab
      if(i >= cameraSettingHodler.childCount) AddCameraSetting();
      
      // setup values
      Transform cameraSetting = cameraSettingHodler.GetChild(i);
      cameraSetting.GetChild(1).gameObject.SetActive(true);
      cameraSetting.GetChild(1).GetComponent<RawImage>().texture = depthCameraHolder.GetChild(i).GetComponentInChildren<RawImage>().texture;
      cameraSetting.GetChild(5).GetComponent<InputField>().text = cameras[i].serialNumber;
      cameraSetting.GetChild(7).GetComponent<InputField>().text = cameras[i].width.ToString();
      cameraSetting.GetChild(9).GetComponent<InputField>().text = cameras[i].height.ToString();
      cameraSetting.GetChild(11).GetComponent<InputField>().text = cameras[i].positionX.ToString();
      cameraSetting.GetChild(13).GetComponent<InputField>().text = cameras[i].positionY.ToString();
      cameraSetting.GetChild(15).GetComponent<InputField>().text = cameras[i].rotationZ.ToString();
    }
  }
}
