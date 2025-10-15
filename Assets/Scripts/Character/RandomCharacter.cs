using UnityEngine;

namespace Character
{
    public class RandomCharacter : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_characterMeshes;
        [SerializeField] private GameObject[] m_characterAccessories;
        
        private void Awake()
        {
            HideAll();
        }
        
        public void RandomizeCharacter()
        {
            HideAll();
            
            if (m_characterMeshes.Length > 0)
            {
                int randomMeshIndex = Random.Range(0, m_characterMeshes.Length);
                m_characterMeshes[randomMeshIndex].SetActive(true);
            }
            
            if (m_characterAccessories.Length > 0)
            {
                int randomAccessoryIndex = Random.Range(0, m_characterAccessories.Length);
                m_characterAccessories[randomAccessoryIndex].SetActive(true);
            }
        }
        
        private void HideAll()
        {
            foreach (GameObject mesh in m_characterMeshes)
            {
                mesh.SetActive(false);
            }
            
            foreach (GameObject accessory in m_characterAccessories)
            {
                accessory.SetActive(false);
            }
        }
    }
}