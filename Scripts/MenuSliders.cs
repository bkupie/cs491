using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// will attach official script to parent container of 2d views
// when slider value changes, get a list of all children (systems) and set scales (sun/planets)
// container has vector4 (x, y, z, y rotation)
// use other c# code from unity manual
// attach light source to head?


public class MenuSliders : MonoBehaviour {

    public float scale_planetSize;
    public float scale_orbitDistance;
    public float scale_orbitalPeriod;
    public float scale_rotationPeriod;

    public void planetSizeUpdate (float value) {

        this.scale_planetSize = value;
		SetColor (); //just for example
    }

    public void orbitDistanceUpdate (float value) {

        this.scale_orbitDistance = value;
		SetColor (); //just for example
    }

    public void orbitalPeriodUpdate (float value) {

        this.scale_orbitalPeriod = value;
		SetColor (); //just for example
    }

    public void rotationPeriodUpdate (float value) {

        this.scale_rotationPeriod = value;
    }

    public void SetColor () { //just for example
        //GetComponent<Renderer> ().material.color = new Color (100f / scale_planetSize, 100f / scale_orbitDistance, 100f / scale_orbitalPeriod);
    
    Debug.Log(scale_planetSize + "    " + scale_orbitDistance + "    " + scale_orbitalPeriod);
    }
}