using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.CALIPSO;
using Unity.CALIPSO.MIC;


//DEPRECATED
//DEPRECATED
//DEPRECATED
//DEPRECATED
//DEPRECATED
//DEPRECATED
//DEPRECATED
//DEPRECATED
//JOINED WITH SOUNDBARCREATION


public class soundBarBiasCreation : MonoBehaviour
{
    
    private processAudio _processAudio;
    
    [Range(1, 100)]
   private float optimizationLevel = 1.0f;

    //public GameObject soundBar;
    public GameObject soundBarBias;
    private GameObject SoundBarCanvas;

    private bool _soundBarBiasActive = false;
    private calipsoManager cm;
    private micController mic;

    void Awake() {
        //cm = GameObject.Find("CalipsoManager").GetComponent<CalipsoManager>();
        _processAudio =  FindObjectOfType<processAudio>();
        //SoundBarCanvas = GameObject.Find("SoundBarCanvas");
        cm = FindObjectOfType<calipsoManager>();
        mic= FindObjectOfType<micController>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
       
        
       SoundBarCanvas = gameObject;
                 
    }

    // Update is called once per frame
    void Update()
    {
        //creare o destruiré desde el manager
        /*
        //SOUNDBAR IN dev MODE
        if(cm.DevMode && !soundBarActive){
            createSoundBar();
            soundBarActive = true;
        }

        //SOUNDBAR IN SETTINGS MODE
        if(
            (!cm.DevMode && soundBarActive) || 
            mic.IsWorking == false
        ){
            Debug.Log("Destroy SoundBar");
            deleteSoundBar();
            soundBarActive = false;
        }
        */


    }


    public void createSoundBar(){

        optimizationLevel = PlayerPrefsManager.GetOptimizeSamples();
        //ya activo o no es dev mode!
        if(_soundBarBiasActive || !cm.DevMode) return;

        Debug.Log("*--createSoundBar--*"+_soundBarBiasActive.ToString());

        //int totalBars = mic.checkSamplesRange();
        int totalBars = mic.checkSamplesRange()/ (int) optimizationLevel;
        int anchoBars = (Screen.width/totalBars);



        for(int i = 0; i < totalBars; i++){
            GameObject soundBarBiasPrefab = Instantiate(soundBarBias, new Vector3(anchoBars*i, 0, 0), Quaternion.identity) as GameObject;
            soundBarBiasPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(anchoBars, 10);
            soundBarBiasPrefab.GetComponent<soundBarManager>().arrayNumber = (mic.checkSamplesRange()/totalBars)*i;
            soundBarBiasPrefab.GetComponent<soundBarManager>().currentWidth = anchoBars;
            soundBarBiasPrefab.transform.SetParent (transform, false);
            soundBarBiasPrefab.name="SoundBarBias";
        }


        _soundBarBiasActive = true;

    }



    public void deleteSoundBar(){

        //ya desactivado!
        if(!_soundBarBiasActive) return;

        Debug.Log("*--deleteSoundBar--*"+_soundBarBiasActive.ToString());
        
        //int totalBars = mic.checkSamplesRange()/(int)optimizationLevel;
        //int anchoBars = Screen.width/totalBars;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        _soundBarBiasActive = false;

    }




}
