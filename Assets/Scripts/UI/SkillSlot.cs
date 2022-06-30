using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SkillSlot : SkillIcon,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public Image skillImage;
    public Image IsActiveSkill;
    public TMP_Text skillName;
    public TMP_Text skillInfo;
    public List<SkillStack> skillStackList = new List<SkillStack>();
    public Button addStackbtn;
    public Button deleteStackbtn;
    public Image LevelUP;
    public int SkillTableId = 0;
    public static DragAndDropContainer _draganddropConstainer;

    PlayerSkillPopupUI _playerskillpopupui;

    private bool _isSkillActive = false;

    public void Set()
    {
        GetSkillStackList();
        addStackbtn.onClick.AddListener(AddSkillStack);
        deleteStackbtn.onClick.AddListener(DeleteSkillStack);
        //_playerskillpopupui = UIManager.Instance.Get<PlayerSkillPopupUI>(UIList.PlayerSkillPopupUI);
        _playerskillpopupui = GetComponentInParent<PlayerSkillPopupUI>();
        _draganddropConstainer = UIManager.Instance.Get<DragAndDropContainer>(UIList.DragAndDropContainer);
    }

    public void SetSkillActive(int skillid)
    {
        if (_playerskillpopupui == null)
            _playerskillpopupui = GetComponentInParent<PlayerSkillPopupUI>();
        if (_playerskillpopupui.CheckPlayerLevel(skillid))
        {
            IsActiveSkill.gameObject.SetActive(false);
            _isSkillActive = true;
        }
        else
        {
            IsActiveSkill.gameObject.SetActive(true);
            LevelUP.gameObject.SetActive(false);
            _isSkillActive = false;
        }
    }

    public void SetSkillImage(int tableid)
    {
        skillImage.sprite = PlayerSkill.SkillIconDic[tableid];
        if(skillImage != null)
            SetImage(skillImage);
    }

    public void SetSkillInfo(int tableid)
    {
        skillName.text = PlayerSkill.PlayerSkillStatDic[tableid].SkillName;
        SetActiveSkillInfo(tableid);
        SetStrengthenSkillInfo(tableid);
        SetInfo(tableid);
    }

    public void SetActiveSkillInfo(int skilltableid)
    {
        if (!PlayerSkill.PlayerSkillStatDic.ContainsKey(skilltableid))
            return;

        SkillType type = PlayerSkill.PlayerSkillStatDic[skilltableid].Type;
        string newskillinfo = string.Empty;
        if (type != SkillType.Active)
            return;
        if(type == SkillType.Active)
        {
            string attacktext = "SKillAttack : " + PlayerSkill.PlayerSkillStatDic[skilltableid].SkillAttack;
            string criticaltext = "SKillCritical : " + PlayerSkill.PlayerSkillStatDic[skilltableid].SkillCritical;
            string cooltimetext = "SKillCoolTime : " + PlayerSkill.PlayerSkillStatDic[skilltableid].CoolTime;
            string needMptext = "Mp : " + PlayerSkill.PlayerSkillStatDic[skilltableid].NeedMP;
            string needplayerlevel = "PlayerLevel : " + PlayerSkill.PlayerSkillStatDic[skilltableid].NeedPlayerLevel;
            newskillinfo = $"{attacktext}\n{criticaltext}\n{cooltimetext}\n{needMptext}\n{needplayerlevel}";
            skillInfo.text = newskillinfo;
        }
    }

    public void SetStrengthenSkillInfo(int skilltableid)
    {
        if (!PlayerSkill.PlayerSkillStatDic.ContainsKey(skilltableid))
            return;

        SkillType type = PlayerSkill.PlayerSkillStatDic[skilltableid].Type;
        string newinfo = string.Empty;
        if (type != SkillType.Strengthen)
            return;
        if(type == SkillType.Strengthen)
        {
            float atkper = PlayerSkill.PlayerSkillStatDic[skilltableid].AttackPercent;
            float defper = PlayerSkill.PlayerSkillStatDic[skilltableid].DefencePercent;
            float spdper = PlayerSkill.PlayerSkillStatDic[skilltableid].SpeedPercent;
            float time = PlayerSkill.PlayerSkillStatDic[skilltableid].SkillTime;

            string percent = $"For{time}time ,Attack:{atkper}%,Defence:{defper}%,Speed{spdper}% Plus";
            string cooltimetext = "SKillCoolTime : " + PlayerSkill.PlayerSkillStatDic[skilltableid].CoolTime;
            string needMptext = "Mp : " + PlayerSkill.PlayerSkillStatDic[skilltableid].NeedMP;
            string needplayerlevel = "PlayerLevel : " + PlayerSkill.PlayerSkillStatDic[skilltableid].NeedPlayerLevel;
            newinfo = $"{percent}\n{cooltimetext}\n{needMptext}\n{needplayerlevel}";
            skillInfo.text = newinfo;
        }
    }

    public void IsLevelUP()
    {
        if (_isSkillActive)
        {
            if (_playerskillpopupui.IsActive())
            {
                LevelUP.gameObject.SetActive(true);
            }
            else
                LevelUP.gameObject.SetActive(false);
        }
    }

    void GetSkillStackList()
    {
        SkillStack[] skillStacks = GetComponentsInChildren<SkillStack>();
        if(skillStacks != null)
        {
            for (int i = 0; i < skillStacks.Length; i++)
            {
                skillStackList.Add(skillStacks[i]);
                if (i == 0)
                {
                    skillStackList[i].DefaultSetting(true);
                }
                else
                {
                    skillStackList[i].DefaultSetting(false);
                }
            }
        }
    }

    void AddSkillStack()
    {
        if (!_isSkillActive)
            return;
        if (_playerskillpopupui.IsActive())
        {
            for (int i = 0; i < skillStackList.Count; i++)
            {
                if (!skillStackList[i].IsAddStack)
                {
                    if ((i + 1) <= skillStackList.Count)
                    {
                        skillStackList[i].AddStack();
                        _playerStat.SkillStack--;
                        PlayerSkill.SkillLevelUP(SkillTableId);
                        SetSkillInfo(SkillTableId);
                        _playerskillpopupui.ListIsActive();
                        return;
                    }

                }
            }
        }
    }

    void DeleteSkillStack()
    {
        if (PlayerSkillPopupUI.IsSaveSkillStat)
            return;
        if (!_isSkillActive)
            return;
        for (int i = skillStackList.Count - 1; i > 0; i--)
        {
            if (skillStackList[i].IsAddStack)
            {
                if ((i - 1) >= 0)
                {
                    skillStackList[i].DeleteStack();
                    PlayerSkill.SkillLevelDown(SkillTableId);
                    _playerStat.SkillStack++;
                    SetSkillInfo(SkillTableId);
                    _playerskillpopupui.ListIsActive();
                    return;
                }

            }
        }
    }

    public void ReSetSkillSlot()
    {
        SetSkillInfo(SkillTableId);
        SetSkillActive(SkillTableId);
        IsLevelUP();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_isSkillActive)
            return;
        _draganddropConstainer.ShowSkill(true, this);
        DragAndDropContainer.IsPlayerSkillPopup = true;
        dropImage = false;
        draging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _draganddropConstainer.Slot.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _draganddropConstainer.Slot.gameObject.SetActive(false);
        DragAndDropContainer.IsPlayerSkillPopup = false;
    }
}
