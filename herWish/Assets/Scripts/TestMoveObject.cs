using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveObject : MonoBehaviour
{
    [Header("Platform Variables")]
    [SerializeField] Vector3 direction = new Vector3(0, 1, 0);
    [SerializeField] Vector3 startOffset = Vector3.zero;
    [SerializeField] float distance = 5.0f;
    [SerializeField] float speed;
    Vector3 move;
    Vector2 startPosition;
    bool startingDirection = true;

    [Header("Player")]
    [SerializeField] LayerMask playerLayer;

    void Start()
    {
        if (startOffset != Vector3.zero)
        {
            startPosition = transform.position + startOffset;
        }
        else
        {
            startPosition = transform.position;
        }
    }

    void Update()
    {
        move = direction * speed * Time.deltaTime;

        if (startingDirection && Vector2.Distance(startPosition, transform.position) >= distance)
            startingDirection = false;
        else if (Vector2.Distance(startPosition, transform.position) <= 0.2f)
            startingDirection = true;

        PlatformMovement();
    }

    public void PlatformMovement()
    {
        if (startingDirection)
            transform.position += move;
        else
            transform.position -= move;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.transform.parent.SetParent(transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent.SetParent(null);
            DontDestroyOnLoad(collision.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 endLocation;
        if (Application.isPlaying)
        {
            endLocation = (Vector3)startPosition + (direction * distance);
            Gizmos.DrawLine((Vector3)startPosition, endLocation);
        }
        else
        {
            if (startOffset != Vector3.zero)
            {
                endLocation = (transform.position) + (direction * distance);
                Gizmos.DrawLine((transform.position + startOffset), (endLocation + (Vector2)startOffset));
            }
            else
            {
                endLocation = transform.position + (direction * distance);
                Gizmos.DrawLine(transform.position, endLocation);
            }
        }

    }
}
