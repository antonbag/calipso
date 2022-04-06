using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*****  cada bar *****/



public class soundBarManager : MonoBehaviour
{

    private processAudio _processAudio;
    public GameObject micController;

    public int arrayNumber;
    public int currentWidth;

    private float valorAnterior;

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
            //IMAGES



            if(gameObject.name=="SoundBarBias"){
                Debug.Log(gameObject.name);
                GetComponent<RectTransform>().sizeDelta = new Vector2(
                        1,
                        _processAudio.spectrumData[arrayNumber]*(_processAudio.powerMultiplier*10)
                );
                GetComponent<Image>().color = new Color(
                    1,
                    1,
                    1,
                    0.2f
                );
            }else{
                Debug.Log(gameObject.name);
                GetComponent<RectTransform>().sizeDelta = new Vector2(
                    currentWidth,
                    _processAudio.spectrumData[arrayNumber]*(_processAudio.powerMultiplier*10)
                );
        
            }

         

            valorAnterior = _processAudio.spectrumData[arrayNumber];
   
            //_processAudio.spectrumData[arrayNumber]*(_processAudio.powerMultiplier*10)
            


            //soundBarPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(_processAudio.spectrumData[arrayNumber], 10);
        }
        
    }
}
