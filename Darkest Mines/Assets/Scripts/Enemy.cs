using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    
    // Start is called before the first frame update
    void Start()
    {
        Highlighter = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        Highlighter.SetActive(true);
    }

    private void OnMouseExit()
    {
        Highlighter.SetActive(false);
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void Damaged()
    {
        throw new System.NotImplementedException();
    }
}
