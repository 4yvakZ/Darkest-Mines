using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    private float length;
    private Vector2 startPos;
    [SerializeField] private float paralaxMultipier = 1f;
    [SerializeField] private float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        length = transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < startPos.x - length || transform.position.x > startPos.x + length)
        {
            transform.position = startPos;
        }
        float horizontalSpeed = Input.GetAxis("Horizontal") * speed;
        transform.Translate(Vector2.left * horizontalSpeed * paralaxMultipier * Time.deltaTime);
    }
}
