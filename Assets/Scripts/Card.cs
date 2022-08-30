using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject cardBack;

    public void SetSprite(Sprite image)
    {
        spriteRenderer.sprite = image;
    }

    public Sprite GetSprite()
    {
        return spriteRenderer.sprite;
    }

    public void changeLayer(string layerName)
    {
        spriteRenderer.sortingLayerName = layerName;
    }
    public void SetFaceVisible(bool faceVisible)
    {
        // if(faceVisible) {
        //     cardBack.SetActive(false); // show card face
        // } else {
        //     cardBack.SetActive(true); // show card back
        // }
        // Debug.Log(spriteRenderer.sprite.name);
        cardBack.SetActive(!faceVisible);
    }

    private void OnMouseDown()
    {
        if(cardBack.activeSelf)
        {
            Messenger<Card>.Broadcast(GameEvent.CARD_CLICKED, this);
        }
        // SetFaceVisible(true);
    }
}
