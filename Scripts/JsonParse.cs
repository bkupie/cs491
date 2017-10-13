using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;  // The System.IO namespace contains functions related to loading and saving files
using System.Collections.Generic; // import the list ability 

[System.Serializable]
public class JsonParse : MonoBehaviour {

    [System.Serializable]
    public struct planet
    {
            public string pl_hostname;
            public string pl_letter;
            public string pl_pnum;
            public string pl_orbper;
            public string pl_orbpererr1;
            public string pl_orbpererr2;
            public string pl_orbperlim;
            public string pl_orbpern;
            public string pl_orbsmax;
            public string pl_orbsmaxerr1;
            public string pl_orbsmaxerr2;
            public string pl_orbsmaxlim;
            public string pl_orbsmaxn;
            public string pl_orbeccen;
            public string pl_orbeccenerr1;
            public string pl_orbeccenerr2;
            public string pl_orbeccenlim;
            public string pl_orbeccenn;
            public string pl_orbincl;
            public string pl_orbinclerr1;
            public string pl_orbinclerr2;
            public string pl_orbincllim;
            public string pl_orbincln;
            public string pl_bmassj;
            public string pl_bmassjerr1;
            public string pl_bmassjerr2;
            public string pl_bmassjlim;
            public string pl_bmassn;
            public string pl_bmassprov;
            public string pl_radj;
            public string pl_radjerr1;
            public string pl_radjerr2;
            public string pl_radjlim;
            public string pl_radn;
            public string pl_dens;
            public string pl_denserr1;
            public string pl_denserr2;
            public string pl_denslim;
            public string pl_densn;
            public string pl_ttvflag;
            public string pl_kepflag;
            public string pl_k2flag;
            public string ra_str;
            public string dec_str;
            public string ra;
            public string st_raerr;
            public string dec;
            public string st_decerr;
            public string st_posn;
            public string st_dist;
            public string st_disterr1;
            public string st_disterr2;
            public string st_distlim;
            public string st_distn;
            public string st_optmag;
            public string st_optmagerr;
            public string st_optmaglim;
            public string st_optmagblend;
            public string st_optband;
            public string gaia_gmag;
            public string gaia_gmagerr;
            public string gaia_gmaglim;
            public string st_teff;
            public string st_tefferr1;
            public string st_tefferr2;
            public string st_tefflim;
            public string st_teffblend;
            public string st_teffn;
            public string st_mass;
            public string st_masserr1;
            public string st_masserr2;
            public string st_masslim;
            public string st_massblend;
            public string st_massn;
            public string st_rad;
            public string st_raderr1;
            public string st_raderr2;
            public string st_radlim;
            public string st_radblend;
            public string st_radn;
            public string pl_nnotes;
            public string rowupdate;
            }

    // name of the json file we are using
	public string gameDataFileName = "exoplanet-archive.json";
    // hold the raw JSON file as text 
	private string jsonText; 


 
   [System.Serializable]
    public class planetData 
    {   // create a list of planets 
        public List<planet> planets; 
    }

    [System.Serializable]
    public class MyMainObject 
    {   // wrapper 
   
        [System.Serializable]
        public class personData 
        {  
            public string name; 
        }

        public personData[] people;
    }
	// Use this for initialization
	void Start () {
        
        // load the json file 
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
         string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if(File.Exists(filePath))
        {   
            Debug.Log("Text file exists");
            // Read the json from the file into a string
            jsonText = File.ReadAllText(filePath); 
            LoadPlanetJSON();
        }
        else
        {
            Debug.LogError("Cannot load planet data!");
        }
        
	}
	// Update is called once per frame
	void Update () {

	}

	private void LoadPlanetJSON()
    {
       // Pass the json to JsonUtility, and tell it to create a GameData object from it
            //planetData data = JsonUtility.FromJson<planetData>(jsonText);
            // Debug.Log(data.planets[0]);
            Debug.Log (jsonText);
            MyMainObject objects = JsonUtility.FromJson<MyMainObject>(jsonText);
            Debug.Log (objects);
            //Debug.Log (data.planets.Count);
       
    }

     
}
