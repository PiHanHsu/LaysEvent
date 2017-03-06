using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBehavior : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position += new Vector3 (0f, -0.03f, 0f);
		transform.Rotate (new Vector3 (0, 0, 180) * Time.deltaTime);
	}

	void OnTriggerEnter2D (Collider2D col){

		if (col.tag == "Player1") {
			
			GameControl.Score1 += 500;
			Destroy (gameObject);
		}

		if (col.tag == "Player2") {
			GameControl.Score2 += 500;
			Destroy (gameObject);
		}
	}
		
}
