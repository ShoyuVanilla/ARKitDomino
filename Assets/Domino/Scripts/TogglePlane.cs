using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePlane : MonoBehaviour {
	LayerMask dominoOnlyMask;
	LayerMask dominoAndPlaneMask;
	void Start() {
		dominoOnlyMask = 1<<LayerMask.NameToLayer("Default");
		var planeMask = 1<<LayerMask.NameToLayer("ARKitPlane");
		dominoAndPlaneMask = dominoOnlyMask|planeMask;
	}
	public void EnableShow(bool isEnable) {
		if (isEnable)
			Camera.main.cullingMask = dominoAndPlaneMask;
		else
			Camera.main.cullingMask = dominoOnlyMask;
	}
}
