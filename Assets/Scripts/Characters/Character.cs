using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public abstract class Character
{
    public static GameController gameController = GameController.gameController;
    public List<Transform> characterGUI = new List<Transform> { };

    public List<Spell> spellbook;
    public float maxHealth;
    public float health;
    public float GCD = .5f;
    public float maxGCD;
    public float inAdditive = 0;
    public float outAdditive = 0;
    public float inMultiplier = 1;
    public float outMultiplier = 1;
    public float deathTime = 2;
    protected float sizeScale = 1f;

    protected String spriteType = "sprite";

    public Vector2 moveTo = new Vector3(-100, 0);
    protected float walkSpeed = 1;
    protected Boolean isMoving = false;

    public Vector2 characterHeight;
    public Vector2 healthBarFloatDistance = new Vector2(0f, .3f);
    public float healthBarScale = 1f;

    public TextMesh textBox;
    public Transform healthBar;
    public Transform healthText;
    public Transform healthBarFill;
    protected float healthBarFillWidth;
    protected Vector3 healthFillScale;
    public Transform sprite;
    public Animator anim;
    public List<Buff> buffs = new List<Buff> { };
    public List<TextMesh> floatingCombatText = new List<TextMesh> { };
    public Transform prefab;
    protected List<SpellCastObserver> spellCastObservers = new List<SpellCastObserver>();

    public String walkAnim = "Walk";
    public String idleAnim = "Idle";
    public String deathAnim = "Death";
    public String takeDamageAnim = "TakeDamage";
    public String entranceAnim = "Entrance";

    /// <summary>
    /// Constructor for Character class.
    /// </summary>
    /// <param name="spellbook"></param>
    /// <param name="textBox"></param>
    /// <param name="anim"></param>
    /// <param name="max_health"></param>
    public Character(String prefabPath,
        float maxHealth = 10,
        float maxGCD = 2)
    {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
        this.spellbook = getSpells();
        this.maxGCD = maxGCD;
        this.prefab = (Transform)Resources.Load(prefabPath, typeof(Transform));
    }

    protected abstract List<Spell> getSpells();

    public virtual void Spawn(Vector2 pos)
    {
        //Transform instance = MonoBehaviour.Instantiate(prefab);
        //anim = instance.GetComponent<Animator>();
        //instance.position = pos;
        //characterGUI.Add(instance);
        //textBox.text = Utils.ToDisplayText(health);
        InstantiateCharacter(new Vector2(pos.x, pos.y));
    }

    public virtual void InstantiateCharacter(Vector2 position)
    {
        instantiateSprite(position);
        instantiateHealthBar(position);
        UpdateStatusBars();
    }

    public void instantiateSprite(Vector2 position)
    {
        sprite = MonoBehaviour.Instantiate(prefab, position, Quaternion.identity);
        anim = sprite.GetComponent<Animator>();
        sprite.localScale *= sizeScale;
        characterGUI.Add(sprite);
    }

    public virtual void instantiateHealthBar(Vector2 position) { 
    
        if(spriteType.Equals("sprite")){
                //Set height one time so health bar doesnt move
            characterHeight = new Vector2(0f,
            sprite.GetComponent<Renderer>().bounds.size.y);
        }
        else
        {
            characterHeight = new Vector2(0f, 1f);
        }

        //Instantiate health bar
        healthBar = MonoBehaviour.Instantiate(
             (Transform)Resources.Load("healthbar_sprite",
             typeof(Transform)), new Vector2(0,0), Quaternion.identity);
        characterGUI.Add(healthBar);
        healthBar.localScale *= healthBarScale;

        // Get healthTextFab
        Transform healthTextFab = (Transform)Resources.Load(
        "health_text", typeof(Transform));

        //Create Health Bar Fill
        healthBarFill =
            MonoBehaviour.Instantiate(
             (Transform)Resources.Load("healthbar_fill",
             typeof(Transform)), new Vector2(0, 0), Quaternion.identity);
        characterGUI.Add(healthBarFill);
        //healthBarFill.localScale *= healthBarScale;
        healthBarFill.localScale *= healthBarScale;
        healthFillScale = healthBarFill.localScale;

        healthBarFillWidth = healthBarFill.GetComponent<Renderer>().bounds.size.x;

        //Instantiate Text
        healthText = MonoBehaviour.Instantiate(healthTextFab, new Vector2(0, 0),
            Quaternion.identity);
        characterGUI.Add(healthText);
        textBox = healthText.GetComponent<TextMesh>();
        textBox.text = Utils.ToDisplayText(health); 
    }

    public virtual void UpdateStatusBars()
    {
        //Get positioning
        Vector2 position = sprite.position;
        Vector2 healthBarOffset = characterHeight +
            healthBarFloatDistance;

        //Move health bar
        healthBar.position = position + healthBarOffset;

        //Update health text
        healthText.position = position + healthBarOffset;

        //Update health bar fill
        healthBarFill.position = position + healthBarOffset;
        float percentFilled = health / maxHealth;
        healthBarFill.localScale =  new Vector2(healthFillScale.x* percentFilled, healthFillScale.y);
        float currentWidth = healthBarFill.GetComponent<Renderer>().bounds.size.x;
        float offset = (healthBarFillWidth - currentWidth)/2;
        healthBarFill.position -= new Vector3(offset, 0);

        textBox.text = Utils.ToDisplayText(health);
    }

    public virtual void Move(Vector2 position)
    {
        if (moveTo.x != position.x || moveTo.y != position.y)
        {
            moveTo = position;
            if (!isMoving)
            {
                isMoving = true;
                IEnumerator coroutine = MoveCoroutine();
                gameController.StartCoroutine(coroutine);
            }
        }
    }

    public Boolean CheckDead()
    {
        if (health <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void MoveUpdate() {
        //Use the coded out one for dynamic move speed
        //characterGUI.ForEach(i => i.position += new Vector3((moveTo.x - startPositions[characterGUI.IndexOf(i)].x) * Time.deltaTime * walkSpeed, 0, 0));
        characterGUI.ForEach(i =>
            i.position -= new Vector3(Time.deltaTime * walkSpeed, 0, 0));
    }

    public virtual void FinishMove()
    {
        if (!CheckDead())
        {
            characterGUI.ForEach(i => i.position = new Vector2(moveTo.x, i.position.y));
            anim.SetBool(idleAnim, true);
        }
        isMoving = false;
    }

    //Moves enemy to it moveTo
    IEnumerator MoveCoroutine()
    {
        if (!CheckDead())
        {
            anim.SetBool(walkAnim, true);

            //Get End position of everything
            List<Vector3> startPositions = new List<Vector3> { };
            foreach (Transform i in characterGUI)
            {
                startPositions.Add(i.position);
            }

            while (!CheckDead() && sprite.position.x > moveTo.x)
            {
                MoveUpdate();
                yield return null;
            }
            FinishMove();
        }
    }

    //Also casts the spell
    public virtual Boolean CastIfAble(Spell spell)
    {
        if (!spellbook.Contains(spell))
        {
            //Throw error
            MonoBehaviour.print("Spellbook does not contain spell.");
        }
        Boolean castable = spell.isCastable();
        if (castable)
        {
            anim.SetBool(spell.animationKey, true);
            spell.Cast();
            spellCastObservers.ForEach(observer => observer.SpellCastUpdate(spell, this));
        }

        return castable;
    }

    /// <summary>
    /// Register object that will listen for spellcasts.
    /// </summary>
    /// <param name="observer"></param>
    public void RegisterCastListener(SpellCastObserver observer)
    {
        spellCastObservers.Add(observer);
    }

    /// <summary>
    /// Unregister object that will listen for spellcasts.
    /// </summary>
    /// <param name="observer"></param>
    public void UnregisterCastListener(SpellCastObserver observer)
    {
        spellCastObservers.Remove(observer);
    }

    //Right now this removes buffs every update... should be changed
    public void Update()
    {
        UpdateStatusBars();
        if(GameController.gameController.stage.inCombat)
        {
            ReduceCooldowns();
            buffs.ForEach(buff => buff.Update());
        }
    }

    /// <summary>
    /// Reduces GCD and cooldowns of every spell in the spellbook
    /// </summary>
    public void ReduceCooldowns()
    {
        //Reduces GCD
        GCD = Math.Max(GCD - Time.deltaTime, 0);

        //Reduces Spell CDs
        if(spellbook != null)
        {
            spellbook.ForEach(spell => spell.ReduceCooldown());
        }
        
    }

    /// <summary>
    /// Sets the max GCD.
    /// </summary>
    /// <param name="newMaxGCD"></param>
    public void SetMaxGCD(float maxGCD)
    {
        this.maxGCD = maxGCD;
    }

    /// <summary>
    /// Get damage the character should deal given a base damage.
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <returns></returns>
    public float GetDamage(float baseDamage)
    {
        return outAdditive + (outMultiplier * baseDamage);
    }

    /// <summary>
    /// Takes damage given a base damage.
    /// </summary>
    /// <param name="baseDamage"></param>
    public virtual float TakeDamage(float baseDamage, Character source)
    {
        float damageTaken = inAdditive + (inMultiplier * baseDamage);
        health -= damageTaken;
        health = Math.Min(maxHealth, health);
        health = Math.Max(health, 0);
        textBox.text = Utils.ToDisplayText(health);
        DrawDamageTaken(damageTaken);
        if(baseDamage > 0)
        {
            anim.SetBool(takeDamageAnim, true);
        }
        CheckDeadAndKill();
        return damageTaken;
    }

    public virtual void CheckDeadAndKill()
    {
        if (CheckDead()) {
            foreach (Transform uiElement in characterGUI)
            {
                MonoBehaviour.Destroy(uiElement.gameObject);
            }
            MonoBehaviour.print("You Lose.");
        }
    }

    /// <summary>
    /// Draws damage taken on the screen next to the character that took damage with a minus sign.
    /// </summary>
    /// <param name="damageTaken"></param>
    public void DrawDamageTaken(float damageTaken) { 
    
        GameObject prefab = (GameObject)Resources.Load("FCTNew");

        //Instantiate floating combat text
        GameObject FCTGameObject = MonoBehaviour.Instantiate(prefab);
        Transform FCT = FCTGameObject.GetComponent<Transform>();
        FCT.transform.SetParent(gameController.canvas.transform);
        Text tmesh = FCT.GetComponent<Text>();

        tmesh.color = new Color(255, 255, 255, 1);

        String symbol = "-";
        if (damageTaken < 0)
        {
            symbol = "+";
        }
        tmesh.text = symbol + " " + Utils.ToDisplayText(Math.Abs(damageTaken));

        //Assigns color to text based on amount of mitigation        
        if(inMultiplier > 1)
        {
            tmesh.color = new Color(255, 255, 0, 1f);
        }
        else if(inMultiplier < 1){
            tmesh.color = Color.blue;
        }

        if (damageTaken < 0)
        {
            tmesh.color = Color.green;
        }

        //Assigns size based on sqrt of damage
        tmesh.fontSize = (int)Math.Round(tmesh.fontSize * Math.Sqrt(Math.Abs(damageTaken)));
        Vector2 size = tmesh.GetComponent<RectTransform>().sizeDelta;
        tmesh.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x * (float)Math.Sqrt(Math.Abs(damageTaken)),
            size.y * (float)Math.Sqrt(Math.Abs(damageTaken)));

        //Scales FCT from non-canvas to canvas
        //103 is the scale from canvas to non-canvas, at least it's really close
        FCT.transform.localScale /= 103;
        //FCTGameObject.

        float xoffset = UnityEngine.Random.Range(0, .5f);
        float yoffset = UnityEngine.Random.Range(0, .5f);

        Vector3 newPos = new Vector3(sprite.position.x + .2f + xoffset,
            sprite.position.y + .4f + yoffset, 0);

        FCT.transform.position = newPos;

        IEnumerator coroutine = DestroyFCT(FCT, 1.5f);
        gameController.StartCoroutine(coroutine);
    }

    /// <summary>
    /// Handles Floating Combat Text until it's destroied
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    IEnumerator DestroyFCT(Transform instance, float time)
    {
        float startTime = 0;
        while (startTime < time)
        {
            time -= Time.deltaTime;
            instance.transform.position += new Vector3(.31f * Time.deltaTime, 0.62f * Time.deltaTime, 0);
            yield return null;
        }
        MonoBehaviour.Destroy(instance.gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    public void TriggerGCD()
    {
        GCD = maxGCD;
    }

    public IEnumerator DestroyAfterTime(float time)
    {
        float startTime = 0;
        while (startTime < time)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        foreach (Transform uiElement in characterGUI)
        {
            if (uiElement != null)
            {
                MonoBehaviour.Destroy(uiElement.gameObject);
            }
            else
            {
                MonoBehaviour.print("Warning!! Tried to destroy null ui element!");
            }
        }
    }
}



