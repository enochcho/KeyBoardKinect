﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class checkCollisionMars : MonoBehaviour {
	public GameObject MarsEnd;
	public Text scoreText;
	private correctlyPlacedScript correctlyPlaced;

	// Use this for initialization
	void OnTriggerEnter2D (Collider2D other) {
		Debug.Log (" something collided");

		if(other.gameObject.name == "Mars") {
			Debug.Log ("correct anwer");
			correctlyPlaced = other.GetComponent <correctlyPlacedScript>();
			correctlyPlaced.correctlyPlaced = true;
			other.transform.position = MarsEnd.transform.position;
			int currentScore = int.Parse(scoreText.text);
			currentScore = currentScore + 100;
			scoreText.text = currentScore.ToString();
		} else {
			Debug.Log ("wrong answer");
		}

	}
}
