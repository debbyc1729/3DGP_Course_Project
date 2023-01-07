using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    private float fadeSpeed = 1.0f;
    private float deltaTime;
    bool sceneOutStarting = false;
    bool sceneInStarting = false;
    private RawImage backImage;
    string SceneName;

    void Start()
    {
        backImage = this.GetComponent<RawImage>();
        backImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        deltaTime = Time.deltaTime;

        this.gameObject.SetActive(true);
    }

    void Update()
    {
        if (sceneOutStarting)
        {
            EndScene();
        }
        if (sceneInStarting)
        {
            StartScene();
        }
    }
    // 渐现
    private void FadeToClear()
    {
        backImage.color = Color.Lerp(backImage.color, Color.clear, fadeSpeed * deltaTime);
    }
    // 渐隐
    private void FadeToBlack()
    {
        backImage.color = Color.Lerp(backImage.color, Color.black, fadeSpeed * deltaTime);
    }
    // 初始化时调用
    private void StartScene()
    {
        backImage.enabled = true;
        FadeToClear();
        if (backImage.color.a <= 0.05f)
        {
            backImage.color = Color.clear;
            backImage.enabled = false;
            sceneInStarting = false;
        }
    }
    // 结束时调用
    public void EndScene()
    {
        backImage.enabled = true;
        FadeToBlack();
        if (backImage.color.a >= 0.99f) {
            if (SceneName == "Ending")
            {
                /*GameObject.Find("/Maze").transform.GetComponent<BGMPlayer>().BGMStop();
                GameObject.Find("/MineMaze").transform.GetComponent<BGMPlayer>().BGMStop();
                GameObject.Find("/MineMaze/BossRoom").transform.GetComponent<BGMPlayer>().BGMStop();
                GameObject.Find("/MineMaze").transform.GetComponent<BGMPlayer>().BGMStop();
                BGMPlayer[] bgmPlayer = FindObjectsOfType<BGMPlayer>();
                foreach (BGMPlayer b in bgmPlayer)
                {
                    b.BGMStop();
                }*/
                return;
            }
            if(SceneName == "Quit")
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneName);
        }
    }

    public void StartFadeOut(string SName)
    {
        BGMPlayer[] bgmPlayer = FindObjectsOfType<BGMPlayer>();
        foreach (BGMPlayer b in bgmPlayer)
        {
            Debug.Log("b.gameObject.name" + b.gameObject.name);
            b.BGMStop();
        }

        sceneOutStarting = true;
        SceneName = SName;

        if(SName == "Ending")
        {
            fadeSpeed = 0.3f;
        }
        else
        {
            fadeSpeed = 1.0f;
        }
    }
    public void StartFadeIn()
    {
        sceneInStarting = true;
    }
}
