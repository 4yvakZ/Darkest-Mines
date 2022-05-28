using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private TMP_Text hpText;
    // Start is called before the first frame update
    void Awake()
    {
        Highlighter = transform.GetChild(1).gameObject;
        Health = Random.Range(20, 40);
        MinDamage = 5;
        MaxDamage = 10;
        hpText.text = "Enemy " + Health + " HP";
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

    public override void Damaged(int damage)
    {
        base.Damaged(damage);
        if (!isActiveAndEnabled)
        {
            Health = Random.Range(20, 40);
            Highlighter.SetActive(false);
        }
        hpText.text = "Enemy " + Health + " HP";
    }
}
