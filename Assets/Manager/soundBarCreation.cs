using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.CALIPSO;
using Unity.CALIPSO.MIC;

public class soundBarCreation : MonoBehaviour
{
    
    private processAudio _processAudio;
    
    [Range(1, 100)]
    public int optimizationLevel = 1;

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


    }


    void createSoundBar(){

        int totalBars = mic.checkSamplesRange()/optimizationLevel;
        int anchoBars = Screen.width/totalBars;

        for(int i = 0; i < totalBars; i++){
            GameObject soundBarPrefab = Instantiate(soundBar, new Vector3(anchoBars*i, 0, 0), Quaternion.identity) as GameObject;
            soundBarPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(anchoBars, 10);
            soundBarPrefab.GetComponent<soundBarManager>().arrayNumber = (mic.checkSamplesRange()/totalBars)*i;
            soundBarPrefab.GetComponent<soundBarManager>().currentWidth = anchoBars;
            soundBarPrefab.transform.SetParent (SoundBarCanvas.transform, false);
        }

    }

    void deleteSoundBar(){

        int totalBars = mic.checkSamplesRange()/optimizationLevel;
        int anchoBars = Screen.width/totalBars;
        foreach (Transform child in SoundBarCanvas.transform)
        {
            Destroy(child.gameObject);
        }

    }




}
