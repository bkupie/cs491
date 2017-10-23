﻿using UnityEngine;
using VRTK;

public class PointerInteract : VRTK_InteractableObject {

    public Renderer rend;

    public override void StartUsing (VRTK_InteractUse usingObject) {
        base.StartUsing (usingObject);
        Debug.Log ("ON");

        //rend = GetComponent<Renderer> ();
        //rend.material.shader = Shader.Find ("Specular");
        //rend.material.SetColor ("_SpecColor", Color.red);

        // Debug.Log(this.name);




        StopUsing(usingObject);
    }

    public override void StopUsing (VRTK_InteractUse usingObject) {
        base.StopUsing (usingObject);
        Debug.Log ("OFF");

        //rend = GetComponent<Renderer> ();
        //rend.material.shader = Shader.Find ("Specular");
        //rend.material.SetColor ("_SpecColor", Color.white);
    }

    void Start () {
    }

    protected override void Update () {
        base.Update ();
    }
}