using GTA;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RopeCreator
{
	class Menu : Script
	{
		#region controls
		static readonly Control[] UNNECESSARY_CONTROLS = new Control[] {
			Control.NextCamera,
			//Control.LookLeftRight,
			//Control.LookUpDown,
			Control.LookUpOnly,
			Control.LookDownOnly,
			Control.LookLeftOnly,
			Control.LookRightOnly,
			Control.CinematicSlowMo,
			//Control.FlyUpDown,
			//Control.FlyLeftRight,
			Control.ScriptedFlyZUp,
			Control.ScriptedFlyZDown,
			Control.WeaponWheelUpDown,
			Control.WeaponWheelLeftRight,
			Control.WeaponWheelNext,
			Control.WeaponWheelPrev,
			Control.SelectNextWeapon,
			Control.SelectPrevWeapon,
			Control.SkipCutscene,
			Control.CharacterWheel,
			Control.MultiplayerInfo,
			//Control.Sprint,
			//Control.Jump,
			//Control.Enter,
			//Control.Attack,
			//Control.Aim,
			//Control.LookBehind,
			Control.Phone,
			Control.SpecialAbility,
			Control.SpecialAbilitySecondary,
			//Control.MoveLeftRight,
			//Control.MoveUpDown,
			Control.MoveUpOnly,
			Control.MoveDownOnly,
			Control.MoveLeftOnly,
			Control.MoveRightOnly,
			Control.Duck,
			Control.SelectWeapon,
			Control.Pickup,
			Control.SniperZoom,
			Control.SniperZoomInOnly,
			Control.SniperZoomOutOnly,
			Control.SniperZoomInSecondary,
			Control.SniperZoomOutSecondary,
			Control.Cover,
			Control.Reload,
			Control.Talk,
			Control.Detonate,
			Control.HUDSpecial,
			Control.Arrest,
			Control.AccurateAim,
			Control.Context,
			Control.ContextSecondary,
			Control.WeaponSpecial,
			Control.WeaponSpecial2,
			Control.Dive,
			Control.DropWeapon,
			Control.DropAmmo,
			Control.ThrowGrenade,
			//Control.VehicleMoveLeftRight,
			Control.VehicleMoveUpDown,
			Control.VehicleMoveUpOnly,
			Control.VehicleMoveDownOnly,
			Control.VehicleMoveLeftOnly,
			Control.VehicleMoveRightOnly,
			Control.VehicleSpecial,
			Control.VehicleGunLeftRight,
			Control.VehicleGunUpDown,
			Control.VehicleAim,
			Control.VehicleAttack,
			Control.VehicleAttack2,
			//Control.VehicleAccelerate,
			//Control.VehicleBrake,
			Control.VehicleDuck,
			Control.VehicleHeadlight,
			//Control.VehicleExit,
			//Control.VehicleHandbrake,
			Control.VehicleHotwireLeft,
			Control.VehicleHotwireRight,
			//Control.VehicleLookBehind,
			Control.VehicleCinCam,
			Control.VehicleNextRadio,
			Control.VehiclePrevRadio,
			Control.VehicleNextRadioTrack,
			Control.VehiclePrevRadioTrack,
			Control.VehicleRadioWheel,
			Control.VehicleHorn,
			Control.VehicleFlyThrottleUp,
			Control.VehicleFlyThrottleDown,
			//Control.VehicleFlyYawLeft,
			//Control.VehicleFlyYawRight,
			Control.VehiclePassengerAim,
			Control.VehiclePassengerAttack,
			Control.VehicleSpecialAbilityFranklin,
			Control.VehicleStuntUpDown,
			Control.VehicleCinematicUpDown,
			Control.VehicleCinematicUpOnly,
			Control.VehicleCinematicDownOnly,
			Control.VehicleCinematicLeftRight,
			Control.VehicleSelectNextWeapon,
			Control.VehicleSelectPrevWeapon,
			Control.VehicleRoof,
			Control.VehicleJump,
			Control.VehicleGrapplingHook,
			Control.VehicleShuffle,
			Control.VehicleDropProjectile,
			Control.VehicleMouseControlOverride,
			Control.VehicleFlyRollLeftRight,
			Control.VehicleFlyRollLeftOnly,
			Control.VehicleFlyRollRightOnly,
			Control.VehicleFlyPitchUpDown,
			Control.VehicleFlyPitchUpOnly,
			Control.VehicleFlyPitchDownOnly,
			Control.VehicleFlyUnderCarriage,
			Control.VehicleFlyAttack,
			Control.VehicleFlySelectNextWeapon,
			Control.VehicleFlySelectPrevWeapon,
			Control.VehicleFlySelectTargetLeft,
			Control.VehicleFlySelectTargetRight,
			Control.VehicleFlyVerticalFlightMode,
			Control.VehicleFlyDuck,
			Control.VehicleFlyAttackCamera,
			Control.VehicleFlyMouseControlOverride,
			Control.VehicleSubTurnLeftRight,
			Control.VehicleSubTurnLeftOnly,
			Control.VehicleSubTurnRightOnly,
			Control.VehicleSubPitchUpDown,
			Control.VehicleSubPitchUpOnly,
			Control.VehicleSubPitchDownOnly,
			Control.VehicleSubThrottleUp,
			Control.VehicleSubThrottleDown,
			Control.VehicleSubAscend,
			Control.VehicleSubDescend,
			Control.VehicleSubTurnHardLeft,
			Control.VehicleSubTurnHardRight,
			Control.VehicleSubMouseControlOverride,
			Control.VehiclePushbikePedal,
			Control.VehiclePushbikeSprint,
			Control.VehiclePushbikeFrontBrake,
			Control.VehiclePushbikeRearBrake,
			Control.MeleeAttackLight,
			Control.MeleeAttackHeavy,
			Control.MeleeAttackAlternate,
			Control.MeleeBlock,
			Control.ParachuteDeploy,
			Control.ParachuteDetach,
			Control.ParachuteTurnLeftRight,
			Control.ParachuteTurnLeftOnly,
			Control.ParachuteTurnRightOnly,
			Control.ParachutePitchUpDown,
			Control.ParachutePitchUpOnly,
			Control.ParachutePitchDownOnly,
			Control.ParachuteBrakeLeft,
			Control.ParachuteBrakeRight,
			Control.ParachuteSmoke,
			Control.ParachutePrecisionLanding,
			Control.Map,
			Control.SelectWeaponUnarmed,
			Control.SelectWeaponMelee,
			Control.SelectWeaponHandgun,
			Control.SelectWeaponShotgun,
			Control.SelectWeaponSmg,
			Control.SelectWeaponAutoRifle,
			Control.SelectWeaponSniper,
			Control.SelectWeaponHeavy,
			Control.SelectWeaponSpecial,
			Control.SelectCharacterMichael,
			Control.SelectCharacterFranklin,
			Control.SelectCharacterTrevor,
			Control.SelectCharacterMultiplayer,
			//Control.SaveReplayClip,
			Control.SpecialAbilityPC,
			Control.PhoneUp,
			Control.PhoneDown,
			Control.PhoneLeft,
			Control.PhoneRight,
			Control.PhoneSelect,
			Control.PhoneCancel,
			Control.PhoneOption,
			Control.PhoneExtraOption,
			Control.PhoneScrollForward,
			Control.PhoneScrollBackward,
			Control.PhoneCameraFocusLock,
			Control.PhoneCameraGrid,
			Control.PhoneCameraSelfie,
			Control.PhoneCameraDOF,
			Control.PhoneCameraExpression,
			//Control.FrontendDown,
			//Control.FrontendUp,
			//Control.FrontendLeft,
			//Control.FrontendRight,
			Control.FrontendRdown,
			Control.FrontendRup,
			Control.FrontendRleft,
			Control.FrontendRright,
			//Control.FrontendAxisX,
			//Control.FrontendAxisY,
			Control.FrontendRightAxisX,
			Control.FrontendRightAxisY,
			Control.FrontendPause,
			Control.FrontendPauseAlternate,
			//Control.FrontendAccept,
			//Control.FrontendCancel,
			Control.FrontendX,
			Control.FrontendY,
			Control.FrontendLb,
			Control.FrontendRb,
			Control.FrontendLt,
			Control.FrontendRt,
			Control.FrontendLs,
			Control.FrontendRs,
			Control.FrontendLeaderboard,
			Control.FrontendSocialClub,
			Control.FrontendSocialClubSecondary,
			Control.FrontendDelete,
			Control.FrontendEndscreenAccept,
			Control.FrontendEndscreenExpand,
			//Control.FrontendSelect,
			Control.ScriptLeftAxisX,
			Control.ScriptLeftAxisY,
			Control.ScriptRightAxisX,
			Control.ScriptRightAxisY,
			Control.ScriptRUp,
			Control.ScriptRDown,
			Control.ScriptRLeft,
			Control.ScriptRRight,
			Control.ScriptLB,
			Control.ScriptRB,
			Control.ScriptLT,
			Control.ScriptRT,
			Control.ScriptLS,
			Control.ScriptRS,
			Control.ScriptPadUp,
			Control.ScriptPadDown,
			Control.ScriptPadLeft,
			Control.ScriptPadRight,
			Control.ScriptSelect,
			Control.CursorAccept,
			Control.CursorCancel,
			Control.CursorX,
			Control.CursorY,
			//Control.CursorScrollUp,
			//Control.CursorScrollDown,
			Control.EnterCheatCode,
			Control.InteractionMenu,
			Control.MpTextChatAll,
			Control.MpTextChatTeam,
			Control.MpTextChatFriends,
			Control.MpTextChatCrew,
			Control.PushToTalk,
			Control.CreatorLS,
			Control.CreatorRS,
			Control.CreatorLT,
			Control.CreatorRT,
			Control.CreatorMenuToggle,
			Control.CreatorAccept,
			Control.CreatorDelete,
			Control.Attack2,
			Control.RappelJump,
			Control.RappelLongJump,
			Control.RappelSmashWindow,
			Control.PrevWeapon,
			Control.NextWeapon,
			Control.MeleeAttack1,
			Control.MeleeAttack2,
			Control.Whistle,
			Control.MoveLeft,
			Control.MoveRight,
			Control.MoveUp,
			Control.MoveDown,
			Control.LookLeft,
			Control.LookRight,
			Control.LookUp,
			Control.LookDown,
			Control.SniperZoomIn,
			Control.SniperZoomOut,
			Control.SniperZoomInAlternate,
			Control.SniperZoomOutAlternate,
			Control.VehicleMoveLeft,
			Control.VehicleMoveRight,
			Control.VehicleMoveUp,
			Control.VehicleMoveDown,
			Control.VehicleGunLeft,
			Control.VehicleGunRight,
			Control.VehicleGunUp,
			Control.VehicleGunDown,
			Control.VehicleLookLeft,
			Control.VehicleLookRight,
			//.ReplayStartStopRecording,
			//Control.ReplayStartStopRecordingSecondary,
			Control.ScaledLookLeftRight,
			Control.ScaledLookUpDown,
			Control.ScaledLookUpOnly,
			Control.ScaledLookDownOnly,
			Control.ScaledLookLeftOnly,
			Control.ScaledLookRightOnly,
			Control.ReplayMarkerDelete,
			Control.ReplayClipDelete,
			Control.ReplayPause,
			Control.ReplayRewind,
			Control.ReplayFfwd,
			Control.ReplayNewmarker,
			//Control.ReplayRecord,
			Control.ReplayScreenshot,
			Control.ReplayHidehud,
			Control.ReplayStartpoint,
			Control.ReplayEndpoint,
			Control.ReplayAdvance,
			Control.ReplayBack,
			Control.ReplayTools,
			Control.ReplayRestart,
			Control.ReplayShowhotkey,
			Control.ReplayCycleMarkerLeft,
			Control.ReplayCycleMarkerRight,
			Control.ReplayFOVIncrease,
			Control.ReplayFOVDecrease,
			Control.ReplayCameraUp,
			Control.ReplayCameraDown,
			//Control.ReplaySave,
			Control.ReplayToggletime,
			Control.ReplayToggletips,
			Control.ReplayPreview,
			Control.ReplayToggleTimeline,
			Control.ReplayTimelinePickupClip,
			Control.ReplayTimelineDuplicateClip,
			Control.ReplayTimelinePlaceClip,
			Control.ReplayCtrl,
			Control.ReplayTimelineSave,
			Control.ReplayPreviewAudio,
			Control.VehicleDriveLook,
			Control.VehicleDriveLook2,
			Control.VehicleFlyAttack2,
			Control.RadioWheelUpDown,
			Control.RadioWheelLeftRight,
			Control.VehicleSlowMoUpDown,
			Control.VehicleSlowMoUpOnly,
			Control.VehicleSlowMoDownOnly,
			Control.MapPointOfInterest,
			Control.VehicleCarJump,
			Control.VehicleRocketBoost,
			Control.VehicleParachute,
		};
		#endregion

		static readonly List<object> ROPE_NAMES = new List<object>(new object[] { "Rope", "Thick rope", "Thick cable", "Medium cable", "Cable" });
		static readonly List<object> NO_ROPES_INDICES = new List<object>(new object[] { -1 });

		static MenuPool menuPool;
		internal static UIMenu mainMenu, editRopeMenu;

		internal static UIMenuItem miDeleteLast, miDeleteAll;
		internal static UIMenuCheckboxItem cbEnabled, cbWindAll, cbUnwindAll, cbShowAimMarker, cbShowEditMarkers, cbBreakable, cbAttachObjBone, cbAttachPedBone;
		internal static UIMenuListItem liType;
		internal static UIMenuSliderItem siSlack, siMinLength;

		internal static UIMenuItem miDelete;
		internal static UIMenuCheckboxItem cbWind, cbUnwind;
		internal static UIMenuListItem liRopeIndex;

		internal static bool modEnabled = false, showAimMarker = true, showEditMarkers = true, breakable = false, attachObjBone = false, attachPedBone = false;
		internal static int type = 1;
		internal static float slack = 0.25f, minLength = 0.25f;

		static bool editRopeMenuWasVisible = false;
		static int lastRopesCount = 0;

		public Menu()
		{
			menuPool = new MenuPool();
			menuPool.ControlDisablingEnabled = false;

			mainMenu = new UIMenu("Rope Creator", "");
			mainMenu.ControlDisablingEnabled = false;
			mainMenu.MouseControlsEnabled = false;
			mainMenu.MouseEdgeEnabled = false;

			menuPool.Add(mainMenu);

			cbEnabled = new UIMenuCheckboxItem("Enabled", modEnabled);
			cbEnabled.CheckboxEvent += MainMenu_CheckboxItem_CheckChanged;
			miDeleteLast = new UIMenuItem("Delete last rope");
			miDeleteLast.Activated += MainMenu_MenuItem_Activated;
			miDeleteAll = new UIMenuItem("Delete all ropes");
			miDeleteAll.Activated += MainMenu_MenuItem_Activated;
			cbWindAll = new UIMenuCheckboxItem("Wind all ropes", false);
			cbWindAll.CheckboxEvent += MainMenu_CheckboxItem_CheckChanged;
			cbUnwindAll = new UIMenuCheckboxItem("Unwind all ropes", false);
			cbUnwindAll.CheckboxEvent += MainMenu_CheckboxItem_CheckChanged;
			cbShowAimMarker = new UIMenuCheckboxItem("Show aim marker", showAimMarker);
			cbShowAimMarker.CheckboxEvent += MainMenu_CheckboxItem_CheckChanged;
			cbShowEditMarkers = new UIMenuCheckboxItem("Show edit rope markers", showEditMarkers);
			cbShowEditMarkers.CheckboxEvent += MainMenu_CheckboxItem_CheckChanged;
			cbBreakable = new UIMenuCheckboxItem("Breakable", breakable);
			cbBreakable.CheckboxEvent += MainMenu_CheckboxItem_CheckChanged;
			liType = new UIMenuListItem("Type", ROPE_NAMES, type - 1);
			liType.OnListChanged += liType_IndexChanged;
			siSlack = new UIMenuSliderItem("Slack", slack.ToString("0.00") + " meters");
			siSlack.Value = (int)(slack * 4);
			siSlack.Multiplier = 1;
			siSlack.Maximum = 200;
			siSlack.OnSliderChanged += MainMenu_SliderItem_ValueChanged;
			siMinLength = new UIMenuSliderItem("Minimum length", minLength.ToString("0.00") + " meters");
			siMinLength.Value = (int)(minLength * 4);
			siMinLength.Multiplier = 1;
			siMinLength.Maximum = 100;
			siMinLength.OnSliderChanged += MainMenu_SliderItem_ValueChanged;
			cbAttachObjBone = new UIMenuCheckboxItem("Attach to object bone", attachObjBone);
			cbAttachObjBone.CheckboxEvent += MainMenu_CheckboxItem_CheckChanged;
			cbAttachPedBone = new UIMenuCheckboxItem("Attach to ped bone", attachPedBone);
			cbAttachPedBone.CheckboxEvent += MainMenu_CheckboxItem_CheckChanged;

			mainMenu.AddItem(cbEnabled);
			editRopeMenu = menuPool.AddSubMenu(mainMenu, "Edit specific rope", "Edit ropes you created");
			mainMenu.AddItem(miDeleteLast);
			mainMenu.AddItem(miDeleteAll);
			mainMenu.AddItem(cbWindAll);
			mainMenu.AddItem(cbUnwindAll);
			mainMenu.AddItem(cbShowAimMarker);
			mainMenu.AddItem(cbShowEditMarkers);
			mainMenu.AddItem(cbBreakable);
			mainMenu.AddItem(liType);
			mainMenu.AddItem(siSlack);
			mainMenu.AddItem(siMinLength);
			mainMenu.AddItem(cbAttachObjBone);
			mainMenu.AddItem(cbAttachPedBone);

			editRopeMenu.ControlDisablingEnabled = false;
			editRopeMenu.MouseControlsEnabled = false;
			editRopeMenu.MouseEdgeEnabled = false;

			liRopeIndex = new UIMenuListItem("Rope index", NO_ROPES_INDICES, 0);
			liRopeIndex.OnListChanged += liRopeIndex_IndexChanged;
			miDelete = new UIMenuItem("Delete");
			miDelete.Activated += miDelete_Activated;
			cbWind = new UIMenuCheckboxItem("Wind", false);
			cbWind.CheckboxEvent += EditRope_CheckboxItem_CheckChanged;
			cbUnwind = new UIMenuCheckboxItem("Unwind", false);
			cbUnwind.CheckboxEvent += EditRope_CheckboxItem_CheckChanged;

			editRopeMenu.AddItem(liRopeIndex);
			editRopeMenu.AddItem(miDelete);
			editRopeMenu.AddItem(cbWind);
			editRopeMenu.AddItem(cbUnwind);

			Tick += Menu_Tick;
		}

		private void MainMenu_MenuItem_Activated(UIMenu sender, UIMenuItem selectedItem)
		{
			if (selectedItem == miDeleteLast)
			{
				RopeCreator.DeleteLastRope();

				if (RopeCreator.ropes.Count == 0)
					ReloadRopeIndices(RopeCreator.ropes.Count);
			}
			else if (selectedItem == miDeleteAll)
			{
				RopeCreator.DeleteAllRopes();
				ReloadRopeIndices(RopeCreator.ropes.Count);
			}
		}

		private void MainMenu_SliderItem_ValueChanged(UIMenuSliderItem sender, int newValue)
		{
			if (sender == siSlack)
			{
				slack = newValue * 0.25f;
				siSlack.Description = slack.ToString("0.00") + " meters";
			}
			else if (sender == siMinLength)
			{
				minLength = newValue * 0.25f;
				siMinLength.Description = minLength.ToString("0.00") + " meters";
			}

			//NativeUI does not redraw descriptions after being changed, so do it manually
			ReDraw();
		}

		private void liType_IndexChanged(UIMenuListItem sender, int newIndex)
		{
			//index is rope type - 1 (rope types range from 1 to 6)
			type = newIndex + 1;
		}

		private void MainMenu_CheckboxItem_CheckChanged(UIMenuCheckboxItem sender, bool check)
		{
			if (sender == cbEnabled) modEnabled = check;
			else if (sender == cbShowAimMarker) showAimMarker = check;
			else if (sender == cbShowEditMarkers) showEditMarkers = check;
			else if (sender == cbBreakable) breakable = check;
			else if (sender == cbAttachObjBone) attachObjBone = check;
			else if (sender == cbAttachPedBone) attachPedBone = check;
			else if (sender == cbWindAll)
			{
				if (check) RopeCreator.StartWindAllRopes();
				else RopeCreator.StopWindAllRopes();

				cbWindAll.Checked = check;
				cbUnwindAll.Checked = false;
			}
			else if (sender == cbUnwindAll)
			{
				if (check) RopeCreator.StartUnwindAllRopes();
				else RopeCreator.StopUnwindAllRopes();

				cbUnwindAll.Checked = check;
				cbWindAll.Checked = false;
			}
		}

		private void liRopeIndex_IndexChanged(UIMenuListItem sender, int newIndex)
		{
			SetWindChecks(newIndex);
		}

		private void miDelete_Activated(UIMenu sender, UIMenuItem selectedItem)
		{
			int lastIndex = liRopeIndex.Index;
			var selectedRopeIndex = (int)liRopeIndex.Items[liRopeIndex.Index];

			if (selectedRopeIndex > -1)
			{
				RopeCreator.ropes[selectedRopeIndex].Delete();
				RopeCreator.ropes.RemoveAt(selectedRopeIndex);

				ReloadRopeIndices(RopeCreator.ropes.Count, lastIndex);

				UI.ShowSubtitle("Deleted");
			}
		}

		private void EditRope_CheckboxItem_CheckChanged(UIMenuCheckboxItem sender, bool check)
		{
			int selectedRopeIndex = (int)liRopeIndex.Items[liRopeIndex.Index];

			if (selectedRopeIndex > -1)
			{
				var selectedRope = RopeCreator.ropes[selectedRopeIndex];

				if (sender == cbWind)
				{
					cbUnwind.Checked = false;

					if (check)
					{
						selectedRope.StopUnwind();
						selectedRope.StartWind();

						cbWindAll.Checked = RopeCreator.AreAllRopesWinding();
					}
					else
					{
						selectedRope.StopWind();

						cbWindAll.Checked = false;
					}
				}
				else if (sender == cbUnwind)
				{
					cbWind.Checked = false;

					if (check)
					{
						selectedRope.StopWind();
						selectedRope.StartUnwind();

						cbUnwindAll.Checked = RopeCreator.AreAllRopesUnwinding();
					}
					else
					{
						selectedRope.StopUnwind();

						cbUnwindAll.Checked = false;
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
			menuPool.ProcessMenus();

			//manually disable controls, because NativeUI breaks Rockstar Editor recording when it disables controls
			if (menuPool.IsAnyMenuOpen())
			{
				foreach (var control in UNNECESSARY_CONTROLS)
				{
					Game.DisableControlThisFrame(0, control);
				}
			}

			int count = RopeCreator.ropes.Count;

			if (count != lastRopesCount)
			{
				ReloadRopeIndices(count);
			}
		}

		private void ReDraw()
		{
			MethodInfo privMethod = mainMenu.GetType().GetMethod("DrawCalculations", BindingFlags.NonPublic | BindingFlags.Instance);
			privMethod.Invoke(mainMenu, new object[] { });
		}

		private static void SetWindChecks(int index)
		{
			var selectedRopeIndex = (int)liRopeIndex.Items[index];

			if (selectedRopeIndex > -1)
			{
				var selectedRope = RopeCreator.ropes[selectedRopeIndex];

				cbWind.Checked = selectedRope.winding;
				cbUnwind.Checked = selectedRope.unwinding;
			}

			cbWindAll.Checked = RopeCreator.AreAllRopesWinding();
			cbUnwindAll.Checked = RopeCreator.AreAllRopesUnwinding();
		}

		internal static void ReloadRopeIndices(int count, int newIndex = -1)
		{
			lastRopesCount = count;

			if (count == 0)
			{
				liRopeIndex.Items = NO_ROPES_INDICES;
				liRopeIndex.Index = 0;

				cbWind.Checked = false;
				cbUnwind.Checked = false;
			}
			else
			{
				liRopeIndex.Items = Enumerable.Range(0, count).Cast<object>().ToList();

				if (newIndex < 0 || newIndex >= count)
				{
					if (liRopeIndex.Index >= count) liRopeIndex.Index = count - 1;
					SetWindChecks(liRopeIndex.Index);
				}
				else
				{
					liRopeIndex.Index = newIndex;
					SetWindChecks(newIndex);
				}
			}
		}

		internal static void Toggle()
		{
			if (editRopeMenu.Visible)
			{
				editRopeMenuWasVisible = true;
				editRopeMenu.Visible = false;
			}
			else
			{
				if (editRopeMenuWasVisible)
				{
					editRopeMenuWasVisible = false;
					editRopeMenu.Visible = true;
				}
				else
				{
					mainMenu.Visible = !mainMenu.Visible;
				}
			}
		}
	}
}
