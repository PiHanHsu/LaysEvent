using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioEffect : MonoBehaviour {

	public AudioSource Explo;
	public AudioSource Hit;

	void OnTriggerEnter2D (Collider2D col) {
	   
		if (col.tag == "Bomb") {
			Explo.Play ();
		} else {
			Hit.Play ();
		}
	}

}
