using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class checkCollisionSaturn : MonoBehaviour {
	public GameObject SaturnEnd;
	public Text scoreText;
	private correctlyPlacedScript correctlyPlaced;

	// Use this for initialization
	void OnTriggerEnter2D (Collider2D other) {
		Debug.Log (" something collided");

		if(other.gameObject.name == "Saturn") {
			Debug.Log ("correct anwer");
			correctlyPlaced = other.GetComponent <correctlyPlacedScript>();
			correctlyPlaced.correctlyPlaced = true;
			other.transform.position = SaturnEnd.transform.position;
			int currentScore = int.Parse(scoreText.text);
			currentScore = currentScore + 100;
			scoreText.text = currentScore.ToString();
		} else {
			Debug.Log ("wrong answer");
		}

	}
}
