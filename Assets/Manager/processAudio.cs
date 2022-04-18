using UnityEngine;
using Unity.CALIPSO;
using Unity.CALIPSO.MIC;


public class processAudio : MonoBehaviour
{
        //scripts (in a class)
        public getScrits _scripts;

        [System.Serializable]
        public class getScrits {
                public settingController sc;
                public calipsoManager cm;
                public micController mic;
        }


        [Space(20)]

        //control from settings. public to get it in real time from soundbarManager
        [Range(1, 1000)] public float powerMultiplier;
        public GameObject gmVolumeValue;

        [Space(20)]


        [Header("===Time range===")]
        [Range(0.1f, 3.0f)] public float stepVolume = 1.0f;
        [Range(0.01f, 0.10f)] public float stepMain = 0.05f;


        [Space(20)]

        [Header("===Spectrum data===")]
        public float[] spectrumData;
        public float[] spectrumDataBalanceo;
        public float[] spectrumDataAnterior;


        [Space(20)]

        [Header("===Fundamentals data===")]
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

       

        
        private Vector2 currentAvMinMax = new Vector2(0, 0);

        [Space(20)]
        [Header("===Average===")]

        public float[] averageMin = new float[8];
        public float[] averageMax = new float[8];
        //supermax es el valor máximo para bajar el volumen 
        public float superMax = 1f;
        public float averageVolume = 0f;


        private float _limitFq;
    
        private int _numberOfSamples;
        private AudioSource _audioSource;
        private FFTWindow fftWindow;


        //cada cierto tiempo
        private float nextTime = 0.0f;
        private float currentUpdateTime = 0.0f;

        private float ponderacionPOW=0.5f;
        private float recThreshold=0.1f;
        private float amplitudPOW=0.5f;

        private float _maxSpectrumValue;

        [Space(20)]
        [Header("===Recording===")]
        public float recordingLimit = 10.0f;
        public float recordingRestLimit = 3.0f;
        public float recordingMinimum = 1.0f;
        public bool isRecording = false;
        private int recordingCounter = 0;
        private int recordingRestCounter = 0;
        private int recordingMinimumCounter = 0;
 

        void Start()
        {
            //cm =  FindObjectOfType<calipsoManager>();
            //sc =  FindObjectOfType<settingController>();

            stepMain = 0.05f;

            //Get power from prefs
            powerMultiplier = PlayerPrefsManager.GetSensitivity ();
            _limitFq = PlayerPrefsManager.GetLimitFq ();


           
            _audioSource = gameObject.GetComponent<AudioSource>();

            //check the number of samples
            _numberOfSamples = _scripts.mic.checkSamplesRange();

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


                     
        }



 
        void Update()
        {   
            

            /*******************************/
            /*******************************/
            /******** cada  50 ms **********/
            /*******************************/
            /*******************************/
            currentUpdateTime += Time.deltaTime;
            if(currentUpdateTime >= stepMain){
                currentUpdateTime = 0f;

                powerMultiplier = PlayerPrefsManager.GetSensitivity ();
                _limitFq = PlayerPrefsManager.GetLimitFq ();
                //check the number of samples
                _numberOfSamples = _scripts.mic.checkSamplesRange();

                ponderacionPOW = PlayerPrefsManager.GetSoundBias ();
                recThreshold = PlayerPrefsManager.GetThreshold ();

                // initialize spectrum array every frame
                //DEV
                float[] spectrum = new float[_numberOfSamples];
                spectrumData = new float[_numberOfSamples];
                spectrumDataBalanceo = new float[_numberOfSamples];


                //relleno el espectrum
                _audioSource.GetSpectrumData(spectrum, 0, fftWindow);
                //_audioSource.GetOutputData(spectrum, 0);

 

                //CONTROL DE VOLUMEN Y REC
                //cojo la frecuencia 440hz y analizo si hay sonido para no malgastar
                
                if(
                    (spectrum[150]*10) <= recThreshold &&
                    !isRecording
                ){
                    //Debug.Log("No hay sonido:"+(spectrum[150]*10)+"<"+recThreshold);
                    return;
                    /*
                        //si estoy grabando y se acaba el sonido
                        if(isRecording && (recordingMinimum >= recordingMinimumCounter)){
                            guardarClip();
                        }else{}
                    */


                    
                    
                }
                
                
                
         
        
                //OPERANDO!
                //spectrumData = spectrum;

                //samples limitados
                int _samplesLimited  = (int)(_numberOfSamples * _limitFq);



                int totalFundamental = _samplesLimited/8;
                fundamentalSpectrum = new float[8];
                averageValue = 0f;
                int fundContador = 0;
                int contadorF = 0;

                //TEST
                f0 = new float[totalFundamental];
                f1 = new float[totalFundamental];
                f2 = new float[totalFundamental];
                f3 = new float[totalFundamental];
                f4 = new float[totalFundamental];
                f5 = new float[totalFundamental];
                f6 = new float[totalFundamental];
                f7 = new float[totalFundamental];
          
                for(int i = 1; i < _samplesLimited+1; i++){

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
                    float mapeo = _scripts.cm.mapToDigital(i, 0, _samplesLimited, 0.0f, 1);
                    float balanceo = (Mathf.Pow(mapeo, ponderacionPOW)) * amplitudPOW;


                    //MUESTRO EL BALANCEO
                    //spectrumDataBalanceo[i] = balanceo/5;

           
                    //spectrumData[i] = (spectrum[i]*balanceo)*powerMultiplier;

                    //balanceo cada cuatro muestras
                    if(i % 4 == 0){
                        spectrumDataBalanceo[i] = balanceo/4;
                    }
   
                    
                    //if(spectrumDataAnterior[i] == 0) spectrumDataAnterior[i] = 0.1f;

                    //SPECTRUM TO DATA
                    spectrumData[i] = (spectrum[i]*balanceo) * (powerMultiplier*1000);


                    //STABILITY BY AVERAGE
                    spectrumData[i] = (spectrumData[i]+spectrumDataAnterior[i])/2;



                    //CONTROL
                    if(spectrumData[i] >= 5000) spectrumData[i] = 5000;

                    //GET AVERAGE
                    GetMinMax(spectrumData[i]);
  
                    averageValue += spectrumData[i];
        
                    //almaceno valores para ver qué pasa... DEV
                    switch (fundContador)
                    {
                        case 0:
                            f0[contadorF] = spectrumData[i];
                            break;
                        case 1:
                            //test lerp smoth
                            f1[contadorF] = Mathf.Lerp(spectrumData[i], averageValue, 0.05f);
                            break;
                        case 2:
                            f2[contadorF] = spectrumData[i];
                            break;
                        case 3:
                            f3[contadorF] = spectrumData[i];
                            break;
                        case 4:
                            f4[contadorF] = spectrumData[i];
                            break;
                        case 5:
                            f5[contadorF] = spectrumData[i];
                            break;
                        case 6:
                            f6[contadorF] = spectrumData[i];
                            break;
                       case 7:
                            f7[contadorF] = spectrumData[i];
                            break;
                        default:
                            break;
                    }
                    
               
                    contadorF++;

                    //FUNDAMENTALS
                    if(i % totalFundamental == 0){   

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

                        contadorF = 0;
                
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


            averageVolume = GetAveragedVolume();



            //START STOP RECORDING
            if(averageVolume > 0.01f){
                
                if(!isRecording && (recordingRestCounter > recordingRestLimit)){
                    isRecording = true;
                    Debug.Log("voy a grabar!: " + averageVolume);
                    recordingMinimumCounter = 0;
                }
            }


            
            /*******************************/
            /*******************************/
            /******** cada  1 seg **********/
            /*******************************/
            /*******************************/
            if (Time.time > nextTime ) {
                
                /*******************************/
                // VOLUMEN 
                spectrumDataAnterior = spectrumData;
                nextTime += stepVolume;

                gmVolumeValue.GetComponent<TMPro.TextMeshProUGUI>().text = averageVolume.ToString();

                //AUTOVOLUME
                if(PlayerPrefsManager.GetAutovolume()){
                    if(superMax > 200){
                        _scripts.sc.sensitivitySlider.value = PlayerPrefsManager.GetSensitivity()*0.9f;
                        //PlayerPrefsManager.SetSensitivity(PlayerPrefsManager.GetSensitivity()-1.0f);
                    }
                    if(superMax < 50){
                        _scripts.sc.sensitivitySlider.value = PlayerPrefsManager.GetSensitivity()*1.1f;
                        //PlayerPrefsManager.SetSensitivity(PlayerPrefsManager.GetSensitivity()-1.0f);
                    }
                }


                /*******************************/
                // RECORDING
                recordingRestCounter++;
                //si estoy grabando y no he llegado a la duracion maxima...
                if(
                    isRecording && 
                    (recordingCounter < recordingLimit)
                ){
                    //Debug.Log("sigo grabando: "+ recordingCounter);
                    recordingCounter++;
                    recordingMinimumCounter++;
                }


                //si estoy grabando y he llegado a la duracion maxima...
                if(
                    isRecording && 
                    (recordingCounter >= recordingLimit) && 
                    (recordingMinimumCounter > recordingMinimum)
                ){
                    guardarClip();

                }

                //Debug.Log("recordingCounter: "+ recordingCounter+" recordingLimit: "+recordingLimit);

            }




        }





        public void guardarClip(){
            _scripts.mic.guardaClip();
            isRecording = false;
            recordingCounter = 0;
            recordingRestCounter = 0;
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
            
            return (a/64)*100;
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



