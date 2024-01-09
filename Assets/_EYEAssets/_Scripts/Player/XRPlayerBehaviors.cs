using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit;
//using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class XRPlayerBehaviors : MonoBehaviour
{
    //private DynamicMoveProvider _continuousMover;
    //private SnapTurnProviderBase _snapTurnProvider;

    void Start()
    {
        //_continuousMover= GetComponent<DynamicMoveProvider>();
        //_snapTurnProvider = GetComponent<SnapTurnProviderBase>();
    }


    void Update()
    {
        
    }


    //Main Behaviors
    public void PrepareToDrive()
    {
        //_continuousMover.enabled = false;
        //_snapTurnProvider.enabled = false;
    }

    public void CancelDriving()
    {
        //_continuousMover.enabled = true;
        //_snapTurnProvider.enabled = true;
    }







}
