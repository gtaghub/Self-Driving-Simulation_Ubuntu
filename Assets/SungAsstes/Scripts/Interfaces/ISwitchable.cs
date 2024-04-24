using UnityEngine;

public interface ISwitchable
{
    public bool IsActive {get;}
    public void Activate();
    public void DeActivate();
}
