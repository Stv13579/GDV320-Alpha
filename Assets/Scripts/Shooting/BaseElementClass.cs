using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;


public class BaseElementClass : MonoBehaviour
{
    //base class that all other player elements derives from

    //VFX instantiation
    //VFX that plays when the element is used (eg a fireball from fire)
    [SerializeField]
    protected GameObject activatedVFX;
    //VFX that appears in the hand while this element is active (perhaps nothing for combos)
    [SerializeField]
    protected GameObject handVFX;

    //VFX that appears around the wrist while this element is active (Mostly unused)
    [SerializeField]
    protected GameObject wristVFX = null;

    [SerializeField]
    protected float manaCost;

    //A string to pass to the animator to activate appropriate triggers when 
    [SerializeField]
    protected string animationToPlay;
    [SerializeField]
    protected string animationToPlayAlt;

    //Animation trigger
    //The animator which the element calls animations on
    [SerializeField]
    protected Animator playerHand;

	//Variables for UI purposes
	[SerializeField]
	string elementName;
	[SerializeField]
	string elementDescription;
	[SerializeField]
	Sprite uiSprite;
	[SerializeField]
	Sprite crosshair;
	[SerializeField]
    HoverOver.LoadoutVariables lVars;

	//Additional variables for the blacksmith
	[SerializeField]
	int upgradeCost = 5;
	[SerializeField]
    string upgradeDescription;

    [SerializeField]
    protected List<BaseEnemyClass.Types> attackTypes;

    [SerializeField]
    protected List<PlayerClass.ManaName> manaTypes;

    protected GameObject player;

    [SerializeField]
    protected PlayerClass playerClass;

    [SerializeField]
    protected Transform shootingTranform;

    [SerializeField]
    protected LayerMask shootingIgnore;

    protected AudioManager audioManager;

    [SerializeField]
    protected string shootingSoundFX;
    [SerializeField]
    protected string otherShootingSoundFX;
    [SerializeField]
    protected string switchElementSFX;
    [SerializeField]
    protected string idleSFX;

    protected bool upgraded = false;

    [SerializeField]
    protected float damage;

	protected float damageMultiplier = 1;
    List<Multiplier> damageMulti = new List<Multiplier>();

    protected ElementStats elementData;

    protected Shooting shootingScript;

    [SerializeField]
    protected string switchAnimationName;

    protected int randomAnimationToPlay;

    [SerializeField]
    protected bool startCoolDown;

    //Cooldown/Firerate 
    //The amount of time before the element can be used again (usually brief)
    [SerializeField]
    protected float cooldownTimer;
    protected float currentCoolDownTimer;

    protected GameplayUI gameplayUI;

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
    protected ElementType currentElementType = ElementType.None;

    public ElementType GetCurrentElementType() { return currentElementType; }
    public string GetSwitchElementSFX() { return switchElementSFX; }
    public bool GetStartCoolDown() { return startCoolDown; }
    public string GetIdleSFX() { return idleSFX; }
    public GameObject GetHandVFX() { return handVFX; }
    public GameObject GetWristVFX() { return wristVFX; }
    public GameObject GetActivatedVFX() { return activatedVFX; }

    public float GetManaCost() { return manaCost; }

    protected virtual void Start()
    {
        //lVars.Name = elementName;
        lVars.Icon = uiSprite;
        //lVars.Description = elementDescription;
        player = GameObject.Find("Player");
        playerClass = player.GetComponent<PlayerClass>();
        //shootingTranform = GameObject.Find("Elements").transform;
        audioManager = FindObjectOfType<AudioManager>();
        elementData = GetComponent<ElementStats>();
        shootingScript = player.GetComponent<Shooting>();
        gameplayUI = FindObjectOfType<GameplayUI>();
        currentCoolDownTimer = cooldownTimer;
    }

    // calls the animation
    // this gets call in activate elements
    protected virtual void StartAnims(string animationName, string animationNameAlt = null)
    {
        if (audioManager)
        {
            audioManager.StopSFX(idleSFX);
            audioManager.StopSFX(shootingSoundFX);
            audioManager.PlaySFX(shootingSoundFX);
        }
    }
    //Called from the hand objects when the appropriate event triggers to turn on the vfx
    // gets called same time as elementeffect
    public virtual void ActivateVFX()
    {

    }
    public void AnimationSwitch(bool isleft)
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
    // event on animation plays this if call the right trigger
    public virtual void ElementEffect()
    {
        // once player has clicked to shoot start the cool cool down timer
        startCoolDown = true;
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
            return false;
        }
    }

    // this gets called first
    // start the animation of the elements if cool down hasnt started and if player has mana
    public void ActivateElement()
    {
        if (!startCoolDown)
        {
            if (PayCosts())
            {
                StartAnims(animationToPlay, animationToPlayAlt);
            }
        }
    }

    //For unique behaviour when the mb is lifted while using an element
    // function to stop the effect of the element
    // used for hold elements such as energy, necrotic, void, laser beam, life steal and curse
    public virtual void LiftEffect()
    {

    }

    protected virtual void Update()
    {
        // cool down starts
        if (startCoolDown == true)
        {
            // cool down timer goes down
            currentCoolDownTimer -= Time.deltaTime;
            // if cooldown timer == 0
            if (currentCoolDownTimer <= 0)
            {
                // reset timer
                // cooldown is off and player can shoot again
                currentCoolDownTimer = cooldownTimer;
                startCoolDown = false;
            }
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
    
	public Sprite GetSprite()
	{
		return uiSprite;
	}
	
	public Sprite GetCrosshair()
	{
		return crosshair;
	}
	
	public HoverOver.LoadoutVariables GetLVars()
	{
		return lVars;
	}
	
	public int GetUpgradeCost()
	{
		return upgradeCost;
	}
	
	public string GetName()
	{
		return elementName;
	}
	
	public string GetDescription()
	{
		return elementDescription;
	}
}
