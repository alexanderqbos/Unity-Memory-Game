using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardSpawnPoint;

    [SerializeField] private Sprite[] cardImages;
    [SerializeField] List<Card> cards;

    
    private bool picking = true;
    private Card card1;
    private Card card2;
    private int score = 0;

    Card CreateCard(Vector3 pos)
    {
        GameObject obj = Instantiate(cardPrefab, pos, cardPrefab.transform.rotation);
        Card card = obj.GetComponent<Card>();
        return card;
    }

    private void Awake() {
        Messenger<Card>.AddListener(GameEvent.CARD_CLICKED, this.CardClicked);
    }

    private void OnDestroy() {
        Messenger<Card>.RemoveListener(GameEvent.CARD_CLICKED, this.CardClicked);
    }

    // Start is called before the first frame update
    void Start()
    {
        cards = CreateCards();
        AssignImagesToCards();
    }

    List<Card> CreateCards()
    {
        List<Card> newCards = new List<Card>();
        int row = 2;
        int column = 4;
        float offsetX = 2f;
        float offsetY = 2.5f;

        for(int y = 0; y < row; y++)
        {
            for(int x = 0; x < column; x++)
            {
                Vector3 pos = cardSpawnPoint.position;
                pos.x += (offsetX * x);
                pos.y -= (offsetY * y);

                Card card = CreateCard(pos);
                newCards.Add(card);
            }
        }
        return newCards;
    }

    private void AssignImagesToCards()
    {
        List<int> imageIndicies = new List<int>();
        for(int i = 0; i < cardImages.Length; i++)
        {
            imageIndicies.Add(i);
            imageIndicies.Add(i);
        }

        for(int c = 0; c < imageIndicies.Count; c++)
        {
            int r = Random.Range(0, imageIndicies.Count);
            int i = imageIndicies[c];
            imageIndicies[c] = imageIndicies[r];
            imageIndicies[r] = i;
        }

        for(int i = 0; i < cards.Count; i++)
        {
            int imageIndex = imageIndicies[i];
            cards[i].SetSprite(cardImages[imageIndex]);
        }
    }

    public void CardClicked(Card card)
    {
        if(picking)
        {
            if(card1 == null)
            {
                card1 = card;
                card1.SetFaceVisible(true);
            } else if(card2 == null)
            {
                card2 = card;
                card2.SetFaceVisible(true);
            }

            if(card2 != null)
            {
                StartCoroutine(EvaluatePair());
            }
        }
    }

    public void OnResetButtonPressed()
    {
        score = 0;
        card1 = null;
        card2 = null;

        foreach( Card c in cards)
        {
            c.SetFaceVisible(false);
        }

        AssignImagesToCards();
    }

    IEnumerator EvaluatePair()
    {
        picking = false;

        if(card1.GetSprite() == card2.GetSprite() && card1 != card2)
        {
            Debug.Log("Matched!");
            score++;
        }
        else {
            Debug.Log("No Match!");

            card1.changeLayer("Foreground");
            card2.changeLayer("Foreground");

            // Tween stuff here
            Vector3 pos1 = card1.gameObject.transform.position;
            Vector3 pos2 = card2.gameObject.transform.position;
            
            yield return new WaitForSeconds(1f);

            iTween.MoveTo(card1.gameObject, pos2, 1f);
            iTween.MoveTo(card2.gameObject, pos1, 1f);

            yield return new WaitForSeconds(1f);

            card1.changeLayer("default");
            card2.changeLayer("default");

            card1.SetFaceVisible(false);
            card2.SetFaceVisible(false);

        }
        card1 = null;
        card2 = null;

        picking = true;
    }
}
