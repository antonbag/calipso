using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.CALIPSO;
using Unity.CALIPSO.MIC;

public class soundBarCreation : MonoBehaviour
{
    
    private processAudio _processAudio;
    
    [Range(1, 100)]
   private float optimizationLevel = 1.0f;

    public GameObject soundBar;
    public GameObject SoundBarCanvas;

    private bool soundBarActive = false;
    private calipsoManager cm;
    private micController mic;

    void Awake() {
        //cm = GameObject.Find("CalipsoManager").GetComponent<CalipsoManager>();
        _processAudio =  FindObjectOfType<processAudio>();
        SoundBarCanvas = GameObject.Find("SoundBarCanvas");
        cm = FindObjectOfType<calipsoManager>();
        mic= FindObjectOfType<micController>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
       
        
       
                 
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
        if(soundBarActive || !cm.DevMode) return;

        Debug.Log("*--createSoundBar--*"+soundBarActive.ToString());

        int totalBars = mic.checkSamplesRange()/ (int) optimizationLevel;
        int anchoBars = Screen.width/totalBars;

        for(int i = 0; i < totalBars; i++){
            GameObject soundBarPrefab = Instantiate(soundBar, new Vector3(anchoBars*i, 0, 0), Quaternion.identity) as GameObject;
            soundBarPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(anchoBars, 10);
            soundBarPrefab.GetComponent<soundBarManager>().arrayNumber = (mic.checkSamplesRange()/totalBars)*i;
            soundBarPrefab.GetComponent<soundBarManager>().currentWidth = anchoBars;
            soundBarPrefab.transform.SetParent (SoundBarCanvas.transform, false);
        }
        soundBarActive = true;

    }

    public void deleteSoundBar(){

        Debug.Log("*--deleteSoundBar--*"+soundBarActive.ToString());
        //ya desactivado!
        if(!soundBarActive) return;
        
        Debug.Log("*--soundBarActive--*"+soundBarActive.ToString());

        int totalBars = mic.checkSamplesRange()/(int)optimizationLevel;
        int anchoBars = Screen.width/totalBars;
        foreach (Transform child in SoundBarCanvas.transform)
        {
            Destroy(child.gameObject);
        }
        soundBarActive = false;

    }




}
