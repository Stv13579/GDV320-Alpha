using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FortunesUI : NPCUI
{

    List<GameObject> buttons = new List<GameObject>();
    List<Prophecy> prophecies = new List<Prophecy>();

    [SerializeField]
    int cost;

    //Present the player with the opporunity to look into the future, for a price.
    //If paid, offer there possible futures in the form of tarot cards.
    void Start()
    {
        base.Start();
    }

    public void FortuneButton()
    {
        if (player.money >= cost)
        {
            //Start placing the buttons
            player.money -= cost;

            SetProphecies();
            audioManager.StopSFX("Shop Buy");
            audioManager.PlaySFX("Shop Buy");
            transform.Find("Buy Fortunes").gameObject.SetActive(false);
        }
    }

    public void TarotButton(int index)
    {

        prophecies[index].InitialEffect();
        buttons[index].SetActive(false);
        audioManager.StopSFX("Shop Buy");
        audioManager.PlaySFX("Shop Buy");
        Close();
        NPC.canSeeOfferings = false;
    }



    void SetProphecies()
    {
        //Choose a set of prophecies from the prophecy manager
        GameObject prophManager = GameObject.Find("ProphecyManager");
        List<Prophecy> potentialProphecies = prophManager.GetComponent<ProphecyManager>().allProphecies;
        List<int> choices = new List<int>();

       
        for (int i = 0; i < 3; i++)
        {
            int choice = Random.Range(0, potentialProphecies.Count);
            //While the choice has already been chosen, choose a new one

            //For safety
            int breakCount = 0;
            
            while (choices.Exists(check => check == choice))
            {
                breakCount++;
                if (breakCount > 1000)
                {
                    Debug.Log("Couldn't make a prophecy choice!");
                    break;
                }

                choice = Random.Range(0, potentialProphecies.Count);
            }

            choices.Add(choice);

            prophecies.Add(potentialProphecies[choice]);
        }

        

        //Set the buttons up with the chosen prophecies
        Transform proph = transform.Find("Prophecies");

        proph.gameObject.SetActive(true);

        //Sets up the buttons
        int j = 0;
        foreach (Transform button in proph)
        {
            buttons.Add(button.gameObject);
            prophecies[j].SetCard(button);
            j++;
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
