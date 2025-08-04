public interface ISavable
{
    string SaveKey { get; }                    
    object CaptureState();                     
    void RestoreState(object state);

    void OnEnable();
    void OnDisable();
}