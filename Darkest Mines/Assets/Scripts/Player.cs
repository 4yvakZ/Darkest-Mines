using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private Character target;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        Highlighter = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking && Input.GetMouseButtonDown(0))
        {       
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                Debug.Log(hit.transform.name);
                Debug.Log("hit");
            }
        }
    }

    public override void Attack()
    {
        isAttacking = true;
        Highlighter.SetActive(true);
    }

    public override void Damaged()
    {
        throw new NotImplementedException();
    }
}
