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

    //bool keyboardState = false;

    public void p_size_buttonUp () {

        Debug.Log ("p_size_buttonUp");

    }

    public void p_size_buttonDown () {

        Debug.Log ("p_size_buttonDown");

    }

    public void orb_dist_buttonUp () {

        Debug.Log ("orb_dist_buttonUp");

    }

    public void orb_dist_buttonDown () {

        Debug.Log ("orb_dist_buttonDown");

    }

    public void orb_period_buttonUp () {

        Debug.Log ("orb_period_buttonUp");

    }

    public void orb_period_buttonDown () {

        Debug.Log ("orb_period_buttonDown");

    }

    public void rot_period_buttonUp () {

        Debug.Log ("rot_period_buttonUp");

    }

    public void rot_period_buttonDown () {

        Debug.Log ("rot_period_buttonDown");

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

        keyboard.SetActive (value);

        Debug.Log (keyboardState);
    }

    public void searchStringUpdate (string value) {

        this.searchString = value;

        //keyboard.SetActive(value);

        //Debug.Log("pressed enter");
        Debug.Log (searchString);
    }

}