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

    void Start()
    {

        powerMultiplier = PlayerPrefsManager.GetSensitivity ();

        //Cambio de sensibilidad
		sensitivitySlider.onValueChanged.AddListener(delegate {
			SensitivityValueChangedHandler(sensitivitySlider);
		});

        float[] spectrum = new float[numberOfSamples];
    }

    // Update is called once per frame
    void Update()
    {   
        // initialize spectrum array
		float[] spectrum = new float[numberOfSamples];

        // populate array with fequency spectrum data
		GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, fftWindow);

        Debug.Log(spectrum[0]);

    }


    public void SensitivityValueChangedHandler(Slider sensitivitySlider){
		powerMultiplier = sensitivitySlider.value;
	}
}
