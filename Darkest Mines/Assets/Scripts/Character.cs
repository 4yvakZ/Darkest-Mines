//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using TMPro;

public class Character : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;

    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private GameObject highlighter;
    private GameObject Highlighter { get => highlighter; }

    public int Health { get;protected set; }
    public int MinDamage { get; protected set; }
    public int MaxDamage { get; protected set; }
    public bool IsDone { get; protected set; }

    public Character Target { get; private set; }

    [SerializeField] private SkeletonAnimation characterSkeletonAnimn;
    protected SkeletonAnimation CharacterSkeletonAnim { get => characterSkeletonAnimn; private set => characterSkeletonAnimn = value; }


    protected Skin IdleSkin { get; set; }
    protected Skin DamageSkin { get; set; }

    private string attackAnim = "PickaxeCharge";
    protected string AttackAnim { get => attackAnim; set => attackAnim = value; }

    public virtual void Attack(Character target)
    {
        IsDone = false;
        Target = target;
        TrackEntry entry = CharacterSkeletonAnim.AnimationState.SetAnimation(0, AttackAnim, false);
        entry.Complete += Idle;
        entry.Event += Hit;
    }

    protected virtual void Hit(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "Hit")
        {
            int damage = UnityEngine.Random.Range(MinDamage, MaxDamage);
            Target.Damaged(damage);
        }
    }

    private void Idle(TrackEntry trackEntry)
    {
        CharacterSkeletonAnim.AnimationState.SetAnimation(0, "Idle", true);
        damageText.gameObject.SetActive(false);
        IsDone = true;
    }

    public virtual void Damaged(int damage)
    {
        IsDone = false;
        damageText.text = (-damage).ToString();
        damageText.transform.localPosition = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
        damageText.gameObject.SetActive(true);

        TrackEntry entry = CharacterSkeletonAnim.AnimationState.SetAnimation(0, "Damage", false);
        entry.Complete += Idle;
        Health -= damage;
        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void Highlight()
    {
        Highlighter.SetActive(true);
    }

    public void Dehighlight()
    {
        Highlighter.SetActive(false);
    }

    public void MoveToForeground()
    {
        meshRenderer.sortingLayerName = "Foreground";
    }

    public void MoveToBack()
    {
        meshRenderer.sortingLayerName = "Default";
    }
}
