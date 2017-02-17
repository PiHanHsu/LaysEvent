using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
    
	public GameObject Explosion;
	public AudioSource Explo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position += new Vector3 (0f, -0.05f, 0f);
	}

	void OnTriggerEnter2D (Collider2D col){

		if (col.tag == "Player1") {
			GameControl.Score1 -= 1000;
			Instantiate (Explosion, this.transform.position, transform.rotation);
			Explo.Play ();
			Destroy (gameObject);
		}

		if (col.tag == "Player2") {
			GameControl.Score2 -= 1000;
			Instantiate (Explosion, this.transform.position, transform.rotation);
			Explo.Play ();
			Destroy (gameObject);
		}
	}
}
