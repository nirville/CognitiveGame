using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrailManager : MonoBehaviour
{
    public GameObject nodePrefab;
    public ConnectionLine line;
    public Transform boardParent;
    public APIClient apiClient;
    public PlayerController player;
    public CharacterMapper map;
    public TMP_Text text;
    
    public float nodeScalingFactor = 20f;

    private List<Node> nodes = new List<Node>();
    private List<string> connections = new List<string>();
    private List<string[]> playerConnections = new List<string[]>();

    private Node lastNode;

    void Start()
    {
        StartCoroutine(apiClient.StartGame(OnStartResponse));
    }
    void OnStartResponse(TaskData response)
    {
	    var task = response.task;
     
        foreach (var kvp in task)
        {
	        string label = kvp.Key;

	        var coords = kvp.Value;
	        if (coords == null)
	        {
		        Debug.LogError($"Value for {label} was not a dictionary.");
		        continue;
	        }

	        float x = ParseNumber(coords.x);
	        float y = ParseNumber(coords.y);

	        GameObject nodeObj = Instantiate(nodePrefab, boardParent);
	        Node node = nodeObj.GetComponent<Node>();
	        node.Init(label, new Vector3(x, 3.65f, y) / nodeScalingFactor, this);
	        
	        GameObject labelObj = map.SpawnNode(node.label, node.nodeLocation.position + Vector3.up * 0.5f, node.nodeLocation);
	        labelObj.transform.rotation = node.nodeLocation.rotation;
	        
	        nodes.Add(node);
	        
        }
    }
    
    
    float ParseNumber(object val)
    {
        if (val is long) return (float)(long)val;
        if (val is int) return (float)(int)val;
        if (val is double) return (float)(double)val;
        return 0f;
    }

    public void OnNodeClicked(Node node)
    {
        if (lastNode == null)
        {
            lastNode = node;
            //return;
        }
        
        line.MoveTo(node.transform.position);
        connections.Add(node.label);

        playerConnections.Add(new string[] { lastNode.label, node.label });
        player.TriggerJump(node.nodeLocation.position);
      
        lastNode = node;
    }

    public void Submit()
    {
        SubmitData req = new SubmitData {
            connections = new List<List<string>>()
        };

        foreach (var conn in playerConnections)
        {
            req.connections.Add(new List<string>(conn));
        }

        StartCoroutine(apiClient.Submit(req, OnSubmitResponse));
    }

    void OnSubmitResponse(TaskData response)
    {
        if (response.success)
        {
            Debug.Log("Submitted trail");
            text.transform.gameObject.SetActive(true);
            playerConnections.Clear();
            lastNode = null;
        }
        else
        {
            Debug.Log("Incorrect sequence, try again!");
        }
    }
    
    // Call this to restart the current level
    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void ClearLast()
    {
        StartCoroutine(apiClient.ClearLast(OnClearResponse));
        if (connections.Count > 0)
        {
            //Destroy(connections[connections.Count - 1].gameObject);
            connections.RemoveAt(connections.Count - 1);
            playerConnections.RemoveAt(playerConnections.Count - 1);
        }
    }

    public void ClearAll()
    {
        StartCoroutine(apiClient.ClearAll(OnClearResponse));
        foreach (var conn in connections)
        {
            //Destroy(conn.gameObject);
        }
        connections.Clear();
        playerConnections.Clear();
        lastNode = null;
    }

    void OnClearResponse(TaskData response)
    {
        // Response contains updated connections list
    }
}