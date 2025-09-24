using System.Collections;
using UnityEngine;

public class ConnectionLine : MonoBehaviour
{
    private TrailRenderer trail;
    // public float yDelta;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    // Call this whenever you click another node
    public void MoveTo(Vector3 target)
    {
        transform.position = new Vector3(target.x, transform.position.y, target.z);
        /*StopAllCoroutines();
        StartCoroutine(MoveTrail(target));*/
    }

    private IEnumerator MoveTrail(Vector3 target)
    {
        Vector3 start = transform.position;
        float duration = 0.3f; // how fast to animate
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Move towards target
            transform.position = Vector3.Lerp(start, target, t);

            yield return null;
        }

        // Snap to final target
        transform.position = target;
    }
    
    /*private LineRenderer lineRenderer;

    public void Init(Vector3 start, Vector3 end)
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }*/
}