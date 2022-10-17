using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCamTarget : MonoBehaviour
{
    [SerializeField] private float sensitivity = 0.15f;
    private Vector3 targetPosition = Vector2.zero;
    private Rect screenRect = Rect.zero;
    [Space(8)]
    [SerializeField] private float movementOffset = 10f;
    [SerializeField] float offsetMoveSpeed = 25f;
    private Vector3 cameraOffset = Vector2.zero;
    private float targetOffsetX = 0f;
    private float targetOffsetY = 0f; 

    private PGunMovement playerController = null;
    public Transform camTarget; 

    private void Start()
    {
        playerController = FindObjectOfType<PGunMovement>();
    }

    private void Update()
    {
        screenRect = new Rect(0f, 0f, Screen.width, Screen.height);
        cameraOffset.x = Mathf.MoveTowards(cameraOffset.x, 0f, offsetMoveSpeed * Time.fixedDeltaTime);
        cameraOffset.y = Mathf.MoveTowards(cameraOffset.y, 0f, offsetMoveSpeed * Time.fixedDeltaTime); 

        if (screenRect.Contains(Input.mousePosition))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + cameraOffset;
            targetPosition = new Vector3(targetPosition.x, targetPosition.y, -10); 
        }

            
        transform.position = Vector3.Lerp(camTarget.position, targetPosition, sensitivity);
    }
}
