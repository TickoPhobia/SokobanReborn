using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

  [CustomEditor(typeof (Root))]
public class RootEditor : UnityEditor.Editor {

	    public override void OnInspectorGUI()
        {
            var grid = (Root) target;
            base.OnInspectorGUI();


            if (GUILayout.Button("Deploy Ground"))
            {
                DeployGrid(grid);
            }
            if (GUILayout.Button("Delete Ground"))
            {
                DeleteGround(grid);
            }
        }

        private void DeleteGround(Root grid)
        {
            List<Transform> deleteList = new List<Transform>(grid.transform.childCount);
            for (int i = 0; i < grid.transform.childCount; i++)
            {
                deleteList.Add(grid.transform.GetChild(i));
            }

            foreach (var item in deleteList)
            {
                DestroyImmediate(item.gameObject);
            }
        }

        private void DeployGrid(Root grid)
        {


            for (int i = 0; i < grid.Height; i++)
            {
                for (int j = 0; j < grid.Width; j++)
                {

                    var ground =
                        (GameObject)
                            Instantiate(grid.GroundPlaceHolderPrefab, new Vector3(i, 0, j), Quaternion.identity);

                    ground.transform.parent = grid.transform;
                    
                }

            }
		}
}
