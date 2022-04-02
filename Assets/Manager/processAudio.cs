using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class processAudio : MonoBehaviour
{
    [Range(1, 100)] public float powerMultiplier;
	[Range(64, 8192)] public int numberOfSamples = 1024; //step by 2
	public FFTWindow fftWindow;
	public float lerpTime = 1;
	public Slider sensitivitySlider;

    // initialize spectrum array
    public GameObject soundBar;

    void Start()
    {
        //Get power from prefs
        powerMultiplier = PlayerPrefsManager.GetSensitivity ();

        //change sensivility (in component) to settings slider
		sensitivitySlider.onValueChanged.AddListener(delegate {
			SensitivityValueChangedHandler(sensitivitySlider);
		});

        //create the spectrum array (based on defined samples)
        float[] spectrum = new float[numberOfSamples];

    

      
    }


    void Update()
    {   
        // initialize spectrum array every frame (for debuging )
		float[] spectrum = new float[numberOfSamples];

        // populate array with fequency spectrum data
        //the magic happens here
		GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, fftWindow);

        soundBar.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 1000*spectrum[0]*powerMultiplier);;   
  
        Debug.Log(spectrum[0]);

    }


    public void SensitivityValueChangedHandler(Slider sensitivitySlider){
		powerMultiplier = sensitivitySlider.value;
	}
}
