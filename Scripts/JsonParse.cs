using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;  // The System.IO namespace contains functions related to loading and saving files
using System.Collections.Generic; // import the list ability 

[System.Serializable]
    public class Planet 
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
        public string p_name;        
        public string p_discovery;
        public float  p_distance;
        public float  p_radius_Earth;
        public float  p_mass_Earth;
        public float  p_rotation_Period;
        public float  p_orbital_Period;  
    }    

[System.Serializable]
public class JsonParse : MonoBehaviour {

    // name of the json file we are using
	public string gameDataFileName = "exoplanet-archive.json";
    // hold the raw JSON file as text 
	private string jsonText;

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
            Planet[] SolarSystem = JsonHelper.FromJson<Planet>(fixJson(jsonText));
            
            // the fallowing is used to make sure everything works propely with the planet.json test file 
             Debug.Log (jsonText);
            
             Debug.Log (SolarSystem.Length);
                int length = SolarSystem.Length;

            for(int i = 0 ; i < length ; i++)
            {
              Debug.Log (SolarSystem[i].p_name);   
            }
    }

     
}
