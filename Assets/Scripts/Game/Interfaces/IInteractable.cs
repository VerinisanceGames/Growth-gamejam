using UnityEngine;

namespace Game.Interfaces
{
    public interface IInteractable
    {
        string GetTitleName();
        Transform GetTransform();

        void OnCursorEnter();
        void OnCursorClick();
        void OnCursorExit();
    }
}