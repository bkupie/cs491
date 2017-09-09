using UnityEngine;
using System.Collections;

public class WandOriginCalibrator : MonoBehaviour {

	public Vector3 wandPosition;
	public Vector3 playerPosition;
    Vector3 initPlayerPosition;

    public Vector3 offsetFromOrigin;

	// Offset from Vive wand tracking center to tip of controller for calibration
	public Vector3 wandCenterToCalibrationTipOffset = new Vector3(0.45f, 0.09f, -0.94f);

    public bool resetPlayerPosition;

	// Use this for initialization
	void Start () {
        initPlayerPosition = transform.root.position;
    }
	
	// Update is called once per frame
	void Update () {
		wandPosition = transform.position;
		playerPosition = transform.root.position;

		offsetFromOrigin = initPlayerPosition - wandPosition + wandCenterToCalibrationTipOffset;

        if(resetPlayerPosition)
        {
            transform.root.position = playerPosition + offsetFromOrigin;
            resetPlayerPosition = false;
        }
	}
}
