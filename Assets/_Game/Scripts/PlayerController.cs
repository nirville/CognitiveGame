using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public float isJumping;
    private float height = 5f;
    public float heightFactor = .45f;
    public float duration = 2f;
    private float time;
    
    void Start()
    {
    }

    public void TriggerJump(Vector3 targetPoint)
    {
        animator.SetTrigger("Jump");
        StartCoroutine(ParabolaMove(transform.position, targetPoint, height, duration));
        
        float distance = Vector3.Distance(transform.position, targetPoint);
        height = distance * heightFactor;
        Vector3 direction = targetPoint - transform.position;
        direction.y = 0; 

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    private IEnumerator ParabolaMove(Vector3 start, Vector3 end, float height, float duration)
    {
        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime / duration;

            Vector3 horizontal = Vector3.Lerp(start, end, time);

            float parabola = 4 * height * time * (1 - time);

            transform.position = new Vector3(horizontal.x, horizontal.y + parabola, horizontal.z);

            yield return null;
        }

        transform.position = end;
    }
}
