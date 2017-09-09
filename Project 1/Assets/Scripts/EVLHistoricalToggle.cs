using UnityEngine;
using System.Collections;

public class EVLHistoricalToggle : MonoBehaviour {

    public GameObject[] historicalSet;
    public bool warp = false;
    public int warpID = 0;
    int lastID;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (CAVE2.GetButtonDown(1, CAVE2.Button.ButtonDown))
        {
            lastID = warpID;
            warpID--;
            WarpTo(warpID);
            //getReal3D.RpcManager.call("WarpTo", warpID);
        }
        if (CAVE2.GetButtonDown(1, CAVE2.Button.ButtonUp))
        {
            lastID = warpID;
            warpID++;
            WarpTo(warpID);
            //getReal3D.RpcManager.call("WarpTo", warpID);
        }
        */
    }

    //[getReal3D.RPC]
    public void WarpTo(int id)
    {
        if (id >= historicalSet.Length || id < 0)
        {
            id = 0;
            warpID = 0;
        }

        historicalSet[lastID].SetActive(false);

        if (id < historicalSet.Length)
        {
            historicalSet[warpID].SetActive(true);
        }
    }

    public void Toggle0()
    {
        lastID = warpID;
        warpID = 0;
        WarpTo(0);
    }

    public void Toggle1()
    {
        lastID = warpID;
        warpID = 1;
        WarpTo(1);
    }

    public void Toggle2()
    {
        lastID = warpID;
        warpID = 2;
        WarpTo(2);
    }
}
