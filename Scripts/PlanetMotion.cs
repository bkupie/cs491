using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlanetMotion : MonoBehaviour {

    LineRenderer orbitPath; // rendered orbit path

    public Transform planet; // child planet

    [Range (4, 36)]
    public int segments = 32; // "resolution" of rendered orbit path

    [Range (0f, 1f)]
    public float orbitProgress = 0f;

    public float orbitalPeriod = 5f; // seconds
    public float rotationPeriod = 3f; // seconds
    public float height = 0;
    public bool orbitActive = true;
    public bool rotateActive = true;

    public Ellipse ellipse;

    void Awake () { // awake gets called before Start
        
        orbitPath = gameObject.AddComponent<LineRenderer> ();
        orbitPath.GetComponent<LineRenderer> ().useWorldSpace = false;
        orbitPath.endWidth = orbitPath.startWidth = .2f;
        orbitPath.material = new Material (Shader.Find ("Particles/Additive"));
        ellipse = new Ellipse(2.0f,2.0f);
        //DrawEllipse ();
    }

    void Start () {

        planet = this.gameObject.transform.GetChild (0); // first child

        if (planet == null) {

            orbitActive = false;
            return;
        }

        SetOrbitingPosition ();
    }

    public void DrawEllipse () {
        
        Vector3[] points = new Vector3[segments + 1];

        for (int i = 0; i < segments; i++) {

            Vector2 pos = ellipse.Calculate ((float) i / (float) segments);
            points[i] = new Vector3 (pos[0], 0, pos[1]);
        }

        points[segments] = points[0]; // set last element equal to first element

        orbitPath.numPositions = segments + 1;
        orbitPath.SetPositions (points);
    }

    void SetOrbitingPosition () {

        Vector2 orbitPos = ellipse.Calculate (orbitProgress);
        planet.localPosition = new Vector3 (orbitPos[0], 0, orbitPos[1]);
    }

    void Update () {

        if (orbitActive) {

            float orbitSpeed;

            if (orbitalPeriod < 0.1f)
                orbitSpeed = 0f;
            else
                orbitSpeed = 1f / orbitalPeriod;

            orbitProgress += Time.deltaTime * orbitSpeed;
            orbitProgress %= 1f;
            SetOrbitingPosition ();
        }

        if (rotateActive) {

             planet.Rotate (0, (360f / (rotationPeriod)) * Time.deltaTime, 0, Space.Self);
        }
    }
}