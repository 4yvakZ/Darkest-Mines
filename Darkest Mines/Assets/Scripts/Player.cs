using Spine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private TMP_Text hpText;

    private int kills = 0;

    // Start is called before the first frame update
    void Start()
    {

        Health = Random.Range(20, 50);
        MinDamage = 10;
        MaxDamage = 15;
        hpText.text = "Miner " + Health + " HP";
    }

    public override void Attack(Character target)
    {
        base.Attack(target);
        if (!target.isActiveAndEnabled)
        {
            kills++;
            if (kills == 1)
            {
                Evolve();
            }
        }
    }

    protected override void Hit(TrackEntry trackEntry, Spine.Event e)
    {
        base.Hit(trackEntry, e);
        if (Target.Health <= 0)
        {
            kills++;
            if (kills == 1)
            {
                Evolve();
            }
        }
    }

    public override void Damaged(int damage)
    {
        base.Damaged(damage);

        hpText.text = "Miner " + Health + " HP";
    }

    /// <summary>
    /// Повышает персонажа до элитного при убистве врага.
    /// Меняет скин, лечит и меняет анимацию и силу атаки.
    /// </summary>
    private void Evolve()
    {
        Debug.Log(gameObject.name + " evolves!!!");
        SkeletonData skeletonData = CharacterSkeletonAnim.Skeleton.Data;
        IdleSkin = skeletonData.FindSkin("elite");
        CharacterSkeletonAnim.Skeleton.SetSkin(IdleSkin);

        AttackAnim = "DoubleShift";
        Health += Random.Range(20, 30);
        MinDamage = 10;
        MaxDamage = 20;
        hpText.text = "Miner " + Health + " HP";
    }
}
