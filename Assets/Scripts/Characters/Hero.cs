using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Character
{

    private TextMesh energyText = GameController.gameController.heroEnergyText;
    private int energy = 0;
    private int maxEnergy = 5;

    /// <summary>
    /// Constructor for Hero class.
    /// </summary>
    /// <param name="spellbook"></param>
    /// <param name="textBox"></param>
    /// <param name="anim"></param>
    /// <param name="health"></param>
    // Use this for initialization
    public Hero()
        : base("blackKnight", GameController.gameController.heroHealthText, 200)

    {
           maxGCD = 1f;
           energyText.text = energy.ToString();
    }

    protected override List<Spell> getSpells()
    {
        Spell spell1 = new EnergyGenerator(this);
        //Spell spell2 = new Fireball(this);
        Spell spell2 = new EnergyDamage(this);
        Spell spell3 = new StealLife(this);
        //Spell spell4 = new Empower(this);
        //Spell spell4 = new Knockback(this);
        //Spell spell4 = new EnergyHeal(this);
        //Spell spell4 = new Block(this);
        //Spell spell4 = new AOEAttack(this);
        Spell spell4 = new Bomb(this);
        return new List<Spell> { spell1, spell2, spell3, spell4 };
    }

    public int GetEnergy()
    {
        return energy;
    }

    public void GainEnergy(int amount)
    {
        energy = Math.Min(maxEnergy, energy + amount);
        energyText.text = energy.ToString();
    }

    public void LoseEnergy(int amount)
    {
        energy = Math.Max(0, energy - amount);
        energyText.text = energy.ToString();
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
        IEnumerator coroutine = MoveRight2(startingPosition);
        gameController.StartCoroutine(coroutine);
    }

    IEnumerator MoveRight2(Vector3 startingPosition)
    {
        float time = 0;
        float walkTime = 2;

        anim.SetBool("Walk", true);

        Vector3 camStartPos = gameController.cam.transform.position;

        while (time < walkTime)
        {
            //Move hero
            instances[0].position += new Vector3(6.09f, 0,0) * Time.deltaTime / walkTime;

            //Move camera
            gameController.cam.transform.position += new Vector3(6.09f, 0, 0) * Time.deltaTime / walkTime;

            time += Time.deltaTime;
            yield return null;
        }
        instances[0].transform.position = new Vector3(startingPosition.x + 6.09f, -2.58f,0);
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
}