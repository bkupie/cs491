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

    public float scale_planetSize;
    public float scale_orbitDistance;
    public float scale_orbitalPeriod;
    public float scale_rotationPeriod;

    public Dropdown dropdown;
    public string search_category;
    public string searchString;

    public bool keyboardState;
    public GameObject keyboard;

    //bool keyboardState = false;

    public void planetSizeUpdate (float value) {

        this.scale_planetSize = value;
        Debug.Log (scale_planetSize);
    }

    public void orbitDistanceUpdate (float value) {

        this.scale_orbitDistance = value;
        Debug.Log (scale_orbitDistance);
    }

    public void orbitalPeriodUpdate (float value) {

        this.scale_orbitalPeriod = value;
        Debug.Log (scale_orbitalPeriod);
    }

    public void rotationPeriodUpdate (float value) {

        this.scale_rotationPeriod = value;
        Debug.Log (scale_rotationPeriod);
    }

    public void buttonNearestEarth () {

        Debug.Log ("buttonNearestEarth clicked");
    }

    public void buttonMostPlanets () {

        Debug.Log ("buttonMostPlanets clicked");
    }

    public void buttonHottestStars () {

        Debug.Log ("buttonHottestStars clicked");
    }

    public void buttonMostHabitablePlanets () {

        Debug.Log ("buttonMostHabitablePlanets clicked");
    }

    public void dropdownCategoryUpdate () {

        int menuIndex = dropdown.value;

        List<Dropdown.OptionData> menuOptions = dropdown.options;

        this.search_category = menuOptions[menuIndex].text;
        Debug.Log (search_category);
    }

    public void toggleKeyboard (bool value) {

        this.keyboardState = value;

        keyboard.SetActive(value);


        Debug.Log (keyboardState);
    }

    public void searchStringUpdate (string value) {

        this.searchString = value;

        //keyboard.SetActive(value);

        //Debug.Log("pressed enter");
        Debug.Log (searchString);
    }


}