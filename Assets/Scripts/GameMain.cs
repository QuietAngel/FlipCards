using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMain : MonoBehaviour {

    public Button btnLevel1;
    public Button btnLevel2;
    public Button btnLevel3;

    public Transform panelStart;
    public Transform panelCard;
    public Transform panelOver;

    // Use this for initialization
    void Start () {
        btnLevel1.onClick.AddListener(()=> {
            panelStart.gameObject.SetActive(false);
            panelCard.gameObject.SetActive(true);
            LoadLevelCard(3, 2);
        });
        btnLevel2.onClick.AddListener(() => {
            panelStart.gameObject.SetActive(false);
            panelCard.gameObject.SetActive(true);
            LoadLevelCard(4, 2);
        });
        btnLevel3.onClick.AddListener(() => {
            panelStart.gameObject.SetActive(false);
            panelCard.gameObject.SetActive(true);
            LoadLevelCard(5, 2);
        });
        Button btnToStart = panelOver.Find("Button_to_start").GetComponent<Button>();
        btnToStart.onClick.RemoveAllListeners();
        btnToStart.onClick.AddListener(ToGameStartPage);
    }

    void LoadLevelCard(int width , int height)
    {
        //1加载卡牌图片
        Sprite [] sps = Resources.LoadAll<Sprite>("Sprite");
         
        //2计算需要加载卡牌的数量
        int totalCount = width * height/2;
        //3计算加载卡牌的索引
        List<Sprite> spsList = new List<Sprite>();
        for( int i = 0; i < sps.Length; i++)
        {
            spsList.Add(sps[i]);
        }

        List<Sprite> needShowCardList = new List<Sprite>();
        while (totalCount > 0)
        {
            int randomIndex = Random.Range(0,spsList.Count);
            needShowCardList.Add(spsList[randomIndex]);
            needShowCardList.Add(spsList[randomIndex]);
            spsList.RemoveAt(randomIndex);
            totalCount--;
        }

        //4显示卡牌到UI上
        Transform contentRoot = panelCard.Find("Panel");
        int maxCount = Mathf.Max( contentRoot.childCount,needShowCardList.Count);
        GameObject itemPrefab = contentRoot.GetChild(0).gameObject;
        for (int i = 0; i < maxCount; i++)
        {
            GameObject itemObject = null;
            if (i < contentRoot.childCount)
            {
                itemObject = contentRoot.GetChild(i).gameObject;
            }
            else
            {
                //clone
                itemObject = GameObject.Instantiate<GameObject>(itemPrefab);
                itemObject.transform.SetParent(contentRoot, false);
            }
            itemObject.transform.Find("Image_front").GetComponent<Image>().sprite = needShowCardList[i];
            CardFlipAnimationCtrl cardAniCtrl = itemObject.GetComponent<CardFlipAnimationCtrl>();
            cardAniCtrl.SetDefaultState();
        }

        GridLayoutGroup glg = contentRoot.GetComponent<GridLayoutGroup>();

        float panelWidth = width * glg.cellSize.x+glg.padding.left+
            glg.padding.right+(width-1)*glg.spacing.x;
        float panelHeight = height * glg.cellSize.y+glg.padding.top+
            glg.padding.bottom+(width-1)*glg.spacing.y;
        contentRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(panelWidth, panelHeight);
    }


    public void CheckIsGameOver()
    {
        CardFlipAnimationCtrl[] allCards =  GameObject.FindObjectsOfType<CardFlipAnimationCtrl>();
        if( allCards != null && allCards.Length > 0)
        {
            List<CardFlipAnimationCtrl> cardInFront = new List<CardFlipAnimationCtrl>();

            for( int i = 0; i < allCards.Length; i++)
            {
                CardFlipAnimationCtrl cardTem = allCards[i];
                if( cardTem.isInFront && !cardTem.isOver)
                {
                    cardInFront.Add(cardTem);
                }

                if(cardInFront.Count >= 2)
                {
                    string cardImageName1 = cardInFront[0].GetCardImageName();
                    string cardImageName2 = cardInFront[1].GetCardImageName();
                    if( cardImageName1 == cardImageName2)
                    {
                        cardInFront[0].MachSucess();
                        cardInFront[1].MachSucess();
                    }
                    else
                    {
                        cardInFront[0].MachFail();
                        cardInFront[1].MachFail();
                    }
                    allCards = GameObject.FindObjectsOfType<CardFlipAnimationCtrl>();
                    bool isAllOver = true;
                    for( int o = 0; o < allCards.Length; o++)
                    {
                        isAllOver &= allCards[o].isOver;
                    }
                    if( isAllOver)
                    {
                        ToGameOverPanel();
                    }  
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    public void ToGameOverPanel ()
    {
        panelStart.gameObject.SetActive(false);
        panelCard.gameObject.SetActive(false);
        panelOver.gameObject.SetActive(true);
    }

    public void ToGameStartPage()
    {
        panelStart.gameObject.SetActive(true);
        panelCard.gameObject.SetActive(false);
        panelOver.gameObject.SetActive(false);
    }
}
