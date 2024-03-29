﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Character
{

    private int energy = 0;
    public int maxEnergy = 5;

    private Sprite emptyEnergy = Resources.Load<Sprite>("energy");
    private Sprite filledEnergy = Resources.Load<Sprite>("energy2");
    private Sprite empoweredEnergy = Resources.Load<Sprite>("energy3");

    private Vector3 healthPos = new Vector3(-3.5f, 1);

    private List<Transform> energyUI = gameController.energyUI;

    /// <summary>
    /// Constructor for Hero class.
    /// </summary>
    /// <param name="spellbook"></param>
    /// <param name="textBox"></param>
    /// <param name="anim"></param>
    /// <param name="health"></param>
    // Use this for initialization
    public Hero()
        : base("blackKnight", 50)
    {
        maxGCD = 1f;
        GCD = 0;
        healthBarScale = 2f;
        healthText = gameController.heroHealthText;
        healthBarFloatDistance = new Vector2(0, 1);
        this.spriteType = "special";
        healthFillScale = new Vector3(55f, 55f);
    }

    protected override List<Spell> getSpells()
    {
        Spell spell1 = new EnergyGenerator(this);
        //Spell spell2 = new Fireball(this);
        Spell spell2 = new BigDamage(this);
        Spell spell3 = new StealLife(this);
        //Spell spell4 = new Empower(this);
        //Spell spell4 = new Knockback(this);
        //Spell spell4 = new EnergyHeal(this);
        //Spell spell4 = new Block(this);
        //Spell spell4 = new AOEAttack(this);
        //Spell spell4 = new Bomb(this);
        return new List<Spell> { spell1, spell2, spell3 };
    }

    public int GetEnergy()
    {
        return energy;
    }

    public void GainEnergy(int amount)
    {
        SetEnergy(Math.Min(maxEnergy, energy + amount));
    }

    public void LoseEnergy(int amount)
    {
        SetEnergy(Math.Max(0, energy - amount));
    }

    public void SetEnergy(int amount)
    {
        energy = amount;
        Sprite energySprite;

        for (int i = 0; i < energyUI.Count; i++)
        {
            if(energy == 5)
            {
                energySprite = empoweredEnergy;
            }
            else if(i < energy)
            {
                energySprite = filledEnergy;
            }
            else
            {
                energySprite = emptyEnergy;
            }
            energyUI[i].GetComponent<SpriteRenderer>().sprite
                = energySprite;
        }
    }

    public void LoseAllEnergy()
    {
        LoseEnergy(energy);
    }

    public override void InstantiateCharacter(Vector2 position)
    {
        base.InstantiateCharacter(position);
        instantiateEnergyBar();
    }

    public void instantiateEnergyBar()
    {
        
    }

    public override void Spawn(Vector2 pos)
    {
        base.Spawn(pos);

        //set camera
        gameController.cam.transform.position = new Vector3(pos.x + 3.91f, 0, -6); 
    }

    //Also casts the spell
    public override Boolean CastIfAble(Spell spell)
    {
        Boolean wasCast = base.CastIfAble(spell);
        if (!wasCast) {
            CantCastMessage();
        }
        return wasCast;
    }

    public override void CheckDeadAndKill()
    {
        base.CheckDeadAndKill();
        if (health <= 0)
        {
            Time.timeScale = 0;
        }
    }

    public void MoveRight(Vector3 startingPosition)
    {
        Vector3 endPosition = startingPosition + new Vector3(6, 0, 0);
        IEnumerator coroutine = MoveRightCoroutine(startingPosition);
        gameController.StartCoroutine(coroutine);
    }

    public override void Move(Vector2 position)
    {
        base.Move(position);
    }

    IEnumerator MoveRightCoroutine(Vector3 startingPosition)
    {
        float time = 0;
        float walkTime = 2;

        anim.SetBool("Walk", true);

        Vector3 camStartPos = gameController.cam.transform.position;

        while (time < walkTime)
        {
            //Move hero
            sprite.position += new Vector3(6.09f, 0, 0) * Time.deltaTime / walkTime;

            //Move camera
            gameController.cam.transform.position += new Vector3(6.09f, 0, 0) * Time.deltaTime / walkTime;

            time += Time.deltaTime;
            yield return null;
        }
        sprite.transform.position = new Vector3(startingPosition.x + 6.09f, -2.58f, 0);
        gameController.cam.transform.position = new Vector3(camStartPos.x + 6.09f, 0, -6);
        anim.SetBool("Idle", true);

    }


    /// <summary>
    /// Displays a fading message when the player attempts to cast an uncastable spell
    /// </summary>
    private void CantCastMessage()
    {
        Boolean toStart = true;
        float r = gameController.cantCast.GetComponent<Text>().color.r;
        float g = gameController.cantCast.GetComponent<Text>().color.g;
        float b = gameController.cantCast.GetComponent<Text>().color.b;

        if (gameController.cantCast.IsActive())
        {
            toStart = false;
        }

        gameController.cantCast.gameObject.SetActive(true);
        gameController.cantCast.GetComponent<Text>().color = new Color(r, g, b, 1f);

        AudioSource source = gameController.GetComponent<AudioSource>();
        source.Play();

        if (toStart)
        {
            IEnumerator coroutine = FadeCantCast(r, g, b);
            gameController.StartCoroutine(coroutine);
        }
    }

    /// <summary>
    /// coroutine for CantCastMessage()
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    IEnumerator FadeCantCast(float r, float g, float b)
    {
        float disappearTime = 3;
        while (gameController.cantCast.GetComponent<Text>().color.a >= 0)
        {
            gameController.cantCast.GetComponent<Text>().color = new Color(r, g, b, gameController.cantCast.GetComponent<Text>().color.a - (1f / disappearTime * Time.deltaTime));
            yield return null;
        }
        gameController.cantCast.gameObject.SetActive(false);
    }

    public override void UpdateStatusBars()
    {
        //Get positioning
        Vector2 position = sprite.position;
        Vector2 healthBarOffset = characterHeight +
            healthBarFloatDistance;

        //Move health bar
        //healthBar.position = position + healthBarOffset;

        //Update health text
        //healthText.position = position + healthBarOffset;

        //Update health bar fill
        float percentFilled = health / maxHealth;
        healthBarFill.localScale = new Vector2(healthFillScale.x * percentFilled, healthFillScale.y);
        float currentWidth = healthBarFill.GetComponent<Renderer>().bounds.size.x;
        float offset = (healthBarFillWidth - currentWidth) / 2;
        healthBarFill.localPosition = (healthPos - new Vector3(offset, 0))* 110.3559f;

        textBox.text = Utils.ToDisplayText(health);
    }

    //public override void instantiateHealthBar(Vector2 position)
    //{
    //    //Set height one time so health bar doesnt move
    //    characterHeight = new Vector2(0f,
    //        sprite.GetComponent<Renderer>().bounds.size.y);
    //    characterGUI.Add(healthBar);
    //    characterGUI.Add((Transform)healthText);
    //    textBox = healthText.GetComponent<TextMesh>();
    //}

    public override void instantiateHealthBar(Vector2 position)
    {
        base.instantiateHealthBar(position);
        healthBar.SetParent(gameController.canvas.transform);
        healthBarFill.SetParent(gameController.canvas.transform);
        healthText.SetParent(gameController.canvas.transform);
        healthFillScale = healthBarFill.localScale;
        healthBar.position = healthPos;
        healthText.position = healthPos;
        healthBarFill.position = healthPos;
    }

    //public override void UpdateStatusBars()
    //{
    //    //Get positioning
    //    Vector2 position = sprite.position;
    //    textBox.text = Utils.ToDisplayText(health);
    //}
}