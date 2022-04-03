using UnityEngine;
using UnityEngine.UI; //for accessing Sliders and Dropdown
using System.Collections.Generic; // So we can use List<>
using UnityEngine.Audio; // So we can use AudioMixer



namespace Unity.CALIPSO.MIC{


	[RequireComponent(typeof(AudioSource))]
	public class micController : MonoBehaviour
	{
	
	    [Range(64,8192)] public int numberOfSamples = 64; //step by 2
		public float minThreshold = 0.0001f;
		public float frequency = 0.0f;
		public int audioSampleRate = 44100;
		public string microphone;
		public FFTWindow fftWindow;
		public TMPro.TMP_Dropdown micDropdown, numSampleDropdown;
		public Slider thresholdSlider;

		private List<string> options = new List<string>();
		private int samples = 128; 
		private AudioSource _audioSource;


		public bool IsWorking = true;
		bool _lastValueOfIsWorking;

		public bool RaltimeOutput = true;
		bool _lastValueOfRaltimeOutput;



		//audiomixer para evitar reverb
		public AudioMixer audioMixer;



		// Start is called before the first frame update
		void Start()
		{
			
			_audioSource = GetComponent<AudioSource>();

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

			//add mics to dropdown
			micDropdown.AddOptions(options);


			micDropdown.onValueChanged.AddListener(delegate {
				micDropdownValueChangedHandler(micDropdown);
			});

			thresholdSlider.onValueChanged.AddListener(delegate {
				thresholdValueChangedHandler(thresholdSlider);
			});

			numSampleDropdown.onValueChanged.AddListener(delegate {
				numSamplesDropdownValueChangedHandler(numSampleDropdown);
			});



			//initialize input with default mic
			//UpdateMicrophone (); 
		}

		void UpdateMicrophone(){
			
			WorkStop();
			
			//pongo el audioMixer en off para que no se escuche reverb
			audioMixer.SetFloat ("Volume", -80.0f);

			WorkStart();

			Debug.Log(PlayerPrefsManager.getSamples());

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
					_audioSource.clip = Microphone.Start(microphone, true, 10, 44100);
					_audioSource.loop = true;
					while (!(Microphone.GetPosition(microphone) > 0))
					{
						//Debug.Log ("recording started with " + microphone);
						_audioSource.Play();
					}
			#endif
		}

		public void WorkStop()
		{
			#if !UNITY_WEBGL
					IsWorking = false;
					Microphone.End(null);
					_audioSource.loop = false;
			#endif
		}


		public void micDropdownValueChangedHandler(TMPro.TMP_Dropdown mic){
			microphone = options[mic.value];
			UpdateMicrophone ();
		}


		public void numSamplesDropdownValueChangedHandler(TMPro.TMP_Dropdown numSample){
			WorkStop();
			IsWorking = false;
			switch (numSample.value) {
				case 0:
					samples = 64;
					break;
				case 1:	
					samples = 128;
					break;
				case 2:
					samples = 256;
					break;
				case 3:
					samples = 512;
					break;
				case 4:	
					samples = 1024;
					break;
				default:
					samples = 128;
					break;
			}
			Debug.Log("samples: " + samples);
			PlayerPrefsManager.SetSamples(samples);
			UpdateMicrophone ();
		}

		public void thresholdValueChangedHandler(Slider thresholdSlider){
			minThreshold = thresholdSlider.value;
		}
		




		public int checkSamplesRange(){
			//check samples
			if(numberOfSamples % 64 != 0){
				numberOfSamples = 64;
			}
			if(numberOfSamples <= 63) numberOfSamples = 64;
			if(numberOfSamples >= 8193) numberOfSamples = 8192;
			return numberOfSamples;
		}


		void Update()
		{



		}

	}
}
