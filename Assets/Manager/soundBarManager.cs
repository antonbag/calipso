using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*****  cada bar *****/



public class soundBarManager : MonoBehaviour
{

    private processAudio _processAudio;
    public GameObject micController;

    public int arrayNumber;
    public int currentWidth;

    void Awake() {
        //cm = GameObject.Find("CalipsoManager").GetComponent<CalipsoManager>();
      

    }

    // Start is called before the first frame update
    void Start()
    {
        micController = GameObject.Find("micController");
        _processAudio =  micController.GetComponent<processAudio>();
     } 

    // Update is called once per frame
    void Update()
    {   
        
        
        if(_processAudio.spectrumData.Length > 0){
           //Debug.Log(_processAudio.spectrumData[arrayNumber]);

            //Debug.Log(arrayNumber);
            GetComponent<RectTransform>().sizeDelta = new Vector2(
                currentWidth,
                _processAudio.spectrumData[arrayNumber]*(_processAudio.powerMultiplier*10)
            );

            //soundBarPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(_processAudio.spectrumData[arrayNumber], 10);
        }
        
    }
}
