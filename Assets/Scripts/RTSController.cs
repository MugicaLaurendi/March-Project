 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class RTSController : MonoBehaviour
{
    public Camera mainCamera ;

    public Transform selectionAreaTransform ;

    public float distanceBetweenLines = 1 ;
    public float distanceBetweenUnits = 1 ;

    public GameObject targetReticule ;

    private Vector3 mouseLeftStartPosition ;
    private Vector3 mouseRightStartPosition ;
    private Vector3 mouseCurrentPosition ;
    private Vector3 mouseLeftFinalPosition ;
    private Vector3 mouseRightFinalPosition ;
    private Vector2 lastRightClickPosition ;

    private List<Unit> selectedUnitsList ;
    private List<Vector2> formationPositionsList ;
    private List<GameObject> targetReticuleList ;

    private List<int> testList ;

    //Awake is called before Start()

    void Awake()
    {
        selectedUnitsList = new List<Unit>();
        formationPositionsList = new List<Vector2>();
        targetReticuleList = new List<GameObject>();

        selectionAreaTransform.gameObject.SetActive(false);


    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mouseCurrentPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        

        if(Input.GetMouseButtonDown(0))
        {
        
            mouseLeftStartPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            selectionAreaTransform.gameObject.SetActive(true);
        }

        if(Input.GetMouseButton(0))
        {
         
            CreateSelectionArea();
            
        }

        if(Input.GetMouseButtonUp(0))
        {
        
            SelecteUnitsInArea();
            
        }

        if (Input.GetMouseButtonDown(1))
        {
            mouseRightStartPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        }

        if(Input.GetMouseButton(1))
        {
            DestroyTargetReticule();
            CreateLineFormation();

            CreateTargetReticule();
            

        }

        if(Input.GetMouseButtonUp(1))
        {
            DestroyTargetReticule();

            MoveSelectedUnitsTo(selectedUnitsList,formationPositionsList);
            lastRightClickPosition = mouseRightStartPosition ;
        }

        if(Input.GetKey(KeyCode.Space))
        {

        }

    


    }

    // Methods

    void CreateLineFormation()
    {
        formationPositionsList.Clear();

        int numberOfUnits = selectedUnitsList.Count();

        int numberOfUnitsOnOneLine = numberOfUnits;
        int numberOfUnitsOnSecondLine = numberOfUnits/2 ; // if total = 9, then total/2 = 4
        int numberOfUnitsOnFirstLine = numberOfUnits - numberOfUnitsOnSecondLine ;

        float OneLineLength = (numberOfUnitsOnOneLine - 1) * distanceBetweenUnits ;
        float firstLineLength = (numberOfUnitsOnFirstLine - 1) * distanceBetweenUnits ;
        float secondLineLength = (numberOfUnitsOnSecondLine - 1) * distanceBetweenUnits ;
        
        Vector2 mouseRightStartPositionV2 = mouseRightStartPosition ;
        Vector2 directionOfLine = mouseCurrentPosition - mouseRightStartPosition ;
        
        if(directionOfLine.x < 1 && directionOfLine.y < 1 && directionOfLine.x > -1 && directionOfLine.y > -1)
        {
            if(lastRightClickPosition != null)
            {
                directionOfLine = mouseRightStartPositionV2 - lastRightClickPosition ;
            }
            
            else 
            {
                directionOfLine = new Vector2(-1,0);
            }
        }


        //one unit

        if(numberOfUnits == 1)
        {
            formationPositionsList.Add(mouseRightStartPositionV2);
        }

        //less than 7 units --> one line formation
        
        else if(numberOfUnitsOnOneLine <7)
        {
            for(int i = 0 ; i < numberOfUnits ; i++)
            {
                Vector2 positionOnLine = mouseRightStartPositionV2 + (-Vector2.Perpendicular(directionOfLine.normalized) * (OneLineLength/2)) + (Vector2.Perpendicular(directionOfLine.normalized) * distanceBetweenUnits * i ) ;

                formationPositionsList.Add(positionOnLine);
            }

        }
        
        
        //more than 6 units --> two lines formation
        
        else
        {
            //First line creation

            for(int k = 0 ; k < numberOfUnitsOnFirstLine ; k++)
            {
                Vector2 positionOnFirstLine = mouseRightStartPositionV2 + (-Vector2.Perpendicular(directionOfLine.normalized) * (firstLineLength/2)) + (Vector2.Perpendicular(directionOfLine.normalized) * distanceBetweenUnits * k ) ;

                formationPositionsList.Add(positionOnFirstLine);
            }

            //Second line creation

            for(int j = 0 ; j < numberOfUnitsOnSecondLine ; j++)
            {
                Vector2 positionOnSecondLine = mouseRightStartPositionV2 + (-Vector2.Perpendicular(directionOfLine.normalized) * (secondLineLength/2)) + ( -(directionOfLine.normalized) * distanceBetweenLines ) + (Vector2.Perpendicular(directionOfLine.normalized) * distanceBetweenUnits * j ) ;

                formationPositionsList.Add(positionOnSecondLine);
            }

        }

    }

    void MoveSelectedUnitsTo(List<Unit> _selectedUnitsList , List<Vector2> _formationPositionsList)
    {
        // voir algorithme hongrois pour optimiser les trajets


        for(int i = 0 ; i < _selectedUnitsList.Count() ; i++)
        {

            Unit unit = _selectedUnitsList[i]; 

            UnitMovement unitMovement = unit.GetComponent<UnitMovement>();

            unitMovement.targetPosition = _formationPositionsList[i];
        }

        
        
    } 

    void CreateSelectionArea()
    {
        Vector3 lowerLeft = new Vector3(
                Mathf.Min(mouseLeftStartPosition.x, mouseCurrentPosition.x),
                Mathf.Min(mouseLeftStartPosition.y, mouseCurrentPosition.y)
            );

            Vector3 upperRight = new Vector3(
                Mathf.Max(mouseLeftStartPosition.x, mouseCurrentPosition.x),
                Mathf.Max(mouseLeftStartPosition.y, mouseCurrentPosition.y)
            );

            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;

    }

    void SelecteUnitsInArea()
    {
            mouseLeftFinalPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(mouseLeftStartPosition,mouseLeftFinalPosition);
            selectionAreaTransform.gameObject.SetActive(false);

            //deselect all ex units
            foreach (Unit unit in selectedUnitsList)
            {
                    unit.setSelectedVisible(false);
            }
            selectedUnitsList.Clear();

            //Select units within selection area

            foreach(Collider2D collider in collider2DArray)
            {
                Unit unit = collider.GetComponent<Unit>();
                selectedUnitsList.Add(unit);
                unit.setSelectedVisible(true);
            }


    }

    void CreateTargetReticule()
    {
       for(int i = 0 ; i < formationPositionsList.Count() ; i++)
        {
            Vector2 targetReticulePosition = formationPositionsList[i];

            GameObject _targetReticule = Instantiate(targetReticule,targetReticulePosition,new Quaternion());

            _targetReticule.SetActive(true);

            targetReticuleList.Add(_targetReticule);
        }
    }

    void DestroyTargetReticule()
    {
        foreach(GameObject targetReticule in targetReticuleList)
            {
                Destroy(targetReticule);
            }
    }


}

