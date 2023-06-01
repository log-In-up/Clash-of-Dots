using Photon.Pun;
using TMPro;
using UnityEngine;

namespace UserInterface
{
#if UNITY_EDITOR
    [DisallowMultipleComponent]
#endif
    public sealed class PopupHandler : MonoBehaviour
    {
        #region Editor Fields
        [SerializeField] private TextMeshProUGUI _message = null;
        #endregion

        #region Public Methods
        internal void ShowPopup(string nickname, int coins)
        {
            _message.text = $"{nickname} with {coins} coins";
        }
        #endregion
    }
}
