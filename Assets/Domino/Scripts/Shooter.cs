using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class Shooter : MonoBehaviour {
	public GameObject ball;
	public Transform shootAnchor;

	void Start() {
		shootAnchor.transform.localPosition *= UnityARMatrixOps.scaleMultiplier;
	}
	public void Shoot() {
		ball.SetActive(true);
		ball.transform.localScale *= UnityARMatrixOps.scaleMultiplier;
		ball.transform.position = shootAnchor.position;
		ball.GetComponent<Rigidbody>().velocity = (shootAnchor.position - Camera.main.transform.position) * 10f;
	}
}
