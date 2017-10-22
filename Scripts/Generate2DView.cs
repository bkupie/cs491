using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Planet2D
{
    public string planetName;
    public string discoveryMethod;

    public float eqTemperature;
    public float semiMajorAxis;

    public float radiusR_Earth;
    public float massR_Earth;

    public float rotationalPeriod;
    public float orbitalPeriod;

    public Planet2D(string name, string method, float radius, float mass, float rotperiod, float semiaxis, float orbperiod, float etemp)
    {
        planetName = name;
        discoveryMethod = method;
        semiMajorAxis = semiaxis;
        radiusR_Earth = radius;
        massR_Earth = mass;
        rotationalPeriod = rotperiod;
        orbitalPeriod = orbperiod;
        eqTemperature = etemp;
    }
}

public class Star2D
{
    public string starName;         // Name of Star2D

    public string spectral;         // Spectral Classification of the Star2D
    public float temperature;       // Temperature of Star2D - [K]

    public float radiusR_Sun;       // Radius of Star2D Relative to Our Sun
    public float massR_Sun;         // Mass of Star2D Relative to OUr Sun

    public Star2D(string name, string spect, float temp, float radius, float mass)
    {
        starName = name;
        spectral = spect;
        temperature = temp;
        radius = radiusR_Sun;
        mass = massR_Sun;
    }
}

public class SolarSystem2D
{
    public int systemID;            // Solar System ID
    public string systemName;

    public List<Planet2D> planets = new List<Planet2D>();
    public List<Star2D> stars = new List<Star2D>();

    public float distanceFromUs;    // Distance from our own Sun - [Light Years]

    public float x, y, z;           // Unity Space X,Y,Z Coordinates

    public float habitableInner;
    public float habitableOuter;

    public SolarSystem2D(int id, string name, float habinner, float habouter, int numplanets, int numstars)
    {
        systemID = id;
        systemName = name;
        habitableInner = habinner;
        habitableOuter = habouter;
    }

    public void addPlanet(Planet2D p)
    {
        planets.Add(p);
    }

    public void addStar(Star2D s)
    {
        stars.Add(s);
    }
}


public class Generate2DView : MonoBehaviour
{
    List<SolarSystem2D> solarSystems = new List<SolarSystem2D>();

    public int maxSystemsShown = 9;

    // Panel Variables
    float panelWidth = 40f;
    float panelHeight = 10f;
    float panelDepth = 0.01f;
    float panelThickness = 0.1f;
    
    float panelYOffset = 2f;
    float planetBoxOffset = 0.8f;
    float panelSunSize = 2f;

    float planetBoxSize = 4f;

    // Scales
    public float distanceScale = 20f;
    public float distanceChangeFactor = 2f;
    public float sizeChangeFactor = 1.2f;
    public float sizeScale = 2f;

    // paging
    int curStartIdx = 0;

    void Start ()
    {
        loadDummyData();

        create2DView();
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            changePlanetScale(1);
        }
        else if (Input.GetKeyDown(KeyCode.O))
            changePlanetScale(-1);
        else if (Input.GetKeyDown(KeyCode.L))
            changePlanetSize(1);
        else if (Input.GetKeyDown(KeyCode.K))
            changePlanetSize(-1);
        else if (Input.GetKeyDown(KeyCode.X))
        {
            pagePlanets();
        }
    }

    public void create2DView()
    {
        GameObject all2DViews = new GameObject();
        all2DViews.name = "All 2D Views";

        for (int i = 0; i < maxSystemsShown; i++)
        {
            GameObject systemParent = new GameObject();

            systemParent.name = "2D View of " + solarSystems[i + curStartIdx].systemName;
            systemParent.transform.parent = all2DViews.transform;

            create2DPanel(solarSystems[i + curStartIdx], systemParent);
            create2DStar(solarSystems[i + curStartIdx], systemParent);
            create2DPlanets(solarSystems[i + curStartIdx], systemParent);

            handlePanelPosition(systemParent, i);
        }

        all2DViews.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void pagePlanets()
    {
        GameObject.Destroy(GameObject.Find("All 2D Views"));
        curStartIdx += maxSystemsShown;
        if (curStartIdx > solarSystems.Count)
            curStartIdx = 0;
        create2DView();
    }

    public void handlePanelPosition(GameObject systemParent, int i)
    {
        if (i < 3)
            systemParent.transform.position = new Vector3(0, i * (panelHeight + panelYOffset), 0);
        else if (i < 6)
        {
            systemParent.transform.position = new Vector3(-(panelWidth - 4), (i - 3) * (panelHeight + panelYOffset), -(panelWidth / 2));
            systemParent.transform.eulerAngles = new Vector3(0f, -60f, 0f);
        }
        else
        {
            systemParent.transform.position = new Vector3(panelWidth - 4, (i - 6) * (panelHeight + panelYOffset), -(panelWidth / 2));
            systemParent.transform.eulerAngles = new Vector3(0f, 60f, 0f);
        }
    }

    public void create2DPanel(SolarSystem2D system, GameObject systemParent)
    {
        GameObject panelParent, orbitLine, borderBox, systemText;

        panelParent = new GameObject();
        panelParent.name = "2D Panel";
        panelParent.transform.parent = systemParent.transform;
        
        
        // Orbit Line:
        orbitLine = GameObject.CreatePrimitive(PrimitiveType.Cube);
        orbitLine.name = "Orbit Line";

        orbitLine.transform.parent = panelParent.transform;
        orbitLine.transform.localPosition = new Vector3(panelSunSize/2, panelHeight / 3, panelDepth*2);
        orbitLine.transform.localScale = new Vector3(panelWidth, panelThickness/2, panelThickness / 2);

        // Borders:
        Material borderMaterial;
        string materialName = "blueborder";

        borderMaterial = new Material(Shader.Find("Unlit/Texture"));
        borderMaterial.mainTexture = Resources.Load(materialName) as Texture;

        borderBox = createBox(panelWidth + panelSunSize, panelHeight, panelDepth, panelThickness, borderMaterial);
        borderBox.name = "Borders";
        borderBox.transform.parent = panelParent.transform;

        // Solar System Text
        systemText = new GameObject();
        systemText.name = system.systemName + " Text";
        systemText.transform.parent = panelParent.transform;
        systemText.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        systemText.transform.localPosition = new Vector3(-panelWidth / 2 + 1, panelHeight / 2 + 1.5f, 0);

        TextMesh starTextMesh = systemText.AddComponent<TextMesh>();
        starTextMesh.text = system.systemName;
        starTextMesh.fontSize = 150;
    }

    public void create2DStar(SolarSystem2D system, GameObject systemParent)
    {
        GameObject starsParent;
        Material sideStarMaterial;

        starsParent = new GameObject();
        starsParent.name = "Stars";
        starsParent.transform.parent = systemParent.transform;

        int numStars = system.stars.Count;
        for (int i = 1; i <= numStars; i++)
        {
            // Star2D:
            GameObject sideStar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sideStar.name = system.stars[i-1].starName;

            sideStar.transform.parent = starsParent.transform;

            float starHeight = panelHeight / numStars;
            sideStar.transform.localPosition = new Vector3(-(panelWidth / 2), ((panelHeight / 2) + starHeight / 2) - (i * starHeight), 0);
            sideStar.transform.localScale = new Vector3(panelSunSize, starHeight, panelDepth);

            sideStarMaterial = new Material(Shader.Find("Unlit/Texture"));
            sideStarMaterial.mainTexture = Resources.Load(getStarColor(system)) as Texture;
            sideStar.GetComponent<MeshRenderer>().material = sideStarMaterial;
        }
    }

    string getStarColor(SolarSystem2D system)
    {
        float temperature = system.stars[0].temperature;
        if (system.systemName == "Our Sun")
            return "sol";

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

    public void create2DPlanets(SolarSystem2D system, GameObject systemParent)
    {
        GameObject lineConvergingPoint = new GameObject();
        lineConvergingPoint.transform.parent = systemParent.transform;
        lineConvergingPoint.transform.localPosition = new Vector3(panelWidth / 2, 0, 0);

        GameObject planetsParent = new GameObject();
        planetsParent.name = "Planets";
        planetsParent.transform.parent = systemParent.transform;

        for (int i = 0; i < system.planets.Count; i++)
        {
            // Planet2D Boxes
            GameObject planetBox;
            Material borderMaterial;
            borderMaterial = new Material(Shader.Find("Unlit/Texture"));

            if (system.planets[i].semiMajorAxis >= system.habitableInner && system.planets[i].semiMajorAxis <= system.habitableOuter)
            {
                string materialName = "habitable";
                borderMaterial.mainTexture = Resources.Load(materialName) as Texture;
            }
            else
            {
                string materialName = "blueborder";
                borderMaterial.mainTexture = Resources.Load(materialName) as Texture;
            }
            
            planetBox = createBox(planetBoxSize, planetBoxSize, planetBoxSize, panelThickness, borderMaterial);
            planetBox.name = system.planets[i].planetName + " Box";

            planetBox.transform.parent = systemParent.transform;

            float boxOffset = i * (planetBoxSize + planetBoxOffset);
            planetBox.transform.localPosition = new Vector3(-(panelWidth / 2) + panelSunSize * 2 + boxOffset, 0, 0);

            // Planet2D Text
            GameObject planetText;

            planetText = new GameObject();
            planetText.name = system.planets[i].planetName + " Text";
            planetText.transform.parent = systemParent.transform;
            planetText.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            planetText.transform.localPosition = new Vector3(-(panelWidth / 2) + panelSunSize * 2 + boxOffset - planetBoxSize / 2, -planetBoxSize / 2, 0);

            TextMesh planetTextMesh = planetText.AddComponent<TextMesh>();
            planetTextMesh.text = system.planets[i].planetName;
            planetTextMesh.fontSize = 80;

            // Grey Planes
            GameObject greyPlane = GameObject.CreatePrimitive(PrimitiveType.Cube);
            greyPlane.name = system.planets[i].planetName + " Grey";

            greyPlane.transform.parent = systemParent.transform;

            greyPlane.transform.localPosition = new Vector3(-(panelWidth / 2) + panelSunSize * 2 + boxOffset, 0, 0);
            greyPlane.transform.localScale = new Vector3(planetBoxSize, planetBoxSize, panelThickness);

            Material greyPlaneMat = new Material(Shader.Find("Transparent/Diffuse"));
            greyPlane.GetComponent<MeshRenderer>().material = greyPlaneMat;
            greyPlane.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 0.85f);
            greyPlane.GetComponent<MeshRenderer>().enabled = false;


            // Planets on Orbit Line
            GameObject orbitPlanet;

            orbitPlanet = GameObject.CreatePrimitive(PrimitiveType.Cube);
            orbitPlanet.name = system.planets[i].planetName;
            orbitPlanet.tag = "Planet";

            Material orbitPlanetMat = new Material(Shader.Find("Unlit/Texture"));
            orbitPlanet.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);

            orbitPlanet.transform.parent = systemParent.transform;

            float leftOffset = -(panelWidth / 2) + panelSunSize / 2;
            float semiMajor = system.planets[i].semiMajorAxis * distanceScale;
            orbitPlanet.transform.localPosition = new Vector3(semiMajor + leftOffset, (panelHeight / 3), 0); 
            orbitPlanet.transform.localScale = new Vector3(1, 1, 1);


            if (semiMajor + leftOffset > (panelWidth / 2))
            {
                orbitPlanet.GetComponent<MeshRenderer>().enabled = false;
                greyPlane.GetComponent<MeshRenderer>().enabled = true; ;
            }
            else
            {
                //orbitPlanet.AddComponent<LineRenderer>();
                //LineRenderer lr = orbitPlanet.GetComponent<LineRenderer>();
                //lr.useWorldSpace = false;

                //Vector3 startPos = systemParent.transform.Find(system.planets[i].planetName + " on Orbit").position;
                //Vector3 endPos = systemParent.transform.Find(system.planets[i].planetName + " Box").transform.Find("Top Border").position;

                //print(systemParent.name + " FROM: " + startPos + " | TO: " + endPos);

                //Vector3 startPos = Vector3.zero;
                //Vector3 endPos = planetBox.transform.Find("Top Border").transform.position;

                //print(planetBox.transform.Find("Top Border").transform.position);

                //lr.SetPosition(0, startPos);
                //lr.SetPosition(1, endPos);
                //lr.SetWidth(panelThickness, panelThickness);
            }

            // Scale too Large Icons
            GameObject tooLargeIcon;
            Material tooLargeMaterial;

            tooLargeIcon = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tooLargeIcon.name = "Size Icon";

            tooLargeIcon.transform.parent = planetBox.transform;
            tooLargeIcon.transform.position = new Vector3(-(panelWidth / 2) + panelSunSize * 2 + boxOffset, (planetBoxSize / 2) - 0.5f, -panelThickness);
            tooLargeIcon.transform.localScale = new Vector3(1.5f, 1, panelThickness/10);
            tooLargeIcon.transform.eulerAngles = new Vector3(0, 180, 0);

            tooLargeMaterial = new Material(Shader.Find("Unlit/Texture"));
            tooLargeMaterial.mainTexture = Resources.Load("toolarge") as Texture;
            tooLargeIcon.GetComponent<MeshRenderer>().material = tooLargeMaterial;
            tooLargeIcon.GetComponent<MeshRenderer>().enabled = false;

            // Planets in Boxes
            GameObject boxPlanet;
            Material sidePlanetMaterial;

            boxPlanet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            boxPlanet.name = system.planets[i].planetName;

            boxPlanet.transform.parent = planetsParent.transform;

            boxPlanet.transform.localPosition = new Vector3(-(panelWidth / 2) + panelSunSize * 2 + boxOffset, 0, 0);

            string planetTextureName = getPlanetTexture(system, system.planets[i]);
            sidePlanetMaterial = new Material(Shader.Find("Standard"));
            sidePlanetMaterial.mainTexture = Resources.Load(planetTextureName) as Texture;
            boxPlanet.GetComponent<MeshRenderer>().material = sidePlanetMaterial;


            // Scaled Radius
            float planetRadius = getPlanetRadius(system.planets[i]) * sizeScale;
            if (planetRadius > planetBoxSize)
            {
                planetRadius = planetBoxSize;
                tooLargeIcon.GetComponent<MeshRenderer>().enabled = true;

                //Material redBorderMat;

                //redBorderMat = new Material(Shader.Find("Unlit/Texture"));
                //redBorderMat.mainTexture = Resources.Load("red") as Texture;

                //planetBox.transform.Find("Top Border").GetComponent<MeshRenderer>().material = redBorderMat;
                //planetBox.transform.Find("Left Border").GetComponent<MeshRenderer>().material = redBorderMat;
                //planetBox.transform.Find("Right Border").GetComponent<MeshRenderer>().material = redBorderMat;
                //planetBox.transform.Find("Bottom Border").GetComponent<MeshRenderer>().material = redBorderMat;
            }
                

            boxPlanet.transform.localScale = new Vector3(planetRadius, planetRadius, panelDepth * 5);
        }
    }

    public GameObject createBox(float width, float height, float depth, float thickness, Material mat)
    {
        GameObject boxParent, leftBorder, rightBorder, topBorder, bottomBorder;
        boxParent = new GameObject();

        float borderThickness = 0.1f;

        

        topBorder = GameObject.CreatePrimitive(PrimitiveType.Cube);
        topBorder.transform.parent = boxParent.transform;
        topBorder.name = "Top Border";
        topBorder.transform.localPosition = new Vector3(0, (height / 2), 0);
        topBorder.transform.localScale = new Vector3(width, thickness, thickness);
        topBorder.GetComponent<MeshRenderer>().material = mat;

        bottomBorder = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bottomBorder.transform.parent = boxParent.transform;
        bottomBorder.name = "Bottom Border";
        bottomBorder.transform.localPosition = new Vector3(0, -(height / 2), 0);
        bottomBorder.transform.localScale = new Vector3(width, thickness, thickness);
        bottomBorder.GetComponent<MeshRenderer>().material = mat;

        leftBorder = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftBorder.transform.parent = boxParent.transform;
        leftBorder.name = "Left Border";
        leftBorder.transform.localPosition = new Vector3((width / 2), 0, 0);
        leftBorder.transform.localScale = new Vector3(thickness, height, thickness);
        leftBorder.GetComponent<MeshRenderer>().material = mat;

        rightBorder = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightBorder.transform.parent = boxParent.transform;
        rightBorder.name = "Right Border";
        rightBorder.transform.localPosition = new Vector3(-(width / 2), 0, 0);
        rightBorder.transform.localScale = new Vector3(thickness, height, thickness);
        rightBorder.GetComponent<MeshRenderer>().material = mat;


        topBorder.transform.parent = boxParent.transform;
        bottomBorder.transform.parent = boxParent.transform;
        leftBorder.transform.parent = boxParent.transform;
        rightBorder.transform.parent = boxParent.transform;

        return boxParent;
    }

    public void changePlanetSize(int direction)
    {
        if (direction == 1)
            sizeScale *= sizeChangeFactor;
        else
            sizeScale /= sizeChangeFactor;

        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");

        foreach (GameObject planet in planets)
        {
            for (int i = 0; i < solarSystems.Count; i++)
            {
                float newPlanetSize = 0;
                for (int j = 0; j < solarSystems[i].planets.Count; j++)
                {
                    newPlanetSize = getPlanetRadius(solarSystems[i].planets[j]) * sizeScale;
                    if (planet.name == solarSystems[i].planets[j].planetName)
                    {
                        if (newPlanetSize > planetBoxSize / 2)
                        {
                            newPlanetSize = planetBoxSize;

                            GameObject p = GameObject.Find("All 2D Views").transform.Find("2D View of " + solarSystems[i].systemName).transform.Find("Planets").transform.Find(planet.name).gameObject;
                            p.transform.localScale = new Vector3(newPlanetSize, newPlanetSize, panelDepth * 5);

                            GameObject sizeIcon = GameObject.Find("All 2D Views").transform.Find("2D View of " + solarSystems[i].systemName).transform.Find(planet.name + " Box").transform.Find("Size Icon").gameObject;
                            sizeIcon.GetComponent<MeshRenderer>().enabled = true;

                            //Material redBorderMat;

                            //redBorderMat = new Material(Shader.Find("Unlit/Texture"));
                            //redBorderMat.mainTexture = Resources.Load("red") as Texture;

                            //GameObject box = GameObject.Find("All 2D Views").transform.Find("2D View of " + solarSystems[i].systemName).transform.Find(planet.name + " Box").gameObject;

                            //box.transform.Find("Top Border").GetComponent<MeshRenderer>().material = redBorderMat;
                            //box.transform.Find("Left Border").GetComponent<MeshRenderer>().material = redBorderMat;
                            //box.transform.Find("Right Border").GetComponent<MeshRenderer>().material = redBorderMat;
                            //box.transform.Find("Bottom Border").GetComponent<MeshRenderer>().material = redBorderMat;
                        }
                        else
                        {
                            GameObject p = GameObject.Find("All 2D Views").transform.Find("2D View of " + solarSystems[i].systemName).transform.Find("Planets").transform.Find(planet.name).gameObject;
                            p.transform.localScale = new Vector3(newPlanetSize, newPlanetSize, panelDepth * 5);

                            GameObject sizeIcon = GameObject.Find("All 2D Views").transform.Find("2D View of " + solarSystems[i].systemName).transform.Find(planet.name + " Box").transform.Find("Size Icon").gameObject;
                            sizeIcon.GetComponent<MeshRenderer>().enabled = false;

                            //Material greenBorderMat;

                            //greenBorderMat = new Material(Shader.Find("Unlit/Texture"));
                            //greenBorderMat.mainTexture = Resources.Load("habitable") as Texture;

                            //GameObject box = GameObject.Find("All 2D Views").transform.Find("2D View of " + solarSystems[i].systemName).transform.Find(planet.name + " Box").gameObject;

                            //box.transform.Find("Top Border").GetComponent<MeshRenderer>().material = greenBorderMat;
                            //box.transform.Find("Left Border").GetComponent<MeshRenderer>().material = greenBorderMat;
                            //box.transform.Find("Right Border").GetComponent<MeshRenderer>().material = greenBorderMat;
                            //box.transform.Find("Bottom Border").GetComponent<MeshRenderer>().material = greenBorderMat;

                        }
                    }

                }
            }
        }
    }

    public void changePlanetScale(int direction)
    {
        if (direction == 1)
            distanceScale *= distanceChangeFactor;
        else
            distanceScale /= distanceChangeFactor;

        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");

        foreach (GameObject planet in planets)
        {
            for (int i = 0; i < solarSystems.Count; i++)
            {
                float leftOffset = -(panelWidth / 2) + panelSunSize / 2;
                float newSemiMajorAxis = 0;
                for (int j = 0; j < solarSystems[i].planets.Count; j++)
                {
                    newSemiMajorAxis = solarSystems[i].planets[j].semiMajorAxis * distanceScale + leftOffset;
                    if (planet.name == solarSystems[i].planets[j].planetName)
                    {
                        if (newSemiMajorAxis > panelWidth / 2)
                        {
                            planet.GetComponent<MeshRenderer>().enabled = false;
                            GameObject.Find("All 2D Views").transform.Find("2D View of " + solarSystems[i].systemName).transform.Find(planet.name + " Grey").GetComponent<MeshRenderer>().enabled = true;
                        }
                        else
                        {
                            planet.GetComponent<MeshRenderer>().enabled = true;
                            planet.transform.localPosition = new Vector3(newSemiMajorAxis, (panelHeight / 3), 0);
                            GameObject.Find("All 2D Views").transform.Find("2D View of " + solarSystems[i].systemName).transform.Find(planet.name + " Grey").GetComponent<MeshRenderer>().enabled = false;
                        }
                    }
                    
                }
            }
        }

    }

    public float getPlanetRadius(Planet2D p)
    {
        float radius = 0;

        if (p.radiusR_Earth == -1)
        {
            
            float mass = p.massR_Earth;

            if (mass < 2)
                radius = 1f;
            else if (mass >= 2 && mass < 5)
                radius = 1.5f;
            else if (mass >= 5 && mass < 10)
                radius = 4f;
            else if (mass >= 10 && mass < 30)
                radius = 7f;
            else
                radius = 15f;
        }
        else
        {
            radius = p.radiusR_Earth;
        }


        return radius;
    }

    public string getPlanetTexture(SolarSystem2D ss, Planet2D p)
    {
        string textureName;

        if (ss.systemName == "Our Sun")
            return p.planetName.ToLower();

        if (p.eqTemperature != -1)
        {
            if (p.eqTemperature < 150)
                return "jupiter";
            else if (p.eqTemperature >= 150 && p.eqTemperature < 250)
                return "mercury";
            else if (p.eqTemperature >= 250 && p.eqTemperature < 800)
                return "neptune";
            else if (p.eqTemperature >= 900 && p.eqTemperature < 1400)
                return "fstar";
            else
                return "uranus";
        }
        else
        {
            if (p.semiMajorAxis < 0.5)
                return "mercury";
            else if (p.semiMajorAxis >= 0.5 && p.semiMajorAxis < 1.5)
                return "venus";
            else if (p.semiMajorAxis >= 1.5 && p.semiMajorAxis < 5)
                return "mars";
            else if (p.semiMajorAxis >= 5 && p.semiMajorAxis < 9)
                return "jupiter";
            else if (p.semiMajorAxis >= 9 && p.semiMajorAxis < 15)
                return "saturn";
            else if (p.semiMajorAxis >= 15 && p.semiMajorAxis < 25)
                return "uranus";
            else if (p.semiMajorAxis >= 25 && p.semiMajorAxis < 35)
                return "neptune";
            else
                return "pluto";
        }

        return textureName;
    }

    
    public void loadDummyData()
    {
        // Our Solar System
        SolarSystem2D OurSun = new SolarSystem2D(0, "Our Sun", 0.5f, 3, 9, 1);
        OurSun.addStar(new Star2D("Sun", "N/A", 5778, 0, 0));
        OurSun.addStar(new Star2D("TestSun", "N/A", 3000, 0, 0));
        //OurSun.addStar(new Star2D("TestSun", "N/A", 3000, 0, 0));
        OurSun.addPlanet(new Planet2D("Mercury", "N/A", 0.383f, 0.0553f, -1, 0.387f, 88, -1));
        OurSun.addPlanet(new Planet2D("Venus", "N/A", 0.950f, 0.815f, -1, 0.723f, 225, -1));
        OurSun.addPlanet(new Planet2D("Earth", "N/A", 1f, 1f, -1, 1f, 365, -1));
        OurSun.addPlanet(new Planet2D("Mars", "N/A", 0.532f, 0.1074f, -1, 1.524f, 687, -1));
        OurSun.addPlanet(new Planet2D("Jupiter", "N/A", 10.97f, 318f, -1, 5.20f, 4333, -1));
        OurSun.addPlanet(new Planet2D("Saturn", "N/A", 9.14f, 95.1f, -1, 9.54f, 10756, -1));
        OurSun.addPlanet(new Planet2D("Uranus", "N/A", 3.98f, 14.53f, -1, 19.19f, 30687, -1));
        OurSun.addPlanet(new Planet2D("Neptune", "N/A", 3.87f, 17.15f, -1, 30.1f, 60190, -1));

        solarSystems.Add(OurSun);

        // Gliese 667
        SolarSystem2D Gliese667C = new SolarSystem2D(1, "Gliese 667 C", 0, 0, 7, 1);
        Gliese667C.addStar(new Star2D("Gliese 667", "N/A", 3350, 0, 0));
        Gliese667C.addPlanet(new Planet2D("Gliese 667 b", "RV", -1, 5.6f, -1, 0.051f, 7.2f, -1));
        Gliese667C.addPlanet(new Planet2D("Gliese 667 h", "RV", -1, 1.1f, -1, 0.089f, 16.95f, -1));
        Gliese667C.addPlanet(new Planet2D("Gliese 667 c", "RV", -1, 3.8f, -1, 0.125f, 28.14f, -1));
        Gliese667C.addPlanet(new Planet2D("Gliese 667 f", "RV", -1, 2.7f, -1, 0.156f, 39.03f, -1));
        Gliese667C.addPlanet(new Planet2D("Gliese 667 e", "RV", -1, 2.7f, -1, 0.213f, 62.2f, -1));
        Gliese667C.addPlanet(new Planet2D("Gliese 667 d", "RV", -1, 5.1f, -1, 0.28f, 91.6f, -1));
        Gliese667C.addPlanet(new Planet2D("Gliese 667 g", "RV", -1, 5.0f, -1, 0.55f, 256f, -1));

        solarSystems.Add(Gliese667C);

        // Tau Ceti
        SolarSystem2D TauCeti = new SolarSystem2D(2, "Tau Ceti", 0, 0, 5, 1);
        TauCeti.addStar(new Star2D("Tau Ceti", "N/A", -1, 0, 0));
        TauCeti.addPlanet(new Planet2D("Tau Ceti b", "RV", -1, 2.0f, 11.9f, 0.1050f, 13.96f, -1));
        TauCeti.addPlanet(new Planet2D("Tau Ceti c", "RV", -1, 3.1f, 11.9f, 0.1950f, 35.4f, -1));
        TauCeti.addPlanet(new Planet2D("Tau Ceti d", "RV", -1, 3.6f, 11.9f, 0.374f, 94.1f, -1));
        TauCeti.addPlanet(new Planet2D("Tau Ceti e", "RV", -1, 4.3f, 11.9f, 0.552f, 168.1f, -1));
        TauCeti.addPlanet(new Planet2D("Tau Ceti f", "RV", -1, 6.6f, 11.9f, 1.350f, 642f, -1));

        solarSystems.Add(TauCeti);

        // Gliese 581
        SolarSystem2D Gliese581 = new SolarSystem2D(3, "Gliese 581", 0, 0, 6, 1);
        Gliese581.addStar(new Star2D("Gliese 581", "N/A", 3498, 0, 0));
        Gliese581.addPlanet(new Planet2D("Gliese 581 b", "RV", -1, 15.8f, 20.3f, 0.0406f, 5.3686f, 419f));
        Gliese581.addPlanet(new Planet2D("Gliese 581 c", "RV", -1, 5.5f, 20.3f, 0.0721f, 12.914f, 313f));
        Gliese581.addPlanet(new Planet2D("Gliese 581 d", "RV", -1, 6.04f, 20.3f, 0.220f, 66.6f, 181.0f));
        Gliese581.addPlanet(new Planet2D("Gliese 581 e", "RV", -1, 1.939f, 20.3f, 0.0282f, 3.1490f, 501f));
        Gliese581.addPlanet(new Planet2D("Gliese 581 f", "RV", -1, 7.0f, 20.3f, 0.758f, 433f, -1));
        Gliese581.addPlanet(new Planet2D("Gliese 581 g", "RV", -1, 3.10f, 20.3f, 0.1460f, 36.5f, 231f));

        solarSystems.Add(Gliese581);

        for (int i = 0; i < 100; i++)
        {
            SolarSystem2D Filler = new SolarSystem2D(i + 4, "Filler" + i, 0, 0, 0, 1);
            Filler.addStar(new Star2D("Filler" + i, "N/A", 3498, 0, 0));

            solarSystems.Add(Filler);
        }
    }


}
