using System.Collections.Generic;
using UnityEngine;

namespace RG.Match3.Scripts {
    public class Tile : MonoBehaviour {
        [SerializeField] private GameObject[] _shapes; // to set in the inspector
        public TileShape TileShape { get; private set; }

        private readonly Dictionary<TileShape, GameObject> _tileShapes = new Dictionary<TileShape, GameObject>();
    
        // Initialize the Tile with TileShape and Instantiate on the Scene
        public void Init(TileShape tileShape) {
            TileShape = tileShape;
            Instantiate(_tileShapes[tileShape], transform);
        }
        
        private void Awake() {
            SetTilesShapes(); // populate the Dictionary in Awake, because Dictionary is not Serializable 
        }

        private void SetTilesShapes() {
            _tileShapes.Add(TileShape.Square, _shapes[0]);
            _tileShapes.Add(TileShape.Sphere, _shapes[1]);
            _tileShapes.Add(TileShape.Cylinder, _shapes[2]);
            _tileShapes.Add(TileShape.Barrel, _shapes[3]);
        }
    }
}
