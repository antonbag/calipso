using UnityEngine;
using Unity.CALIPSO;

public class createCave : MonoBehaviour
{

    public GameObject tile;

    public int maxAncho = 60;
    public int maxLargo = 60;

    private float currentUpdateTime = 0.0f;

    private calipsoManager cm;

    public float balanceoValor  = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        cm = FindObjectOfType<calipsoManager>();

        //position center of camera
        transform.position = new Vector3((maxAncho / 2) * -1.0f,-3,0);
   
        createHexatiles();

    }

    // Update is called once per frame
    void Update()
    {
        


        //obtengo el volumen cada 0.05
        currentUpdateTime += Time.deltaTime;
        if(currentUpdateTime >= 1.0f){
            currentUpdateTime = 0f;


            //Material imparMaterial = Resources.Load<Material>("Materials/tileImpar");
            //Material parMaterial = Resources.Load<Material>("Materials/tilePar");
        }


    }


    private void createHexatiles()
    {

        //bucle para el Ancho
        for(int i = 1; i < maxAncho; i++){

            //GameObject instaTile = Instantiate(tile, new Vector3(2*i, 0, 0), Quaternion.identity) as GameObject;
            //instaTile.transform.SetParent (transform, false);

            //bucle para el Largo
            for(int f = 1; f < maxLargo; f++){

                //instancia del tile
                Quaternion rot = transform.rotation * Quaternion.AngleAxis(-90, Vector3.right);
                GameObject instaTile = Instantiate(tile, new Vector3(0, 0, 0), rot) as GameObject;
                
                //ajuste para el tile perfecto  
                float newZ = f*(1.5f);
                float newX = i+0.5f;

                //front bias
                float newZDigital = cm.mapToDigital(f, maxLargo/2, maxLargo, 0, 1);
                float exponencial = (Mathf.Pow(newZDigital, balanceoValor)) * Random.Range(18f, 22f);

                //right bias
                float newXDigital = cm.mapToDigital(i, maxAncho/2, maxAncho, 0, 1);
                float exponencialX = (Mathf.Pow(newXDigital, balanceoValor)) * Random.Range(18f, 22f);

                //left bias
                float newSinDigital = cm.mapToDigital(i, 1, maxAncho/4, 1, 0);
                float exponencialSin = Mathf.Pow(Mathf.Sin(newSinDigital),balanceoValor) * Random.Range(18f, 22f);



                //PAR //IMPAR //visualización de los tiles
                if(f % 2 == 0){
                    //instaTile.transform.position = new Vector3(i, 0, f*(1.5f));
                    instaTile.transform.position = new Vector3(newX, 0, newZ/2);
                    //instaTile.GetComponent<MeshRenderer>().materials[0].color = Color.blue;
                }else{
                    instaTile.transform.position = new Vector3(i, 0, newZ/2);
                    //instaTile.GetComponent<MeshRenderer>().materials[0].color = Color.red;
                }

                instaTile.transform.localScale = new Vector3(
                    instaTile.transform.localScale.x, 
                    instaTile.transform.localScale.y, 
                    Random.Range(0.6f, 1.5f)+((exponencial+exponencialX+exponencialSin)/3)
                );
                
                instaTile.transform.SetParent (transform, false);


            }
        }
    }


    private void destoyHexatiles()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

}
