using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PlantUmlBuilder
{
	public static class Utils
	{
		public static string ALPHA = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

		public static char FirstNameChar(this Type type)
		{
			return type.Name[0];
		}
		public static char FirstNameChar(this FieldInfo field)
		{
			return field.Name[0];
		}
		public static char FirstNameChar(this PropertyInfo property)
		{
			return property.Name[0];
		}
		public static char FirstNameChar(this MethodInfo method)
		{
			return method.Name[0];
		}


		public static string SGUID(this Type type)
		{
			return type.GUID.ToString();
		}
		public static string SGUID(this FieldInfo field)
		{
			return field.FieldType.GUID.ToString();
		}
		public static string SGUID(this PropertyInfo property)
		{
			return property.PropertyType.GUID.ToString();
		}
		public static string SGUID(this MethodInfo method)
		{
			return method.ReturnType.GUID.ToString();
		}


		public static bool IsExtraSpecialName(this Type type)
		{
			return type.IsSpecialName || !ALPHA.Contains(type.FirstNameChar());
		}
		public static bool IsExtraSpecialName(this FieldInfo field)
		{
			return field.IsSpecialName || !ALPHA.Contains(field.FirstNameChar());
		}
		public static bool IsExtraSpecialName(this PropertyInfo property)
		{
			return property.IsSpecialName || !ALPHA.Contains(property.FirstNameChar());
		}
		public static bool IsExtraSpecialName(this MethodInfo method)
		{
			return method.IsSpecialName || !ALPHA.Contains(method.FirstNameChar());
		}
	
	
		public static string AccessibilitySign(this FieldInfo field, Resources resources)
		{
			return field.IsPublic? resources.TOKEN_PUBLIC : (field.IsPrivate ? resources.TOKEN_PRIVATE : resources.TOKEN_PROTECTED);
		}
		public static string AccessibilitySignGet(this PropertyInfo property, Resources resources)
		{
			MethodInfo getInfo = property.GetGetMethod();
			return getInfo == null
				? resources.TOKEN_MISSING
				: AccessibilitySign(getInfo, resources);
		}
		public static string AccessibilitySignSet(this PropertyInfo property, Resources resources)
		{
			MethodInfo setInfo = property.GetSetMethod();
			return setInfo == null
				? resources.TOKEN_MISSING
				: AccessibilitySign(setInfo, resources);
		}
		public static string AccessibilitySign(this MethodInfo method, Resources resources)
		{
			return method.IsPublic 
				? resources.TOKEN_PUBLIC 
				: (method.IsPrivate 
					? resources.TOKEN_PRIVATE 
					: resources.TOKEN_PROTECTED);
		}


		public static bool IsOverride(this MethodInfo methodInfo)
		{
			return methodInfo.ReflectedType.GetTypeInfo().InheritingTypes()
				.Where(u => u.GetMethods()
					.Where(m => m.Name.Equals(methodInfo.Name))
					.Any())
				.Any();
		}


		public static IEnumerable<Type> InheritingTypes(this TypeInfo type)
		{
			Type baseType = type.BaseType;
			if (baseType != null)
				return type.ImplementedInterfaces.Append(type.BaseType);
			else
				return type.ImplementedInterfaces;
		}

		public static IEnumerable<Type> MostGenericImplementedInterfaces(this Type type)
		{
			return type.GetTypeInfo().ImplementedInterfaces.Select(i => i.IsGenericType ? i.GetGenericTypeDefinition() : i);
		}
	}
}
