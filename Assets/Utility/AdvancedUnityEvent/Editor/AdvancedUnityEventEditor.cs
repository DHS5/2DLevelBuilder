using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Text;

namespace Dhs5.Utility.Events
{
    [CustomPropertyDrawer(typeof(AdvancedUnityEvent))]
    public class AdvancedUnityEventEditor : PropertyDrawer
    {
        SerializedProperty objProperty;
        SerializedProperty compInstIDProperty;
        SerializedProperty tokenProperty;
        SerializedProperty actionProperty;

        int methodIndex;
        float propertyOffset;
        float propertyHeight;

        private bool ValidParameters(ParameterInfo[] parameters)
        {
            foreach (var param in parameters)
            {
                if (param.ParameterType != typeof(int)
                    && param.ParameterType != typeof(float)
                    && param.ParameterType != typeof(bool)
                    && param.ParameterType != typeof(string))
                    return false;
            }
            return true;
        }

        private string MethodName(MethodInfo method)
        {
            StringBuilder sb = new();
            sb.Append(method.Name);

            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.Length > 0)
            {
                sb.Append(" (");
                for (int i = 0; i < parameters.Length - 1; i++)
                {
                    sb.Append(ParameterName(parameters[i]));
                    sb.Append(", ");
                }
                sb.Append(ParameterName(parameters[parameters.Length - 1]));
                sb.Append(")");
            }
            return sb.ToString();
        }

        private string ParameterName(ParameterInfo param)
        {
            StringBuilder sb = new();
            switch (param.ParameterType.Name)
            {
                case "String":
                    sb.Append("string");
                    break;
                case "Int32":
                    sb.Append("int");
                    break;
                case "Single":
                    sb.Append("float");
                    break;
                case "Boolean":
                    sb.Append("bool");
                    break;
            }
            sb.Append(" ");
            sb.Append(param.Name);

            return sb.ToString();
        }

        private string GetComponentName(Component component)
        {
            string[] strings = component.GetType().Name.Split('.');
            return strings[strings.Length - 1];
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            propertyOffset = 0;
            propertyHeight = 0;

            compInstIDProperty = property.FindPropertyRelative("componentInstanceID");
            actionProperty = property.FindPropertyRelative("action");
            tokenProperty = property.FindPropertyRelative("metadataToken");
            objProperty = property.FindPropertyRelative("obj");

            EditorGUI.BeginProperty(position, label, property);

            // FOLDOUT
            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, EditorStyles.foldoutHeader);
            propertyOffset += EditorGUIUtility.singleLineHeight * 1.5f;
            propertyHeight += EditorGUIUtility.singleLineHeight * 1.5f;

            if (property.isExpanded)
            {
                Rect inspectorBackgroundRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 1.25f, position.width + 5, property.FindPropertyRelative("propertyHeight").floatValue - EditorGUIUtility.singleLineHeight * 1.5f);
                EditorGUI.DrawRect(inspectorBackgroundRect, new Color(0.3f, 0.3f, 0.3f));

                EditorGUI.indentLevel++;
                Rect objRect = new Rect(position.x, position.y + propertyOffset, position.width * 0.49f, EditorGUIUtility.singleLineHeight);
                Rect compRect = new Rect(position.x + position.width * 0.51f, position.y + propertyOffset, position.width * 0.49f, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(objRect, objProperty, new GUIContent(""));
                propertyOffset += EditorGUIUtility.singleLineHeight * 1.2f;
                propertyHeight += EditorGUIUtility.singleLineHeight * 1.2f;

                // Object
                UnityEngine.Object obj = objProperty.objectReferenceValue;
                object target = null;
                Component component = null;
                MethodInfo[] methods = null;

                if (obj == null)
                {
                    EditorGUI.EndProperty();
                    propertyHeight += EditorGUIUtility.singleLineHeight * 0.25f;
                    property.FindPropertyRelative("propertyHeight").floatValue = propertyHeight;
                    return;
                }

                if (obj is Component cmp)
                {
                    obj = cmp.gameObject;
                }
                // GameObject components
                if (obj is GameObject go)
                {
                    int compIndex = 0;
                    Component[] goComponents = go.GetComponents(typeof(Component));
                    List<string> compNames = new();
                    int j = 0;
                    foreach (Component comp in goComponents)
                    {
                        if (comp.GetInstanceID() == compInstIDProperty.intValue) compIndex = j;
                        j++;

                        string compName = GetComponentName(comp);
                        int i = 2;
                        string compNameTemp = compName;
                        while (compNames.Contains(compNameTemp))
                        {
                            compNameTemp = compName + i;
                            i++;
                        }
                        compNames.Add(compNameTemp);
                    }

                    compIndex = EditorGUI.Popup(compRect, compIndex, compNames.ToArray());
                    component = goComponents[compIndex];
                    target = component;
                    compInstIDProperty.intValue = component.GetInstanceID();
                    methods = component.GetType().GetMethods();
                }
                else if (obj is ScriptableObject so)
                {
                    target = so;
                    methods = so.GetType().GetMethods();
                }
                else
                {
                    property.FindPropertyRelative("propertyHeight").floatValue = propertyHeight;
                    EditorGUI.EndProperty();
                    return;
                }

                // Methods
                List<MethodInfo> methodInfos = new();
                List<string> methodNames = new();
                foreach (MethodInfo method in methods)
                {
                    if (method.IsPublic && !method.IsAbstract && !method.IsGenericMethod && !method.IsConstructor && !method.IsAssembly
                        && !method.IsFamily && !method.ContainsGenericParameters && !method.IsSpecialName && method.ReturnType == typeof(void)
                        && ValidParameters(method.GetParameters()))
                    {
                        methodNames.Add(MethodName(method));
                        methodInfos.Add(method);
                    }
                }
                methodIndex = methodInfos.FindIndex(m => m.MetadataToken == tokenProperty.intValue);
                if (methodIndex == -1) methodIndex = 0;
                //_______________________________________

                Rect methodsRect = new Rect(position.x, position.y + propertyOffset, position.width, EditorGUIUtility.singleLineHeight);
                methodIndex = EditorGUI.Popup(methodsRect, methodIndex, methodNames.ToArray());
                propertyOffset += EditorGUIUtility.singleLineHeight * 1.2f;
                propertyHeight += EditorGUIUtility.singleLineHeight * 1.2f;

                tokenProperty.intValue = methodInfos[methodIndex].MetadataToken;
                ParameterInfo[] parameters = methodInfos[methodIndex].GetParameters();
                object[] parameterValues = new object[parameters.Length];

                // ---------------
                for (int i = 0; i < parameters.Length; i++)
                {
                    Rect valueRect = new Rect(position.x, position.y + propertyOffset, position.width, EditorGUIUtility.singleLineHeight);

                    switch (parameters[i].ParameterType.Name)
                    {
                        case nameof(System.Single):
                            property.FindPropertyRelative("float" + i).floatValue = EditorGUI.FloatField(valueRect, parameters[i].Name, property.FindPropertyRelative("float" + i).floatValue);
                            parameterValues[i] = property.FindPropertyRelative("float" + i).floatValue;
                            break;
                        case nameof(System.Boolean):
                            property.FindPropertyRelative("bool" + i).boolValue = EditorGUI.Toggle(valueRect, parameters[i].Name, property.FindPropertyRelative("bool" + i).boolValue);
                            parameterValues[i] = property.FindPropertyRelative("bool" + i).boolValue;
                            break;
                        case nameof(System.Int32):
                            property.FindPropertyRelative("int" + i).intValue = EditorGUI.IntField(valueRect, parameters[i].Name, property.FindPropertyRelative("int" + i).intValue);
                            parameterValues[i] = property.FindPropertyRelative("int" + i).intValue;
                            break;
                        case nameof(System.String):
                            property.FindPropertyRelative("string" + i).stringValue = EditorGUI.TextField(valueRect, parameters[i].Name, property.FindPropertyRelative("string" + i).stringValue);
                            parameterValues[i] = property.FindPropertyRelative("string" + i).stringValue;
                            break;
                    }
                    propertyOffset += EditorGUIUtility.singleLineHeight * 1.2f;
                    propertyHeight += EditorGUIUtility.singleLineHeight * 1.2f;
                }

                FieldInfo objField = property.serializedObject.targetObject.GetType().GetField(property.propertyPath);
                FieldInfo actionField = typeof(AdvancedUnityEvent).GetField("action");

                if (actionField != null && objField != null)
                {
                    switch (parameters.Length)
                    {
                        case 0:
                            Action action = (Action)methodInfos[methodIndex].CreateDelegate(typeof(Action), target);// Property.serializedObject.targetObject);
                            EventAction eventAction = new(action);
                            actionField.SetValue(objField.GetValue(property.serializedObject.targetObject), eventAction);
                            break;
                        case 1:
                            dynamic arg0 = parameterValues[0];
                            actionField.SetValue(objField.GetValue(property.serializedObject.targetObject), CreateAction(arg0, methodInfos[methodIndex], target));//Property.serializedObject.targetObject));
                            break;
                        case 2:
                            dynamic arg0_1 = parameterValues[0];
                            dynamic arg1_1 = parameterValues[1];
                            actionField.SetValue(objField.GetValue(property.serializedObject.targetObject), CreateAction(arg0_1, arg1_1, methodInfos[methodIndex], target));//Property.serializedObject.targetObject));
                            break;
                        case 3:
                            dynamic arg0_2 = parameterValues[0];
                            dynamic arg1_2 = parameterValues[1];
                            dynamic arg2_2 = parameterValues[2];
                            actionField.SetValue(objField.GetValue(property.serializedObject.targetObject), CreateAction(arg0_2, arg1_2, arg2_2, methodInfos[methodIndex], target));// Property.serializedObject.targetObject));
                            break;
                        case 4:
                            dynamic arg0_3 = parameterValues[0];
                            dynamic arg1_3 = parameterValues[1];
                            dynamic arg2_3 = parameterValues[2];
                            dynamic arg3_3 = parameterValues[3];
                            actionField.SetValue(objField.GetValue(property.serializedObject.targetObject), CreateAction(arg0_3, arg1_3, arg2_3, arg3_3, methodInfos[methodIndex], target));// Property.serializedObject.targetObject));
                            break;
                        case 5:
                            dynamic arg0_4 = parameterValues[0];
                            dynamic arg1_4 = parameterValues[1];
                            dynamic arg2_4 = parameterValues[2];
                            dynamic arg3_4 = parameterValues[3];
                            dynamic arg4_4 = parameterValues[4];
                            actionField.SetValue(objField.GetValue(property.serializedObject.targetObject), CreateAction(arg0_4, arg1_4, arg2_4, arg3_4, arg4_4, methodInfos[methodIndex], target));// Property.serializedObject.targetObject));
                            break;
                    }

                }
                propertyOffset += EditorGUIUtility.singleLineHeight * 0.25f;
                propertyHeight += EditorGUIUtility.singleLineHeight * 0.25f;
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();

            property.FindPropertyRelative("propertyHeight").floatValue = propertyHeight;
        }

        private EventAction<T1> CreateAction<T1>(T1 arg0, MethodInfo methodInfo, object target)
        {
            Action<T1> action = (Action<T1>)methodInfo.CreateDelegate(typeof(Action<T1>), target);
            return new EventAction<T1>(action, arg0);
        }
        private EventAction<T1, T2> CreateAction<T1, T2>(T1 arg0, T2 arg1, MethodInfo methodInfo, object target)
        {
            Action<T1, T2> action = (Action<T1, T2>)methodInfo.CreateDelegate(typeof(Action<T1, T2>), target);
            return new EventAction<T1, T2>(action, arg0, arg1);
        }
        private EventAction<T1, T2, T3> CreateAction<T1, T2, T3>(T1 arg0, T2 arg1, T3 arg2, MethodInfo methodInfo, object target)
        {
            Action<T1, T2, T3> action = (Action<T1, T2, T3>)methodInfo.CreateDelegate(typeof(Action<T1, T2, T3>), target);
            return new EventAction<T1, T2, T3>(action, arg0, arg1, arg2);
        }
        private EventAction<T1, T2, T3, T4> CreateAction<T1, T2, T3, T4>(T1 arg0, T2 arg1, T3 arg2, T4 arg3, MethodInfo methodInfo, object target)
        {
            Action<T1, T2, T3, T4> action = (Action<T1, T2, T3, T4>)methodInfo.CreateDelegate(typeof(Action<T1, T2, T3, T4>), target);
            return new EventAction<T1, T2, T3, T4>(action, arg0, arg1, arg2, arg3);
        }
        private EventAction<T1, T2, T3, T4, T5> CreateAction<T1, T2, T3, T4, T5>(T1 arg0, T2 arg1, T3 arg2, T4 arg3, T5 arg4, MethodInfo methodInfo, object target)
        {
            Action<T1, T2, T3, T4, T5> action = (Action<T1, T2, T3, T4, T5>)methodInfo.CreateDelegate(typeof(Action<T1, T2, T3, T4, T5>), target);
            return new EventAction<T1, T2, T3, T4, T5>(action, arg0, arg1, arg2, arg3, arg4);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.FindPropertyRelative("propertyHeight").floatValue;
        }
    }
}
