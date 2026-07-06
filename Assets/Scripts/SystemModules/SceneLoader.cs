using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    [SerializeField] private Image _transitionImage;
    [SerializeField] private float _fadeTime = 3.5f;
    private Color _color;
    
    private const string MAINMENU = "MainMenu";
    private const string GAMEPLAY = "Gameplay";
    private const string SCORING = "Scoring";

    private IEnumerator LoadingCoroutine(string sceneName)
    {
         // 后台加载场景
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;
        
        // Fade out
        _transitionImage.gameObject.SetActive(true);
        while (_color.a < 1f)
        {
            _color.a = Mathf.Clamp01(_color.a + Time.unscaledDeltaTime / _fadeTime);
            _transitionImage.color = _color;
            yield return null;
        }

        yield return new WaitUntil(() => loadingOperation.progress >= 0.9f);
        
        // 激活后台加载好的场景
        loadingOperation.allowSceneActivation = true;
        
        // Fade In
        while (_color.a > 0f)
        {
            _color.a = Mathf.Clamp01(_color.a - Time.unscaledDeltaTime / _fadeTime);
            _transitionImage.color = _color;
            yield return null;
        }
        _transitionImage.gameObject.SetActive(false);
    }

    public void LoadGamePlayScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(GAMEPLAY));
    } 
    
    public void LoadMainMenuScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(MAINMENU));
    }
    
    public void LoadScoringScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(SCORING));
    }
}
