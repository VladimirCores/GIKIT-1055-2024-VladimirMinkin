using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Bot : MonoBehaviour
{
    float minDistance;
    Vector3 positionTarget;

    public UnityEvent eventDestroyed = new UnityEvent();

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        GenerateRandomTargetPosition();
        eventDestroyed.AddListener(OnEventDestroy);
    }

    void OnEventDestroy()
    {
        Debug.Log("> Bot -> OnEventDestroy");
    }

    // Update is called once per frame
    [System.Obsolete]
    void FixedUpdate()
    {
        if (ShouldChangeTargetPosition())
        {
            GenerateRandomTargetPosition();
        }

        transform.position = Vector3.Lerp(transform.position, positionTarget, Time.fixedDeltaTime);
    }

    [System.Obsolete]
    void GenerateRandomTargetPosition()
    {
        positionTarget = Main.getRandomPositionFromCameraView();
        minDistance = 0.1f * Vector3.Distance(transform.position, positionTarget);
    }

    bool ShouldChangeTargetPosition()
    {
        return Vector3.Distance(transform.position, positionTarget) < minDistance;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("> Bot -> OnTriggerEnter2D: " + collider.gameObject.name);
        Destroy(gameObject);
        eventDestroyed.Invoke();
        eventDestroyed.RemoveAllListeners();
    }
}
