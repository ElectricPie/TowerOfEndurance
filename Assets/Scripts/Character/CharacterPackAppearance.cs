using UnityEngine;

namespace Character
{
    public struct CharacterPackLoadout
    {
        public CharacterLoadout Loadout;
        public int PackIndex;

        public CharacterPackLoadout(int packIndex, CharacterLoadout loadout)
        {
            PackIndex = packIndex;
            Loadout = loadout;
        }
    }
    
    public class CharacterPackAppearance : MonoBehaviour
    {
        [SerializeField] private CharacterLoadoutAppearance[] m_characters;
        
        public void SetLoadout(CharacterPackLoadout loadout)
        {
            if (loadout.PackIndex < 0 || loadout.PackIndex >= m_characters.Length)
            {
                Debug.LogError($"CharacterPackAppearance: Pack index {loadout.PackIndex} is out of bounds.");
                return;
            }

            m_characters[loadout.PackIndex].gameObject.SetActive(true);
            m_characters[loadout.PackIndex].SetLoadout(loadout.Loadout);
        }
        
        public CharacterPackLoadout GetRandomPackLoadout()
        {
            if (m_characters.Length == 0)
            {
                Debug.LogError("CharacterPackAppearance: No character packs available.");
                return new CharacterPackLoadout(-1, new CharacterLoadout(-1, -1));
            }

            int randomPackIndex = Random.Range(0, m_characters.Length);
            CharacterLoadoutAppearance selectedPack = m_characters[randomPackIndex];
            CharacterLoadout loadout = selectedPack.GetRandomLoadout();

            return new CharacterPackLoadout(randomPackIndex, loadout);
        }
        
        public CharacterLoadoutAppearance GetCharacterLoadout(int index)
        {
            if (index < 0 || index >= m_characters.Length)
            {
                Debug.LogError($"CharacterPackAppearance: Index {index} is out of bounds.");
                return null;
            }

            return m_characters[index];
        }
    }
}