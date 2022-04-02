using UnityEngine;
using UnityEngine.UI; //for accessing Sliders and Dropdown
using System.Collections.Generic; // So we can use List<>
using UnityEngine.Audio; // So we can use AudioMixer

[RequireComponent(typeof(AudioSource))]
public class micController : MonoBehaviour
{
 
    public float minThreshold = 0.0001f;
	public float frequency = 0.0f;
	public int audioSampleRate = 44100;
	public string microphone;
	public FFTWindow fftWindow;
	public TMPro.TMP_Dropdown micDropdown;
	public Slider thresholdSlider;

	private List<string> options = new List<string>();
	private int samples = 8192; 
	private AudioSource audioSource;



    public bool IsWorking = true;
    bool _lastValueOfIsWorking;

    public bool RaltimeOutput = true;
    bool _lastValueOfRaltimeOutput;

	float _lastVolume = 0;


	//audiomixer para evitar reverb
	public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
       	
		audioSource = GetComponent<AudioSource> ();

		//Listado de micrófonos disponibles
		foreach (string device in Microphone.devices) {
			if (microphone == null) {
				//default mic
				microphone = device;
			}
			options.Add(device);
		}
		microphone = options[PlayerPrefsManager.GetMicrophone()];
		minThreshold = PlayerPrefsManager.GetThreshold ();

        //Debug.Log(options);

		//add mics to dropdown
		micDropdown.AddOptions(options);

		micDropdown.onValueChanged.AddListener(delegate {
			micDropdownValueChangedHandler(micDropdown);
		});

		thresholdSlider.onValueChanged.AddListener(delegate {
			thresholdValueChangedHandler(thresholdSlider);
		});

		//initialize input with default mic
		//UpdateMicrophone (); 
    }

	void UpdateMicrophone(){
		audioSource.Stop(); 
		audioMixer.SetFloat ("Volume", -80.0f);
		WorkStart();

		

		return;

		/*
		//Start recording to audioclip from the mic
		audioSource.clip = Microphone.Start(microphone, true, 10, audioSampleRate);
		audioSource.loop = true; 

		// Mute the sound with an Audio Mixer group becuase we don't want the player to hear it
		Debug.Log(Microphone.IsRecording(microphone).ToString());

		if (Microphone.IsRecording (microphone)) { //check that the mic is recording, otherwise you'll get stuck in an infinite loop waiting for it to start
			while (!(Microphone.GetPosition (microphone) > 0)) {
			} // Wait until the recording has started. 
		
			Debug.Log ("recording started with " + microphone);

			// Start playing the audio source
			audioSource.Play (); 


		} else {
			//microphone doesn't work for some reason
			Debug.Log (microphone + " doesn't work!");
		}
		*/
	}



    public void WorkStart()
    {
        #if !UNITY_WEBGL
                IsWorking = true;
                audioSource.clip = Microphone.Start(microphone, true, 10, 44100);
                audioSource.loop = true;
                while (!(Microphone.GetPosition(microphone) > 0))
                {
					Debug.Log ("recording started with " + microphone);
                    audioSource.Play();
                }
        #endif
    }

    public void WorkStop()
    {
        #if !UNITY_WEBGL
                IsWorking = false;
                Microphone.End(null);
                audioSource.loop = false;
        #endif
    }



	public void micDropdownValueChangedHandler(TMPro.TMP_Dropdown mic){
		microphone = options[mic.value];
		UpdateMicrophone ();
	}

	public void thresholdValueChangedHandler(Slider thresholdSlider){
		minThreshold = thresholdSlider.value;
	}
	
	public float GetAveragedVolume()
	{ 
		float[] data = new float[256];
		float a = 0;
		audioSource.GetOutputData(data,0);
		foreach(float s in data)
		{
			a += Mathf.Abs(s);
		}
		return a/256;
	}
	
	public float GetFundamentalFrequency()
	{
		float fundamentalFrequency = 0.0f;
		float[] data = new float[samples];
		audioSource.GetSpectrumData(data,0,fftWindow);
		float s = 0.0f;
		int i = 0;
		for (int j = 1; j < samples; j++)
		{
			if(data[j] > minThreshold) // volumn must meet minimum threshold
			{
				if ( s < data[j] )
				{
					s = data[j];
					i = j;
				}
			}
		}
		fundamentalFrequency = i * audioSampleRate / samples;
		frequency = fundamentalFrequency;
		return fundamentalFrequency;
	}
}
