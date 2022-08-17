using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseElementClass : MonoBehaviour
{
    //base class that all other player elements derives from

    //VFX instantiation
    //VFX that plays when the element is used (eg a fireball from fire)
    [SerializeField]
    public GameObject activatedVFX = null;
    //VFX that appears in the hand while this element is active (perhaps nothing for combos)
    [SerializeField]
    public GameObject handVFX;

    //VFX that appears around the wrist while this element is active (Mostly unused)
    [SerializeField]
    public GameObject wristVFX = null;

    [SerializeField]
    public float manaCost;

    //A string to pass to the animator to activate appropriate triggers when 
    [SerializeField]
    protected string animationToPlay;
    [SerializeField]
    protected string animationToPlayAlt;
    //Cooldown/Firerate 
    //The amount of time before the element can be used again (usually brief)
    [SerializeField]
    public float useDelay;

    //Animation trigger
    //The animator which the element calls animations on
    [SerializeField]
    protected Animator playerHand;

    float currentUseDelay = 0;

    //Variables for UI purposes
    public string elementName;
    public Sprite uiSprite;
    public Sprite crosshair;

    //Additional variables for the blacksmith
    public int upgradeCost = 5;

    [SerializeField]
    protected List<BaseEnemyClass.Types> attackTypes;

    [SerializeField]
    protected List<PlayerClass.ManaName> manaTypes;
    GameObject player;
    [SerializeField]

    protected PlayerClass playerClass;

    [SerializeField]
    protected Transform shootingTranform;
    [SerializeField]
    protected LayerMask shootingIgnore;

    protected AudioManager audioManager;

    [SerializeField]
    string shootingSoundFX;

    protected bool upgraded = false;

    [SerializeField]
    protected float damage;

    [HideInInspector]
    public float damageMultiplier = 1;
    public List<Multiplier> damageMulti = new List<Multiplier>();

    protected ElementStats elementData;

    protected Shooting shootingScript;

    public string switchAnimationName;

    [SerializeField]
    protected int leftIndex;

    [SerializeField]
    protected int rightIndex;

    protected int randomAnimationToPlay;
    public enum ElementType
    {
        None = 0,
        
        Fire = 1,
        Crystal = 2,
        Water = 3,

        Necrotic = 101,
        Energy = 102,
        Void = 103,

        LaserBeam = 201,
        ShardCannon = 202,
        AcidCloud = 203,
        LandMine = 204,
        Curse = 205,
        IceSlash = 206,
        LifeSteal = 207,
        CrystalGrenade = 208,
        StasisTrap = 209
    }
    [SerializeField]
    protected ElementType currentElementType = ElementType.Fire;
    protected virtual void Start()
    {
        player = GameObject.Find("Player");
        playerClass = player.GetComponent<PlayerClass>();
        //shootingTranform = GameObject.Find("Elements").transform;
        audioManager = FindObjectOfType<AudioManager>();
        elementData = Resources.Load<ElementStats>("Element/ElementData");
        shootingScript = player.GetComponent<Shooting>();
    }
    protected virtual void StartAnims(string animationName, string animationNameAlt = null)
    {
        playerHand.SetTrigger("CancelBack");
    }
    //Called from the hand objects when the appropriate event triggers to turn on the vfx
    public virtual void ActivateVFX()
    {

    }
    public virtual void AnimationSwitch(bool isleft)
    {

        switch (currentElementType)
        {
            case ElementType.Fire:
            case ElementType.Crystal:
            case ElementType.Water:
            case ElementType.Necrotic:
            case ElementType.Energy:
            case ElementType.Void:
                // if its these cases
                // make elementc = none
                // checks if its left hand or right hand then returns the appropriate index
                playerHand.SetInteger("ElementC", 0);
                playerHand.SetInteger(isleft ? "ElementL" : "ElementR", (int)currentElementType);
                break;
            case ElementType.LaserBeam:
            case ElementType.ShardCannon:
            case ElementType.AcidCloud:
            case ElementType.LandMine:
            case ElementType.Curse:
            case ElementType.IceSlash:
            case ElementType.LifeSteal:
            case ElementType.CrystalGrenade:
            case ElementType.StasisTrap:
                // if its these cases
                // make elementL = none
                // make elementR = none
                // Gives elementC the appropriate index
                playerHand.SetInteger("ElementL", 0);
                playerHand.SetInteger("ElementR", 0);
                playerHand.SetInteger("ElementC", (int)currentElementType);
                break;
        }
    }
    //The actual mechanical effect (eg fire object etc)
    public virtual void ElementEffect()
    {
        audioManager.Stop(shootingSoundFX);
        audioManager.Play(shootingSoundFX);
    }

    //deduct mana from the mana pool. If unable too, return false, otherwise true
    protected virtual bool PayCosts(float modifier = 1)
    {
        if (playerClass.ManaCheck(manaCost * modifier, manaTypes))
        {
            playerClass.ChangeMana(-manaCost * modifier, manaTypes);
            return true;
        }
        else
        {
            //Set out of mana anim
            playerHand.SetTrigger("NoMana");
            return false;
        }
    }

    //Activates the element, essentially the very start of everything, only if the delay has expired
    public void ActivateElement()
    {
        if (currentUseDelay <= 0)
        {
            if(PayCosts())
            {
                StartAnims(animationToPlay, animationToPlayAlt);
                currentUseDelay = useDelay;
            }
        }
        
    }

    //For unique behaviour when the mb is lifted while using an element
    public virtual void LiftEffect()
    {

    }

    protected virtual void Update()
    {
        if(currentUseDelay > 0)
        {
            currentUseDelay -= Time.deltaTime;
        }
    }

    public PlayerClass.ManaName GetManaName()
    {
        return manaTypes[0];
    }

    //Changes the functionality or in some way improves the element
    public virtual void Upgrade()
    {
        if(!upgraded)
        {
            upgraded = true;

            Multiplier dam = new Multiplier(1.25f, "Upgrade");
            damageMultiplier = Multiplier.AddMultiplier(damageMulti, dam);

        }
    }

    public Animator GetPlayerHand() {return playerHand;}
}
