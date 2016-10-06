﻿using UnityEngine;
using System.Collections;

public class EvandriaUpdate : MonoBehaviour {

    HealthBarScript sliderScript;
    public Candidate chosenCandidate;
    public bool goodOutcome;

    void Start()
    {
        sliderScript = FindObjectOfType<HealthBarScript>();
    }

	
    public void DecisionMade (string candidateName)
    {
        //Temporarily Hardcoded attributes
        int[] gabrielGoodArray = { 3, 6, 3, 5, 5 };
        int[] gabrielBadArray = { 1, 3, 1, 2, 2 };
        int[] jessicaGoodArray = { -2,-3,-2,5,-3 };
        int[] jessicaBadArray = {-5,-5,-5,2,-5 };

        int[] goodArray; //Replace with retrieving array from candidate
        int[] badArray;

        int changeInMorale = 0;

        float badProbability = 0;

        if (candidateName.Equals("Gabriel Johan"))
        {
            badProbability = 0.2f;
            goodArray = gabrielGoodArray;
            badArray = gabrielBadArray;
        } else
        {
            goodArray = jessicaGoodArray;
            badArray = jessicaBadArray;
            badProbability = 0.8f;
        }

        //Retrieve decision stats for candidate
        //5 stats good/bad values placed in array in order
        
        int bestCase =0;

        foreach (int x in goodArray)
        {
            bestCase += x;
        }
                

        //Retrieve probability for candidate (e.g: 80/20)
        

        
        int i;
        for (i=0; i<5; i++)
        {
            if( Random.Range(0f,1f) <= badProbability)
            {
                changeInMorale += badArray[i];
            } else
            {
                changeInMorale += goodArray[i];
            }
        }

        if (bestCase / 2 > changeInMorale)
        {
            goodOutcome = false;
        } else
        {
            goodOutcome = true;
        }

        //Call UpdateSlider on morale slider with input changeInMorale
        sliderScript.UpdateHealth(changeInMorale);

        
    }
}
