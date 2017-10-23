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
        public float x;
        public float y;
        public float z;
        public float  radius_Solar;
        public float  mass_Solar;
        public float  rotation_Period;
        public float luminosity;
        public float  habitable_inner;
        public float  habitable_outer;
        public int numPlanets;
        public Planet [] systems_planets; 
        public int habitable_counter;
    }

[System.Serializable]
    public class Planet 
    {  
        public int ID; 
        public string p_name;        
        public string p_discovery;
        public string p_discoveryYear;
        public float  p_semiMajor;
        public float  p_radius_Earth;
        public float  p_mass_Earth;
        public float  p_rotation_Period;
        public float  p_orbital_Period;  
        public float p_temperature;
    }    

[System.Serializable]
public class JsonParse : MonoBehaviour {


    private Planet[] Planets;
    private SolarSystem[] SolarSystems;
    // name of the json file we are using
	public string systemsFileName = "systems.json";
    public string planetsFileName = "planets.json";
    // hold the raw JSON file as text 
	private string systemJsonText;
    private string planetJsonText; 

    public float orbitXScale = 1.0f;
    public float planetOrbitSpeed = .1f;
    public float planetRotateSpeed = 10.0f;

    
    // the scaling values for how fast things will changed     
    public float planetSizeScalingValue = 1.5f;
    public float RotationPeriodScalingValue = 1.1f;
    public float OrbitDistanceScalingValue = 1.1f;
    public float OrbitPeriodScalingValue = 1.1f;
    // distance between y distance 
    public float gapBetweenPlanets3d = 1.0f;
    private GameObject allThreeDimensionalSystems;


    private float changedRotationPeriod = 1.0f;
    private float changedOrbitDistance = 1.0f;
    private float changedOrbitPeriod = 1.0f;
    private float changedPlanetSize = 0.1f;

    // arrays to keep track of sizes for the planet sizes 
    private float[] OrbitPeriodArray;
    private float[] RotationPeriodArray;
    private float[] OrbitDistanceArray;
    private float[] PlanetSizeArray;

    

    private GameObject[] fourSystems = new GameObject[4];
    private int fourSystemsCounter = 0;
	// Use this for initialization
	void Start () {
        // create the 4 scaling arrays

    OrbitPeriodArray = new float[100];
    RotationPeriodArray = new float[100];
    OrbitDistanceArray = new float[100];
    PlanetSizeArray = new float[100];

    


        // load the json file 
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
         string filePathSystems = Path.Combine(Application.streamingAssetsPath, systemsFileName);
         string filePathPlanets = Path.Combine(Application.streamingAssetsPath, planetsFileName);

        if(File.Exists(filePathSystems) && File.Exists(filePathPlanets) )
        {   
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

            {
                int randomNumber = UnityEngine.Random.Range(0,SolarSystems.Length);
                int result = createSystemFromID(randomNumber);
                Debug.Log("Random number was :" + randomNumber);
            }
        
        if(Input.GetKeyDown("z"))
        {
                 Debug.Log("Z");
                 scalePlanetsDown();
        }

        if(Input.GetKeyDown("x"))
        {
             Debug.Log("X");
             scalePlanetsUp();
             
        }


        if(Input.GetKeyDown("c"))
        {
             Debug.Log("c");
             scaleOrbitPeriodUp();
             
        }

        if(Input.GetKeyDown("v"))
        {
             Debug.Log("v");
             scaleOrbitPeriodDown();
             
        }
        
	}


    void scalePlanetsUp()
    {
        GameObject[] spheres;
                    spheres = GameObject.FindGameObjectsWithTag("Planet"); 
                    // Iterate through them and turn each one off
                    foreach (GameObject sphere in spheres)
                    { 
                        Vector3 temp = new Vector3(
                        sphere.transform.localScale.x * planetSizeScalingValue,
                        sphere.transform.localScale.y * planetSizeScalingValue,
                        sphere.transform.localScale.z * planetSizeScalingValue
                        );
                    sphere.transform.localScale = temp;
                    } 
                    changedPlanetSize = changedPlanetSize * planetSizeScalingValue;
    }
    
    void scalePlanetsDown()
    {
                 
                 GameObject[] spheres;
                    spheres = GameObject.FindGameObjectsWithTag("Planet"); 
                    // Iterate through them and turn each one off
                    foreach (GameObject sphere in spheres)
                    { 
                        Vector3 temp = new Vector3(
                        sphere.transform.localScale.x / planetSizeScalingValue,
                        sphere.transform.localScale.y / planetSizeScalingValue,
                        sphere.transform.localScale.z / planetSizeScalingValue
                        );
                    sphere.transform.localScale = temp;
                    } 
                    changedPlanetSize = changedPlanetSize / planetSizeScalingValue;
                 
        }

    void scaleOrbitPeriodUp()
    {
                GameObject[] pivots;
                    pivots = GameObject.FindGameObjectsWithTag("Pivot"); 
                    // Iterate through them and turn each one off
                    foreach (GameObject aPivot in pivots)
                    { 
                       aPivot.GetComponent<PlanetMotion>().orbitalPeriod = aPivot.GetComponent<PlanetMotion>().orbitalPeriod * OrbitPeriodScalingValue; 
                    }
                    changedOrbitPeriod = changedOrbitPeriod * OrbitPeriodScalingValue; 
    }

    void scaleOrbitPeriodDown()
    {
                GameObject[] pivots;
                    pivots = GameObject.FindGameObjectsWithTag("Pivot"); 
                    // Iterate through them and turn each one off
                    foreach (GameObject aPivot in pivots)
                    { 
                       aPivot.GetComponent<PlanetMotion>().orbitalPeriod = aPivot.GetComponent<PlanetMotion>().orbitalPeriod / OrbitPeriodScalingValue; 
                    }
                    changedOrbitPeriod = changedOrbitPeriod / OrbitPeriodScalingValue; 
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
        int systemsCounter = 0;
        int planetCounter = 0;
        int counter = 0;
        int length = Planets.Length;

        
        SolarSystems[counter].systems_planets = new Planet[9];
       SolarSystems[counter].habitable_counter = 0;
        for(int i = 0 ; i < length ; i++)
        {
            if(Planets[i].ID > (counter) )
            {   
                SolarSystems[counter].numPlanets = planetCounter;
                counter++;
                planetCounter = 0;
                SolarSystems[counter].systems_planets = new Planet[9];
                SolarSystems[counter].habitable_counter = 0;
            }

            if(Planets[i].p_semiMajor >= SolarSystems[counter].habitable_inner && Planets[i].p_semiMajor <= SolarSystems[counter].habitable_outer )
            SolarSystems[counter].habitable_counter++;
            // add the planet to the solar system 
            SolarSystems[counter].systems_planets[planetCounter] = Planets[i];
            planetCounter++;
        
        }
        SolarSystem [] temp = SortSystemsDistance();
    
        // our view has to always be visible, create that view 
        int result = createSystemFromID(0);
        
    }

     public SolarSystem[] SortSystemsMostHabitable()
    {
    SolarSystem [] temp = SolarSystems;
        Array.Sort(temp,
    delegate(SolarSystem x, SolarSystem y) 
    { return x.habitable_counter.CompareTo(y.habitable_counter);});    
    return temp;

    }


    public SolarSystem[] SortSystemsMostPlanets()
    {
    SolarSystem [] temp = SolarSystems;
        Array.Sort(temp,
    delegate(SolarSystem x, SolarSystem y) 
    { return y.numPlanets.CompareTo(x.numPlanets); });    
    return temp;

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

    private string getStarTexture(float temperature)
    {
        if (temperature > 25000)
            return "ostar";
        else if (temperature > 11000 || temperature <= 25000)
            return "bstar";
        else if (temperature > 7500 || temperature <= 11000)
            return "astar";
        else if (temperature >= 6000 || temperature <= 7500)
            return "fstar";
        else if (temperature >= 5000 || temperature <= 6000)
            return "gstar";
        else if (temperature >= 3500 || temperature <= 5000)
            return "kstar";
        else
            return "mstar";
    }

    private string getPlanetTexture(float semiMajorAxis,float temperature)
    {   

            if (temperature > 1)
            {
            if (temperature < 150)
                return "jupiter";
            else if (temperature >= 150 && temperature < 250)
                return "mercury";
            else if (temperature >= 250 && temperature < 800)
                return "neptune";
            else if (temperature >= 900 && temperature < 1400)
                return "fstar";
            else
                return "uranus";
            }

            if (semiMajorAxis < 0.5)
                return "mercury";
            else if (semiMajorAxis >= 0.5 && semiMajorAxis < 1.5)
                return "venus";
            else if (semiMajorAxis >= 1.5 && semiMajorAxis < 5)
                return "mars";
            else if (semiMajorAxis >= 5 && semiMajorAxis < 9)
                return "jupiter";
            else if (semiMajorAxis >= 9 && semiMajorAxis < 15)
                return "saturn";
            else if (semiMajorAxis >= 15 && semiMajorAxis < 25)
                return "uranus";
            else if (semiMajorAxis >= 25 && semiMajorAxis < 35)
                return "neptune";
            else
                return "pluto";
        }

    GameObject CreateOurSolarSystem(SolarSystem thisSolarSystem)
    {
        // based off the values from the solar system, make a 3d VIEW
		//first the sun 	
		GameObject theStar;
        GameObject SolarSystem = new GameObject();
        SolarSystem.name = thisSolarSystem.star + " " + thisSolarSystem.ID;
		// create the star 
		 float starSize = thisSolarSystem.radius_Solar;
        theStar = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		theStar.name = thisSolarSystem.star;
       theStar.transform.localScale = new Vector3 (starSize * 0.7f , starSize * 0.7f, starSize * 0.7f);
         Material starMaterial = new Material (Shader.Find ("Unlit/Texture"));
		theStar.GetComponent<MeshRenderer> ().material = starMaterial;
        starMaterial.mainTexture = Resources.Load ("sol") as Texture;
        theStar.transform.parent = SolarSystem.transform;

        for(int i = 0 ; i < thisSolarSystem.numPlanets;i++)
        {
            float planetSize = thisSolarSystem.systems_planets[i].p_radius_Earth * planetSizeScalingValue;
            float PlanetOrbitalPeriod = thisSolarSystem.systems_planets[i].p_orbital_Period;
            float planetDistance = thisSolarSystem.systems_planets[i].p_semiMajor;
			float planetSpeed = thisSolarSystem.systems_planets[i].p_rotation_Period;
			float planetsemiMajorAxis = thisSolarSystem.systems_planets[i].p_semiMajor;
			string planetName = thisSolarSystem.systems_planets[i].p_name;
            //create the plaent and the pivot point for it 
            GameObject planetPivot = new GameObject();
            GameObject thisPlanet = GameObject.CreatePrimitive (PrimitiveType.Sphere);
            thisPlanet.tag = "Planet";
            planetPivot.tag = "Pivot";
            planetPivot.name = planetName +"_Pivot_"+thisSolarSystem.systems_planets[i].ID;
            thisPlanet.transform.parent = planetPivot.transform;
            planetPivot.transform.parent = SolarSystem.transform;
            // set planet variables to the planet motion script 
            planetPivot.AddComponent<PlanetMotion>();
            planetPivot.GetComponent<PlanetMotion>().ellipse.xAxis = planetDistance ;
            planetPivot.GetComponent<PlanetMotion>().ellipse.zAxis = planetDistance ;
            planetPivot.GetComponent<PlanetMotion>().orbitalPeriod = PlanetOrbitalPeriod * planetOrbitSpeed;
            planetPivot.GetComponent<PlanetMotion>().rotationPeriod = planetSpeed * planetRotateSpeed; 
            planetPivot.GetComponent<PlanetMotion>().height = fourSystemsCounter * gapBetweenPlanets3d; 
            planetPivot.GetComponent<PlanetMotion>().planet = thisPlanet.GetComponent<Transform>();
            planetPivot.GetComponent<PlanetMotion>().DrawEllipse();
            Material planetMaterial = new Material (Shader.Find ("Standard"));
			thisPlanet.GetComponent<MeshRenderer>().material = planetMaterial;
            planetMaterial.mainTexture = Resources.Load(planetName) as Texture;
            thisPlanet.transform.localScale = new Vector3 (planetSize * changedPlanetSize, planetSize * changedPlanetSize, planetSize * changedPlanetSize);
            thisPlanet.transform.position = new Vector3 (0, 0, planetDistance * orbitXScale);
        }
        return SolarSystem;
    }

    GameObject CreateView(SolarSystem thisSolarSystem)
	{   
        // if this our solar system 
        if(thisSolarSystem.ID == 0)
            { return CreateOurSolarSystem(thisSolarSystem); }
		// based off the values from the solar system, make a 3d VIEW
		//first the sun 	
		GameObject theStar;
        GameObject SolarSystem = new GameObject();
        SolarSystem.name = thisSolarSystem.spectral + " " + thisSolarSystem.ID;

		// create the star 
		 float starSize = thisSolarSystem.radius_Solar;
        theStar = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		theStar.name = thisSolarSystem.star;
        theStar.transform.localScale = new Vector3 (starSize * 0.7f, starSize * 0.7f, starSize * 0.7f);
        // get the texture name of the star 
        string textureNameStar = getStarTexture(thisSolarSystem.temperature);
        if(thisSolarSystem.ID == 0)
        { textureNameStar = "sol";}
        Material starMaterial = new Material (Shader.Find ("Unlit/Texture"));
		theStar.GetComponent<MeshRenderer> ().material = starMaterial;
        starMaterial.mainTexture = Resources.Load (textureNameStar) as Texture;

        theStar.transform.parent = SolarSystem.transform;
        //SolarSystem.transform.parent = allThreeDimensionalSystems.transform;
        // now we have to go and add each planet :-)

        for(int i = 0 ; i < thisSolarSystem.numPlanets;i++)
        {
            
            float planetSize = thisSolarSystem.systems_planets[i].p_radius_Earth * planetSizeScalingValue;
            float PlanetOrbitalPeriod = thisSolarSystem.systems_planets[i].p_orbital_Period;
            float planetDistance = thisSolarSystem.systems_planets[i].p_semiMajor;
			float planetSpeed = thisSolarSystem.systems_planets[i].p_rotation_Period;
			float planetsemiMajorAxis = thisSolarSystem.systems_planets[i].p_semiMajor;
            float planetTemp = thisSolarSystem.systems_planets[i].p_temperature;
			string planetName = thisSolarSystem.systems_planets[i].p_name;
            string textureName = planetName; // for now get picture texture based off name 
            GameObject planetPivot = new GameObject();
            GameObject thisPlanet = GameObject.CreatePrimitive (PrimitiveType.Sphere);
            thisPlanet.tag = "Planet";
            planetPivot.name = planetName +"_Pivot_"+thisSolarSystem.systems_planets[i].ID;
            planetPivot.tag = "Pivot";
            thisPlanet.transform.parent = planetPivot.transform;
            planetPivot.transform.parent = SolarSystem.transform;
             // set planet variables to the planet motion script 
            planetPivot.AddComponent<PlanetMotion>();
            // if we don't know speed of rotation, turn it off
            if(planetSpeed == 0.0f)
            {
                planetPivot.GetComponent<PlanetMotion>().rotateActive = false;
            }
            planetPivot.GetComponent<PlanetMotion>().ellipse.xAxis = planetDistance ;
            planetPivot.GetComponent<PlanetMotion>().ellipse.zAxis = planetDistance ;
            planetPivot.GetComponent<PlanetMotion>().orbitalPeriod = PlanetOrbitalPeriod * planetOrbitSpeed * changedOrbitPeriod;
            planetPivot.GetComponent<PlanetMotion>().rotationPeriod = planetSpeed * planetRotateSpeed * changedRotationPeriod; 
            planetPivot.GetComponent<PlanetMotion>().height = fourSystemsCounter * gapBetweenPlanets3d; 
            planetPivot.GetComponent<PlanetMotion>().planet = thisPlanet.GetComponent<Transform>();
            planetPivot.GetComponent<PlanetMotion>().DrawEllipse();
            Material planetMaterial = new Material (Shader.Find ("Standard"));
			thisPlanet.GetComponent<MeshRenderer>().material = planetMaterial;
            planetMaterial.mainTexture = Resources.Load(getPlanetTexture(planetsemiMajorAxis,planetTemp)) as Texture;


            thisPlanet.transform.localScale = new Vector3 (planetSize * changedPlanetSize, planetSize * changedPlanetSize, planetSize * changedPlanetSize);
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
