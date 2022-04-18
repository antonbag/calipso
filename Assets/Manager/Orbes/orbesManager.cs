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

        Vector3 positionInicial = new Vector3(maincam.transform.position.x, maincam.transform.position.y+Random.Range(-1.0f, 1.0f), (maincam.transform.position.z-2.0f));
        GameObject orbeInstaGroup = Instantiate(orbeGroup, positionInicial, Quaternion.identity) as GameObject;
        orbeGroup.transform.SetParent (orbesWrapper.transform, false);
    } 

}
