using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbesManager : MonoBehaviour
{
    //prefabs
    public GameObject orbeGroup;
    public GameObject orbesWrapper;

    public Camera maincam;

    // Start is called before the first frame update
    void Start()
    {
        maincam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {  
            Debug.Log("orbe");
            createOrbe();
        }  
    }


    public void createOrbe(){

        int randomX = Random.Range(1, 5);
        Color randomColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        AudioClip createdClip = (AudioClip) Resources.Load("sounds/"+randomX);

        Vector3 positionInicial = new Vector3(0, Random.Range(-1.0f, 1.0f), (2.0f));
        GameObject orbeInstaGroup = Instantiate(orbeGroup, positionInicial, Quaternion.identity) as GameObject;
        orbeInstaGroup.GetComponent<AudioSource>().clip = createdClip;

        //PLAY ON FINAL 
        orbeInstaGroup.GetComponent<AudioSource>().Play();


        GameObject orbeSphere = orbeInstaGroup.transform.GetChild(0).gameObject;
        GameObject orbeLight = orbeInstaGroup.transform.GetChild(1).gameObject;
        


        //LIGHT
        orbeLight.GetComponentInChildren<Light>().color = randomColor;
        //orbeInstaGroup.transform.GetChild(1).GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
  

        //MATERIAL
        orbeSphere.GetComponentInChildren<Renderer>().material.SetColor("_color2", randomColor);


        //orbeInstaGroup.GetComponentInChildren<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        //orbeInstaGroup.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

         orbeInstaGroup.transform.SetParent (orbesWrapper.transform, false);

    } 

}
