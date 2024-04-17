using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour {
    public PluginInterface pluginInterface;
    public Button[] buttons;

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
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnButtonClick(int id){		
		bool windowOn = pluginInterface.ToggleShowWindow(id);
        buttons[id].GetComponentInChildren<Text>().text = windowOn ? closeWindowPlugin : showWindowPlugin;
	}    
}
