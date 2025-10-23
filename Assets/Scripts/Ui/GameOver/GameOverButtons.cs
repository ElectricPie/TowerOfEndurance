using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ui.GameOver
{
    public class GameOverButtons : MonoBehaviour
    {
        [SerializeField] private string m_mainMenuSceneName = "MainMenu";
        
        public void RestartLevel()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        public void QuitToMainMenu()
        {
            SceneManager.LoadScene(m_mainMenuSceneName);
        }
    }
}