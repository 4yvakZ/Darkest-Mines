using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private TMP_Text hpText;

    // Start is called before the first frame update
    void Start()
    {
        Health = Random.Range(20, 40);
        MinDamage = 8;
        MaxDamage = 12;
        hpText.text = "Enemy " + Health + " HP";
    }

    private void OnMouseEnter()
    {
        Highlight();
    }

    private void OnMouseExit()
    {
        Dehighlight();
    }

    public override void Damaged(int damage)
    {
        base.Damaged(damage);
        if (!isActiveAndEnabled)
        {
            Health = Random.Range(20, 40);
            Dehighlight();
        }
        hpText.text = "Enemy " + Health + " HP";
    }

    public override void Attack(Character target)
    {
        base.Attack(target);
    }
}
