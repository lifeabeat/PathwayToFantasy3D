using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;
using System;

public class DialogueUI : MonoBehaviour
{
    PlayerConservant playerConservant;
    [SerializeField] TextMeshProUGUI AIText;
    [SerializeField] Button nextButton;
    [SerializeField] GameObject AIResponse;
    [SerializeField] Transform choiceRoot;
    [SerializeField] GameObject choicePrefab;
    [SerializeField] Button quitButton;
    [SerializeField] TextMeshProUGUI conservantName;
    void Start() 
    
    {
        playerConservant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConservant>();
        playerConservant.onConversationUpdated += UpdateUI;
        nextButton.onClick.AddListener(()=> playerConservant.Next());
        quitButton.onClick.AddListener(() => playerConservant.Quit());

        UpdateUI();
    }

    void Next()
    {
        playerConservant.Next();
 
    }

    void UpdateUI()
    {
        gameObject.SetActive(playerConservant.IsActive());
        if(!playerConservant.IsActive())
        {
            return;
        }
        conservantName.text = playerConservant.GetCurrentConservantName();
        AIResponse.SetActive(!playerConservant.IsChoosing());
        choiceRoot.gameObject.SetActive(playerConservant.IsChoosing());
        if (playerConservant.IsChoosing())
        {
            BuildChoiceList();
        }
        else
        {
            AIText.text = playerConservant.GetText();
            nextButton.gameObject.SetActive(playerConservant.HasNext());
        }


    }

    private void BuildChoiceList()
    {
        // choiceRoot.DetachChildren();
        foreach (Transform item in choiceRoot)
        {
            Destroy(item.gameObject);
        }
        foreach (DialogueNode choice in playerConservant.GetChoices())
        {
            GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
            var textComp = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
            textComp.text = choice.GetText();
            Button button = choiceInstance.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => 
            // When have onClick Event call code in {}
            {
                playerConservant.SelectChoice(choice);
                
            });
        }
    }
}
