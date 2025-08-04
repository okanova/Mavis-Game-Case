using _Project.Scripts.Core;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        ObjectPoolManager.InitializePools();
        
        // SaveManager.LoadAll();
       // SaveManager.SaveAll();
      
    }
}
