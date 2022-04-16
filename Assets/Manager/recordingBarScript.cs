using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recordingBarScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
   
 
    }

    // Update is called once per frame
    void Update()
    {

        float valorSine = Mathf.Sin(Time.time)*100;

        GetComponent<RectTransform>().sizeDelta = new Vector2(
            10f,
            Mathf.Abs(valorSine)
        );
        
    }
}
