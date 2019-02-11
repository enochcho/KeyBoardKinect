using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SimpleGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
	// GUI Text to display the gesture messages.
	public Text gestureInfo;

	//planets
	public GameObject mercury;
	public GameObject mars;
	public GameObject venus;
	public GameObject earth;
	public GameObject jupiter;
	public GameObject saturn;
	public GameObject neptune;
	public GameObject uranus;
	public GameObject pluto;

	public GameObject spaceship;

	// private bool to track if progress message has been displayed
	private bool progressDisplayed;

	private bool planetClicked = false;
	public GameObject selectedPlanet;
	private correctlyPlacedScript correctlyPlaced;

	public void UserDetected(uint userId, int userIndex)
	{
		// as an example - detect these user specific gestures
		KinectManager manager = KinectManager.Instance;

		if (gestureInfo != null)
		{
			gestureInfo.text = "Capture planets by clicking on them!";
		}
	}

	public void UserLost(uint userId, int userIndex)
	{
		if (gestureInfo != null)
		{
			gestureInfo.text = string.Empty;
		}
	}

	public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture,
		float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
	{
		if (gesture == KinectGestures.Gestures.Click && progress > 0.3f)
		{
			string sGestureText = string.Format("capture {0:F1}% complete", progress * 100);

			if (gestureInfo != null)
				gestureInfo.text = sGestureText;

			progressDisplayed = true;
		}
	}

	public bool GestureCompleted(uint userId, int userIndex, KinectGestures.Gestures gesture,
		KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
	{
		string sGestureText = "";
		if (gesture == KinectGestures.Gestures.Click)
		{
			
			switch (planetClicked)
			{
			case true:
				if (correctlyPlaced.correctlyPlaced == false){
					sGestureText = selectedPlanet.tag + " deposited.";
				}


				planetClicked = false;
				selectedPlanet = null;

				break;
			case false:
				Vector2 rayPos = new Vector2(spaceship.transform.position.x, spaceship.transform.position.y);
				RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
				if (hit)
				{
					Debug.Log(hit.transform.name);
					Debug.Log(hit.transform.gameObject.tag);
					planetClicked = true;
					selectedPlanet = hit.transform.gameObject;
					correctlyPlaced = selectedPlanet.GetComponent <correctlyPlacedScript>();
					sGestureText = selectedPlanet.tag + " captured";
				}
				break;
			}
		}



		if (gestureInfo != null)
		{
			gestureInfo.text = sGestureText;
		}

		progressDisplayed = false;

		return true;
	}

	public bool GestureCancelled(uint userId, int userIndex, KinectGestures.Gestures gesture,
		KinectWrapper.NuiSkeletonPositionIndex joint)
	{
		if (progressDisplayed)
		{
			// clear the progress info
			if (gestureInfo != null)
				gestureInfo.text = String.Empty;

			progressDisplayed = false;
		}

		return true;
	}

	void Update()
	{
		
		if (planetClicked == true && selectedPlanet != null && correctlyPlaced.correctlyPlaced != true)
		{
			selectedPlanet.transform.position = spaceship.transform.position;
		}
	}
}