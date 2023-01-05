using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Story : MonoBehaviour, IPointerDownHandler
{
    public int pageIndex;
    public StoryProperty[] property;
    Text chineseText;
    Text englishText;
    Button next;
    bool toPlay;
    Coroutine lastCoroutine;

    void Start()
    {
        this.pageIndex = 0;
        this.chineseText = transform.Find("Chinese").GetComponent<Text>();
        this.englishText = transform.Find("English").GetComponent<Text>();
        this.next = transform.Find("Next").GetComponent<Button>();
        this.next.onClick.AddListener(Next);
        this.SetUpProperty();
        this.toPlay = false;
        this.lastCoroutine = StartCoroutine(StoryAnimation());
    }

    void SetUpProperty()
    {
        this.property[0].chineseStory = "在錯綜復雜的迷宮裡，探險家孤獨地前行著。三天前觸發了迷宮裡的詛咒之後，迷宮的入口便消失了，而探險家至今仍未找到出口。糧食已經快要用盡，迷宮中甚至潛伏著魔物，飢餓勞累的探險家近乎絕望，只是漫無目的地行走著...";
        this.property[0].englishStory = "In the intricate maze, the explorer walks alone. Three days ago, the entrance to the maze disappeared after a curse was triggered, and the explorer has yet to find the exit. With food almost exhausted and even monsters lurking in the maze, the hungry and exhausted explorer is near desperate and just walks aimlessly ......";
        this.property[1].chineseStory = "映入眼簾的一絲光亮引起了探險家的注意，抬頭望去，眼前是一幅巨大的壁畫。壁畫中的人物一手舉著神秘的法杖、一手握著散發溫和光暈的寶石。強大而神聖的魔法從法杖中射出，不斷擊退追逐而來的魔物。壁畫的最後，迷宮的盡頭閃爍著耀眼的光芒，象徵著希望與自由...";
        this.property[1].englishStory = "A glimmer of light caught the explorer's attention, and when he looked up, he saw a huge fresco in front of him. The figure in the fresco holds a mysterious wand in one hand and a gemstone that emits a mild glow in the other. Powerful and sacred magic shoots out from the wand, constantly repelling the chasing demons. At the end of the mural, a dazzling light shines at the end of the maze, symbolizing hope and freedom...";
        this.property[2].chineseStory = "探險家低下頭，看見壁畫下方鑲嵌於牆壁中的法杖。突然間，耳邊傳來魔物的腳步聲，越來越近、越來越多。探險家不再猶豫，迅速拿起身前的法杖，回頭射出一發猛烈的火球。時而攻擊、時而退避，通往自由之旅才正要開始...";
        this.property[2].englishStory = "The explorer looked down and saw the wand embedded in the wall beneath the fresco. Suddenly, the sound of monster footsteps reached his ears, getting closer and closer. The explorer no longer hesitated, quickly picked up the wand in front of him and turned back to shoot a fierce fireball. Sometimes attacking, sometimes retreating, the journey to freedom is about to begin ...";
    }

    IEnumerator StoryAnimation()
    {
        int page = this.pageIndex;
        GetComponent<Image>().sprite = this.property[this.pageIndex].background;
        string chineseStory = this.property[this.pageIndex].chineseStory;
        string englishStory = this.property[this.pageIndex].englishStory;

        float timer = 0f;
        float duration = chineseStory.Length * 0.15f;

        while (timer < duration && page == this.pageIndex)
        {
            timer += Time.deltaTime;
            int chineseLength = (int) Mathf.Lerp(0, chineseStory.Length, timer / duration);
            int englishLength = (int) Mathf.Lerp(0, englishStory.Length, timer / duration);
            this.chineseText.text = chineseStory.Substring(0, chineseLength);
            this.englishText.text = englishStory.Substring(0, englishLength);
            yield return null;
        }

        yield break;
    }

    void Next()
    {
        if (this.toPlay)
        {
            Debug.Log("Play");
            return;
        }

        Debug.Log("Next");
        this.pageIndex += 1;

        if (this.pageIndex + 1 >= this.property.Length)
        {
            this.next.gameObject.transform.Find("Text").GetComponent<Text>().text = "Play";
            this.toPlay = true;
        }
        this.lastCoroutine = StartCoroutine(StoryAnimation());
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        // Debug.Log("OnPointerDown");
        if (this.lastCoroutine != null)
        {
            StopCoroutine(this.lastCoroutine);
            this.chineseText.text = this.property[this.pageIndex].chineseStory;
            this.englishText.text = this.property[this.pageIndex].englishStory;
        }
    }
}