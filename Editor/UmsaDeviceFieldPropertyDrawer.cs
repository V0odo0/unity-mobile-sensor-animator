using System.Collections;
using System.Collections.Generic;
using UMSA.Runtime;
using UnityEditor;
using UnityEngine;

namespace UMSA.Editor
{
    [CustomPropertyDrawer(typeof(UmsaDeviceFieldAttribute), true)]
    public class UmsaDeviceFieldPropertyDrawer : PropertyDrawer
    {
        protected GUIContent DropdownButtonContent => _dropdownButtonContent ?? (_dropdownButtonContent = new GUIContent());
        private GUIContent _dropdownButtonContent;

        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                base.OnGUI(position, property, label);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            Rect propertyPos = position;
            propertyPos.height = EditorGUIUtility.singleLineHeight;
            propertyPos = EditorGUI.PrefixLabel(propertyPos, label);

            if (string.IsNullOrEmpty(property.stringValue))
            {
                DropdownButtonContent.text = $"Any [{(UmsaManager.Devices.Count == 0 ? "None" : $"{UmsaManager.Devices.Count}")}]";
            }
            else DropdownButtonContent.text = property.stringValue;

            if (EditorGUI.DropdownButton(propertyPos, DropdownButtonContent, FocusType.Keyboard))
            {
                GenericMenu menu = new GenericMenu();

                menu.AddItem(new GUIContent("Any"),string.IsNullOrEmpty(property.stringValue), () =>
                {
                    property.stringValue = string.Empty;
                    property.serializedObject.ApplyModifiedProperties();
                });

                for (int i = 0; i < UmsaManager.Devices.Count; i++)
                {
                    string device = UmsaManager.Devices[i];
                    menu.AddItem(new GUIContent(device), property.stringValue == device, () =>
                    {
                        property.stringValue = device;
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }

                menu.DropDown(propertyPos);
            }


            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.EndProperty();
        }
    }
}
