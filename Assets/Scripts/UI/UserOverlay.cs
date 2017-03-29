using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UserOverlay : MonoBehaviour {

    private GameObject actionMenuPanel;
    private GameObject actionMenuButton;

    // Create button for menu
    private GameObject CreateMenuButton(Transform panel, Vector3 position, Vector2 size, UnityAction call)
    {
        GameObject button = new GameObject();

        button.transform.parent = panel;
        button.AddComponent<RectTransform>();
        button.AddComponent<Button>();
        button.AddComponent<Image>();
        button.transform.position = position;
        button.GetComponent<RectTransform>().sizeDelta = size;
        button.GetComponent<Button>().onClick.AddListener(call);

        return button;
    }

    // Create panel for action menu
    private void CreateActionMenuPanel(UnityAction call)
    {
        Transform panel = actionMenuPanel.GetComponent<Transform>();
        Vector3 position = panel.position;
        Vector2 size = new Vector2(100, 100);

        ClearSelection();
        actionMenuButton = CreateMenuButton(panel, position, size, call);
    }

    // Clear UI for selection
    public void ClearSelection()
    {
        Destroy(actionMenuButton);
    }

    // Render UI depending on object selected by player
    public void RenderSelection(GameObject selected, UnityAction call)
    {
        CreateActionMenuPanel(call);
    }

    void Start()
    {
        actionMenuPanel = GameObject.FindGameObjectWithTag("ActionMenuPanel");
    }
}
