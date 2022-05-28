using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public bool IsDone { get; protected set; }
    protected GameObject Highlighter { get; set; }
    public int Health { get;protected set; }
    public int MinDamage { get; protected set; }
    public int MaxDamage { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Attack(Character target)
    {
        int damage = UnityEngine.Random.Range(MinDamage, MaxDamage);
        target.Damaged(damage);
        Debug.Log(gameObject.name + " " + damage + "->" + target.name + " " + target.Health);
    }

    public virtual void Damaged(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void Hightlight()
    {

    }
}
