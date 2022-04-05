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

        public float stepVolume = 1.0f;
        public float stepMain = 0.05f;
        private float _limitFq;
    


        private int _numberOfSamples;
        private AudioSource _audioSource;
        private FFTWindow fftWindow;

        private micController mic;

        //cada cierto tiempo
        private float nextActionTime = 0.0f;
        private float currentUpdateTime = 0.0f;


        void Start()
        {

            stepMain = 0.05f;

            //Get power from prefs
            powerMultiplier = PlayerPrefsManager.GetSensitivity ();
            _limitFq = PlayerPrefsManager.GetLimitFq ();


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

           //obtengo el volumen cada secundo
            if (Time.time > nextActionTime ) {
                spectrumDataAnterior = spectrumData;
                nextActionTime += stepVolume;
                gmVolumeValue.GetComponent<TMPro.TextMeshProUGUI>().text = GetAveragedVolume().ToString();
            }

             //obtengo el volumen cada 0.05
            currentUpdateTime += Time.deltaTime;
            if(currentUpdateTime >= stepMain){
                currentUpdateTime = 0f;

                powerMultiplier = PlayerPrefsManager.GetSensitivity ();
                _limitFq = PlayerPrefsManager.GetLimitFq ();
                //check the number of samples
                _numberOfSamples = mic.checkSamplesRange();

                //Debug.Log(_numberOfSamples);

                // initialize spectrum array every frame
                //DEV
                float[] spectrum = new float[_numberOfSamples];
                spectrumData = new float[_numberOfSamples];


                //relleno el espectrum
                _audioSource.GetSpectrumData(spectrum, 0, fftWindow);
        
                //OPERANDO!
                //spectrumData = spectrum;
            
                for(int i = 0; i < ((int) _numberOfSamples*_limitFq); i++){

                    if(spectrumData[i] == 0) spectrumData[i] = 0.1f;
                    

                    //raw
                    //spectrumData[i] = spectrum[i];

                    //1A aproximacion: media entre valor anterior y actual
                    //spectrumData[i] = ((spectrum[i]*spectrumData[i])/2)*powerMultiplier;

                    //2A aproximacion: clampear
                    //spectrumData[i] = Mathf.Clamp(spectrum[i], 0,1)*powerMultiplier;

                    //3A aproximacion: media con anterior
                    //spectrumData[i] = (spectrum[i]+spectrumDataAnterior[i])/2;

                    //3A aproximacion: media con anterior
                    spectrumData[i] = Mathf.Clamp(((spectrum[i]*powerMultiplier+spectrumDataAnterior[i])/2), 0,1);
       
                }
   

            }



            


     
            //StartCoroutine(getSpectrum());




 

 

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



    //CORUTINA PARA LE PROCESO DE AUDIO
    //algo no estoy haciendo bien...
    //TODO
    IEnumerator getSpectrum(){
        for(;;){
            yield return new WaitForSeconds(2.0f);
            float[] spectrum = new float[_numberOfSamples];
            spectrumData = new float[_numberOfSamples];
            _audioSource.GetSpectrumData(spectrum, 0, fftWindow);
            spectrumData = spectrum;
        }
    }


}

