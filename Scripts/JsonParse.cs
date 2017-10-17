using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;  // The System.IO namespace contains functions related to loading and saving files
using System.Collections.Generic; // import the list ability 

[System.Serializable]
    public class SolarSystem
    {  
        public int ID;
        public string star;      
        public string system;    
        public string spectral;      
        public float  temperature;
        public float  distance;
        public float  x;
        public float  y;
        public float  z;
        public float  radius_Solar;
        public float  mass_Solar;
        public float  rotation_Period;
        public float  habitable_inner;
        public float  habitable_outer;
        public Planet [] systems_planets; 
    }

[System.Serializable]
    public class Planet 
    {  
        public int ID; 
        public string p_name;        
        public string p_discovery;
        public float  p_distance;
        public float  p_radius_Earth;
        public float  p_mass_Earth;
        public float  p_rotation_Period;
        public float  p_orbital_Period;  

        public Planet(int id, string a,string b,float c,float d, float e, float f,float g)
        {
            ID = id;
            p_name = a;        
            p_discovery = b;
            p_distance = c;
            p_radius_Earth = d;
            p_mass_Earth = e;
            p_rotation_Period = f;
            p_orbital_Period = g;
        }


    }    




[System.Serializable]
public class JsonParse : MonoBehaviour {

    private Planet[] Planets;
    private SolarSystem[] SolarSystems;
    // name of the json file we are using
	public string systemsFileName = "solar_systems.json";
    public string planetsFileName = "planets.json";
    // hold the raw JSON file as text 
	private string systemJsonText;
    private string planetJsonText; 

	// Use this for initialization
	void Start () {
        
        // load the json file 
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
         string filePathSystems = Path.Combine(Application.streamingAssetsPath, systemsFileName);
         string filePathPlanets = Path.Combine(Application.streamingAssetsPath, planetsFileName);

        if(File.Exists(filePathSystems) && File.Exists(filePathPlanets) )
        {   
            Debug.Log("Text file exists");
            // Read the json from the file into a string
            systemJsonText = File.ReadAllText(filePathSystems);
            planetJsonText = File.ReadAllText(filePathPlanets); 
            LoadPlanetJSON();
        }
        else
        {
            Debug.LogError("Cannot load planet or systems data!");
        }
        
	}
	// Update is called once per frame
	void Update () {

	}

    

    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }


	private void LoadPlanetJSON()
    {
       // Pass the json to JsonUtility, and tell it to create a GameData object from it
            Planets = JsonHelper.FromJson<Planet>(fixJson(planetJsonText));
            SolarSystems = JsonHelper.FromJson<SolarSystem>(fixJson(systemJsonText));

             
            CreateProperSystems();
    }

    private void CreateProperSystems()
    {

        Debug.Log ("Lengths:");
        int length = Planets.Length;
        Debug.Log (length);
        
        SolarSystems[0].systems_planets = new Planet[length];
        int planetCounter = 0;
        int counter = 0;
        for(int i = 0 ; i < length ; i++)
        {
            if(Planets[i].ID == (counter+1) )
            {
                counter++;
                planetCounter = 0;
            }

            // add the planet to the solar system 
            SolarSystems[counter].systems_planets[planetCounter] = Planets[i];
            planetCounter++;
        
        }

        Debug.Log (SolarSystems[0].systems_planets[0].p_name);
    }

}
