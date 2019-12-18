using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/***
 * 
 * 卡牌功能类
 * 
 * **/
public class CardFlipAnimationCtrl : MonoBehaviour,IPointerClickHandler {

    public Transform cardFront;
    public Transform cardBack;

    public float flipDuaration = 0.2f;

    public bool isInFront = false;
    public bool isOver = false;
    
    void Awake () {
        cardFront = transform.Find("Image_front");
        cardBack = transform.Find("Image_back");
	}


    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isInFront)
        {
            StartCoroutine(FlipCardToFront());
        }
        else
        {
            StartCoroutine(FlipCardToBack());
        }
              
    }

    IEnumerator FlipCardToFront()
    {
        //1.翻转反面到90度
        cardFront.gameObject.SetActive(false);
        cardBack.gameObject.SetActive(true);
        cardFront.rotation = Quaternion.identity;
        while( cardBack.rotation.eulerAngles.y>90)
        {
            cardBack.rotation *= Quaternion.Euler(0, Time.deltaTime*90f*(1f/flipDuaration), 0);
            if (cardBack.rotation.eulerAngles.y > 90)
            {
                cardBack.rotation = Quaternion.Euler( 0, 90 , 0 );
                break;
            }
            yield return new WaitForFixedUpdate();
        }

        //2.翻转正面到0度
        cardFront.gameObject.SetActive(true);
        cardBack.gameObject.SetActive(false);
        cardFront.rotation = Quaternion.Euler(0,90,0);
        while (cardFront.rotation.eulerAngles.y > 0)
        {
            cardFront.rotation *= Quaternion.Euler(0, -Time.deltaTime * 90f * (1f / flipDuaration), 0);
            if (cardFront.rotation.eulerAngles.y > 90)
            {
                cardFront.rotation = Quaternion.Euler(0, 0, 0);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        isInFront = true;

        Camera.main.gameObject.GetComponent<GameMain>().CheckIsGameOver();
    }

    IEnumerator FlipCardToBack()
    {
        //1.翻转正面到90度
        cardFront.gameObject.SetActive(true);
        cardBack.gameObject.SetActive(false);
        cardFront.rotation = Quaternion.identity;
        while (cardFront.rotation.eulerAngles.y < 90)
        {
            cardFront.rotation *= Quaternion.Euler(0, Time.deltaTime * 90f * (1f / flipDuaration), 0);
            if (cardFront.rotation.eulerAngles.y > 90)
            {
                cardFront.rotation = Quaternion.Euler(0, 90, 0);
                break;
            }
            yield return new WaitForFixedUpdate();
        }

        //2.翻转正面到0度
        cardFront.gameObject.SetActive(false);
        cardBack.gameObject.SetActive(true);
        cardBack.rotation = Quaternion.Euler(0, 90, 0);
        while (cardBack.rotation.eulerAngles.y > 0)
        {
            cardBack.rotation *= Quaternion.Euler(0, -Time.deltaTime * 90f * (1f / flipDuaration), 0);
            if (cardBack.rotation.eulerAngles.y > 90)
            {
                cardBack.rotation = Quaternion.Euler(0, 0, 0);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        isInFront = false;
    }

    internal void SetDefaultState()
    {
        cardFront.gameObject.SetActive(false);
        cardBack.gameObject.SetActive(true);
        isOver = false;
        isInFront = false;
        cardFront.rotation = Quaternion.identity;
        cardBack.rotation = Quaternion.identity;
    }

    internal string GetCardImageName()
    {
        return cardFront.GetComponent<Image>().sprite.name;
    }

    internal void MachSucess()
    {
        isOver = true;
        cardFront.gameObject.SetActive(false);
        cardBack.gameObject.SetActive(false);
    }

    internal void MachFail()
    {
        StartCoroutine(FlipCardToBack());
    }
}
