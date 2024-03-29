namespace Interfaces
{
    public interface IStackableWindow
    {
        bool IsInFocus { get; set; }
        void SetActive(bool isActive);
    }
}