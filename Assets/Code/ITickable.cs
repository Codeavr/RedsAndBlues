namespace RedsAndBlues
{
    public interface ITickable
    {
        bool IsEnabled { get; set; }
        void Tick();
    }
}