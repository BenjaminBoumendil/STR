using UnityEngine;
using UnityEngine.UI;

public class UserOverlay : MonoBehaviour {

    private GameObject actionMenuPanel;

    // Create button for menu
    private GameObject CreateMenuButton()
    {
        GameObject button = new GameObject();

        Transform panel = actionMenuPanel.GetComponent<Transform>();
        button.transform.parent = panel;
        button.AddComponent<RectTransform>();
        button.AddComponent<Button>();
        //button.GetComponent<Button>().onClick.AddListener();

        return button;
    }

    // Create panel for action menu
    private void CreateActionMenuPanel()
    {
        CreateMenuButton();
    }

    // Render UI depending on object selected by player
    public void RenderSelection(GameObject selected)
    {
        CreateActionMenuPanel();
    }

    void Start()
    {
        actionMenuPanel = GameObject.FindGameObjectWithTag("ActionMenuPanel");
    }
}
