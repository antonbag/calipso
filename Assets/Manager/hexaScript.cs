using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hexaScript : MonoBehaviour
{

    public bool isActive = false;
    public bool isPlaying = false;

    public float zeta = 0.0f;

    public float scaleZOriginal = 0;

    public float altoZ = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        scaleZOriginal = transform.localScale.z;
        altoZ = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        

        if(isActive && transform.localScale.z <= (scaleZOriginal+altoZ)-0.01f){

            Debug.Log(transform.localScale.z);

            gameObject.name = "HexaActive";
            //zeta = (Mathf.Sin(Time.fixedTime*1)*10f);
            //transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y, scaleZOriginal+Time.deltaTime);
            
            
            //LERP SOLUTION
            float distancia = Mathf.Abs(scaleZOriginal+altoZ - transform.localScale.z);

            Debug.Log(distancia);
            
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x,transform.localScale.y, scaleZOriginal+scaleZOriginal+altoZ), Time.deltaTime*distancia);


            if(isPlaying == false){
                transform.GetComponent<AudioSource>().Play();
                isPlaying = true;
            }
            
        }


    }
}
