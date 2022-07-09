using UnityEngine;

namespace RG.Match3.Scripts {
    public class Container : MonoBehaviour {
        public Tile tile;
        
        private void OnMouseUp() {
            // Don't allow clicking when the Container is empty or while some tiles are moving  
            if (tile == null || GridUpdater._instance.IsMoving) { 
                return;
            }

            // Destroy the Tile
            Destroy(tile.gameObject);
            tile = null;
        
            // Record changes in the Grid matrix and update the Grid on the screen
            GridUpdater._instance.UpdateGrid();
        }
    }
}