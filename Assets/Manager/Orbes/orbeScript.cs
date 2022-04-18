using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.CALIPSO;

public class orbeScript : MonoBehaviour
{

    private Camera maincam;
    public float speedMultiplier = 1.0f;
    public float speed = 1.0f;

    private processOrbe _processOrbe;
    calipsoManager cm;

    float minBass       = 1.0f;
    float maxBass       = 0.0f;
    float currentBass   = 1.0f;

    float minMed        = 1.0f;
    float maxMed        = 0.0f;
    float currentMed    = 1.0f;

    float minTreb       = 1.0f;
    float maxTreb       = 0.0f;

    public bool cannonfired = false;

    //SMOTHNESS
    Vector3 cameraPos;
    public float smoothSpeed = 0.125f;

    float randomX;


    //ROTATION
    private float totalRun = 1.0f;
 
    ConstantForce force;
    Rigidbody rb;

    Vector3 positionInicial;
    Vector3 destinoFinal;
    Vector3 haciaDestino;


    //PARAMS
    public float ySinFreq = 0.5f;
    public float ySinMultiplier = 0.1f;

    private float lerpTime = 0.0f;


    void Awake() {
        maincam = Camera.main;
        cm = FindObjectOfType<calipsoManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //POSITION INICIAL
        positionInicial = new Vector3(maincam.transform.position.x, maincam.transform.position.y-1.0f, (maincam.transform.position.z-2.0f) + speedMultiplier);
        transform.position = positionInicial;

         _processOrbe = GetComponent<processOrbe>();

        randomX = Random.Range(-50.0f, 50.0f);

        transform.Rotate(0, randomX, 0);

        force = GetComponent<ConstantForce>();
        rb = GetComponent<Rigidbody>();
        

        destinoFinal = new Vector3(Random.Range(-30f,30f), 0, 30f);


       
    }

    // Update is called once per frame
    void Update()
    {

        //TIempos
        lerpTime += Time.deltaTime;




        minBass = getMin(_processOrbe.MeanLevels[0], minBass);
        maxBass = getMax(_processOrbe.MeanLevels[0], maxBass);
        currentBass = getCurrent(minBass, maxBass, _processOrbe.MeanLevels[0])*100000;

        minMed = getMin(_processOrbe.MeanLevels[2], minMed);
        maxMed = getMax(_processOrbe.MeanLevels[2], maxMed);
        currentMed = getCurrent(minMed, maxMed, _processOrbe.MeanLevels[2])*100000;

        cameraPos = maincam.transform.position;

       
        //speed += Time.deltaTime;
  
        float y = 
            (currentBass
            -Mathf.Lerp(
                transform.position.y-(currentBass), 
                transform.position.z, 
                smoothSpeed
            )
            +Mathf.Sin(Time.fixedTime*ySinFreq)*Time.deltaTime)
            
        ;

        float sinY = Mathf.Sin(Time.fixedTime*ySinFreq)*(ySinMultiplier/10);
        float minMaxY = getYminMAX(currentBass,maxBass);
        float minMaxYResult = (minMaxY*speed) * Time.deltaTime;
        
        
        float explosiveZ = (Mathf.Lerp(10,0, lerpTime)*Time.deltaTime);
        
        Debug.Log(explosiveZ);
 
              
        float z = (maincam.transform.position.z-2f) + (speedMultiplier*speed);

        //MOVIMIENTO BASE
        float step =  speed * Time.deltaTime; // calculate distance to move
        haciaDestino = Vector3.MoveTowards(transform.position, destinoFinal, step);


/*
        Vector3 desiredPosition = destinoFinal;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
      
*/



        //WITH ROTATION
     
        Vector3 p = new Vector3(0, 0, explosiveZ+0.01f);
        
        /*totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
        
        p = p * 1.0f;
        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        */
        transform.Translate(p);

        //transform.rotation = Quaternion.LookRotation(new Vector3(maxBass/10,y*5,1));
        
/*      
        
        float cosa = cm.mapToDigital(transform.position.z, positionInicial.z, destinoFinal.z, 0, 1);
        Debug.Log(cosa);
        
        //sin
        //p = new Vector3(0, Mathf.Sin(Time.time * y) * 0.1f, 0);

        p = new Vector3(0, Mathf.Sin(cosa * 2.0f) * 0.1f, 0);

        transform.Translate(p);

        //force.force = new Vector3(0, Mathf.Lerp(50,5,1), 1);

*/
/*

        //rb.AddForce(0, Mathf.Sin(Time.deltaTime), 1f, ForceMode.Impulse);

        Vector3 toTarget = transform.position - destinoFinal;

        speed = 2.0f;

        // Set up the terms we need to solve the quadratic equations.
        float gSquared = Physics.gravity.sqrMagnitude;
        float b = speed * speed + Vector3.Dot(toTarget, Physics.gravity);    
        float discriminant = b * b - gSquared * toTarget.sqrMagnitude;


        rb.AddForce(new Vector3(0,1,1));

        Debug.Log(toTarget.z);
*/


    }

 
        
    private float getYminMAX(float currentValue, float maxValue){
        
        if(currentValue*100 > maxValue/2){
            Debug.Log("maxMaxY");
            return Mathf.Lerp(currentValue, maxValue, currentValue*Time.deltaTime);
            //return maxValue*0.125f;
        }else{
            Debug.Log("minMaxY");
            return Mathf.Lerp(currentValue, ((maxValue*Time.deltaTime)*500)*-1, Time.deltaTime);
            //return ((maxValue*Time.deltaTime)*100)*-1;
        }
    }






    float getMin(float value, float min)
    {   
        if(value == 0f) return min;
        
        if (value*100 < min)
        {
            min = value*100;
        }
        return min;
    }


    float getMax(float value, float max)
    {   
        if(value == 0f) return max;
        
        if (value*100 > max)
        {
            max = value*100;
        }
        return max;
    }


    float getCurrent(float min, float max, float value)
    {   
        float current = value;
        if(value == 0f) return value;
        
        if (max/2 > value)
        {
            current = value*min;
        }else{
            current = value/min;
        }
        return current;
    }



    public static float EaseInQuad(float start, float end, float value)
    {
        end -= start;
        return end * value * value + start;
    }

}
