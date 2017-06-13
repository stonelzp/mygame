// ----------------------------------------------------------------------------------
//
// FXMaker
// Created by ismoon - 2012 - ismoonto@gmail.com
//
// ----------------------------------------------------------------------------------

// --------------------------------------------------------------------------------------
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;

[CustomEditor(typeof(NcUvAnimation))]

public class NcUvAnimationEditor : FXMakerEditor
{
	// Attribute ------------------------------------------------------------------------
	protected	NcUvAnimation		m_Sel;

	// Property -------------------------------------------------------------------------
	// Event Function -------------------------------------------------------------------
    void OnEnable()
    {
 		m_Sel = target as NcUvAnimation;
 		m_UndoManager	= new FXMakerUndoManager(m_Sel, "NcUvAnimation");
   }

    void OnDisable()
    {
    }

	public override void OnInspectorGUI()
	{
		AddScriptNameField(m_Sel);
		m_UndoManager.CheckUndo();
		// --------------------------------------------------------------
		bool bClickButton = false;
		EditorGUI.BeginChangeCheck();
		{
//			DrawDefaultInspector();
			m_Sel.m_fScrollSpeedX		= EditorGUILayout.FloatField(GetHelpContent("m_fScrollSpeedX")	, m_Sel.m_fScrollSpeedX);
			m_Sel.m_fScrollSpeedY		= EditorGUILayout.FloatField(GetHelpContent("m_fScrollSpeedY")	, m_Sel.m_fScrollSpeedY);
			m_Sel.m_fTilingX			= EditorGUILayout.FloatField(GetHelpContent("m_fTilingX")		, m_Sel.m_fTilingX);
			m_Sel.m_fTilingY			= EditorGUILayout.FloatField(GetHelpContent("m_fTilingY")		, m_Sel.m_fTilingY);
			m_Sel.m_fOffsetX			= EditorGUILayout.FloatField(GetHelpContent("m_fOffsetX")		, m_Sel.m_fOffsetX);
			m_Sel.m_fOffsetY			= EditorGUILayout.FloatField(GetHelpContent("m_fOffsetY")		, m_Sel.m_fOffsetY);
			m_Sel.m_bFixedTileSize		= EditorGUILayout.Toggle	(GetHelpContent("m_bFixedTileSize")	, m_Sel.m_bFixedTileSize);
			m_Sel.m_bRepeat				= EditorGUILayout.Toggle	(GetHelpContent("m_bRepeat")		, m_Sel.m_bRepeat);
			if(!m_Sel.m_bRepeat)
				m_Sel.m_bAutoDestruct				= EditorGUILayout.Toggle	(GetHelpContent("m_bAutoDestruct")		, m_Sel.m_bAutoDestruct);

			// Texture --------------------------------------------------------------
			Rect rect = EditorGUILayout.BeginHorizontal(GUILayout.Height(150));
			{
				GUILayout.Label("");

				Rect subRect = rect;
				FXMakerLayout.GetOffsetRect(rect, 0, 5, 0, -5);

				// draw texture
				if (m_Sel.GetComponent<Renderer>() != null && m_Sel.GetComponent<Renderer>().sharedMaterial != null && m_Sel.GetComponent<Renderer>().sharedMaterial.mainTexture != null)
				{
					GUI.DrawTexture(subRect, m_Sel.GetComponent<Renderer>().sharedMaterial.mainTexture, ScaleMode.StretchToFill, true);
				}
				GUI.Box(rect, "");
			}
			EditorGUILayout.EndHorizontal();
		}
		m_UndoManager.CheckDirty();
		// --------------------------------------------------------------
		if ((EditorGUI.EndChangeCheck() || bClickButton) && GetFXMakerMain())
			GetFXMakerMain().CreateCurrentInstanceEffect(true);
		// ---------------------------------------------------------------------
		if (GUI.tooltip != "")
			m_LastTooltip	= GUI.tooltip;
		HelpBox(m_LastTooltip);
	}

	// ----------------------------------------------------------------------------------
	// ----------------------------------------------------------------------------------
	protected GUIContent GetHelpContent(string tooltip)
	{
		string caption	= tooltip;
		string text		= FXMakerTooltip.GetHsEditor_NcUvAnimation(tooltip);
		return GetHelpContent(caption, text);
	}

	protected override void HelpBox(string caption)
	{
		string	keyword	= caption;
		int		index	= caption.IndexOf('\n');
		if (0 <= index)
			keyword	= caption.Substring(0, index);
		base.HelpBox(FXMakerTooltip.GetHsEditor_NcUvAnimation(keyword));
	}
}
