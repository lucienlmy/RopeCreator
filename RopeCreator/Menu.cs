using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using GTA;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;

namespace RopeCreator;

internal class Menu : Script
{
	private static readonly string[] ROPE_NAMES = new string[5] { "Rope", "Thick rope", "Thick cable", "Medium cable", "Cable" };

	private static readonly List<int> NO_ROPES_INDICES = new List<int> { -1 };

	private static ObjectPool objPool;

	internal static NativeMenu mainMenu;

	internal static NativeMenu editGroupMenu;

	internal static NativeMenu editRopeMenu;

	internal static NativeItem miDeleteLast;

	internal static NativeItem miDeleteAll;

	internal static NativeCheckboxItem cbEnabled;

	internal static NativeCheckboxItem cbWindAll;

	internal static NativeCheckboxItem cbUnwindAll;

	internal static NativeCheckboxItem cbShowAimMarker;

	internal static NativeCheckboxItem cbShowEditMarkers;

	internal static NativeCheckboxItem cbBreakable;

	internal static NativeCheckboxItem cbAttachObjBone;

	internal static NativeCheckboxItem cbAttachPedBone;

	internal static NativeCheckboxItem cbAttachNothing;

	internal static NativeListItem<int> liGroupIndex;

	internal static NativeListItem<string> liType;

	internal static NativeSliderItem siSlack;

	internal static NativeSliderItem siMinLength;

	internal static NativeItem miDeleteGroup;

	internal static NativeCheckboxItem cbWindGroup;

	internal static NativeCheckboxItem cbUnwindGroup;

	internal static NativeItem miDelete;

	internal static NativeCheckboxItem cbWind;

	internal static NativeCheckboxItem cbUnwind;

	internal static NativeListItem<int> liRopeIndex;

	internal static bool modEnabled = false;

	internal static bool showAimMarker = true;

	internal static bool showEditMarkers = true;

	internal static bool breakable = false;

	internal static bool attachObjBone = false;

	internal static bool attachPedBone = false;

	internal static bool attachNothing = false;

	internal static int type = 1;

	internal static float slack = 0.25f;

	internal static float minLength = 0.25f;

	private static NativeMenu lastVisibleMenu;

	private static bool settingChecks = false;

	public Menu()
	{
		objPool = new ObjectPool();
		mainMenu = new NativeMenu("Rope Creator");
		mainMenu.UseMouse = true;
		mainMenu.Opening += MainMenu_Opening;
		mainMenu.ItemActivated += MainMenu_ItemActivated;
		lastVisibleMenu = mainMenu;
		objPool.Add(mainMenu);
		cbEnabled = new NativeCheckboxItem("Enabled", modEnabled);
		cbEnabled.CheckboxChanged += MainMenu_CheckboxItem_CheckChanged;
		liGroupIndex = new NativeListItem<int>("Rope group", 1, 2, 3, 4, 5);
		liGroupIndex.ItemChanged += liGroupIndex_ItemChanged;
		miDeleteLast = new NativeItem("Delete last rope");
		miDeleteAll = new NativeItem("Delete all ropes");
		cbWindAll = new NativeCheckboxItem("Wind all ropes", check: false);
		cbWindAll.CheckboxChanged += MainMenu_CheckboxItem_CheckChanged;
		cbUnwindAll = new NativeCheckboxItem("Unwind all ropes", check: false);
		cbUnwindAll.CheckboxChanged += MainMenu_CheckboxItem_CheckChanged;
		cbShowAimMarker = new NativeCheckboxItem("Show aim marker", showAimMarker);
		cbShowAimMarker.CheckboxChanged += MainMenu_CheckboxItem_CheckChanged;
		cbShowEditMarkers = new NativeCheckboxItem("Show edit rope markers", showEditMarkers);
		cbShowEditMarkers.CheckboxChanged += MainMenu_CheckboxItem_CheckChanged;
		cbBreakable = new NativeCheckboxItem("Breakable", breakable);
		cbBreakable.CheckboxChanged += MainMenu_CheckboxItem_CheckChanged;
		liType = new NativeListItem<string>("Type", ROPE_NAMES);
		liType.ItemChanged += liType_ItemChanged;
		siSlack = new NativeSliderItem("Slack", slack.ToString("0.00") + " meters");
		siSlack.Value = (int)(slack * 4f);
		siSlack.Multiplier = 1;
		siSlack.Maximum = 200;
		siSlack.ValueChanged += MainMenu_SliderItem_ValueChanged;
		siMinLength = new NativeSliderItem("Minimum length", minLength.ToString("0.00") + " meters");
		siMinLength.Value = (int)(minLength * 4f);
		siMinLength.Multiplier = 1;
		siMinLength.Maximum = 100;
		siMinLength.ValueChanged += MainMenu_SliderItem_ValueChanged;
		cbAttachObjBone = new NativeCheckboxItem("Attach to object bone", attachObjBone);
		cbAttachObjBone.CheckboxChanged += MainMenu_CheckboxItem_CheckChanged;
		cbAttachPedBone = new NativeCheckboxItem("Attach to ped bone", attachPedBone);
		cbAttachPedBone.CheckboxChanged += MainMenu_CheckboxItem_CheckChanged;
		cbAttachNothing = new NativeCheckboxItem("Attach to nothing", attachNothing);
		cbAttachNothing.CheckboxChanged += MainMenu_CheckboxItem_CheckChanged;
		editGroupMenu = new NativeMenu("Edit Group");
		editGroupMenu.UseMouse = true;
		editGroupMenu.Parent = mainMenu;
		editGroupMenu.Opening += EditGroupMenu_Opening;
		objPool.Add(editGroupMenu);
		miDeleteGroup = new NativeItem("Delete all");
		miDeleteGroup.Activated += miDeleteGroup_Activated;
		cbWindGroup = new NativeCheckboxItem("Wind all");
		cbWindGroup.CheckboxChanged += EditGroup_CheckboxItem_CheckChanged;
		cbUnwindGroup = new NativeCheckboxItem("Unwind all");
		cbUnwindGroup.CheckboxChanged += EditGroup_CheckboxItem_CheckChanged;
		editRopeMenu = new NativeMenu("Edit Rope", "Edit specific rope");
		editRopeMenu.UseMouse = true;
		editRopeMenu.Opening += EditRopeMenu_Opening;
		objPool.Add(editRopeMenu);
		liRopeIndex = new NativeListItem<int>("Rope index", NO_ROPES_INDICES.ToArray());
		liRopeIndex.ItemChanged += liRopeIndex_ItemChanged;
		miDelete = new NativeItem("Delete");
		miDelete.Activated += miDelete_Activated;
		cbWind = new NativeCheckboxItem("Wind", check: false);
		cbWind.CheckboxChanged += EditRope_CheckboxItem_CheckChanged;
		cbUnwind = new NativeCheckboxItem("Unwind", check: false);
		cbUnwind.CheckboxChanged += EditRope_CheckboxItem_CheckChanged;
		mainMenu.Add(cbEnabled);
		mainMenu.Add(liGroupIndex);
		mainMenu.Add(miDeleteLast);
		mainMenu.Add(miDeleteAll);
		mainMenu.Add(cbWindAll);
		mainMenu.Add(cbUnwindAll);
		mainMenu.Add(cbShowAimMarker);
		mainMenu.Add(cbShowEditMarkers);
		mainMenu.Add(cbBreakable);
		mainMenu.Add(liType);
		mainMenu.Add(siSlack);
		mainMenu.Add(siMinLength);
		mainMenu.Add(cbAttachObjBone);
		mainMenu.Add(cbAttachPedBone);
		mainMenu.Add(cbAttachNothing);
		editGroupMenu.AddSubMenu(editRopeMenu);
		editGroupMenu.Add(miDeleteGroup);
		editGroupMenu.Add(cbWindGroup);
		editGroupMenu.Add(cbUnwindGroup);
		editRopeMenu.Add(liRopeIndex);
		editRopeMenu.Add(miDelete);
		editRopeMenu.Add(cbWind);
		editRopeMenu.Add(cbUnwind);
		base.Tick += Menu_Tick;
	}

	private void MainMenu_Opening(object sender, CancelEventArgs e)
	{
		lastVisibleMenu = mainMenu;
		SetWindChecks();
	}

	private void liGroupIndex_ItemChanged(object sender, ItemChangedEventArgs<int> e)
	{
		ReloadRopeIndices(selectLast: true);
	}

	private void MainMenu_ItemActivated(object sender, ItemActivatedArgs e)
	{
		_ = RopeCreator.ropeGroups[liGroupIndex.SelectedIndex];
		if (e.Item == liGroupIndex)
		{
			//mainMenu.Close();
			mainMenu.Visible = false;
			if (!mainMenu.Visible)
			{
				editGroupMenu.Title = new ScaledText(PointF.Empty, $"Edit Group {liGroupIndex.SelectedItem}", 1.02f, GTA.Font.HouseScript)
				{
					Color = Color.White,
					Alignment = Alignment.Center
				};
				editGroupMenu.Recalculate();
				//editGroupMenu.Open();
				editGroupMenu.Visible = true;
			}
		}
		else if (e.Item == miDeleteLast)
		{
			RopeCreator.DeleteLastRope();
			ReloadRopeIndices();
		}
		else if (e.Item == miDeleteAll)
		{
			RopeCreator.DeleteAllRopes();
			ReloadRopeIndices();
		}
	}

	private void MainMenu_SliderItem_ValueChanged(object sender, EventArgs e)
	{
		if (sender == siSlack)
		{
			slack = (float)siSlack.Value * 0.25f;
			siSlack.Description = slack.ToString("0.00") + " meters";
		}
		else if (sender == siMinLength)
		{
			minLength = (float)siMinLength.Value * 0.25f;
			siMinLength.Description = minLength.ToString("0.00") + " meters";
		}
		mainMenu.Recalculate();
	}

	private void liType_ItemChanged(object sender, ItemChangedEventArgs<string> e)
	{
		type = liType.SelectedIndex + 1;
	}

	private void MainMenu_CheckboxItem_CheckChanged(object sender, EventArgs e)
	{
		if (settingChecks)
		{
			return;
		}
		if (sender == cbEnabled)
		{
			modEnabled = cbEnabled.Checked;
		}
		else if (sender == cbShowAimMarker)
		{
			showAimMarker = cbShowAimMarker.Checked;
		}
		else if (sender == cbShowEditMarkers)
		{
			showEditMarkers = cbShowEditMarkers.Checked;
		}
		else if (sender == cbBreakable)
		{
			breakable = cbBreakable.Checked;
		}
		else if (sender == cbAttachObjBone)
		{
			attachObjBone = cbAttachObjBone.Checked;
		}
		else if (sender == cbAttachPedBone)
		{
			attachPedBone = cbAttachPedBone.Checked;
		}
		else if (sender == cbAttachNothing)
		{
			attachNothing = cbAttachNothing.Checked;
		}
		else if (sender == cbWindAll)
		{
			if (cbWindAll.Checked)
			{
				RopeCreator.StartWindAllRopes();
			}
			else
			{
				RopeCreator.StopWindAllRopes();
			}
			cbWindAll.Checked = cbWindAll.Checked;
			cbUnwindAll.Checked = false;
		}
		else if (sender == cbUnwindAll)
		{
			if (cbUnwindAll.Checked)
			{
				RopeCreator.StartUnwindAllRopes();
			}
			else
			{
				RopeCreator.StopUnwindAllRopes();
			}
			cbUnwindAll.Checked = cbUnwindAll.Checked;
			cbWindAll.Checked = false;
		}
	}

	private void EditGroupMenu_Opening(object sender, CancelEventArgs e)
	{
		lastVisibleMenu = editGroupMenu;
		SetWindChecks();
	}

	private void EditGroup_CheckboxItem_CheckChanged(object sender, EventArgs e)
	{
		if (settingChecks)
		{
			return;
		}
		RopeGroup ropeGroup = RopeCreator.ropeGroups[liGroupIndex.SelectedIndex];
		if (sender == cbWindGroup)
		{
			if (cbWindGroup.Checked)
			{
				cbUnwindGroup.Checked = false;
				ropeGroup.StartWindRopes();
			}
			else
			{
				ropeGroup.StopWindRopes();
			}
		}
		else if (sender == cbUnwindGroup)
		{
			if (cbUnwindGroup.Checked)
			{
				cbWindGroup.Checked = false;
				ropeGroup.StartUnwindRopes();
			}
			else
			{
				ropeGroup.StopUnwindRopes();
			}
		}
	}

	private void miDeleteGroup_Activated(object sender, EventArgs e)
	{
		RopeCreator.ropeGroups[liGroupIndex.SelectedIndex].DeleteRopes();
	}

	private void EditRopeMenu_Opening(object sender, CancelEventArgs e)
	{
		lastVisibleMenu = editRopeMenu;
		ReloadRopeIndices();
	}

	private void liRopeIndex_ItemChanged(object sender, ItemChangedEventArgs<int> e)
	{
		SetWindChecks();
	}

	private void miDelete_Activated(object sender, EventArgs e)
	{
		RopeGroup ropeGroup = RopeCreator.ropeGroups[liGroupIndex.SelectedIndex];
		int selectedItem = liRopeIndex.SelectedItem;
		if (selectedItem > -1)
		{
			ropeGroup.ropes[selectedItem].Delete();
			ropeGroup.ropes.RemoveAt(selectedItem);
			ReloadRopeIndices();
			UI.ShowSubtitle("Deleted");
		}
	}

	private void EditRope_CheckboxItem_CheckChanged(object sender, EventArgs e)
	{
		if (settingChecks)
		{
			return;
		}
		RopeGroup ropeGroup = RopeCreator.ropeGroups[liGroupIndex.SelectedIndex];
		int selectedItem = liRopeIndex.SelectedItem;
		if (selectedItem > -1)
		{
			AttachedRope attachedRope = ropeGroup.ropes[selectedItem];
			if (sender == cbWind)
			{
				cbUnwind.Checked = false;
				if (cbWind.Checked)
				{
					attachedRope.StopUnwind();
					attachedRope.StartWind();
				}
				else
				{
					attachedRope.StopWind();
				}
			}
			else if (sender == cbUnwind)
			{
				cbWind.Checked = false;
				if (cbUnwind.Checked)
				{
					attachedRope.StopWind();
					attachedRope.StartUnwind();
				}
				else
				{
					attachedRope.StopUnwind();
				}
			}
		}
		else
		{
			cbWind.Checked = false;
			cbUnwind.Checked = false;
		}
	}

	private void Menu_Tick(object sender, EventArgs e)
	{
		objPool.Process();
	}

	private static void SetWindChecks()
	{
		settingChecks = true;
		if (RopeCreator.AreAllRopesWinding())
		{
			cbWindAll.Checked = true;
			cbUnwindAll.Checked = false;
		}
		else if (RopeCreator.AreAllRopesUnwinding())
		{
			cbUnwindAll.Checked = true;
			cbWindAll.Checked = false;
		}
		else
		{
			cbWindAll.Checked = false;
			cbUnwindAll.Checked = false;
		}
		RopeGroup ropeGroup = RopeCreator.ropeGroups[liGroupIndex.SelectedIndex];
		if (ropeGroup.AreAllRopesWinding())
		{
			cbWindGroup.Checked = true;
			cbUnwindGroup.Checked = false;
		}
		else if (ropeGroup.AreAllRopesUnwinding())
		{
			cbUnwindGroup.Checked = true;
			cbWindGroup.Checked = false;
		}
		else
		{
			cbWindGroup.Checked = false;
			cbUnwindGroup.Checked = false;
		}
		if (ropeGroup.ropes.Count == 0)
		{
			cbWind.Checked = false;
			cbUnwind.Checked = false;
		}
		else
		{
			int num = liRopeIndex.Items[liRopeIndex.SelectedIndex];
			if (num > -1)
			{
				AttachedRope attachedRope = ropeGroup.ropes[num];
				cbWind.Checked = attachedRope.winding;
				cbUnwind.Checked = attachedRope.unwinding;
			}
		}
		settingChecks = false;
	}

	internal static void ReloadRopeIndices(bool selectLast = false)
	{
		int selectedIndex = liRopeIndex.SelectedIndex;
		liRopeIndex.SelectedIndex = 0;
		int count = RopeCreator.ropeGroups[liGroupIndex.SelectedIndex].ropes.Count;
		if (count == 0)
		{
			liRopeIndex.Items = NO_ROPES_INDICES;
			cbWind.Checked = false;
			cbUnwind.Checked = false;
			return;
		}
		liRopeIndex.Items = Enumerable.Range(0, count).ToList();
		if (selectLast || selectedIndex >= count)
		{
			liRopeIndex.SelectedIndex = count - 1;
		}
		else
		{
			liRopeIndex.SelectedIndex = selectedIndex;
		}
		SetWindChecks();
	}

	internal static void Toggle()
	{
		if (objPool.AreAnyVisible)
		{
			if (mainMenu.Visible)
			{
				lastVisibleMenu = mainMenu;
			}
			else if (editGroupMenu.Visible)
			{
				lastVisibleMenu = editGroupMenu;
			}
			else
			{
				lastVisibleMenu = editRopeMenu;
			}
			//lastVisibleMenu.Close();
            lastVisibleMenu.Visible = false;
        }
		else
		{
			//lastVisibleMenu.Open();
            lastVisibleMenu.Visible = true;
        }
	}
}
