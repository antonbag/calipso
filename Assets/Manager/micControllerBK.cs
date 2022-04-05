using UnityEngine;
using UnityEngine.UI; //for accessing Sliders and Dropdown
using System.Collections.Generic; // So we can use List<>
using UnityEngine.Audio; // So we can use AudioMixer



namespace Unity.CALIPSO.MIC{


	[RequireComponent(typeof(AudioSource))]
	public class micControllerBK : MonoBehaviour
	{
	
	 	private int _numberOfSamples = 128;

		//public float minThreshold = 0.0001f;
		//private float _optimizeSample = 1.0f;
		//private float _sensitivity = 1.0f;
		public float frequency = 0.0f;
		public FFTWindow fftWindow;

		public AudioMixer audioMixer;
		
		//private int samples = 128; 
		private AudioSource _audioSource;
		
		public bool IsWorking = true;
		bool _lastValueOfIsWorking;

		public bool RaltimeOutput = true;
		bool _lastValueOfRaltimeOutput;


		private soundBarCreation sb;
		private settingController sc;

		// Start is called before the first frame update
		void Start()
		{
			
			_audioSource = GetComponent<AudioSource>();



			sc = FindObjectOfType<settingController>();
			sb = FindObjectOfType<soundBarCreation>();



			//initialize input with default mic
			UpdateMicrophone (); 
		}

		public void UpdateMicrophone(){

			Debug.Log("----------****UpdateMicrophone***------------");
			
			sb.deleteSoundBar();
			WorkStop();
			
			//pongo el audioMixer en off para que no se escuche reverb
			audioMixer.SetFloat ("Volume", -80.0f);

			WorkStart();
			//sb.createSoundBar();
			
			//Debug.Log(PlayerPrefsManager.getSamples());

		}

 

		public void WorkStart()
		{
			#if !UNITY_WEBGL
					
					_audioSource.clip = Microphone.Start(sc.getMicrophone(), true, 10, sc.audioSampleRate);
					_audioSource.loop = true;
					IsWorking = true;
					while (!(Microphone.GetPosition(sc.getMicrophone()) > 0))
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




/*
 		public void sensitivityValueChangedHandler(Slider SensitivitySlider){
			_sensitivity = SensitivitySlider.value;
			PlayerPrefsManager.SetSensitivity(_sensitivity);
		}
		public void thresholdValueChangedHandler(Slider thresholdSlider){
			minThreshold = thresholdSlider.value;
		}
		public void optimizeSampleSliderValueChangedHandler(Slider optimizeSampleSlider){
			_optimizeSample = optimizeSampleSlider.value;
			PlayerPrefsManager.SetOptimizeSamples(_optimizeSample);
			UpdateMicrophone ();
		}
*/

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
