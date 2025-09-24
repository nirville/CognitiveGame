using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TrailManager trailManager;
    public Button submitButton;
    public Button clearLastButton;
    public Button clearAllButton;

    void Start()
    {
        submitButton.onClick.AddListener(() => trailManager.Submit());
        clearLastButton.onClick.AddListener(() => trailManager.ClearLast());
        clearAllButton.onClick.AddListener(() => trailManager.ClearAll());
    }
}