using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    [SerializeField] Transform player;
    Vector3 startDisnatce;
    Vector3 moveVector;

	// Use this for initialization
	void Start () {
        startDisnatce = transform.position - player.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        moveVector = player.position + startDisnatce;

        moveVector.x = 0;
        moveVector.y = startDisnatce.y;
        
       

        transform.position = moveVector;
	}
}
