using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Generate2DView : MonoBehaviour
{
    //List<SolarSystem2D> solarSystems = new List<SolarSystem2D>();

    public int maxSystemsShown = 1;

    // Panel Variables
    float panelWidth = 40f;
    float panelHeight = 10f;
    float panelDepth = 0.01f;
    float panelThickness = 0.1f;
    
    float panelYOffset = 2f;
    float panelXOffset = 4f;
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
    public SolarSystem[] ss;
    JsonParse jsonscript;

    //other
    public GameObject HaloPrefab;
    public int filterCounter = 0;
    public string searchQuery;

    void Start ()
    {
        jsonscript = this.GetComponent<JsonParse>();
        ss = jsonscript.SortSystemsMostPlanets();
        //print(ss.Length);

        create2DView();
        createUniverseView();
    }
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P))
            changePlanetScale(1);
        else if (Input.GetKeyDown(KeyCode.O))
            changePlanetScale(-1);
        else if (Input.GetKeyDown(KeyCode.L))
            changePlanetSize(1);
        else if (Input.GetKeyDown(KeyCode.K))
            changePlanetSize(-1);


        else if (Input.GetKeyDown(KeyCode.M))
        {
            pagePlanets(1);
            updateUniverseView();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            pagePlanets(-1);
            updateUniverseView();
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            filterPlanets();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            searchSystems();
        }
    }

    public void searchSystems()
    {
        ss = jsonscript.SortSystemsMostHabitable();
        Debug.Log(searchQuery);
        if (searchQuery[searchQuery.Length - 1] == ';')
        {
            string command = searchQuery.Substring(0, 3);
            Debug.Log("Command: " + command);
            if (command == "ID:")
            {
                int idToFind = int.Parse(searchQuery.Substring(3).TrimEnd(';'));
                maxSystemsShown = 1;
                GameObject.Destroy(GameObject.Find("All 2D Views"));
                for (int i = 0; i < ss.Length; i++)
                {
                    if (ss[i].ID == idToFind)
                    {
                        maxSystemsShown++;
                        Debug.Log("Found " + idToFind + " | System: " + ss[i].star + " | ID: " + ss[i].ID);
                        curStartIdx = i;
                    }
                        
                }
                create2DView();
            }
            else if (command == "SN:") //SN:Kepler;
            {
                string starToFind = (searchQuery.Substring(3).TrimEnd(';'));
                maxSystemsShown = 1;
                GameObject.Destroy(GameObject.Find("All 2D Views"));
                List<SolarSystem> queryList = new List<SolarSystem>();
                for (int i = 0; i < ss.Length; i++)
                {
                    if (ss[i].star.Contains(starToFind))
                    {
                        maxSystemsShown++;
                        if (maxSystemsShown > 9)
                            maxSystemsShown = 9;

                        Debug.Log("Found " + ss[i].star);
                        queryList.Add(ss[i]);
                    }
                }
                curStartIdx = 0;
                ss = queryList.ToArray();
                create2DView();
            }
            else if (command == "SY:") // System Type TODO: Doesn't work too well, maybe remove?
            {
                // TODO: CHECK THIS
                float typeToFind = float.Parse(searchQuery.Substring(3).TrimEnd(';'));

                maxSystemsShown = 1;
                GameObject.Destroy(GameObject.Find("All 2D Views"));
                List<SolarSystem> queryList = new List<SolarSystem>();
                for (int i = 0; i < ss.Length; i++)
                {
                    if (float.Parse(ss[i].system) == typeToFind)
                    {
                        //print(ss[i]);
                        maxSystemsShown++;
                        if (maxSystemsShown > 9)
                            maxSystemsShown = 9;

                        queryList.Add(ss[i]);
                    }

                }
                //print(queryList.Count);
                curStartIdx = 0;
                ss = queryList.ToArray();
                create2DView();
            }
            else if (command == "SC:")
            {
                Debug.Log("Searching for Spectral Class");
                string classToFind = (searchQuery.Substring(3).TrimEnd(';'));
                maxSystemsShown = 1;
                Debug.Log("Searching for " + classToFind);
                GameObject.Destroy(GameObject.Find("All 2D Views"));
                List<SolarSystem> queryList = new List<SolarSystem>();
                for (int i = 0; i < ss.Length; i++)
                {
                    if (ss[i].spectral.Contains(classToFind))
                    {
                        maxSystemsShown++;
                        if (maxSystemsShown > 9)
                            maxSystemsShown = 9;

                        Debug.Log("Found " + ss[i].star);
                        queryList.Add(ss[i]);
                    }
                }
                curStartIdx = 0;
                ss = queryList.ToArray();
                create2DView();
            }
            else if (command == "ST:")
            {
                Debug.Log("Searching for Star Temperature");
                float tempToFind = float.Parse(searchQuery.Substring(3).TrimEnd(';'));
                maxSystemsShown = 1;
                GameObject.Destroy(GameObject.Find("All 2D Views"));
                List<SolarSystem> queryList = new List<SolarSystem>();

                for (int i = 0; i < ss.Length; i++)
                {
                    if (ss[i].temperature < tempToFind + 100 && ss[i].temperature > tempToFind - 100)
                    {
                        maxSystemsShown++;
                        if (maxSystemsShown > 9)
                            maxSystemsShown = 9;

                        Debug.Log("Found " + ss[i].star);
                        queryList.Add(ss[i]);
                    }
                }
                curStartIdx = 0;
                ss = queryList.ToArray();
                create2DView();
            }
            else if (command == "NP:")
            {
                Debug.Log("Searching for Number of Planets");
            }
            else if (command == "DM:")
            {
                Debug.Log("Searching for Discovery Method");
            }
            else if (command == "PT:")
            {
                Debug.Log("Searching for Planet Temperature");
            }
            else
            {
                Debug.Log("Unknown Command");
            }
        }
        else
        {
            Debug.Log("Do Nothing");
        }
    }

    public void create2DView()
    {
        
        GameObject all2DViews = new GameObject();
        all2DViews.name = "All 2D Views";

        all2DViews.transform.parent = this.gameObject.transform.GetChild(1);

        for (int i = 0; i < maxSystemsShown; i++)
        {
            GameObject systemParent = new GameObject();

            if (i == 0)
            {
                systemParent.name = ss[0].star;
                systemParent.transform.parent = all2DViews.transform;

                create2DPanel(ss[0], systemParent);
                create2DStar(ss[0], systemParent);
                create2DPlanets(ss[0], systemParent);

                handlePanelPosition(systemParent, i);
            }
            else
            {
                systemParent.name = ss[i + curStartIdx].star;
                systemParent.transform.parent = all2DViews.transform;

                create2DPanel(ss[i + curStartIdx], systemParent);
                create2DStar(ss[i + curStartIdx], systemParent);
                create2DPlanets(ss[i + curStartIdx], systemParent);

                handlePanelPosition(systemParent, i);
            }
            
        }

        all2DViews.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
        all2DViews.transform.localPosition = new Vector3(0, 0, 0);
        all2DViews.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public void updateUniverseView()
    {
        GameObject.Destroy(GameObject.Find("Universe"));
        createUniverseView();
    }

    public void pagePlanets(int direction)
    {
        Debug.Log("Switching Page " + direction);
        GameObject.Destroy(GameObject.Find("All 2D Views"));

        if (direction == 1)
        {
            curStartIdx += maxSystemsShown;
            if (curStartIdx > ss.Length)
                curStartIdx = 0;
        }
        else
        {
            curStartIdx -= maxSystemsShown;
            if (curStartIdx < 0)
                curStartIdx = ss.Length - 1 - maxSystemsShown;
        }

        
        create2DView();
    }

    public void filterPlanets()
    {
        GameObject.Destroy(GameObject.Find("All 2D Views"));

        switch (filterCounter)
        {
            case 0:
                ss = jsonscript.SortSystemsDistance();
                Debug.Log("Sorted by Distance");
                break;
            case 1:
                Debug.Log("Sorted by Most Habitable");
                ss = jsonscript.SortSystemsMostHabitable();
                break;
            case 2:
                Debug.Log("Sorted by Most Planets");
                ss = jsonscript.SortSystemsMostPlanets();
                break;
            case 3:
                Debug.Log("Sorted by Temperature");
                ss = jsonscript.SortSystemsTemparture();
                break;
            default:
                Debug.Log("Shouldn't be here");
                break;
        }


        filterCounter++;
        if (filterCounter > 3)
            filterCounter = 0;

        create2DView();
        updateUniverseView();
    }

    public void handlePanelPosition(GameObject systemParent, int i)
    {
        int colNum = i % 5;
        int rowNum = i / 5;
        
        switch(colNum)
        {
            case 0:
                systemParent.transform.position = new Vector3(colNum * (panelWidth + panelXOffset), rowNum * (panelHeight + panelYOffset), 0);
                break;
            case 1:
                systemParent.transform.position = new Vector3(colNum * (panelWidth + panelXOffset), rowNum * (panelHeight + panelYOffset), 0);
                break;
            case 2:
                systemParent.transform.position = new Vector3(colNum * (panelWidth + panelXOffset), rowNum * (panelHeight + panelYOffset), -20);
                systemParent.transform.eulerAngles = new Vector3(0, 45, 0);
                break;
            case 3:
                systemParent.transform.position = new Vector3(colNum * (panelWidth + panelXOffset) - 26, rowNum * (panelHeight + panelYOffset), -65);
                systemParent.transform.eulerAngles = new Vector3(0, 90, 0);
                break;
            case 4:
                systemParent.transform.position = new Vector3((colNum - 1) * (panelWidth + panelXOffset) - 26, (rowNum) * (panelHeight + panelYOffset), -65 - panelWidth - panelXOffset);
                systemParent.transform.eulerAngles = new Vector3(0, 90, 0);
                break;
        }
        //if (i < 3)
        //    systemParent.transform.position = new Vector3(0, i * (panelHeight + panelYOffset), 0);
        //else if (i < 6)
        //{
        //    systemParent.transform.position = new Vector3(-(panelWidth - 4), (i - 3) * (panelHeight + panelYOffset), -(panelWidth / 2));
        //    systemParent.transform.eulerAngles = new Vector3(0f, -60f, 0f);
        //}
        //else
        //{
        //    systemParent.transform.position = new Vector3(panelWidth - 4, (i - 6) * (panelHeight + panelYOffset), -(panelWidth / 2));
        //    systemParent.transform.eulerAngles = new Vector3(0f, 60f, 0f);
        //}
    }

    public void create2DPanel(SolarSystem system, GameObject systemParent)
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

        // Solar System Text + Info
        systemText = new GameObject();
        systemText.name = system.star + " Text";
        systemText.transform.parent = panelParent.transform;
        systemText.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        systemText.transform.localPosition = new Vector3(-panelWidth / 2 + 1, panelHeight / 2 + 1.5f, 0);

        TextMesh starTextMesh = systemText.AddComponent<TextMesh>();
        starTextMesh.text = system.star + " (" + system.distance + " AU)"; // TODO: confirm if AU or light years?
        starTextMesh.fontSize = 150;

        // Discovery Method:
        GameObject discoveryText = new GameObject();
        discoveryText.name = system.star + " Discovery Text";
        discoveryText.transform.parent = panelParent.transform;
        discoveryText.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        discoveryText.transform.localPosition = new Vector3(3, panelHeight / 2 + 1.5f, 0);

        TextMesh discoveryTextMesh = discoveryText.AddComponent<TextMesh>();
        discoveryTextMesh.text = "Discovery: " + system.systems_planets[0].p_discovery;
        discoveryTextMesh.fontSize = 150;
    }

    public void create2DStar(SolarSystem system, GameObject systemParent)
    {
        GameObject starsParent;
        Material sideStarMaterial;

        starsParent = new GameObject();
        starsParent.name = "Stars";
        starsParent.transform.parent = systemParent.transform;

        int numStars = 1; // our data only has 1 star systems
        for (int i = 1; i <= numStars; i++)
        {
            // Star2D:
            GameObject sideStar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sideStar.name = system.star;

            sideStar.transform.parent = starsParent.transform;

            float starHeight = panelHeight / numStars;
            sideStar.transform.localPosition = new Vector3(-(panelWidth / 2), ((panelHeight / 2) + starHeight / 2) - (i * starHeight), 0);
            sideStar.transform.localScale = new Vector3(panelSunSize, starHeight, panelDepth);

            sideStarMaterial = new Material(Shader.Find("Unlit/Texture"));
            sideStarMaterial.mainTexture = Resources.Load(getStarColor(system)) as Texture;
            sideStar.GetComponent<MeshRenderer>().material = sideStarMaterial;
        }
    }

    string getStarColor(SolarSystem system)
    {
        float temperature = system.temperature;
        if (system.star == "Sun")
            return "sol";

        if (system.spectral[0] == 'o' || system.spectral[0] == 'O')
            return "ostar";
        else if (system.spectral[0] == 'b' || system.spectral[0] == 'B')
            return "bstar";
        else if (system.spectral[0] == 'a' || system.spectral[0] == 'A')
            return "astar";
        else if (system.spectral[0] == 'f' || system.spectral[0] == 'F')
            return "fstar";
        else if (system.spectral[0] == 'k' || system.spectral[0] == 'K')
            return "kstar";
        else if (system.spectral[0] == 'm' || system.spectral[0] == 'M')
            return "mstar";


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

    public void create2DPlanets(SolarSystem system, GameObject systemParent)
    {
        GameObject lineConvergingPoint = new GameObject();
        lineConvergingPoint.transform.parent = systemParent.transform;
        lineConvergingPoint.transform.localPosition = new Vector3(panelWidth / 2, 0, 0);

        GameObject planetsParent = new GameObject();
        planetsParent.name = "Planets";
        planetsParent.transform.parent = systemParent.transform;

        for (int i = 0; i < system.numPlanets; i++)
        {
            // Planet2D Boxes
            GameObject planetBox;
            Material borderMaterial;
            borderMaterial = new Material(Shader.Find("Unlit/Texture"));

            if (system.systems_planets[i].p_semiMajor >= system.habitable_Inner && system.systems_planets[i].p_semiMajor <= system.habitable_Outer)
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
            planetBox.name = system.star + " " + system.systems_planets[i].p_name + " Box";

            planetBox.transform.parent = systemParent.transform;

            float boxOffset = i * (planetBoxSize + planetBoxOffset);
            planetBox.transform.localPosition = new Vector3(-(panelWidth / 2) + panelSunSize * 2 + boxOffset, 0, 0);

            // Planet2D Text
            GameObject planetText;

            planetText = new GameObject();
            planetText.name = system.star + " " + system.systems_planets[i].p_name + " Text";
            planetText.transform.parent = systemParent.transform;
            planetText.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            planetText.transform.localPosition = new Vector3(-(panelWidth / 2) + panelSunSize * 2 + boxOffset - planetBoxSize / 2, -planetBoxSize / 2, 0);

            TextMesh planetTextMesh = planetText.AddComponent<TextMesh>();
            if (system.star == "Sun")
            {
                planetTextMesh.text = "" + (i + 1) + "." + system.systems_planets[i].p_name;
            }
            else
            {
                planetTextMesh.text = "" + (i + 1) + "." + system.star + " " + system.systems_planets[i].p_name;
            }
            
            planetTextMesh.fontSize = 60;

            // Grey Planes
            GameObject greyPlane = GameObject.CreatePrimitive(PrimitiveType.Cube);
            greyPlane.name = system.star + " " + system.systems_planets[i].p_name + " Grey";

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
            orbitPlanet.name = system.star + " " + system.systems_planets[i].p_name;
            orbitPlanet.tag = "Planet";

            Material orbitPlanetMat = new Material(Shader.Find("Unlit/Texture"));
            orbitPlanet.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);

            orbitPlanet.transform.parent = systemParent.transform;

            float leftOffset = -(panelWidth / 2) + panelSunSize / 2;
            float semiMajor = system.systems_planets[i].p_semiMajor * distanceScale;
            orbitPlanet.transform.localPosition = new Vector3(semiMajor + leftOffset, (panelHeight / 3), 0); 
            orbitPlanet.transform.localScale = new Vector3(1, 1, 1);

            // Text on Orbit Planets
            GameObject orbitPlanetText;

            orbitPlanetText = new GameObject();
            orbitPlanetText.name = system.star + " " + system.systems_planets[i].p_name + " Orbit Text";
            orbitPlanetText.transform.parent = systemParent.transform;
            orbitPlanetText.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            orbitPlanetText.transform.localPosition = new Vector3(semiMajor + leftOffset - 0.25f, (panelHeight / 3) + 1, 0);

            TextMesh orbitPlanetTextMesh = orbitPlanetText.AddComponent<TextMesh>();
            orbitPlanetTextMesh.text = "" + (i + 1);
            orbitPlanetTextMesh.fontSize = 80;



            if (semiMajor + leftOffset > (panelWidth / 2))
            {
                orbitPlanet.GetComponent<MeshRenderer>().enabled = false;
                greyPlane.GetComponent<MeshRenderer>().enabled = true;
                orbitPlanetText.GetComponent<MeshRenderer>().enabled = false;
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
            boxPlanet.name = system.star + " " + system.systems_planets[i].p_name;

            boxPlanet.transform.parent = planetsParent.transform;

            boxPlanet.transform.localPosition = new Vector3(-(panelWidth / 2) + panelSunSize * 2 + boxOffset, 0, 0);

            string planetTextureName = getPlanetTexture(system, system.systems_planets[i]);
            sidePlanetMaterial = new Material(Shader.Find("Standard"));
            sidePlanetMaterial.mainTexture = Resources.Load(planetTextureName) as Texture;
            boxPlanet.GetComponent<MeshRenderer>().material = sidePlanetMaterial;

            // Check if earth-like
            if (system.systems_planets[i].p_semiMajor >= system.habitable_Inner && system.systems_planets[i].p_semiMajor <= system.habitable_Outer && system.systems_planets[i].p_radius_Earth < 1.25f && system.systems_planets[i].p_radius_Earth > 0.75f && system.systems_planets[i].p_mass_Earth < 2)
            {
                sidePlanetMaterial.mainTexture = Resources.Load("earth") as Texture;
                boxPlanet.GetComponent<MeshRenderer>().material = sidePlanetMaterial;
            }


                // Scaled Radius
                float planetRadius = getPlanetRadius(system.systems_planets[i]) * sizeScale;
            if (planetRadius > planetBoxSize)
            {
                planetRadius = planetBoxSize;
                tooLargeIcon.GetComponent<MeshRenderer>().enabled = true;
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
        Debug.Log("Changing Distance Size: " + direction);
        if (direction == 1)
            sizeScale *= sizeChangeFactor;
        else
            sizeScale /= sizeChangeFactor;

        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");

        foreach (GameObject planet in planets)
        {
            //Debug.Log(planet.name);
            for (int i = 0; i < ss.Length; i++)
            {
                float newPlanetSize = 0;
                for (int j = 0; j < ss[i].numPlanets; j++)
                {
                    newPlanetSize = getPlanetRadius(ss[i].systems_planets[j]) * sizeScale;
                    string planetName = ss[i].star + " " + ss[i].systems_planets[j].p_name;

                    if (planet.name == planetName) //ss[i].systems_planets[j].p_name
                    {
                        if (newPlanetSize > planetBoxSize / 2)
                        {
                            newPlanetSize = planetBoxSize;

                            GameObject p = GameObject.Find("All 2D Views").transform.Find("2D View of " + ss[i].star).transform.Find("Planets").transform.Find(planet.name).gameObject;
                            //print(p.name);
                            p.transform.localScale = new Vector3(newPlanetSize, newPlanetSize, panelDepth * 5);

                            GameObject sizeIcon = GameObject.Find("All 2D Views").transform.Find("2D View of " + ss[i].star).transform.Find(planet.name + " Box").transform.Find("Size Icon").gameObject;
                            sizeIcon.GetComponent<MeshRenderer>().enabled = true;
                        }
                        else
                        {
                            GameObject p = GameObject.Find("All 2D Views").transform.Find("2D View of " + ss[i].star).transform.Find("Planets").transform.Find(planet.name).gameObject;
                            p.transform.localScale = new Vector3(newPlanetSize, newPlanetSize, panelDepth * 5);

                            GameObject sizeIcon = GameObject.Find("All 2D Views").transform.Find("2D View of " + ss[i].star).transform.Find(planet.name + " Box").transform.Find("Size Icon").gameObject;
                            sizeIcon.GetComponent<MeshRenderer>().enabled = false;
                        }
                    }

                }
            }
        }
    }

    public void changePlanetScale(int direction)
    {
        Debug.Log("Changing Distance " + direction);
        if (direction == 1)
            distanceScale *= distanceChangeFactor;
        else
            distanceScale /= distanceChangeFactor;

        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        //Debug.Log("here");
        foreach (GameObject planet in planets)
        {
            for (int i = 0; i < ss.Length; i++)
            {
                float leftOffset = -(panelWidth / 2) + panelSunSize / 2;
                float newSemiMajorAxis = 0;

                for (int j = 0; j < ss[i].numPlanets; j++)
                {
                    string planetName = ss[i].star + " " + ss[i].systems_planets[j].p_name;

                    if (planet.name == planetName)
                    {
                        //Debug.Log(ss[i].systems_planets[j].p_name);
                        newSemiMajorAxis = ss[i].systems_planets[j].p_semiMajor * distanceScale + leftOffset;

                        if (newSemiMajorAxis > panelWidth / 2)
                        {
                            planet.GetComponent<MeshRenderer>().enabled = false;
                            GameObject.Find("All 2D Views").transform.Find("2D View of " + ss[i].star).transform.Find(planet.name + " Grey").GetComponent<MeshRenderer>().enabled = true;

                            GameObject orbitText = GameObject.Find("All 2D Views").transform.Find("2D View of " + ss[i].star).transform.Find(planet.name + " Orbit Text").gameObject;
                            orbitText.GetComponent<MeshRenderer>().enabled = false;
                        }
                        else
                        {
                            planet.GetComponent<MeshRenderer>().enabled = true;
                            planet.transform.localPosition = new Vector3(newSemiMajorAxis, (panelHeight / 3), 0);
                            GameObject orbitText = GameObject.Find("All 2D Views").transform.Find("2D View of " + ss[i].star).transform.Find(planet.name + " Orbit Text").gameObject;
                            orbitText.transform.localPosition = new Vector3(newSemiMajorAxis - 0.25f, (panelHeight / 3) + 1, 0);
                            orbitText.GetComponent<MeshRenderer>().enabled = true;
                            GameObject.Find("All 2D Views").transform.Find("2D View of " + ss[i].star).transform.Find(planet.name + " Grey").GetComponent<MeshRenderer>().enabled = false;

                        }
                    }

                }
            }
        }

    }

    public float getPlanetRadius(Planet p)
    {
        float radius = 0;

        if (p.p_radius_Earth == -1)
        {
            
            float mass = p.p_mass_Earth;

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
            radius = p.p_radius_Earth;
        }


        return radius;
    }

    public string getPlanetTexture(SolarSystem ss, Planet p)
    {
        string textureName;

        if (ss.star == "Sun")
            return p.p_name.ToLower();

        if (p.p_temperature != -1)
        {
            if (p.p_temperature < 150)
                return "jupiter";
            else if (p.p_temperature >= 150 && p.p_temperature < 250)
                return "mercury";
            else if (p.p_temperature >= 250 && p.p_temperature < 800)
                return "neptune";
            else if (p.p_temperature >= 900 && p.p_temperature < 1400)
                return "fstar";
            else
                return "uranus";
        }
        else
        {
            if (p.p_semiMajor < 0.5)
                return "mercury";
            else if (p.p_semiMajor >= 0.5 && p.p_semiMajor < 1.5)
                return "venus";
            else if (p.p_semiMajor >= 1.5 && p.p_semiMajor < 5)
                return "mars";
            else if (p.p_semiMajor >= 5 && p.p_semiMajor < 9)
                return "jupiter";
            else if (p.p_semiMajor >= 9 && p.p_semiMajor < 15)
                return "saturn";
            else if (p.p_semiMajor >= 15 && p.p_semiMajor < 25)
                return "uranus";
            else if (p.p_semiMajor >= 25 && p.p_semiMajor < 35)
                return "neptune";
            else
                return "pluto";
        }
    }

    // ------------------------------------------------------------------------------
    // ----------------------------UNIVERSE VIEW-------------------------------------
    // ------------------------------------------------------------------------------

    public float universeScale = 10;
    public void createUniverseView()
    {
        GameObject universeParent = new GameObject();
        universeParent.name = "Universe";

        universeParent.transform.parent = this.gameObject.transform;

        for (int i = 0; i < ss.Length; i++)
        {
            GameObject systemSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            systemSphere.name = ss[i].star + " Universe";
            systemSphere.transform.parent = universeParent.transform;
            systemSphere.transform.localScale = new Vector3(2, 2, 2);
            systemSphere.transform.localPosition = new Vector3(ss[i].x / universeScale, ss[i].y / universeScale, ss[i].z / universeScale); // TODO: Dont skew scale

            //Material systemSphereMat = new Material(Shader.Find("Transparent/Diffuse"));
            Material systemSphereMat = new Material(Shader.Find("Standard"));
            systemSphere.GetComponent<MeshRenderer>().material = systemSphereMat;

            if (ss[i].star == "Sun")
            {
                systemSphere.transform.localScale = new Vector3(10, 10, 10);
                systemSphere.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0);

                Material earthMat = new Material(Shader.Find("Standard"));
                earthMat.mainTexture = Resources.Load("earth") as Texture;
                systemSphere.GetComponent<MeshRenderer>().material = earthMat;
            }
            else
            {
                systemSphere.GetComponent<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

                


            }
        }

        for (int i = 0; i < maxSystemsShown; i++)
        {
            string systemName = ss[i + curStartIdx].star + " Universe";

            if (ss[i].star != "Sun")
            {
                universeParent.transform.Find(systemName).localScale *= 2;
                //universeParent.transform.Find(systemName).GetComponent<MeshRenderer>().material.color = new Color(1, 0, 1);
                Material starMat = new Material(Shader.Find("Standard"));
                starMat.mainTexture = Resources.Load(getStarColor(ss[i])) as Texture;
                universeParent.transform.Find(systemName).GetComponent<MeshRenderer>().material = starMat;

                GameObject halo = Instantiate(HaloPrefab) as GameObject;
                halo.transform.SetParent(universeParent.transform.Find(systemName).transform, false);

                GameObject universeViewText;

                universeViewText = new GameObject();
                universeViewText.name = ss[i + curStartIdx].star + " Text";
                universeViewText.transform.parent = universeParent.transform;
                universeViewText.transform.localScale = new Vector3(1f, 1f, 1f);
                universeViewText.transform.eulerAngles = new Vector3(0, 180, 0);

                Vector3 systemVec = universeParent.transform.Find(systemName).transform.position;
                universeViewText.transform.localPosition = systemVec;

                TextMesh universeViewTextMesh = universeViewText.AddComponent<TextMesh>();
                universeViewTextMesh.text = ss[i + curStartIdx].star;
                universeViewTextMesh.fontSize = 80;

            }

        }

        universeParent.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        universeParent.transform.localPosition = new Vector3(0, 10, -26);
        //universeParent.transform.eulerAngles = new Vector3(45, -50, -20);
    }
}

