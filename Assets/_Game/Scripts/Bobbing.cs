using UnityEngine;

public class Bobbing : MonoBehaviour
{
    public float amplitude = 0.25f;  // how high it bobs
    public float frequency = 2f;     // how fast it bobs

    private Vector3 startPos;

    void Awake()
    {
        enabled = false;
    }

    void Start()
    {
        enabled = true;
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}