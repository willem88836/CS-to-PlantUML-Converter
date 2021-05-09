
namespace myApp
{
	public class Resources
	{
		public string DOCUMENT_HEADER = "'Made with the PlantUMLBuilder.\n@startuml {0}\nskinparam classAttributeIconSize 0\n";
		public string DOCUMENT_FOOTER = "@enduml";

		public string COLOR_CLASS = "#FBFFAD";
		public string COLOR_INTERFACE = "#34C3EB";
		public string COLOR_ABSTRACT_CLASS = "#EB8034";

		public string CLASS_TOKEN = "class";
		public string INTERFACE_TOKEN = "interface";
		public string ABSTRACT_CLASS_TOKEN = "abstract";

		public string CLASS_PREFIX = "{0} \"{1}\" as {2} {3} {{\n";
		public string CLASS_SUFFIX = "\n}\n";

		public string FIELD_FORMAT = "\t{0} {1}: {2}\n";
		public string PROPERTY_FORMAT = "\t<{0},{1}> {2}: {3}\n";

		public string METHOD_PREFIX = "\t{0} {1}(";
		public string METHOD_SUFFIX = "): {0}\n";

		public string PARAMETER_FORMAT = "{0}: {1}, ";

		public string INHERITS_FORMAT = "\"{0}\" <|-- \"{1}\"\n";
		public string IMPLEMENTS_FORMAT = "\"{0}\" o-- \"{1}\"\n";

		public string USES_FIELD_FORMAT = "\"{0}\" --> \"{1}\": \" {2} {3}\"\n";
		public string USES_PROPERTY_FORMAT = "\"{0}\" --> \"{1}\": \" <{2},{3}> {4}\"\n";

		public string TOKEN_PUBLIC = "+";
		public string TOKEN_PRIVATE = "-";
		public string TOKEN_PROTECTED = "#";
		public string TOKEN_MISSING = "x";
	}
}
