using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.CALIPSO.MIC;


public class processAudio : MonoBehaviour
    {
        [Range(1, 1000)] public float powerMultiplier;
        [Range(64,8192)] public int numberOfSamples = 96; //step by 2

        private AudioSource _audioSource;
        private FFTWindow fftWindow;
        public float lerpTime = 1;
        public Slider sensitivitySlider;

        // initialize spectrum array
        public GameObject soundBar;
        private micController mic;

        public float[] spectrumData;

        float[] spectrum;


        //cada cierto tiempo
        private float nextActionTime = 0.0f;
        public float periodo = 1.0f;



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

            mic = gameObject.GetComponent<micController>();
            _audioSource = gameObject.GetComponent<AudioSource>();

        

            
        }


        void Update()
        {   
            // initialize spectrum array every frame (for debuging )
            //DEV
            float[] spectrum = new float[numberOfSamples];

            //relleno el espectrum
            _audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Hamming);

            spectrumData = spectrum;


            //obtengo el volumen cada secundo
            if (Time.time > nextActionTime ) {
                nextActionTime += periodo;
                Debug.Log (GetAveragedVolume());
            }



            /*
            var audioScale = Mathf.Pow(spectrumData[i] * AudioScale, Power);
            var newScale = new Vector3(_originalScale.x, _originalScale.y + audioScale, _originalScale.z);
            var halfScale = newScale / 2.0f;
            */
        }


        public void SensitivityValueChangedHandler(Slider sensitivitySlider){
            powerMultiplier = sensitivitySlider.value;
        }




        //VOLUMEN
        public float GetAveragedVolume()
        { 
            float[] data = new float[64];
            float a = 0;
            _audioSource.GetOutputData(data,0);
            foreach(float s in data)
            {
                a += Mathf.Abs(s);
            }
            return a/64;
        }



        //prueba
        /*
        public float GetFundamentalFrequency()
        {
            float fundamentalFrequency = 0.0f;
            float[] data = new float[samples];
            _audioSource.GetSpectrumData(data,0,fftWindow);
            float s = 0.0f;
            int i = 0;
            for (int j = 1; j < samples; j++)
            {
                Debug.Log (data[j]);
            //Debug.Log (minThreshold);
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
        */







    }

