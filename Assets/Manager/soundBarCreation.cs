using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class soundBarCreation : MonoBehaviour
{
    
    private processAudio _processAudio;
    
    [Range(1, 100)]
    public int optimizationLevel = 1;

    public GameObject soundBar;
    public GameObject SoundBarCanvas;
 

    void Awake() {
        //cm = GameObject.Find("CalipsoManager").GetComponent<CalipsoManager>();
        _processAudio =  FindObjectOfType<processAudio>();
        SoundBarCanvas = GameObject.Find("SoundBarCanvas");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        int totalBars = _processAudio.numberOfSamples/optimizationLevel;
        int anchoBars = Screen.width/totalBars;

        for(int i = 0; i < totalBars; i++){
            GameObject soundBarPrefab = Instantiate(soundBar, new Vector3(anchoBars*i, 0, 0), Quaternion.identity) as GameObject;
            soundBarPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(anchoBars, 10);
            soundBarPrefab.GetComponent<soundBarManager>().arrayNumber = (_processAudio.numberOfSamples/totalBars)*i;
            soundBarPrefab.GetComponent<soundBarManager>().currentWidth = anchoBars;
            soundBarPrefab.transform.SetParent (SoundBarCanvas.transform, false);
        }
        
        //copies of soundBar
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
