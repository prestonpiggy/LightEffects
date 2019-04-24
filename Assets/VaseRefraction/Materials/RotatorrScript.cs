using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorrScript : MonoBehaviour {
    private Camera cam;
    public GameObject player;
    float x;
    float y;
    public float smooth = 2.0F;
    // Use this for initialization
    void Start () {
        cam = player.GetComponentInChildren<Camera>();
        
	}
	
	// Update is called once per frame
	void Update () {
        //x = cam.transform.rotation.x;
        //y = player.transform.rotation.y;
        //Debug.Log(x +"aaaa"+ y);
        Vector3 newRotation = new Vector3(cam.transform.eulerAngles.x, player.transform.eulerAngles.y, 90);
        gameObject.transform.eulerAngles = newRotation;


    }
}
