using UnityEngine;

using VRTK;

public class MenuToggleKeyboard : MonoBehaviour {

	public GameObject keyboard;

	bool keyboardState = false;

	public void toggleKeyBoard () {

        Debug.Log ("keyboard clicked");
		keyboardState = !keyboardState;
        keyboard.SetActive(keyboardState);
    }
}
