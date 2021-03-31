using GTA;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace RopeCreator
{
	class Menu : Script
	{
		static readonly string[] ROPE_NAMES = { "Rope", "Thick rope", "Thick cable", "Medium cable", "Cable" };
		static readonly List<int> NO_ROPES_INDICES = new List<int> { -1 };

		static ObjectPool objPool;
		internal static NativeMenu mainMenu, editGroupMenu, editRopeMenu;

		internal static NativeItem miDeleteLast, miDeleteAll;
		internal static NativeCheckboxItem cbEnabled, cbWindAll, cbUnwindAll, cbShowAimMarker, cbShowEditMarkers, cbBreakable, cbAttachObjBone, cbAttachPedBone, cbAttachNothing;
		internal static NativeListItem<int> liGroupIndex;
		internal static NativeListItem<string> liType;
		internal static NativeSliderItem siSlack, siMinLength;

		internal static NativeItem miDeleteGroup;
		internal static NativeCheckboxItem cbWindGroup, cbUnwindGroup;

		internal static NativeItem miDelete;
		internal static NativeCheckboxItem cbWind, cbUnwind;
		internal static NativeListItem<int> liRopeIndex;

		internal static bool modEnabled = false, showAimMarker = true, showEditMarkers = true, breakable = false, attachObjBone = false, attachPedBone = false, attachNothing = false;
		internal static int type = 1;
		internal static float slack = 0.25f, minLength = 0.25f;

		static NativeMenu lastVisibleMenu;
		static bool settingChecks = false;

		public Menu()
		{
			objPool = new ObjectPool();

			mainMenu = new NativeMenu("Rope Creator");
			mainMenu.UseMouse = false;
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
			cbWindAll = new NativeCheckboxItem("Wind all ropes", false);
			cbWindAll.CheckboxChanged += MainMenu_CheckboxItem_CheckChanged;
			cbUnwindAll = new NativeCheckboxItem("Unwind all ropes", false);
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
			siSlack.Value = (int)(slack * 4);
			siSlack.Multiplier = 1;
			siSlack.Maximum = 200;
			siSlack.ValueChanged += MainMenu_SliderItem_ValueChanged;
			siMinLength = new NativeSliderItem("Minimum length", minLength.ToString("0.00") + " meters");
			siMinLength.Value = (int)(minLength * 4);
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
			editGroupMenu.UseMouse = false;
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
			editRopeMenu.UseMouse = false;
			editRopeMenu.Opening += EditRopeMenu_Opening;

			objPool.Add(editRopeMenu);

			liRopeIndex = new NativeListItem<int>("Rope index", NO_ROPES_INDICES.ToArray());
			liRopeIndex.ItemChanged += liRopeIndex_ItemChanged;
			miDelete = new NativeItem("Delete");
			miDelete.Activated += miDelete_Activated;
			cbWind = new NativeCheckboxItem("Wind", false);
			cbWind.CheckboxChanged += EditRope_CheckboxItem_CheckChanged;
			cbUnwind = new NativeCheckboxItem("Unwind", false);
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

			Tick += Menu_Tick;
		}

		private void MainMenu_Opening(object sender, CancelEventArgs e)
		{
			lastVisibleMenu = mainMenu;
			SetWindChecks();
		}

		private void liGroupIndex_ItemChanged(object sender, ItemChangedEventArgs<int> e)
		{
			ReloadRopeIndices(true);
		}

		private void MainMenu_ItemActivated(object sender, ItemActivatedArgs e)
		{
			var selectedGroup = RopeCreator.ropeGroups[liGroupIndex.SelectedIndex];

			if (e.Item == liGroupIndex)
			{
				mainMenu.Close();

				if (!mainMenu.Visible)
				{
					editGroupMenu.Title = new ScaledText(PointF.Empty, $"Edit Group {liGroupIndex.SelectedItem}", 1.02f, GTA.Font.HouseScript)
					{
						Color = Color.White,
						Alignment = Alignment.Center
					};

					editGroupMenu.Recalculate();

					editGroupMenu.Open();
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
				slack = siSlack.Value * 0.25f;
				siSlack.Description = slack.ToString("0.00") + " meters";
			}
			else if (sender == siMinLength)
			{
				minLength = siMinLength.Value * 0.25f;
				siMinLength.Description = minLength.ToString("0.00") + " meters";
			}

			mainMenu.Recalculate(); //so new text is drawn
		}

		private void liType_ItemChanged(object sender, ItemChangedEventArgs<string> e)
		{
			//index is rope type - 1 (rope types range from 1 to 6)
			type = liType.SelectedIndex + 1;
		}

		private void MainMenu_CheckboxItem_CheckChanged(object sender, EventArgs e)
		{
			if (settingChecks) return; //fix so can set checkboxes

			if (sender == cbEnabled) modEnabled = cbEnabled.Checked;
			else if (sender == cbShowAimMarker) showAimMarker = cbShowAimMarker.Checked;
			else if (sender == cbShowEditMarkers) showEditMarkers = cbShowEditMarkers.Checked;
			else if (sender == cbBreakable) breakable = cbBreakable.Checked;
			else if (sender == cbAttachObjBone) attachObjBone = cbAttachObjBone.Checked;
			else if (sender == cbAttachPedBone) attachPedBone = cbAttachPedBone.Checked;
			else if (sender == cbAttachNothing) attachNothing = cbAttachNothing.Checked;
			else if (sender == cbWindAll)
			{
				if (cbWindAll.Checked) RopeCreator.StartWindAllRopes();
				else RopeCreator.StopWindAllRopes();

				cbWindAll.Checked = cbWindAll.Checked;
				cbUnwindAll.Checked = false;
			}
			else if (sender == cbUnwindAll)
			{
				if (cbUnwindAll.Checked) RopeCreator.StartUnwindAllRopes();
				else RopeCreator.StopUnwindAllRopes();

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
			if (settingChecks) return; //fix so can set checkboxes

			var group = RopeCreator.ropeGroups[liGroupIndex.SelectedIndex];

			if (sender == cbWindGroup)
			{
				if (cbWindGroup.Checked)
				{
					cbUnwindGroup.Checked = false;
					group.StartWindRopes();
				}
				else group.StopWindRopes();
			}
			else if (sender == cbUnwindGroup)
			{
				if (cbUnwindGroup.Checked)
				{
					cbWindGroup.Checked = false;
					group.StartUnwindRopes();
				}
				else group.StopUnwindRopes();
			}
		}

		private void miDeleteGroup_Activated(object sender, EventArgs e)
		{
			var group = RopeCreator.ropeGroups[liGroupIndex.SelectedIndex];

			group.DeleteRopes();
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
			var group = RopeCreator.ropeGroups[liGroupIndex.SelectedIndex];

			var selectedRopeIndex = liRopeIndex.SelectedItem;

			if (selectedRopeIndex > -1)
			{
				group.ropes[selectedRopeIndex].Delete();
				group.ropes.RemoveAt(selectedRopeIndex);

				ReloadRopeIndices();

				UI.ShowSubtitle("Deleted");
			}
		}

		private void EditRope_CheckboxItem_CheckChanged(object sender, EventArgs e)
		{
			if (settingChecks) return; //fix so can set checkboxes

			var group = RopeCreator.ropeGroups[liGroupIndex.SelectedIndex];

			int selectedRopeIndex = liRopeIndex.SelectedItem;

			if (selectedRopeIndex > -1)
			{
				var selectedRope = group.ropes[selectedRopeIndex];

				if (sender == cbWind)
				{
					cbUnwind.Checked = false;

					if (cbWind.Checked)
					{
						selectedRope.StopUnwind();
						selectedRope.StartWind();
					}
					else
					{
						selectedRope.StopWind();
					}
				}
				else if (sender == cbUnwind)
				{
					cbWind.Checked = false;

					if (cbUnwind.Checked)
					{
						selectedRope.StopWind();
						selectedRope.StartUnwind();
					}
					else
					{
						selectedRope.StopUnwind();
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
			settingChecks = true; //fix so can set checks without setting off events

			//main menu
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

			//edit group menu
			var group = RopeCreator.ropeGroups[liGroupIndex.SelectedIndex];

			if (group.AreAllRopesWinding())
			{
				cbWindGroup.Checked = true;
				cbUnwindGroup.Checked = false;
			}
			else if (group.AreAllRopesUnwinding())
			{
				cbUnwindGroup.Checked = true;
				cbWindGroup.Checked = false;
			}
			else
			{
				cbWindGroup.Checked = false;
				cbUnwindGroup.Checked = false;
			}

			//edit rope menu
			if (group.ropes.Count == 0)
			{
				cbWind.Checked = false;
				cbUnwind.Checked = false;
			}
			else
			{
				var selectedRopeIndex = liRopeIndex.Items[liRopeIndex.SelectedIndex];

				if (selectedRopeIndex > -1)
				{
					var selectedRope = group.ropes[selectedRopeIndex];

					cbWind.Checked = selectedRope.winding;
					cbUnwind.Checked = selectedRope.unwinding;
				}
			}

			settingChecks = false;
		}

		internal static void ReloadRopeIndices(bool selectLast = false)
		{
			int oldIndex = liRopeIndex.SelectedIndex;

			//select first item, otherwise will get Exception when SelectedIndex > liRopeIndex.Items.Count
			liRopeIndex.SelectedIndex = 0;

			var group = RopeCreator.ropeGroups[liGroupIndex.SelectedIndex];
			int count = group.ropes.Count;

			if (count == 0)
			{
				liRopeIndex.Items = NO_ROPES_INDICES;

				cbWind.Checked = false;
				cbUnwind.Checked = false;
			}
			else
			{
				liRopeIndex.Items = Enumerable.Range(0, count).ToList();

				if (selectLast || oldIndex >= count) liRopeIndex.SelectedIndex = count - 1;
				else liRopeIndex.SelectedIndex = oldIndex;

				SetWindChecks();
			}
		}

		internal static void Toggle()
		{
			if (objPool.AreAnyVisible)
			{
				if (mainMenu.Visible) lastVisibleMenu = mainMenu;
				else if (editGroupMenu.Visible) lastVisibleMenu = editGroupMenu;
				else lastVisibleMenu = editRopeMenu;

				lastVisibleMenu.Close();
			}
			else lastVisibleMenu.Open();
		}
	}
}
