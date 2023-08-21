using UnityEngine;
using UnityEngine.UI;
using GameProperties;

public class MinimapController : MonoBehaviour
{
    private GameObject sid;
    
    public GameStateTracker gameStateTracker;
    
    /// Images
    
    public RawImage iconSid;
    
    public RawImage iconForSideYardAcorn;
    
    public RawImage iconForYard1A1;
    
    public RawImage iconForYard1A2;
    
    public RawImage iconForYard1A3;
    
    public RawImage iconForYard1A4;
    
    public RawImage iconForYard1A5;
    
    public RawImage iconForYard1A6;
    
    public RawImage iconForYard2A1;
    
    public RawImage iconForYard2A2;
    
    public RawImage iconForYard2A3;
    
    public RawImage iconForTreeOfPlenty;
    
    /// Game Objects
    
    public GameObject sideYardAcorn;
    
    public GameObject yard1Acorn1;
    
    public GameObject yard1Acorn2;
    
    public GameObject yard1Acorn3;
    
    public GameObject yard1Acorn4;
    
    public GameObject yard1Acorn5;
    
    public GameObject yard1Acorn6;
    
    public GameObject yard2Acorn1;
    
    public GameObject yard2Acorn2;
    
    public GameObject yard2Acorn3;
    
    public GameObject treeOfPlenty;
    
    /// <summary>
    /// Half of the minimap's width.
    /// </summary>
    const float halfMapWidth = 150f;
    
    /// <summary>
    /// The drawable radius of items on the map. 
    /// Items beyond this distance are drawn on the map's border.
    /// </summary>
    const float maxDrawRadius = 130f;
    
    /// <summary>
    /// Distance multiplier to create visual separation from Sid for objects that are near to him.
    /// </summary>
    const int mapDistanceMultiplier = 10;
    
    void Start()
    {
        sid = GameObject.Find("Sid");
    }

    void Update()
    {
        if (!sideYardAcorn)
        {
            return;
        }
        
        PlotIconForObject(iconForSideYardAcorn, sideYardAcorn, Levels.sideYard);
        
        PlotIconForObject(iconForYard1A1, yard1Acorn1, Levels.yardOne);
        
        PlotIconForObject(iconForYard1A2, yard1Acorn2, Levels.yardOne);
        
        PlotIconForObject(iconForYard1A3, yard1Acorn3, Levels.yardOne);
        
        PlotIconForObject(iconForYard1A4, yard1Acorn4, Levels.yardOne);
        
        PlotIconForObject(iconForYard1A5, yard1Acorn5, Levels.yardOne);
        
        PlotIconForObject(iconForYard1A6, yard1Acorn6, Levels.yardOne);
        
        PlotIconForObject(iconForYard2A1, yard2Acorn1, Levels.yardTwo);
        
        PlotIconForObject(iconForYard2A2, yard2Acorn2, Levels.yardTwo);
        
        PlotIconForObject(iconForYard2A3, yard2Acorn3, Levels.yardTwo);
        
        PlotIconForObject(iconForTreeOfPlenty, treeOfPlenty, Levels.yardThree);
    }
    
    void PlotIconForObject(RawImage icon, GameObject target, Levels level)
    {
        
        var isActive = target.activeSelf && level == gameStateTracker.currentLevel;
        
        icon.gameObject.SetActive(isActive);
        
        if (!isActive)
        {
            return;
        }
        
        var objectTransform = sid.transform.InverseTransformPoint(target.transform.position);
        
        var targetRadians = Mathf.Atan2(
            objectTransform.x, 
            objectTransform.z
        );
        
        float distance = Mathf.Min(
            maxDrawRadius,
            Vector3.Distance(sid.transform.position, target.transform.position) * mapDistanceMultiplier
        );
        
        float xOffset = distance * Mathf.Cos(targetRadians) + halfMapWidth;
        float yOffset = distance * Mathf.Sin(targetRadians) + halfMapWidth;
        
        icon.rectTransform.position = new Vector3(yOffset, xOffset, 0f);
    }
}
