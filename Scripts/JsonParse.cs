using UnityEngine;
using System.Collections;
using System;
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

    public float orbitXScale = 1.0f;

    public float planetScale = .1f;
    public float planetOrbitSpeed = .1f;
    public float planetRotateSpeed = 10.0f;
    public float gapBetweenPlanets3d = 1.0f;
    private GameObject allThreeDimensionalSystems;
    
    private GameObject[] fourSystems = new GameObject[4];
    private int fourSystemsCounter = 0;

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

        if (Input.GetKeyDown("space"))
            {int result = createSystemFromID(0);
            Debug.Log("Result:" + result);}
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
        SolarSystem [] temp = SortSystemsDistance();
    
        // our view has to always be visible, create that view 
        int result = createSystemFromID(0);
        
    }


    public SolarSystem[] SortSystemsDistance()
    {
    SolarSystem [] temp = SolarSystems;
        Array.Sort(temp,
    delegate(SolarSystem x, SolarSystem y) 
    { return x.distance.CompareTo(y.distance); });    
    return temp;

    }

    public SolarSystem[] SortSystemsTemparture()
    {
        SolarSystem [] temp = SolarSystems;
        Array.Sort(temp,
    delegate(SolarSystem x, SolarSystem y) 
    { return x.temperature.CompareTo(y.temperature); });    
    
    return temp;

    }


    GameObject CreateView(SolarSystem thisSolarSystem)
	{
		// based off the values from the solar system, make a 3d VIEW
		//first the sun 	
		GameObject theStar;
        GameObject SolarSystem = new GameObject();
        SolarSystem.name = thisSolarSystem.system;

		// create the star 
		theStar = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		theStar.name = thisSolarSystem.star;
        Material starMaterial = new Material (Shader.Find ("Unlit/Texture"));
		theStar.GetComponent<MeshRenderer> ().material = starMaterial;
        starMaterial.mainTexture = Resources.Load ("gstar") as Texture;

        theStar.transform.parent = SolarSystem.transform;
        //SolarSystem.transform.parent = allThreeDimensionalSystems.transform;
        // now we have to go and add each planet :-)

        for(int i = 0 ; i < thisSolarSystem.systems_planets.Length;i++)
        {
            float planetSize = thisSolarSystem.systems_planets[i].p_radius_Earth * planetScale;
            float PlanetOrbitalPeriod = thisSolarSystem.systems_planets[i].p_orbital_Period;
            float planetDistance = thisSolarSystem.systems_planets[i].p_distance;
			float planetSpeed = thisSolarSystem.systems_planets[i].p_rotation_Period;
			
			string planetName = thisSolarSystem.systems_planets[i].p_name;
            string textureName = planetName; // for now get picture texture based off name 
            
            GameObject planetPivot = new GameObject();
            GameObject thisPlanet = GameObject.CreatePrimitive (PrimitiveType.Sphere);
        
            planetPivot.name = planetName +"_Pivot";
            thisPlanet.transform.parent = planetPivot.transform;
            planetPivot.transform.parent = SolarSystem.transform;

            // set planet variables to the planet motion script 
            planetPivot.AddComponent<PlanetMotion>();
            planetPivot.GetComponent<PlanetMotion>().ellipse.xAxis = planetDistance ;
            planetPivot.GetComponent<PlanetMotion>().ellipse.zAxis = planetDistance ;
            planetPivot.GetComponent<PlanetMotion>().orbitalPeriod = PlanetOrbitalPeriod * planetOrbitSpeed;
            planetPivot.GetComponent<PlanetMotion>().rotationPeriod = planetSpeed * planetOrbitSpeed; 
            planetPivot.GetComponent<PlanetMotion>().height = fourSystemsCounter * gapBetweenPlanets3d; 
            planetPivot.GetComponent<PlanetMotion>().planet = thisPlanet.GetComponent<Transform>();
            planetPivot.GetComponent<PlanetMotion>().DrawEllipse();
            

            Material planetMaterial = new Material (Shader.Find ("Standard"));
			thisPlanet.GetComponent<MeshRenderer>().material = planetMaterial;
			planetMaterial.mainTexture = Resources.Load(textureName) as Texture;


            thisPlanet.transform.localScale = new Vector3 (planetSize, planetSize, planetSize);
            thisPlanet.transform.position = new Vector3 (0, 0, planetDistance * orbitXScale);
			
        }

    return SolarSystem;
	}

    public int createSystemFromID(int id)
    {
        SolarSystem temp = findSolarSystemByID(id);
        // now that we have our solar system, we add it on top of the other solar systems 
        
        // first check the counter
        if(fourSystemsCounter < 4)
        {
            Destroy(fourSystems[fourSystemsCounter]);
            fourSystems[fourSystemsCounter] = CreateView(temp);
            fourSystems[fourSystemsCounter].transform.position = new Vector3 (0, fourSystemsCounter * gapBetweenPlanets3d, 0);
            fourSystemsCounter++;
            // depending on counter value, that's where we'll move our system 
        return 1;
        }
        else{
            fourSystemsCounter = 1;
            Destroy(fourSystems[fourSystemsCounter]);
            fourSystems[fourSystemsCounter] = CreateView(temp);
            fourSystems[fourSystemsCounter].transform.position = new Vector3 (0, fourSystemsCounter * gapBetweenPlanets3d, 0);
            fourSystemsCounter++;
            
        }
        return 0;
    }

    private SolarSystem findSolarSystemByID(int id)
    {
        foreach(SolarSystem system in SolarSystems)
        {
            if (system.ID == id)
            { return system;}
        }

        return SolarSystems[0]; // send default, your thing wasn't found :-(
    }

}
