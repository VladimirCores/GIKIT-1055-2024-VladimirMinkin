using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hero : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 _targetPosition = Vector3.zero;
    public float speedMultiplier = 0.3f;
    public int health = 10;
    public GameObject prefBullet;
    private LineRenderer _lineRenderer;

    private int _maxHealth;

    public UnityEvent eventHitted = new UnityEvent();

    void Start()
    {
        _maxHealth = health;
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        GetAllowedMousePosinionWhenPossible(mousePosition);

        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Space pressed");
            FireBulletAtMouseCursorPosition();
        }

        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, mousePosition);

        if (Vector3.Distance(transform.position, _targetPosition) > 0.01) {
            transform.position = Vector3.Lerp(
                transform.position, 
                _targetPosition, 
                Time.deltaTime * speedMultiplier
            );
        }
    }


    void GetAllowedMousePosinionWhenPossible(Vector3 mousePosition) {
        if (Input.GetMouseButtonUp(0)) {
            _targetPosition = mousePosition;
        }
    }

    void FireBulletAtMouseCursorPosition() {
        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y + 1);
        Vector2 direction = targetPosition - currentPosition;
        direction.Normalize();
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        GameObject bullet = Instantiate(prefBullet, this.transform.position, rotation);
        bullet.GetComponent<Bullet>().SetVelocity(direction);
    }

     void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("> Hero -> OnTriggerEnter2D: " + collider.gameObject.name);
        if (collider.gameObject.name.Contains("pBot")) {
            health--;
        }
        else if (collider.gameObject.name.Contains("pHealth") && health < _maxHealth) {
            Destroy(collider.gameObject);
            health++;
        }
        eventHitted.Invoke();
    }
}
