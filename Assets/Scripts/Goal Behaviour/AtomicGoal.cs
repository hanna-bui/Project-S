
namespace Goal_Behaviour
{
    public class AtomicGoal<T> : Goal<T>
    {
        public AtomicGoal(T owner) : base(owner)
        {
            
        }

        public override void Activate()
        {
            
        }

        public override ProcessOptions Process()
        {
            return CurrentStatus;
        }

        public override void Terminate()
        {
            
        }

        public override bool HandleMessage(string message)
        {
            return true;
        }
    }
}
