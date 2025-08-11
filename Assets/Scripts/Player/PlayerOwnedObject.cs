using UnityEngine;

namespace Player
{
    public class PlayerOwnedObject : MonoBehaviour
    {
        [SerializeField] private PlayerManager m_owner;
        
        public PlayerManager Owner => m_owner;
    }
}