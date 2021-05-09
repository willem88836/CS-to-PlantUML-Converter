using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace myApp
{
	/// <summary>
	///		Builds a PlantUML data file using a C# assembly file.
	/// </summary>
	public class PlantUMLBuilder
	{
		private readonly Assembly assembly;
		private readonly Resources resources;
		private readonly Type[] types;
		private readonly StringBuilder nodeBuilder;
		private readonly StringBuilder edgeBuilder;


		public PlantUMLBuilder(string assemblyPath, Resources resources)
		{
			this.assembly = Assembly.LoadFrom(assemblyPath);
			this.resources = resources;
			this.nodeBuilder = new StringBuilder();
			this.edgeBuilder = new StringBuilder();
			this.types = assembly.GetTypes();
		}

		/// <summary>
		///		Compiles the code from the given assembly.
		/// </summary>
		/// <returns>Path of the written file.</returns>
		public string CompilePlantUMLCode()
		{
			nodeBuilder.AppendFormat(resources.DOCUMENT_HEADER, assembly.GetName().Name);

			CreateNodes();
			nodeBuilder.Append(edgeBuilder);

			nodeBuilder.Append(resources.DOCUMENT_FOOTER);

			return nodeBuilder.ToString();
		}

		private void CreateNodes()
		{
			foreach(Type type in types)
			{
				TypeInfo typeInfo = type.GetTypeInfo();

				// Filters types that are not self-made.
				if (typeInfo.IsExtraSpecialName())
					continue;

				// Sets what color the class should get, and with what token it is displayed.
				string classColor = typeInfo.IsInterface ? resources.COLOR_INTERFACE : typeInfo.IsAbstract ? resources.COLOR_ABSTRACT_CLASS : resources.COLOR_CLASS;
				string classToken = typeInfo.IsInterface ? resources.INTERFACE_TOKEN : typeInfo.IsAbstract ? resources.ABSTRACT_CLASS_TOKEN : resources.CLASS_TOKEN;
				nodeBuilder.AppendFormat(resources.CLASS_PREFIX, classToken, type.Name, type.SGUID(), classColor);

				BuildInheritance(typeInfo);
				BuildFields(typeInfo);
				BuildProperties(typeInfo);
				BuildMethods(typeInfo);

				nodeBuilder.Append(resources.CLASS_SUFFIX);
			}
		}

		/// <summary>
		///		Draws the inheritance arrows between elements.
		/// </summary>
		/// <param name="typeInfo">Current type.</param>
		private void BuildInheritance(TypeInfo typeInfo)
		{
			Type baseType = typeInfo.BaseType;
			var interfaces = typeInfo.MostGenericImplementedInterfaces();

			foreach (Type t in interfaces
				.Where(u => !interfaces
					.Where(v => u.IsAssignableFrom(v) && v != u)
					.Any()
				&& (!baseType?.GetTypeInfo().ImplementedInterfaces.Contains(u) ?? true)
				&& baseType != u
				&& this.types.Contains(u)))
			{
				edgeBuilder.AppendFormat(resources.IMPLEMENTS_FORMAT, t.SGUID(), typeInfo.SGUID());
			}

			if (baseType != null 
				&& !typeInfo.BaseType.IsExtraSpecialName()
				&& types.Contains(typeInfo.BaseType))
			{
				edgeBuilder.AppendFormat(resources.INHERITS_FORMAT, typeInfo.BaseType.SGUID(), typeInfo.SGUID());
			}
		}

		private void BuildFields(TypeInfo typeInfo)
		{
			foreach (FieldInfo field in typeInfo.DeclaredFields)
			{
				if (field.IsExtraSpecialName())
					continue;

				string sign = field.AccessibilitySign(this.resources);

				if(field.FieldType.IsPrimitive || !this.types.Contains(field.FieldType))
					nodeBuilder.AppendFormat(resources.FIELD_FORMAT, sign, field.Name, field.FieldType.Name);
				else
					edgeBuilder.AppendFormat(resources.USES_FIELD_FORMAT, typeInfo.SGUID(), field.SGUID(), sign, field.Name);
			}
		}

		private void BuildProperties(TypeInfo typeInfo)
		{
			foreach (PropertyInfo property in typeInfo.DeclaredProperties
				.Where(p => !typeInfo.ImplementedInterfaces
					.Where(i => i.GetProperty(p.Name) != null)
					.Any()))
			{
				if (property.IsExtraSpecialName())
					continue;

				string getAccess = property.AccessibilitySignGet(this.resources);
				string setAccess = property.AccessibilitySignSet(this.resources);

				if (property.PropertyType.IsPrimitive || !this.types.Contains(property.PropertyType))
					nodeBuilder.AppendFormat(resources.PROPERTY_FORMAT, getAccess, setAccess, property.Name, property.PropertyType.Name);
				else
					edgeBuilder.AppendFormat(resources.USES_PROPERTY_FORMAT, typeInfo.SGUID(), property.SGUID(), getAccess, setAccess, property.Name);
			}
		}

		private void BuildMethods(TypeInfo typeInfo)
		{
			var methodInfos = typeInfo.DeclaredMethods;
			foreach (MethodInfo method in methodInfos.Where(m => !m.IsOverride()))
			{
				if (method.IsExtraSpecialName())
					continue;

				string sign = method.AccessibilitySign(this.resources);
				nodeBuilder.AppendFormat(resources.METHOD_PREFIX, sign, method.Name);

				var parameters = method.GetParameters();
				foreach (ParameterInfo par in parameters)
					nodeBuilder.AppendFormat(resources.PARAMETER_FORMAT, par.Name, par.ParameterType.Name);

				if (parameters.Length > 0)
					nodeBuilder.Remove(nodeBuilder.Length - 2, 2);

				nodeBuilder.AppendFormat(resources.METHOD_SUFFIX, method.ReturnParameter.ParameterType.Name);
			}

			if (methodInfos.Count() > 0)
				nodeBuilder.Remove(nodeBuilder.Length - 1, 1);
		}
	}
}
