using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.CALIPSO;

public class settingController : MonoBehaviour
{
	public TMPro.TMP_Dropdown microphone;
	public Slider sensitivitySlider, thresholdSlider;
	public GameObject canvasSetting;
	//public GameObject openButton;

    private CalipsoManager cm;  

 	private bool panelActive = false;

 

    void Awake() {
        //cm = GameObject.Find("CalipsoManager").GetComponent<CalipsoManager>();
        cm =  FindObjectOfType<CalipsoManager>();
    }

	// Use this for initialization
	void Start () {
		microphone.value = PlayerPrefsManager.GetMicrophone ();
		sensitivitySlider.value = PlayerPrefsManager.GetSensitivity ();
		thresholdSlider.value = PlayerPrefsManager.GetThreshold ();
	}
 
	public void SaveAndExit (){
		PlayerPrefsManager.SetMicrophone (microphone.value);
		PlayerPrefsManager.SetSensitivity (sensitivitySlider.value);
		PlayerPrefsManager.SetThreshold (thresholdSlider.value);

		//panelActive = !panelActive;
		//canvasSetting.GetComponent<Animator> ().SetBool ("PanelActive",panelActive);
        Debug.Log("settingsMode: " + cm.settingsMode);
        cm.exitSettings();
        Debug.Log("settingsMode: " + cm.settingsMode);
	}

	public void SetDefaults(){
		microphone.value = 0;
		sensitivitySlider.value = 100f;
		thresholdSlider.value = 0.001f;
	}

	public void OpenSettings(){
		panelActive = !panelActive;
		//canvasSetting.GetComponent<Animator> ().SetBool ("PanelActive",panelActive);
	}

	public void TogglePanel(){
		if (!panelActive) {
			OpenSettings ();
		} else {
			SaveAndExit ();
		}
	}
}
