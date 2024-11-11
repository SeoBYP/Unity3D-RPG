# Unity3D-RPG

## 프로젝트 개요

이 RPG 게임은 Unity를 배우며 처음 제작한 프로젝트로, 다양한 RPG 게임의 필수 기능들을 구현하고 최적화에 집중한 프로젝트입니다. WOW, 리니지, 로스트아크 등 다양한 RPG 게임에서 사용되는 시스템을 직접 구현하여 사용자에게 몰입감 있는 RPG 경험을 제공하는 것을 목표로 했습니다.

## 프로젝트 요약

**프로젝트 기간**: 2021년 9월 - 2022년 3월  
**플랫폼**: PC
**엔진**: Unity  
**도구**: Git, C#

[게임 플레이 영상](https://www.youtube.com/watch?v=lf2wziqD6Kw&t=5s)

## ![게임 플레이 사진](image5.gif)

## 구현 상세

### 1. 인벤토리 시스템

![게임 플레이 사진](image7.gif)

- **회복 아이템**: 사용 시 체력 및 마나가 회복되는 아이템으로, 퀘스트 중이나 전투 중 필요할 때 바로 사용할 수 있도록 구현했습니다.
- **장비 아이템(무기, 방어구)**: 무기와 방어구 장비 아이템을 장착하면 캐릭터의 공격력과 방어력이 증가합니다. 각 장비는 다양한 능력치를 제공하며, 캐릭터의 전략적 성장을 돕습니다.
- **퀵슬롯**: 퀵슬롯에 회복 아이템이나 스킬을 장착하면, 단축키를 통해 빠르게 사용할 수 있도록 설정하여 전투에서의 편리함을 제공했습니다.

### 2. 전투 시스템

![게임 플레이 사진](image6.gif)

- **스킬 시스템**: 스킬을 퀵슬롯에 장착하면 해당 스킬을 사용할 수 있으며, 일반 공격보다 높은 데미지를 입힐 수 있습니다. 각 스킬은 특정 효과와 범위를 가지고 있어 다양한 전투 전략을 구성할 수 있습니다.
- **기본 콤보 공격**: 기본 공격으로 3단 콤보를 구현하여, 플레이어가 연속적으로 공격을 이어나갈 수 있게 했습니다. 이를 통해 몬스터와의 전투에서 더 큰 타격을 입힐 수 있습니다.
- **스킬 강화 시스템**: 캐릭터가 레벨업할 때마다 스킬 포인트를 획득하게 되고, 이 포인트를 사용하여 스킬의 데미지와 범위를 강화할 수 있도록 설계했습니다. 이를 통해 캐릭터 성장을 체감할 수 있도록 했습니다.

### 3. 상점 시스템

- **아이템 구매**: 게임 내 상점을 통해 회복 아이템 및 다양한 장비 아이템을 구매할 수 있도록 했습니다. 각 아이템의 가격과 설명이 표시되어, 플레이어가 전략적으로 자원을 활용해 아이템을 구매할 수 있습니다.

### 4. 퀘스트 시스템

- **퀘스트 수령 및 진행**: 플레이어는 게임 내 NPC를 통해 몬스터 사냥 등의 퀘스트를 받을 수 있으며, 퀘스트를 완료하면 보상을 획득하게 됩니다.
- **퀘스트 진행 상황 추적**: 퀘스트의 진행 상황을 실시간으로 확인할 수 있도록 하여, 플레이어가 퀘스트를 쉽게 관리하고 목표를 달성하도록 지원했습니다.

### 5. 던전 시스템

- **던전 입장 및 보스 전투**: 특정 지역에 입장하면 던전이 시작되며, 보스 몬스터를 포함한 고난도 전투를 통해 던전을 클리어할 수 있습니다.
- **던전 보상**: 던전을 클리어하면 보상 아이템을 획득할 수 있으며, 이는 캐릭터의 성장을 촉진시키는 데 도움을 줍니다. 던전 시스템은 플레이어에게 도전 욕구를 주고, 보상을 통해 성취감을 높였습니다.

---
