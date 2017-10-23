using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// will attach official script to parent container of 2d views
// when slider value changes, get a list of all children (systems) and set scales (sun/planets)
// container has vector4 (x, y, z, y rotation)
// use other c# code from unity manual
// attach light source to head?

public class MenuEvents : MonoBehaviour {

    public Dropdown dropdown;
    public string search_category;
    public string searchString;

    public bool keyboardState;
    public GameObject keyboard;

    Generate2DView generateScript;
    JsonParse parseScript;

    void Start() {
        
        Debug.Log("start here");
        generateScript = this.GetComponent<Generate2DView>();
        parseScript = this.GetComponent<JsonParse>();

    }


    //bool keyboardState = false;

    public void p_size_buttonUp () {

        Debug.Log ("p_size_buttonUp");

        generateScript.changePlanetSize(1);
        parseScript.scalePlanetsUp();

    }

    public void p_size_buttonDown () {

        Debug.Log ("p_size_buttonDown");

        generateScript.changePlanetSize(-1);
        parseScript.scalePlanetsDown();

    }

    public void orb_dist_buttonUp () {

        Debug.Log ("orb_dist_buttonUp");

        //generateScript.changePlanetScale(1);

        generateScript.changePlanetScale(1);
        parseScript.scaleOrbitDistanceUp();

    }

    public void orb_dist_buttonDown () {

        Debug.Log ("orb_dist_buttonDown");
    
        generateScript.changePlanetScale(-1);
        parseScript.scaleOrbitDistanceDown();


    }

    public void orb_period_buttonUp () {

        Debug.Log ("orb_period_buttonUp");

        parseScript.scaleOrbitPeriodUp();



    }

    public void orb_period_buttonDown () {

        Debug.Log ("orb_period_buttonDown");

        parseScript.scaleOrbitPeriodDown();

    }

    public void rot_period_buttonUp () {

        Debug.Log ("rot_period_buttonUp");

        parseScript.scaleRotationPeriodUp();

    }

    public void rot_period_buttonDown () {

        Debug.Log ("rot_period_buttonDown");

        parseScript.scaleRotationPeriodDown();

    }




    public void buttonNearestEarth () {

        Debug.Log ("buttonNearestEarth clicked");

        parseScript.resetView();
    }

    public void buttonMostPlanets () {

        Debug.Log ("buttonMostPlanets clicked");

        parseScript.resetView();
    }

    public void buttonHottestStars () {

        Debug.Log ("buttonHottestStars clicked");

        parseScript.resetView();
    }

    public void buttonMostHabitablePlanets () {

        Debug.Log ("buttonMostHabitablePlanets clicked");

        parseScript.resetView();
    }

    public void dropdownCategoryUpdate () {

        int menuIndex = dropdown.value;

        List<Dropdown.OptionData> menuOptions = dropdown.options;

        this.search_category = menuOptions[menuIndex].text;
        Debug.Log (search_category);
    }

    public void toggleKeyboard (bool value) {

        this.keyboardState = value;

        keyboard.SetActive (value);

        Debug.Log (keyboardState);
    }

    public void searchStringUpdate (string value) {

        this.searchString = value;

        //keyboard.SetActive(value);

        //Debug.Log("pressed enter");
        Debug.Log (searchString);

        generateScript.searchSystems(search_category, searchString);

    }

}