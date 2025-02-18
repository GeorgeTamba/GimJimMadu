#if UNITY_EDITOR
public interface ITab
{
    string TabName { get; }
    void OnGUI();
}
#endif