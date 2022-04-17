using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbeScript : MonoBehaviour
{

    public Camera maincam;
    public float distance = 1.0f;

    private float speed = 1.0f;

    void Awake() {
        maincam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(maincam.transform.position.x, maincam.transform.position.y, maincam.transform.position.z + distance);
    }

    // Update is called once per frame
    void Update()
    {
        speed += Time.deltaTime;

        float x = Mathf.Cos(speed) * distance;
        float y = Mathf.Sin(speed) * distance;

        transform.position = new Vector3(x, y, maincam.transform.position.z + (distance*speed));
    }
}
