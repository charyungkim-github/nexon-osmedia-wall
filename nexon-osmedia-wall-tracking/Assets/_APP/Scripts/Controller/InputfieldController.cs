using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InputfieldController : MonoBehaviour
{
  public float step = 0.1f;

  [HideInInspector] public float _float { 
    get { 
      if(!inputField) Initialize();
      try {
        return float.Parse(inputField.text); 
      }
      catch {
        return 0;
      }
    }
    set {
      if(!inputField) Initialize();
      inputField.text = value.ToString();
    }
  }

  [HideInInspector] public int _int { 
    get { 
      if(!inputField) Initialize();
      try {
        return int.Parse(inputField.text); 
      }
      catch {
        return 0;
      }
    }
    set {
      if(!inputField) Initialize();
      inputField.text = value.ToString();
    }
  }

  [HideInInspector] public string _string { 
    get { 
      if(!inputField) Initialize();
      return inputField.text; 
    }
    set {
      if(!inputField) Initialize();
      inputField.text = value;
    }
  }

  InputField inputField;  

  void Start() {
  }

  public void Initialize() {
    inputField = GetComponent<InputField>();
  }

  void Update() {
    if(!inputField) Initialize();
    if(!inputField.isFocused) return;
    if(step == 0) return;

    // arrow key
    if(Input.GetKey(KeyCode.UpArrow)) _float += step;
    else if(Input.GetKey(KeyCode.DownArrow)) _float -= step;
  }

  public void AddDebugEventListner(UnityAction<string> e) {

    if(!inputField) Initialize();
    inputField.onValueChanged.AddListener(e);
  }
}
