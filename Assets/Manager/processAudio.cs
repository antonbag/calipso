using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.CALIPSO.MIC;


public class processAudio : MonoBehaviour
{
        //control from settings. public to get it in real time from soundbarManager
        [Range(1, 1000)] public float powerMultiplier;
        public float lerpTime = 1;

        public GameObject gmVolumeValue;

        public float[] spectrumData;
        public float[] spectrumDataAnterior;

        public float periodo = 1.0f;



        private int _numberOfSamples;
        private AudioSource _audioSource;
        private FFTWindow fftWindow;

        private micController mic;

        //cada cierto tiempo
        private float nextActionTime = 0.0f;



        void Start()
        {


            //Get power from prefs
            powerMultiplier = PlayerPrefsManager.GetSensitivity ();


            mic = gameObject.GetComponent<micController>();
            _audioSource = gameObject.GetComponent<AudioSource>();

            //check the number of samples
            _numberOfSamples = mic.checkSamplesRange();

            //create the spectrum array (based on defined samples)
            //float[] spectrum = new float[_numberOfSamples];

            spectrumDataAnterior = new float[_numberOfSamples];
            spectrumData = new float[_numberOfSamples];
                     
        }

 
        void Update()
        {   

            //check the number of samples
            _numberOfSamples = mic.checkSamplesRange();

            //Debug.Log(_numberOfSamples);

            // initialize spectrum array every frame
            //DEV
            float[] spectrum = new float[_numberOfSamples];
            spectrumData = new float[_numberOfSamples];

            //relleno el espectrum
            _audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Hamming);
       
            //OPERANDO!
            //spectrumData = spectrum;
            for(int i = 0; i < _numberOfSamples; i++){

            
                spectrumData[i] = spectrum[i]*(i/10);

                //Debug.Log(spectrum[i]);
        /*         if(spectrum[i] >=0.1f){
                    spectrumData[i] = spectrum[i];
                }else{
                    spectrumData[i] = 1.0f;
                } */
            }
     
            

            //obtengo el volumen cada secundo
            if (Time.time > nextActionTime ) {
                nextActionTime += periodo;
                gmVolumeValue.GetComponent<TMPro.TextMeshProUGUI>().text = GetAveragedVolume().ToString();
            }



            /*
            var audioScale = Mathf.Pow(spectrumData[i] * AudioScale, Power);
            var newScale = new Vector3(_originalScale.x, _originalScale.y + audioScale, _originalScale.z);
            var halfScale = newScale / 2.0f;
            */
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

