using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.CALIPSO{

    public class calipsoManager : MonoBehaviour
    {

        [Header("Options")] [Tooltip("Generic options to Calipso Game Manager")]
        public bool DevMode = false;
        public bool settingsMode = false;

        private Camera mainCamera;
        private Camera devCamera;

        private float _deltaTime;

        public GameObject canvasSettings;
        public GameObject fpsController;

        /*
        [Header("==Sound DEFAULTS==")]

        public int samples = 256;
        public int sensitivity = 500;
        public float threshold = 0.5f;


        void Awake(){
            PlayerPrefsManager.SetSamples(samples);
            PlayerPrefsManager.SetSensitivity(sensitivity);
            if(threshold > 1f) threshold = 1f;
            PlayerPrefsManager.SetThreshold(threshold); 
        }
        */

        [Space(20)]
        public int orbeNumber = 1;   

        
        // Start is called before the first frame update
        void Start()
        {   
            mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            devCamera = GameObject.Find("DevCamera").GetComponent<Camera>();

            canvasSettings.SetActive(false);

            

            //Select the main camera depending on the dev mode
            ChangeCameras(this.DevMode);
        }
        // Update is called once per frame
        void Update()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
            //Enter in Dev Mode
            if (Input.GetKeyDown(KeyCode.E))
            {
               this.DevMode = !this.DevMode;

                //change the cameras
               ChangeCameras(this.DevMode);
            }

            /***************************************/
            /************* DEV MODE ***************/
            /***************************************/

            //what to do on dev mode
            if(this.DevMode)
            {
                //FPSs
                float fps = 1.0f / _deltaTime;
                fpsController.GetComponent<TMPro.TextMeshProUGUI>().text = Mathf.Ceil (fps).ToString ();
    
            }

            //OPEN/close SETTINGS
            if(Input.GetKeyDown(KeyCode.Escape))
            {
               exitSettings();
            }




        }
        
     
        public void exitSettings(){
            this.settingsMode =  !this.settingsMode;
            canvasSettings.SetActive(this.settingsMode);
        }

        public void ChangeCameras(bool DevMode){

            if(DevMode)
            {
                mainCamera.enabled = false;
                devCamera.enabled = true;
            }else{
                mainCamera.enabled = true;
                devCamera.enabled = false;
            }

        }

        /** map calculation como en p5 **/
        public float mapToDigital(float value, float min1, float max1, float min2, float max2){

            float normalizedValue = Mathf.InverseLerp(min1, max1, value);
            float result = Mathf.Lerp(min2, max2, normalizedValue);

            return result;
        }
        

    }
}
