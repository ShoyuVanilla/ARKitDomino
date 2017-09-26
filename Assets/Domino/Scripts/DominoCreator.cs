using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class DominoCreator : MonoBehaviour {
	float touchLapse;
	public GameObject dominoPrefab;
	public float createHeight;
	public float dominoDistance;
	List<GameObject> dominos;
	Vector3 prevDominoPosition;

	void Start () {
		dominos = new List<GameObject>();
		Physics.gravity *= UnityARMatrixOps.scaleMultiplier;
		dominoDistance *= UnityARMatrixOps.scaleMultiplier;
		createHeight *= UnityARMatrixOps.scaleMultiplier;
	}

	void Update () {
		if (Input.touchCount > 0) {
			var touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began) {
				touchLapse = 0f;
				var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
				var point = new ARPoint {
					x = screenPosition.x,
					y = screenPosition.y
				};

				var hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, 
					ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);
				if (hitResults.Count > 0) {
					foreach (var hitResult in hitResults) {
						var position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
						CreateDomino(new Vector3 (position.x, position.y + createHeight, position.z));
						break;
					}
				}
				else {
					Debug.Log("No hit");
				}
			}
			else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
				touchLapse += Time.deltaTime;
				if (touchLapse < 0.5f)
					return;
				var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
				var point = new ARPoint {
					x = screenPosition.x,
					y = screenPosition.y
				};

				var hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, 
					ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);
				if (hitResults.Count > 0) {
					foreach (var hitResult in hitResults) {
						var position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
						if (Vector3.Distance(prevDominoPosition, position) < dominoDistance)
							continue;
						CreateDomino(new Vector3(position.x, position.y + createHeight, position.z));
						break;
					}
				}
			}
		}
	}

	void CreateDomino(Vector3 position) {
		var newDomino = Instantiate(dominoPrefab, position, Quaternion.identity);
		newDomino.transform.localScale *= UnityARMatrixOps.scaleMultiplier;
		var relative = prevDominoPosition - position;
		var lookAtAngle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
		newDomino.transform.eulerAngles = new Vector3(0, lookAtAngle, 0);
		dominos.Add(newDomino);
		prevDominoPosition = position;
	}

	public void ClearDomino() {
		foreach (var domino in dominos) {
			Destroy(domino);
		}
		dominos.Clear();
	}
}
