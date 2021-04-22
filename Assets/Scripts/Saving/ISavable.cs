
namespace RPG.Saves
{
    interface ISavable
    {
        object CaptureState();

        void RestoreState(object state);
    }
}