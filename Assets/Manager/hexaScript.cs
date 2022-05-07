using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hexaScript : MonoBehaviour
{

    public bool isActive = false;
    public bool isPlaying = false;

    public float zeta = 0.0f;

    public float scaleZOriginal = 0;

    // Start is called before the first frame update
    void Start()
    {
        scaleZOriginal = transform.localScale.z;
    }

    // Update is called once per frame
    void Update()
    {
        

        if(isActive){
            gameObject.name = "HexaActive";
            zeta = (Mathf.Sin(Time.fixedTime*1)*10f);
            transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y, scaleZOriginal+(Mathf.Sin(Time.fixedTime)*1f));

            if(isPlaying == false){
                transform.GetComponent<AudioSource>().Play();
                isPlaying = true;
            }
            
        }


    }
}
