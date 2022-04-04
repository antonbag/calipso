using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.CALIPSO;

public class settingController : MonoBehaviour
{
	public TMPro.TMP_Dropdown microphone;
	public TMPro.TMP_Dropdown numSamplesDropdown;
	public Slider sensitivitySlider, thresholdSlider, optimizeSampleSlider;
	public GameObject canvasSetting;
	//public GameObject openButton;

    private calipsoManager cm;  

 	private bool panelActive = false;

 

    void Awake() {
        //cm = GameObject.Find("CalipsoManager").GetComponent<CalipsoManager>();
        cm =  FindObjectOfType<calipsoManager>();
    }

	// INICIO LOS VALORES DE LOS SLIDERS
	void Start () {
		microphone.value = PlayerPrefsManager.GetMicrophone ();
		sensitivitySlider.value = PlayerPrefsManager.GetSensitivity ();
		thresholdSlider.value = PlayerPrefsManager.GetThreshold ();
		optimizeSampleSlider.value = 1.0f;
		numSamplesDropdown.value = 2; //2=256
	}
 
	public void SaveAndExit (){
		//Lo guardo on the fly
		//PlayerPrefsManager.SetMicrophone (microphone.value);
		//PlayerPrefsManager.SetSensitivity (sensitivitySlider.value);
		//PlayerPrefsManager.SetThreshold (thresholdSlider.value);
		//PlayerPrefsManager.SetSamples (numSamplesDropdown.value);
	
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
		optimizeSampleSlider.value = 1;
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
