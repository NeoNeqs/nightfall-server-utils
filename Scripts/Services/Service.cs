using Godot;


namespace NightFallServersUtils.Scripts.Services
{
    public class Service : Node
    {

        public void Quit(int errorCode)
        {
#if DEBUG
            if (IsInsideTree())
#endif
                GetTree().Quit(-errorCode);
        }
    }
}