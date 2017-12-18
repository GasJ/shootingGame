using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject player;

    private Vector3 DisBTCameraAndCat;
	// Use this for initialization
	void Start () {
        	DisBTCameraAndCat = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        	transform.position = player.transform.position + DisBTCameraAndCat;
	}
}
