using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public ImageTimer HarvestTimerImg;
    public ImageTimer EatingTimerImg;
    public AudioSource eatSound;
    public AudioSource harvSound;
    public AudioSource warriorSound;
    public AudioSource cannonSound;
    public AudioSource peasantSound;
    public AudioSource battleSound;
    public Image RaidTimerImg;
    public Image PeasantTimerImg;
    public Image WarriorTimerImg;
    public Image CannonTimerImg;
    public Button peasantButton;
    public Button forgeButton;
    public Button barracsButton;
    public Button warriorButton;
    public Button cannonButton;
    public Text resorcesText;
    public Text gameOverText;
    public int peasantCount;
    public int warriorsCount;
    public double cannon;
    public int wheatCount;
    public int wheatPerPeasant;
    public int wheatToWarriors;
    public int peasantCost;
    public int warriorCost;
    public float peasantCreateTime;
    public float warriorCreateTime;
    public float cannonCreateTime;
    public float raidMaxTime;
    public int raidIncrease;
    public int nextRaid;
    private int sumRaid=-1;
    private int sumHarv;
    private int sumPeasant;
    private int sumWarrior;
    private int sumCannon;
    private int barracsOn;
    private int forgeOn;
    


    public float peasantTimer = -2;
    public float warriorTimer = -2;
    public float cannonTimer = -2;
    private float raidTimer;
    public GameObject winScreen;
    public GameObject GameOverScreen;
    public Text NextRaidText;
    public int freedomOfRaid = 10;
    
    void Start()
    {
        UpdateText();
        raidTimer = raidMaxTime;
    }
        
    void Update()
    {
        AddHarv();
        Eated();
        AddPeasant();
        AddWarrior();
        AddCannon();
        Raid();
        NoMoney();
        UpdateText();
        GameOver();
        Win();
    }
    public void Win()
    {
        if (cannon == 5)
        {
            winScreen.SetActive(true);
            cannon = 4;
        }        
    }
    private void AddHarv()
    {
        if (HarvestTimerImg.Tick)
        {
            wheatCount += peasantCount * wheatPerPeasant;
            sumHarv += peasantCount * wheatPerPeasant;
            harvSound.Play();
            if (wheatCount > 99)
            {
                barracsButton.interactable = true;
            }
            if (wheatCount > 500)
            {
                forgeButton.interactable = true;
            }
        }
    }

    private void Eated()
    {
        if (EatingTimerImg.Tick)
        {
            wheatCount -= warriorsCount * wheatToWarriors;
            if (wheatCount < 0) wheatCount = 0;
            eatSound.Play();
        }
    }

    private void Raid()
    {
        raidTimer -= Time.deltaTime;
        RaidTimerImg.fillAmount = raidTimer / raidMaxTime;        
        if (raidTimer <= 0)
        {
            freedomOfRaid -= 1;
            raidTimer = raidMaxTime;
            if (freedomOfRaid <= 0)
            {
                if (warriorsCount < nextRaid)
                {
                    var ourDamage = (int)(cannon * 4 + warriorsCount);
                    var enemyDamage = ourDamage - nextRaid;
                    cannon -= Math.Abs (enemyDamage) / 2;
                    if (cannon <= 1 && warriorsCount < 6)
                    {
                        cannon = 0;
                    }
                    cannon = Math.Round(cannon, MidpointRounding.ToEven);
                    peasantCount -= Math.Abs(enemyDamage) * 4;
                    if (peasantCount < 0)
                    { 
                        peasantCount = 0; 
                    }
                    warriorsCount = 0;
                    RaidSum();
                }
                else
                {
                    var ourDamage = (int)((cannon * 5) + warriorsCount);
                    var enemyDamage = ourDamage - nextRaid;
                    if (enemyDamage <= 5)
                    { 
                    warriorsCount -= nextRaid;
                        RaidSum();
                    }
                    if (enemyDamage > 5)
                    {
                        warriorsCount -= nextRaid / 2;
                        RaidSum();
                    }
                }
            }
        }
    }
     private void RaidSum()
    {       
        nextRaid += raidIncrease;
        NextRaidText.text = nextRaid.ToString();
        sumRaid++;
        battleSound.Play();
    }
    private void GameOver()
    {
        if (warriorsCount <= 0 && cannon <= 0 && peasantCount <=0)
        {
            Time.timeScale = 0;
            GameOverScreen.SetActive(true);
            peasantButton.interactable = false;
            warriorButton.interactable = false;
            GameOverResult();
        }
    }
        private void NoMoney()
    {
        if (warriorsCount < 100)
        {
            cannonButton.interactable = false;
            if (wheatCount < 8)
            {
                warriorButton.interactable = false;
                if (wheatCount < 4)
                {
                    peasantButton.interactable = false;
                }

            }
        }
        if (wheatCount > 3 && peasantTimer < 0)
        {
            peasantButton.interactable = true;            
        }
        if (wheatCount > 7 && warriorTimer < 0 && barracsOn > 0)
        {
            warriorButton.interactable = true;

        }
        if (wheatCount > 99 && cannonTimer < 0 && forgeOn > 0)
        {
            cannonButton.interactable = true;
        }
    }
    private void AddPeasant()
    {
        if (peasantTimer > 0)
        {
            peasantTimer -= Time.deltaTime;
            PeasantTimerImg.fillAmount = peasantTimer / peasantCreateTime;
        }
        else if (peasantTimer > -1)
        {
            PeasantTimerImg.fillAmount = 1;
            peasantButton.interactable = true;
            peasantCount += 1;
            sumPeasant += 1;
            peasantTimer = -2;
            peasantSound.Play();
        }
    }

    private void AddWarrior()
    {
        if (warriorTimer > 0)
        {
            warriorTimer -= Time.deltaTime;
            WarriorTimerImg.fillAmount = warriorTimer / warriorCreateTime;
        }
        else if (warriorTimer > -1)
        {
            WarriorTimerImg.fillAmount = 1;
            warriorButton.interactable = true;
            warriorsCount += 1;
            sumWarrior += 1;
            warriorTimer = -2;
            warriorSound.Play();
        }
    }
    private void AddCannon()
    {
        if (cannonTimer > 0)
        {
            cannonTimer -= Time.deltaTime;
            CannonTimerImg.fillAmount = cannonTimer / cannonCreateTime;
        }
        else if (cannonTimer > -1)
        {
            CannonTimerImg.fillAmount = 1;
            cannonButton.interactable = true;
            cannon += 1;
            sumCannon += 1;
            cannonTimer = -2;
            cannonSound.Play();
        }
    }
    public void CreatePeasant()
    {
        if (wheatCount <= 0)
        {
            peasantButton.interactable = false;          
        }
        wheatCount -= peasantCost;
        peasantTimer = peasantCreateTime;
        peasantButton.interactable = false;
    }
    public void CreateWarrior()
    {
        if (wheatCount <= 0)
        {            
            warriorButton.interactable = false;
        }
        wheatCount -= warriorCost;
        warriorTimer = warriorCreateTime;
        warriorButton.interactable = false;
    }
    public void CreateCannon()
    {
        if (wheatCount <= 0)
        {
            cannonButton.interactable = false;
        }
            wheatCount -= 100; //Стоимость пушки
            cannonTimer = cannonCreateTime;
            cannonButton.interactable = false;                
    }
    public void CreateBarracs()
    {
        if (wheatCount > 99)
        {
            wheatCount -= 100;
            warriorButton.interactable = true;
            barracsOn = 1;
        }
    }

    public void CreateForge()
    {
        if (wheatCount > 500)
        { 
            wheatCount -= 500;
            cannonButton.interactable = true;
            forgeOn = 1;
        }
    }
    private void UpdateText()
    {
        resorcesText.text = peasantCount + "\n" + warriorsCount + "\n" + cannon + "\n" + wheatCount;
    }

    private void GameOverResult()
    {
        gameOverText.text = sumRaid + "\n\n\n" + sumHarv + "\n\n" + sumPeasant + "\n" + sumWarrior + "\n" + sumCannon;
    }
}
