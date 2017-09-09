using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class LODByHeight : MonoBehaviour {

	public float cameraHeight;

	public float minHeight;
	public float maxHeight;

	public GameObject lodObject;

	// Update is called once per frame
	void Update () {
		cameraHeight = Camera.main.transform.position.y;

        
		if( cameraHeight >= minHeight && cameraHeight <= maxHeight && !lodObject.activeSelf )
		{
            if (GetComponentInParent<LODScript>() && GetComponentInParent<LODScript>().lodActive)
            {
                lodObject.SetActive(true);
            }
		}
		else if( cameraHeight >= minHeight && cameraHeight <= maxHeight && lodObject.activeSelf )
		{

		}
		else if( lodObject.activeSelf )
		{
			lodObject.SetActive(false);
		}

        if (GetComponentInParent<LODScript>() && !GetComponentInParent<LODScript>().lodActive)
        {
            lodObject.SetActive(false);
        }
	}
}
