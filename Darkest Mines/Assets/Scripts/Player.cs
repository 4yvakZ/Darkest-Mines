using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private TMP_Text hpText;

    // Start is called before the first frame update
    void Awake()
    {
        Highlighter = transform.GetChild(1).gameObject;
        Health = Random.Range(20, 50);
        MinDamage = 10;
        MaxDamage = 15;
        hpText.text = "Miner " + Health + " HP";
    }

    // Update is called once per frame
    void Update()
    {
        /*if (isAttacking && Input.GetMouseButtonDown(0))
        {       
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Enemy target = hit.collider.gameObject.GetComponent<Enemy>();
                target.Damaged(UnityEngine.Random.Range(MinDamage, MaxDamage));
                Debug.Log(hit.transform.name);
                Debug.Log("hit");
            }
        }*/
    }

    public override void Attack(Character target)
    {
        base.Attack(target);
        Highlighter.SetActive(false);
    }

    public override void Damaged(int damage)
    {
        base.Damaged(damage);
        hpText.text = "Miner " + Health + " HP";
    }

    public override void Hightlight()
    {
        Highlighter.SetActive(true);
    }

    public void Skip()
    {
        Highlighter.SetActive(false);
    }
}
