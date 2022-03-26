using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.CALIPSO{

    public class CalipsoManager : MonoBehaviour
    {

        [Header("Options")] [Tooltip("Generic options to Calipso Game Manager")]
        public bool DevMode = false;
        public bool settingsMode = false;

        private Camera mainCamera;
        private Camera devCamera;

        public GameObject canvasSettings;


        //public static CalipsoManager instance;

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
            
            //Enter in Dev Mode
            if (Input.GetKeyDown(KeyCode.E))
            {
               this.DevMode = !this.DevMode;

                //change the cameras
               ChangeCameras(this.DevMode);
            }

            //what to do on dev mode
            if(this.DevMode)
            {
                //things to do in dev mode
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


        

    }
}
