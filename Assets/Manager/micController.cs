using UnityEngine;
using UnityEngine.UI; //for accessing Sliders and Dropdown
using System.Collections.Generic; // So we can use List<>
using UnityEngine.Audio; // So we can use AudioMixer



namespace Unity.CALIPSO.MIC{


	[RequireComponent(typeof(AudioSource))]
	public class micController : MonoBehaviour
	{
	
	 	private int _numberOfSamples = 128;
		public float minThreshold = 0.0001f;
		private float _optimizeSample = 1.0f;
		public float frequency = 0.0f;
		public int audioSampleRate = 44100;
		public string microphone;
		public FFTWindow fftWindow;
		public TMPro.TMP_Dropdown micDropdown, numSampleDropdown;
		public Slider thresholdSlider, sensitivitySlider, optimizeSampleSlider;

		private List<string> options = new List<string>();
		//private int samples = 128; 
		private AudioSource _audioSource;


		public bool IsWorking = true;
		bool _lastValueOfIsWorking;

		public bool RaltimeOutput = true;
		bool _lastValueOfRaltimeOutput;



		//audiomixer para evitar reverb
		public AudioMixer audioMixer;

		private soundBarCreation sb;

		// Start is called before the first frame update
		void Start()
		{
			
			_audioSource = GetComponent<AudioSource>();

			sb = FindObjectOfType<soundBarCreation>();

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
			_optimizeSample = PlayerPrefsManager.GetOptimizeSamples ();

			//add mics to dropdown
			micDropdown.AddOptions(options);


			micDropdown.onValueChanged.AddListener(delegate {
				micDropdownValueChangedHandler(micDropdown);
			});

			numSampleDropdown.onValueChanged.AddListener(delegate {
				numSamplesDropdownValueChangedHandler(numSampleDropdown);
			});


			thresholdSlider.onValueChanged.AddListener(delegate {
				thresholdValueChangedHandler(thresholdSlider);
			});

			optimizeSampleSlider.onValueChanged.AddListener(delegate {
				optimizeSampleSliderValueChangedHandler(optimizeSampleSlider);
			});



			//initialize input with default mic
			//UpdateMicrophone (); 
		}

		void UpdateMicrophone(){

			Debug.Log("****UpdateMicrophone***");
			sb.deleteSoundBar();
			WorkStop();
			
			//pongo el audioMixer en off para que no se escuche reverb
			audioMixer.SetFloat ("Volume", -80.0f);

			WorkStart();
			sb.createSoundBar();

			//Debug.Log(PlayerPrefsManager.getSamples());

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

			int currentSamples = PlayerPrefsManager.getSamples();

			int[] samplesArray = {64,128,256,512,1024,2048,4096,8192};
			_numberOfSamples = samplesArray[numSample.value];

			//Si ha cambiado, actualizo bars
			if(currentSamples != _numberOfSamples){
				Debug.Log("NUEVOS SAMPLES: " + _numberOfSamples);
				PlayerPrefsManager.SetSamples(_numberOfSamples);
				UpdateMicrophone();
			}else{
				Debug.Log("NO HAY CAMBIO EN samples: " + _numberOfSamples);
			}

			//UpdateMicrophone ();
		}
 
		public void thresholdValueChangedHandler(Slider thresholdSlider){
			minThreshold = thresholdSlider.value;
		}
		public void optimizeSampleSliderValueChangedHandler(Slider optimizeSampleSlider){
			_optimizeSample = optimizeSampleSlider.value;
			PlayerPrefsManager.SetOptimizeSamples(_optimizeSample);
			UpdateMicrophone ();
		}


		public int checkSamplesRange(){

			_numberOfSamples = PlayerPrefsManager.getSamples();
			//check samples
			if(_numberOfSamples % 64 != 0){
				_numberOfSamples = 64;
			}
			if(_numberOfSamples <= 63) _numberOfSamples = 64;
			if(_numberOfSamples >= 8193) _numberOfSamples = 8192;

			return _numberOfSamples;
		}


		void Update()
		{


 
		}

	}
}
