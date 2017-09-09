using UnityEngine;
using System.Collections;

public class CameraScreenshot : MonoBehaviour {

    new public Camera camera;
    public bool takeAScreenShot;
    public string screenshotPath = "CAVE2SkyboxScreenShots";
    public string screenshotLabel = "";

    public RenderTexture targetTexture;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(takeAScreenShot)
        {
            CreateScreenshotFromCamera(camera);
            takeAScreenShot = false;
        }
	}

    void CreateScreenshotFromCamera(Camera c)
    {
        RenderTexture rc = targetTexture;

        RenderTexture tempRT = new RenderTexture(rc.width, rc.height, 24);
        c.targetTexture = tempRT;
        c.Render();

        RenderTexture.active = tempRT;

        Texture2D tx = new Texture2D(rc.width, rc.height, TextureFormat.RGB24, false);
        // false, meaning no need for mipmaps
        tx.ReadPixels(new Rect(0, 0, rc.width, rc.height), 0, 0);

        RenderTexture.active = null; //can help avoid errors 
        c.targetTexture = rc;
        Destroy(tempRT);

        byte[] bytes;
        bytes = tx.EncodeToPNG();

        System.IO.File.WriteAllBytes(Application.dataPath + "/" + screenshotPath + "/" + screenshotLabel + "-" + c.name + ".png", bytes);
    }
}
