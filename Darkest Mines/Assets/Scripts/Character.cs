using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using TMPro;

public class Character : MonoBehaviour
{
    public int Health { get; protected set; }
    public int MinDamage { get; protected set; }
    public int MaxDamage { get; protected set; }
    /// <summary>
    /// Переменная конца действий(анимации) персонажа.
    /// </summary>
    public bool IsDone { get; protected set; }
    public Character Target { get; private set; }

    [SerializeField] private TMP_Text damageText;

    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private GameObject highlighter;
    private GameObject Highlighter { get => highlighter; }


    [SerializeField] private SkeletonAnimation characterSkeletonAnimn;
    protected SkeletonAnimation CharacterSkeletonAnim { get => characterSkeletonAnimn; private set => characterSkeletonAnimn = value; }
    protected Skin IdleSkin { get; set; }
    protected Skin DamageSkin { get; set; }

    private string attackAnim = "PickaxeCharge";
    protected string AttackAnim { get => attackAnim; set => attackAnim = value; }

    /// <summary>
    /// Запускает анимацию атаки
    /// </summary>
    /// <param name="target">Цель атаки</param>
    public virtual void Attack(Character target)
    {
        IsDone = false;
        Target = target;
        TrackEntry entry = CharacterSkeletonAnim.AnimationState.SetAnimation(0, AttackAnim, false);
        entry.Complete += Idle;
        entry.Event += Hit;
    }

    /// <summary>
    /// Наносит урон цели при соответствующем событии анимации
    /// </summary>
    /// <param name="trackEntry">Трэк анимации</param>
    /// <param name="e">Событие анимации</param>
    protected virtual void Hit(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "Hit")
        {
            int damage = Random.Range(MinDamage, MaxDamage);
            Target.Damaged(damage);
        }
    }

    /// <summary>
    /// Запускает циклическую анимацию ожидания, при окончании других анимаций
    /// </summary>
    /// <param name="trackEntry">трэк анимации</param>
    private void Idle(TrackEntry trackEntry)
    {
        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
        CharacterSkeletonAnim.AnimationState.SetAnimation(0, "Idle", true);
        damageText.gameObject.SetActive(false);
        IsDone = true;
    }

    /// <summary>
    /// Получение урона и вызов соответствующей анимации анимации.
    /// </summary>
    /// <param name="damage">Полученный урон</param>
    public virtual void Damaged(int damage)
    {
        IsDone = false;
        damageText.text = (-damage).ToString();
        damageText.transform.localPosition = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
        damageText.gameObject.SetActive(true);

        TrackEntry entry = CharacterSkeletonAnim.AnimationState.SetAnimation(0, "Damage", false);
        entry.Complete += Idle;
        Health -= damage;
    }

    /// <summary>
    /// Выделяет персонажа.
    /// </summary>
    public void Highlight()
    {
        Highlighter.SetActive(true);
    }

    /// <summary>
    /// Убирает выделение персонажа.
    /// </summary>
    public void Dehighlight()
    {
        Highlighter.SetActive(false);
    }

    /// <summary>
    /// Переносит персонажа на передний план.
    /// </summary>
    public void MoveToForeground()
    {
        meshRenderer.sortingLayerName = "Foreground";
    }

    /// <summary>
    /// Переносит персонажа на план битвы.
    /// </summary>
    public void MoveToBack()
    {
        meshRenderer.sortingLayerName = "Default";
    }
}
