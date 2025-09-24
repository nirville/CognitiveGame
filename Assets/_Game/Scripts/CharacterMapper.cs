using System.Collections.Generic;
using UnityEngine;

public class CharacterMapper : MonoBehaviour
{
    [Header("Letter Prefabs (A–K)")]
    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject D;
    public GameObject E;
    public GameObject F;
    public GameObject G;
    public GameObject H;
    public GameObject I;
    public GameObject J;
    public GameObject K;

    [Header("Number Prefabs (0–9)")]
    public GameObject num0;
    public GameObject num1;
    public GameObject num2;
    public GameObject num3;
    public GameObject num4;
    public GameObject num5;
    public GameObject num6;
    public GameObject num7;
    public GameObject num8;
    public GameObject num9;

    private Dictionary<string, GameObject> prefabMap;

    private void Awake()
    {
        prefabMap = new Dictionary<string, GameObject>
        {
            {"A", A}, {"B", B}, {"C", C}, {"D", D}, {"E", E},
            {"F", F}, {"G", G}, {"H", H}, {"I", I}, {"J", J}, {"K", K},
            {"0", num0}, {"1", num1}, {"2", num2}, {"3", num3}, {"4", num4},
            {"5", num5}, {"6", num6}, {"7", num7}, {"8", num8}, {"9", num9}
        };
    }

    public GameObject SpawnNode(string label, Vector3 position, Transform parent)
    {
        if (!prefabMap.ContainsKey(label))
        {
            Debug.LogError($"No prefab mapped for label {label}");
            return null;
        }

        GameObject prefab = prefabMap[label];
        GameObject nodeObj = Instantiate(prefab, position, Quaternion.identity, parent);

        return nodeObj;
    }
}