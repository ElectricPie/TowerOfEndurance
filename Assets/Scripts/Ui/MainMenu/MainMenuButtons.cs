using UnityEngine;

namespace Ui.MainMenu
{
    public class MainMenuButtons : MonoBehaviour
    {
        [SerializeField] string m_gameSceneName = "IslandTower";

        public void OnPlayButtonPressed()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(m_gameSceneName);
        }

        public void OnQuitButtonPressed()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif

            Application.Quit();
        }
    }
}