using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Text cardnameText;
    public Text costText;
    public Text amountText;
    public Text effectText;

    public Image background;
    public Color attackColor;
    public Color defendColor;
    public Color skillColor;

    public Card card;
    
    // Start is called before the first frame update    
    void Start()
    {
        ShowCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCard()
    {
        cardnameText.text = card.CardName;
        
        if (card is AttackCard)
        {
            var temp = card as AttackCard;
            amountText.text = temp.Amount.ToString();
            costText.text = temp.Cost.ToString();
            background.color = attackColor;        

            effectText.gameObject.SetActive(false);
        }

        if (card is DefendCard)
        {
            var temp = card as DefendCard;
            amountText.text = temp.Amount.ToString();
            costText.text = temp.Cost.ToString();
            background.color = defendColor;

            effectText.gameObject.SetActive(false);
        }

        if (card is SkillCard)
        {
            var temp = card as SkillCard;
            effectText.text = temp.Effect.ToString();
            costText.text = temp.Cost.ToString();
            background.color = skillColor;

            amountText.gameObject.SetActive(false);
        }
    }
}
