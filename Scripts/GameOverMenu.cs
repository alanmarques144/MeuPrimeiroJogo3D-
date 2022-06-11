using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
  private LTDescr restartAnimation;

  [SerializeField]
  private TMPro.TextMeshProUGUI highScore;

  private void OnEnable() 
  {
    highScore.text = $"High Score: {GameManager.Instance.HighScore}";
    var rectTransform = GetComponent<RectTransform>();
    rectTransform.anchoredPosition = new Vector2(0, rectTransform.rect.height);  

    rectTransform.LeanMoveY(0, 0.75f).setEaseInOutElastic().delay = 0.5f;
    if(restartAnimation is null)
    {
    restartAnimation = GetComponentInChildren<TMPro.TextMeshProUGUI>().gameObject.LeanScale(new Vector3(1.2f, 1.2f), 0.3f).setLoopPingPong();
    }
    restartAnimation.resume();
  }
  public void Restart()
  {
      restartAnimation.pause();
      gameObject.SetActive(false);
      GameManager.Instance.Enable();
  }

  public void Quit()
  {
  #if UNITY_EDITOR    
      EditorApplication.isPlaying = false;
  #else      
      Application.Quit();
  #endif    
  }
}
