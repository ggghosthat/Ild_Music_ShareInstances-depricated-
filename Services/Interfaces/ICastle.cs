namespace ShareInstances.Services.Interfaces;
public interface ICastle
{
    public bool IsCenterActive {get; set;}

    public void OnCenterRegisterActivate();
    
    public void RegisterCube(ICube cube); 

    public void RegisterPlayer(IPlayer player);
}
