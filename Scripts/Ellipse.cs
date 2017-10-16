using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ellipse {

	public float xAxis; // major
    public float zAxis; // minor

	public Ellipse (float xAxis, float zAxis) {

		this.xAxis = xAxis;
		this.zAxis = zAxis;
	}

	public Vector2 Calculate (float t) {

		float angle = Mathf.Deg2Rad * 360f * t;

		float x = Mathf.Sin (angle) * xAxis;
        float z = Mathf.Cos (angle) * zAxis;

		return new Vector2 (x, z);
	}
}
