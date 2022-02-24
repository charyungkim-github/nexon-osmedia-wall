using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// public class _CameraSettings {
//   private static int _count = 0 ;
//   public static int count { get { return _count; } set { _count = value; }}
//   public static List<string> serialNubmers { get; set; }
//   public static List<int> orders { get; set; }
//   public static List<int> widths { get; set; }
//   public static List<int> heights { get; set; }
// }
public class CameraSettingsController : MonoBehaviour
{
  public Transform cameraSettingHodler;
  public GameObject cameraSettingPrefab;
  [HideInInspector] public int count;
  [HideInInspector] public List<string> serialNumbers;
  [HideInInspector] public List<int> orders;
  [HideInInspector] public List<int> widths;
  [HideInInspector] public List<int> heights;

  void Start() {
      
  }

  void Update() {
      
  }

  public void AddCameraSetting() {
    GameObject cameraSetting = Instantiate(cameraSettingPrefab);
    cameraSetting.transform.SetParent(cameraSettingHodler);

    // delete button onclick event
    cameraSetting.transform.GetChild(2).GetComponent<Button>().onClick.AddListener( () => {
      Destroy(cameraSetting);
    });

    // set default order
    cameraSetting.transform.GetChild(6).GetComponent<InputField>().text = (cameraSettingHodler.childCount-1).ToString();
  }

  public void SaveData() {
    serialNumbers.Clear();
    orders.Clear();
    widths.Clear();
    heights.Clear();
    count = cameraSettingHodler.childCount;

    for(int i=0; i<cameraSettingHodler.childCount; i++) {
      Transform cameraSetting = cameraSettingHodler.GetChild(i);
      serialNumbers.Add(cameraSetting.GetChild(4).GetComponent<InputField>().text);
      orders.Add(int.Parse(cameraSetting.GetChild(6).GetComponent<InputField>().text));
      widths.Add(int.Parse(cameraSetting.GetChild(8).GetComponent<InputField>().text));
      heights.Add(int.Parse(cameraSetting.GetChild(10).GetComponent<InputField>().text));
    }
  }

  // get data

  // set data
  // -> instantiate cam on load

}
