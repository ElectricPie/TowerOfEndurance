using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    [Serializable]
    public struct TabButtonMapping : IEquatable<TabButtonMapping>
    {
        public Button TabButtonGameObject;
        public GameObject TabContentGameObject;

        public bool Equals(TabButtonMapping other)
        {
            return Equals(TabButtonGameObject, other.TabButtonGameObject) && Equals(TabContentGameObject, other.TabContentGameObject);
        }

        public override bool Equals(object obj)
        {
            return obj is TabButtonMapping other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TabButtonGameObject, TabContentGameObject);
        }
    }

    public class TabManager : MonoBehaviour
    {
        [SerializeField] private TabButtonMapping[] m_tabButtonMappings;

        private void Awake()
        {
            if (m_tabButtonMappings.Length == 0)
                return;
            
            // Register button click listeners
            foreach (TabButtonMapping mapping in m_tabButtonMappings)
            {
                TabButtonMapping capturedMapping = mapping;
                mapping.TabButtonGameObject.onClick.AddListener(() => ChangeTab(capturedMapping));
            }
            
            // Set initial tab
            ChangeTab(m_tabButtonMappings[0]);
        }
        
        private void ChangeTab(TabButtonMapping selectedMapping)
        {
            foreach (TabButtonMapping mapping in m_tabButtonMappings)
            {
                bool isSelected = mapping.Equals(selectedMapping);
                
                if (mapping.TabContentGameObject != null)
                {
                    mapping.TabContentGameObject.SetActive(isSelected);
                }

                if (mapping.TabButtonGameObject != null)
                {
                    mapping.TabButtonGameObject.interactable = !isSelected;
                }
            }
        }
    }
}