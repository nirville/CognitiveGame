using UnityEngine;

public class Node : MonoBehaviour
{
    public string label;
    private TrailManager trailManager;
    public Transform nodeLocation;

    public void Init(string label, Vector3 position, TrailManager manager)
    {
        this.label = label;
        this.trailManager = manager;
        transform.localPosition = position;
        //GetComponentInChildren<Text>().text = label;
    }
   
    void OnMouseDown()
    {
        if(gameObject.CompareTag("Node"))
            trailManager.OnNodeClicked(this);
    }
}