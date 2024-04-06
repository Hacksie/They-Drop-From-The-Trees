namespace HackedDesign
{
    public interface IState
    {
        bool Playing { get; }
        void Begin();
        void Update();
        void FixedUpdate();
        void End();
        void Menu();
        void Select();
    }
}