using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour {
    [SerializeField] ExternalWindowController _externalWindowController;
    [SerializeField] Button[] buttons;

    public static readonly string showWindowPlugin = "Show Window";
	static readonly string closeWindowPlugin = "Close Window";

	// Use this for initialization
	void Start () {
        int i = 0;
        foreach (Button button in buttons) {
            button.gameObject.SetActive(true);
            int temp = i;
            button.onClick.AddListener(() => OnButtonClick(temp));
            button.GetComponentInChildren<Text>().text = showWindowPlugin;
            i += 1;
        }
	}

	public void OnButtonClick(int id){
        _externalWindowController.ToggleShowWindow(id);
        //buttons[id].GetComponentInChildren<Text>().text = windowOn ? closeWindowPlugin : showWindowPlugin;
	}    
}
