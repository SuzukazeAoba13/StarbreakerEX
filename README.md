# 🚀 StarbreakerEX

> 一款基于 Unity URP 的 2D 横版太空射击游戏（STG / Shmup），采用模块化架构与对象池管理，支持 Wave 制敌人生成与 Boss 战。

---

## 🎮 项目简介

| 项目 | 详情 |
|------|------|
| **游戏名称** | StarbreakerEX |
| **游戏类型** | 2D 横版卷轴射击（STG） |
| **核心玩法** | 操控星际战机，在弹幕中闪避并击破敌人，击败 Boss 获取高分 |
| **开发目的** | 个人练习 / 求职作品 |

---

## 📷 项目预览

<!-- TODO: 替换为实际截图/GIF -->

| 截图 | GIF 演示 | 视频链接 |
|------|----------|----------|
| ![screenshot](placeholder.png) | ![gif](placeholder.gif) | [▶️ 观看视频]() |

---

## 🛠 技术栈

| 类别 | 技术 |
|------|------|
| **引擎** | Unity 2022.3.62f3c1 |
| **语言** | C# |
| **渲染管线** | URP（Universal Render Pipeline） |
| **输入系统** | Unity Input System（InputActions + ScriptableObject） |
| **UI 文字** | TextMeshPro |
| **第三方资源** | Cartoon FX（VFX） |
| **动画** | Unity Animator / StateMachineBehaviour |

> 未发现 DOTween、Cinemachine、Addressables、Odin 等第三方插件依赖。

---

## ✅ 已实现功能

### 🎯 玩家系统
- [x] WASD 移动（加速 / 减速梯度 + 视口边界限制）
- [x] 武器系统 — 3 档火力等级，不同枪口配置
- [x] 闪避翻滚 — 能量消耗 + 贝塞尔曲线缩放动画 + 无敌帧
- [x] Overdrive 超载模式 — 子弹时间 + 射速/移速提升 + 材质切换 + 持续耗能
- [x] 导弹系统 — 有限弹药 + CD 冷却 + 拾取补给
- [x] 受伤无敌帧 + 自动回血

### 🤖 敌人系统
- [x] 普通敌人 — 右侧半场随机移动 + 随机射击间隔
- [x] Boss 战 — 每 N 波触发，含连续射击 + 光束武器 + 玩家位置追踪
- [x] Wave 波次制 — 难度递增（敌人数量 / 血量随波次增长）
- [x] 敌人血量头顶显示

### 🎒 掉落 & 拾取
- [x] 百分比概率掉落系统（LootSetting 数据驱动）
- [x] 拾取类型：武器升级、导弹补给、护盾回复、分数加成、能量
- [x] 拾取物追踪玩家动画 + 拾取特效

### 🧩 UI 系统
- [x] 主菜单 → 游戏 → 结算 完整流程
- [x] HUD（血量 / 能量 / 波次 / 导弹数量 / 分数）
- [x] 暂停菜单（继续 / 设置占位 / 返回主菜单）
- [x] Game Over 动画 + 确认进入计分
- [x] 排行榜 — Top 10 高分记录（JSON 持久化）
- [x] 新纪录输入界面
- [x] 异步场景加载 + 淡入淡出过渡

### 🎨 表现层
- [x] 2D 视差滚动星空背景
- [x] 子弹时间（Slow-Mo 缓入缓出）
- [x] 角色死亡 VFX + 命中 VFX
- [x] 引擎火焰 VFX（普通 / Overdrive 双状态）
- [x] 音效系统 — 随机音调变化 + AudioData 配置

### ⚙️ 工程化
- [x] 全对象池管理（敌人 / 玩家弹幕 / 敌人弹幕 / VFX / 掉落物）
- [x] 对象池运行时尺寸 Editor 警告
- [x] 通用泛型单例（场景内 + 跨场景持久化）

---

## 📁 项目结构

```
Assets
├── Animations/       # 动画控制器 & 动画片段（Boss/UI/LootItem）
├── Audio/            # 音效 & 音乐（角色/UI/环境/掉落物）
├── Fonts/            # 字体资源
├── Images/           # 图片素材（HUD/血条/背景/掉落物）
├── Materials/        # 材质（VFX/背景/光束/UI）
├── Models/           # 3D/2D 模型（战机/敌舰/弹幕/导弹）
├── Prefabs/          # 预制体（Player/Enemies/Projectiles/VFX/UI/SystemModules）
├── Resources/        # 动态加载资源（Cartoon FX）
├── Scenes/           # 场景（MainMenu / GamePlay / Scoring）
├── Scripts/
│   ├── Audio/        # 音效管理器 & AudioData
│   ├── Characters/   # 角色基类、Player、Enemy、Boss
│   ├── Input/        # InputActions & PlayerInput (ScriptableObject)
│   ├── LootItem/     # 掉落物 & 掉落配置
│   ├── Miscs/        # 工具（贝塞尔曲线/背景滚动/视口/自动旋转）
│   ├── PoolSystem/   # 通用对象池 & PoolManager
│   ├── Projectile/   # 弹幕基类 & 导弹制导 & 敌弹
│   ├── SystemModules/# 核心系统（GameManager/ScoreManager/SceneLoader/TimeController/SaveSystem/单例基类）
│   └── UI/           # UI 控制器（MainMenu / GamePlay / GameOver / Scoring）
├── Settings/         # URP 配置 & Input Action Asset
├── Shaders/          # Cartoon FX Shader
└── TextMesh Pro/     # TMP 资源
```

---

## 🏗 核心架构

| 模式 | 说明 |
|------|------|
| **Manager 管理器** | `GameManager`、`EnemyManager`、`ScoreManager`、`AudioManager`、`PoolManager` 各司其职，统一管理子系统 |
| **泛型单例** | `Singleton<T>`（场景内）与 `PersistentSingleton<T>`（跨场景 + DontDestroyOnLoad），避免重复实例 |
| **对象池** | `Pool` + `PoolManager` 统一管理所有可回收对象，支持 Editor 下尺寸不足警告 |
| **ScriptableObject 输入** | `PlayerInput` 基于 InputActions 生成，用 `CreateAssetMenu` 实例化，事件驱动解耦 |
| **继承层级** | `Character` → `Player` / `Enemy` → `Boss`；`EnemyController` → `BossController`，共享基类逻辑 |
| **数据驱动掉落** | `LootSetting` 配置掉落预制体 + 概率百分比，`LootSpawner` 执行 |
| **协程驱动** | 移动/射击/血量回复/时间缩放均通过 Coroutine 实现非阻塞时序控制 |
| **状态机** | `GameState` 枚举控制全局流程；Animator 驱动 UI 与角色动画状态 |

---

## ✨ 核心亮点

1. **完整的游戏闭环** — 主菜单 → 战斗 → 死亡 → 结算 → 排行榜 → 返回，流程完整可玩
2. **模块化解耦** — 输入 / 音效 / 分数 / 场景加载均通过 Manager + 事件隔离，模块间低耦合
3. **高性能对象池** — 所有频繁生成销毁的对象（弹幕/VFX/敌人/掉落物）纳入池管理，避免 GC Spike
4. **Overdrive 系统** — 子弹时间 + 属性增益 + VFX 材质切换，提供爽快的高风险高回报体验
5. **数据持久化** — JSON 本地存储 Top 10 排行榜，支持新纪录检测与玩家名输入
6. **Editor 工具** — 对象池尺寸不足自动警告，便于调试优化

---

## 🚀 如何运行

| 步骤 | 说明 |
|------|------|
| **Unity 版本** | 2022.3.62f3c1 或更高（Unity 2022 LTS） |
| **打开方式** | 在 Unity Hub 中 `Open` → 选择项目根目录 |
| **启动场景** | `Assets/Scenes/MainMenu.unity` |
| **额外资源** | 无需额外下载，所有依赖已包含在工程内 |

> ⚠️ 使用 URP 渲染管线，打开后若出现材质丢失，请在 `Window > Rendering > Render Pipeline Converter` 中转换材质。

---

## 📋 后续计划

- [ ] 更多敌人类型与弹幕模式
- [ ] 更多关卡 / 背景
- [ ] 设置菜单（音量 / 键位）
- [ ] 本地双人合作模式
- [ ] Boss Rush 模式

---

<p align="center">
  <sub>Made with Unity & C# | 个人作品</sub>
</p>
