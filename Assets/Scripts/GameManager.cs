using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    WorldGenerator      _worldGenerator;
    ExteriorBuilding    _exteriorBuilding;



    void Start()
    {
        _worldGenerator = FindObjectOfType<WorldGenerator>();
        _exteriorBuilding = FindObjectOfType<ExteriorBuilding>();

        print(_worldGenerator.name);
        print(_exteriorBuilding.name);

        var gridCells = _worldGenerator.Grid;
        foreach (var cell in gridCells)
        {
            if (_exteriorBuilding.IsUnlocked)
                cell.CheckIfInsideExteriorBuilding(_exteriorBuilding.BoxColliders);
        }
    }




    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(StartBuildingRoom());
        }
    }

    // Determine the start
    //###   grid cell at top left
    //###   
    //###   grid cell at bottom right

    private IEnumerator StartBuildingRoom()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        GridCell startCell = null;
        GridCell endCell = null;

        // ray cast to cell
        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            startCell = objectHit.GetComponentInParent<GridCell>();

            print(objectHit.parent.name);
        }

        // mark start cell under construction and build the room
        if (startCell != null)
        {
            startCell.CellUnderConstruction();

            while (Input.GetMouseButton(0))
            {
                yield return new WaitForEndOfFrame();

                

                
            }

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                endCell = objectHit.GetComponentInParent<GridCell>();

                print(objectHit.parent.name);
            }

            if (endCell != null)
            {
                endCell.CellUnderConstruction();

                FormRoom(startCell, endCell);
            }
        }

        // release or null
    }

    private List<GridCell> FormRoom(GridCell start, GridCell end)
    {
        List<GridCell> cells = _worldGenerator.SelectedGrid(start, end);

        // used to determine if we go up or down the list
        int direction = end.Index - start.Index;
        var grid = _worldGenerator.Grid;

        int i;
        if (direction > 0)
            i = start.Index;
        else
            i = end.Index;

        if (direction > 0)
        {
            for (; i < grid.Count; i++)
            {

            }
        }
        else
        {

        }

        // end - start // x,y
        return cells;
    }



    private void BuildRoom()
    {

    }
}
