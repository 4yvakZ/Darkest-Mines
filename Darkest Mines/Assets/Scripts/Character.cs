using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{

    protected GameObject Highlighter { get; set; }
    public int Helth { get;protected set; }
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

    public abstract void Attack();
    public abstract void Damaged();
}
