using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.UI
{
    public class PanelManager : MonoBehaviour
    {
        [SerializeField] private List<PanelEntry> panelEntries;
        private Dictionary<int, WindowBase> panelDictionary;

        private void Awake()
        {
            panelDictionary = new Dictionary<int, WindowBase>();
            foreach (var entry in panelEntries)
            {
                if (!panelDictionary.ContainsKey(entry.index))
                    panelDictionary.Add(entry.index, entry.panel);
                else
                    Debug.LogWarning($"Duplicate index {entry.index} found for panel {entry.panel.name}!");
            }
        }

        public void OpenPanelByIndex(int index)
        {
            if (panelDictionary.TryGetValue(index, out var panel))
            {
                foreach (var p in panelDictionary.Values)
                {
                    p.gameObject.SetActive(false);
                }

                panel.Open();
            }
            else
                Debug.LogWarning($"No panel found for index {index}");
        }

        public void CloseAllPanels()
        {
            foreach (var panel in panelDictionary.Values)
            {
                panel.Close();
            }
        }

        internal void OpenPanelByIndex(object winScreenIdx)
        {
            throw new NotImplementedException();
        }
    }
}

