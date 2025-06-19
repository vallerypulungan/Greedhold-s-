using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    public Transform target;

    Vector2 lastClickPos;

    bool moving;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            moving = true;
        }

        if (moving && (Vector2)transform.position != lastClickPos)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, lastClickPos, step);
        }
        else
        {
            moving = false;
        }
    }


}
