using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldScene : BaseScene
{
    public override void Init()
    {
        base.Init();
        _player.transform.position = transform.position;
        if(_ingameui != null)
        {
            _ingameui.miniMap.Setting();
        }
    }

    private void Update()
    {
        base.Run();
    }

}
