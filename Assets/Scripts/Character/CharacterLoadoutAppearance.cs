using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Character
{
    public struct CharacterLoadout
    {
        public int MeshIndex;
        public int AccessoryIndex;

        public CharacterLoadout(int meshIndex, int accessoryIndex)
        {
            MeshIndex = meshIndex;
            AccessoryIndex = accessoryIndex;
        }
    }
    
    public class CharacterLoadoutAppearance : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_characterMeshes = Array.Empty<GameObject>();
        [SerializeField] private GameObject[] m_characterAccessories = Array.Empty<GameObject>();

        public int MeshCount => m_characterMeshes.Length;
        public int AccessoryCount => m_characterAccessories.Length;
        
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
        
        public void SetLoadout(CharacterLoadout loadout)
        {
            if (loadout.MeshIndex < 0 || loadout.MeshIndex >= m_characterMeshes.Length)
            {
                Debug.LogError($"CharacterLoadoutAppearance: Mesh index {loadout.MeshIndex} is out of bounds.");
                return;
            }
            
            if (loadout.AccessoryIndex < 0 || loadout.AccessoryIndex >= m_characterAccessories.Length)
            {
                Debug.LogError($"CharacterLoadoutAppearance: Accessory index {loadout.AccessoryIndex} is out of bounds.");
                return;
            }
            
            SetCharacterLoadout(loadout.MeshIndex, loadout.AccessoryIndex);
        }
        
        public CharacterLoadout GetRandomLoadout()
        {
            int meshIndex = -1;
            int accessoryIndex = -1;

            if (m_characterMeshes.Length > 0)
            {
                meshIndex = Random.Range(0, m_characterMeshes.Length);
            }

            if (m_characterAccessories.Length > 0)
            {
                accessoryIndex = Random.Range(0, m_characterAccessories.Length);
            }

            return new CharacterLoadout(meshIndex, accessoryIndex);
        }
        
        public void SetCharacterLoadout(int meshIndex, int accessoryIndex)
        {
            HideAll();

            if (meshIndex < 0 || meshIndex >= m_characterMeshes.Length)
            {
                Debug.LogWarning($"CharacterLoadoutAppearance: Mesh index {meshIndex} is out of bounds.");
                return;
            }

            if (accessoryIndex < 0 || accessoryIndex >= m_characterAccessories.Length)
            {
                Debug.LogWarning($"CharacterLoadoutAppearance: Accessory index {accessoryIndex} is out of bounds.");
                return;
            }

            m_characterMeshes[meshIndex].SetActive(true);
            m_characterAccessories[accessoryIndex].SetActive(true);
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