using UnityEngine;
using VRTK;

public class PointerInteractPageLeft : VRTK_InteractableObject {

    public Renderer rend;
    public GameObject parent;

    public override void StartUsing (VRTK_InteractUse usingObject) {
        base.StartUsing (usingObject);
        Debug.Log ("ON");

        //rend = GetComponent<Renderer> ();
        //rend.material.shader = Shader.Find ("Specular");
        //rend.material.SetColor ("_SpecColor", Color.red);

        parent = this.transform.parent.gameObject;

        Generate2DView generateScript = parent.GetComponent<Generate2DView>();

        generateScript.pagePlanets();





        StopUsing(usingObject);
    }

    public override void StopUsing (VRTK_InteractUse usingObject) {
        base.StopUsing (usingObject);
        Debug.Log ("OFF");

        //rend = GetComponent<Renderer> ();
        //rend.material.shader = Shader.Find ("Specular");
        //rend.material.SetColor ("_SpecColor", Color.white);
    }

    protected void Start () {

    }

    protected override void Update () {
        base.Update ();
    }
}