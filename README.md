# 一个玩绳子的 Mod

[查看详细信息](https://www.gta5-mods.com/scripts/rope-creator#description_tab)

## 安装

1. 安装 Script Hook V。
2. 安装 Script Hook v .NET。
3. 安装 LemonUI（将 LemonUI.SHVDN2.dll 提取到 scripts 文件夹）。
4. 将 RopeCreator.dll 和 RopeCreator.ini 复制到 GTA V 目录的 scripts 文件夹中。

## 键盘上的用法

- 按住瞄准键，然后按 E 设置第一个位置，然后再次执行此操作以创建绳索。
- 按 Z 键可删除您创建的最后一根绳索。
- 按 F6 打开菜单。

## 控制器上的使用

- 按住瞄准按钮，然后按冲刺按钮，然后再次执行此操作以创建绳索。
- 按左 DPad 按钮删除您创建的最后一根绳索。
- 按 RB 然后一起冲刺按钮打开菜单。

## 编辑绳索组

选择一个绳索组（Rope Group），然后在绳索组（Rope Group）列表上按 Enter，这将打开一个子菜单。您还可以通过选择 “Edit specific rope” 项，从组子菜单中编辑特定绳索。

## INI 文件中的自定义设置

- **AttachKey**：用于创建绳索的键。
- **AttachPadButton**：用于创建绳索的控制器按钮。
- **RemoveLastRopeKey**：用于删除您创建的最后一根绳索的键。
- **RemoveLastRopePadButtons**：用于删除您创建的最后一根绳索的控制器按钮（用 + 分隔多个按钮）。
- **MenuToggleKey**：打开/关闭菜单的键。
- **MenuTogglePadButtons**：用于打开/关闭菜单的控制器按钮（用 + 分隔多个按钮）。
- **EnabledOnStartup**：是否应该在启动时启用该 mod。
- **ShowAimMarker**：在绳索连接或不连接的位置显示标记。
- **ShowRopeMarkers**：在编辑特定绳索时显示标记。
- **OnlyCreateRopeWhenAiming**：只允许在瞄准或不瞄准时创建绳索。
- **OnlyShowMarkerWhenAiming**：仅在瞄准或不瞄准时显示瞄准标记。
- **AllowTargetingOfCurrentVehicle**：允许将绳子连接到玩家所在的车辆上。
- **MaxRopes**：一次允许生成的最大绳索数量。
- **MaxDistance**：您可以瞄准的与玩家的最大距离。
- **DefaultType**：绳子的默认类型。
- **DefaultBreakable**：启动时绳索的默认可破坏设置。
- **DefaultAttachToObjectBone**：是否将绳索附加到对象的骨骼上。这允许绳子随对象的骨骼移动。这也使得物体在绳索的另一端是不可移动的。
- **DefaultAttachToPedBone**：是否将绳索附加到脚的骨骼上。这允许绳索随着脚踏的骨骼移动。这也使得 ped 在绳索的另一端是不可移动的。
- **DefaultAttachToNothing**：将绳索附加到任何对象（例如天空）或不附加。

## 更新日志

### 1.2 版

- 添加绳组。
- 从 NativeUI 转换为 LemonUI。
- 添加了附加到任何事物（例如天空）的能力。
- 修复了一些错误。

### 1.1 版

- 添加菜单项以更改游戏中的“附加到对象骨骼”设置。
- 添加 “attach to ped bone” 菜单项和设置。
- 默认情况下，现在使 peds 可移动。
- 为每个附加点分别考虑“attach to ped bone”和“attach to object bone”设置。这意味着您可以更改菜单中一个连接点的设置，然后更改回下一个连接点的设置。
- 将“AttachToObjectBone”设置重命名为“DefaultAttachToObjectBone”。
