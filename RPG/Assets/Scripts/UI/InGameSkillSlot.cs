using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InGameSkillSlot : SkillIcon,IDropHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private SkillCoolTimer timer;
    private SkillCoolTime coolTimeImage;
    private Button button;
    private DragAndDropContainer dropContainer;
    public Image Skillimage;
    public bool update = false;
    private float ElapsedTIme = 0;
    private int thisskillslotid = 0;

    public void SetSkillImage(Image sprite)
    {
        SetImage(sprite);
        
        //SetSprite(sprite);
        //Skillimage.sprite = sprite;
    }

    public void SetSkillSlotID(int index)
    {
        thisskillslotid = index;
    }

    public void SetSKillSprite(int skillid)
    {
        if(PlayerSkill.SkillIconDic.ContainsKey(skillid))
            Skillimage.sprite = PlayerSkill.SkillIconDic[skillid];
        SetImage(Skillimage);
    }

    public void Set()
    {
        timer = GetComponentInChildren<SkillCoolTimer>(true);
        if (timer != null)
            timer.Init();
        coolTimeImage = GetComponentInChildren<SkillCoolTime>(true);
        if (coolTimeImage != null)
            coolTimeImage.Init();
        button = GetComponentInChildren<Button>();
        if (button != null)
            button.onClick.AddListener(OnClick);
        dropContainer = UIManager.Instance.Get<DragAndDropContainer>(UIList.DragAndDropContainer);
        LoadEmptyIcon();
        SetImage(Skillimage);
    }

    void OnClick()
    {
        _player.SkillTriger(thisskillslotid);
    }

    public void Execute()
    {
        if (IsEmpty())
        {
            return;
        }
        if (button.enabled == false)
            return;
        
        if (timer != null)
            timer.Execute(SkillCoolTime);
        if (coolTimeImage != null)
            coolTimeImage.Execute(SkillCoolTime);
        button.enabled = false;
        update = true;
        ElapsedTIme = 0;
    }

    private void Update()
    {
        if (update)
        {
            ElapsedTIme += Time.deltaTime / SkillCoolTime;
            if (ElapsedTIme > 1.0f)
            {
                button.enabled = true;
                update = false;
                ElapsedTIme = 0;
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (dropContainer.skillicon == null)
            return;
        if(IsEmpty())
        {
            int skilltableid = dropContainer.skillicon.SkillId;
            if (!DragAndDropContainer.IsPlayerSkillPopup)
                dropContainer.skillicon.Icon = _EmptyIcon;
            if (DragAndDropContainer.IsPlayerSkillPopup)
            {
                SkillAndItemSlotGroup skillgroup = GetComponentInParent<SkillAndItemSlotGroup>();
                skillgroup.CheckSkillSlotList(skilltableid);
            }
            SetInfo(skilltableid);

            dropContainer.skillicon.IsHaveSkill = false;
            dropContainer.skillicon = null;
            dropContainer.SetActive(false);
            DragAndDropContainer.IsPlayerSkillPopup = false;
        }

        else if(!IsEmpty())
        {
            int prevtableID = SkillId;
            int skilltableid = dropContainer.skillicon.SkillId;

            SetInfo(skilltableid);

            dropContainer.skillicon.SetInfo(prevtableID);
            dropContainer.skillicon = null;
            DragAndDropContainer.IsItemPopup = false;
        }
        IsHaveSkill = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (update)
            return;
        if (!IsEmpty())
        {
            dropContainer.ShowSkill(true, this);
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        dropContainer.Slot.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dropContainer.skillicon = null;
        DragAndDropContainer.IsPlayerSkillPopup = false;
        dropContainer.SetActive(false);
    }
}
