using UnityEngine;

namespace Game.Interfaces
{
    public interface IInteractable
    {
        string GetTitleName();
        Transform GetTransform();

        void OnCursorEnter(Vector3 hitPosition);
        void OnCursorClick();
        void OnCursorExit();
    }
}