using UnityEngine;

namespace RG.Match3.Scripts {
    public class GameData : MonoBehaviour {
        [Range(4, 10)] [SerializeField] private int _numOfRows = 10;
        [Range(4, 10)] [SerializeField] private int _numOfColumns = 10;
    
        public int NumOfRows => _numOfRows; 
        public int NumOfColumns => _numOfColumns; 
    
        public const float StartPosX = -4.5f;
        public const float StartPosY = 4.5f;
        public const float HopSize = 1f;
        
        public const float TileMovementTime = 0.7f;
        public const float MovementOffset = 1.05f; // 5%
        
        public Container[][] Grid { get; set; } // Grid of Containers

    }

    public enum TileShape {
        Square,
        Sphere,
        Cylinder,
        Barrel
    }
}