using UnityEngine;
using Unity.CALIPSO;
using Unity.CALIPSO.MIC;


public class processAudio : MonoBehaviour
{
        //control from settings. public to get it in real time from soundbarManager
        [Range(1, 1000)] public float powerMultiplier;
        public float lerpTime = 1;

        public GameObject gmVolumeValue;

        public float[] spectrumData;
        public float[] spectrumDataBalanceo;
        public float[] spectrumDataAnterior;

        public settingController sc;


        //fundamental frequencies
        public float[] fundamentalSpectrum = new float[8];
        public float[] f0 = new float[0];
        public float[] f1 = new float[0];
        public float[] f2 = new float[0];
        public float[] f3 = new float[0];
        public float[] f4 = new float[0];
        public float[] f5 = new float[0];
        public float[] f6 = new float[0];
        public float[] f7 = new float[0];
        private float averageValue = 0f;
        private int fundContador = 0;
            
    
        private Vector2 currentAvMinMax = new Vector2(0, 0);
  
        public float[] averageMin = new float[8];
        public float[] averageMax = new float[8];
        //supermax es el valor máximo para bajar el volumen 
        public float superMax = 1f;

        public debugFundamental debugF;


        public float stepVolume = 1.0f;
        public float stepMain = 0.05f;
        private float _limitFq;






        private int _numberOfSamples;
        private AudioSource _audioSource;
        private FFTWindow fftWindow;

        private micController mic;
        private calipsoManager cm;

        //cada cierto tiempo
        private float nextActionTime = 0.0f;
        private float currentUpdateTime = 0.0f;

        [Range(0, 10)] public float ponderacionPOW=0.5f;
        [Range(0, 10)] public float amplitudPOW=0.5f;




        private float _maxSpectrumValue;


        void Start()
        {
            cm =  FindObjectOfType<calipsoManager>();
            sc =  FindObjectOfType<settingController>();

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
            spectrumDataBalanceo = new float[_numberOfSamples];

            fundamentalSpectrum = new float[8];
            f0 = new float[0];
            f1 = new float[0];
            f2 = new float[0];
            f3 = new float[0];
            f4 = new float[0];
            f5 = new float[0];
            f6 = new float[0];
            f7 = new float[0];
            currentAvMinMax = new Vector2(0, 0);
            averageMin[0] = 1f;
            averageMax[0] = 1f;


            float ponderacionPOW = PlayerPrefsManager.GetSoundBias ();
                     
        }



 
        void Update()
        {   
            


           //obtengo el volumen cada segundo
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

                ponderacionPOW = PlayerPrefsManager.GetSoundBias ();

                // initialize spectrum array every frame
                //DEV
                float[] spectrum = new float[_numberOfSamples];
                spectrumData = new float[_numberOfSamples];
                spectrumDataBalanceo = new float[_numberOfSamples];


                //relleno el espectrum
                _audioSource.GetSpectrumData(spectrum, 0, fftWindow);
        
                //OPERANDO!
                //spectrumData = spectrum;

                //samples limitados
                int _samplesLimited =  (int)(_numberOfSamples * _limitFq);



                int totalFundamental = _samplesLimited/8;
                fundamentalSpectrum = new float[8];
                averageValue = 0f;
                fundContador = 0;

                //TEST
                f0 = new float[_samplesLimited];
                f1 = new float[_samplesLimited];
                f2 = new float[_samplesLimited];
                f3 = new float[_samplesLimited];
                f4 = new float[_samplesLimited];
                f5 = new float[_samplesLimited];
                f6 = new float[_samplesLimited];
                f7 = new float[_samplesLimited];
          
                for(int i = 0; i < _samplesLimited; i++){

                    //if(spectrumData[i] == 0) spectrumData[i] = 0.1f;
                    
                    float miSino = Mathf.Sin(i);

                       
                    //raw
                    //spectrumData[i] = spectrum[i];

                    //1A aproximacion: media entre valor anterior y actual
                    //spectrumData[i] = ((spectrum[i]*spectrumData[i])/2)*powerMultiplier;

                    //2A aproximacion: clampear
                    //spectrumData[i] = Mathf.Clamp(spectrum[i], 0,1)*powerMultiplier;

                    //3A aproximacion: media con anterior
                    //spectrumData[i] = (spectrum[i]+spectrumDataAnterior[i])/2;

                    //4A aproximacion: media con anterior
                    //spectrumData[i] = Mathf.Clamp(((spectrum[i]*powerMultiplier+spectrumDataAnterior[i])/2), 0,1);

                    //5A aproximacion: sino
                    //spectrumData[i] = Mathf.Clamp(((spectrum[i]*powerMultiplier+spectrumDataAnterior[i])/2)*miSino, 0,1);
  
                    //MAPEO DE Samples 
                    float mapeo = cm.mapToDigital(i, 0, _samplesLimited, 0.0f, 1);
                    float balanceo = (Mathf.Pow(mapeo, ponderacionPOW)) * amplitudPOW;


                    //MUESTRO EL BALANCEO
                    //spectrumDataBalanceo[i] = balanceo/5;

           
                    //spectrumData[i] = (spectrum[i]*balanceo)*powerMultiplier;

                    //balanceo cada cuatro muestras
                    if(i % 4 == 0){
                        spectrumDataBalanceo[i] = balanceo/4;
                    }
   

      

                    spectrumData[i] = 
                    
                    (
                        (
                            //spectro + media superior /2
                            //(spectrum[i]+averageMax[fundContador])/2
                            spectrum[i]
                        )
                    *balanceo
                    )
                    *(powerMultiplier*100);


                    //CONTROL
                    if(spectrumData[i] >= 1000) spectrumData[i] = 10000;

                    //GET AVERAGE
                    GetMinMax(spectrumData[i]);
  
                    averageValue += spectrumData[i];
        
                    //almaceno valores para ver qué pasa... DEV
                    switch (fundContador)
                    {
                        case 0:
                            f0[i] = spectrumData[i];
                            break;
                        case 1:
                            f1[i] = spectrumData[i];
                            break;
                        case 2:
                            f2[i] = spectrumData[i];
                            break;
                        case 3:
                            f3[i] = spectrumData[i];
                            break;
                        case 4:
                            f4[i] = spectrumData[i];
                            break;
                        case 5:
                            f5[i] = spectrumData[i];
                            break;
                        case 6:
                            f6[i] = spectrumData[i];
                            break;
                       case 7:
                            f7[i] = spectrumData[i];
                            break;
                        default:
                            break;
                    }
                    
               
                 

                    //FUNDAMENTALS
                    if(i % totalFundamental == (totalFundamental/2)){   

                        //el average es el valor partido por el total
                        averageValue = averageValue/totalFundamental;

                        //el espectro fundamental de cada octava es el average
                        //fundamentalSpectrum[fundContador] = averageValue;
                        fundamentalSpectrum[fundContador] = (currentAvMinMax.x+currentAvMinMax.y)/2;
                        
                        //el espectro fundamental de cada octava es el average
                        averageMin[fundContador] = currentAvMinMax.x;
                        averageMax[fundContador] = currentAvMinMax.y;

                        
                        //restablezco currents
                        currentAvMinMax = new Vector2(0, 0);
                        if(fundContador < 7){
                            fundContador++;
                        }
                
                    }



      
                }
   

            }

            //SUPERMAX
            //calculo el volumen maximo con la media de todos los averageMax
            float superMaxSuma = 0f;
            for(int f = 0; f < averageMax.Length; f++){
                superMaxSuma += averageMax[f];
            }

            superMax = superMaxSuma/averageMax.Length;
            if(superMax > 300){
                sc.sensitivitySlider.value = PlayerPrefsManager.GetSensitivity()-1.0f;
                //PlayerPrefsManager.SetSensitivity(PlayerPrefsManager.GetSensitivity()-1.0f);
            }
            if(superMax < 100){
                sc.sensitivitySlider.value = PlayerPrefsManager.GetSensitivity()+1.0f;
                //PlayerPrefsManager.SetSensitivity(PlayerPrefsManager.GetSensitivity()-1.0f);
            }
            Debug.Log(superMax);

           //if(PlayerPrefsManager.SetSensitivity(sensitivitySlider.value);)    

        }


        //COMPRUEBO MIN y max
        public Vector2 GetMinMax(float numero)
        { 
            //Vector2 minMax = new Vector2(numero, numero);

            //MIN
            if(numero < currentAvMinMax.x || currentAvMinMax.x==0){
                currentAvMinMax.x = numero;
            }
            //Debug.Log(currentAvMinMax.y);
            if(numero > currentAvMinMax.y || currentAvMinMax.y==0){
                currentAvMinMax.y = numero;
            }

            if(currentAvMinMax == new Vector2(0,0)){
                return new Vector2(1,1);
            }

            return currentAvMinMax;
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
    /*
    IEnumerator getSpectrum(){
        for(;;){
            yield return new WaitForSeconds(2.0f);
            float[] spectrum = new float[_numberOfSamples];
            spectrumData = new float[_numberOfSamples];
            _audioSource.GetSpectrumData(spectrum, 0, fftWindow);
            spectrumData = spectrum;
        }
    }
    */


}

[System.Serializable]
public class debugFundamental {
    public float f1 = 0f;
}