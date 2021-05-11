# CS-to-PlantUML-Converter
Mini C#/.NET project that compiles a PlantUML class diagram using a C# Project's assembly file. 

Note, this program only creates the PlantUML code, no PNGs or any other visuals. 
I recommend using the [PlantUML](https://marketplace.visualstudio.com/items?itemName=jebbs.plantuml) plugin available in visual studio code to view the corresponding image.

## Quick-start
* Clone the project
* Compile and run it
* Open the folder in which the compiled code is located (e.g. ``Debug`` or ``Release``). 
* Open the ``settings.json`` file. 
* Set the ``AssemblyPath`` variable to your C# assembly file. 
* Run the ``PlantUMLConverer.exe``
* You can find your generated PlantUMLCode in the ``out.wsd`` file.

## Customisation
This tool has a number of customisation options.
The ``settings.json`` file contains the values ``AssemblyPath``, ``ResourcesPath``, and ``SavePath``. 
By default, these (excluding ``AssemblyPath``) point to the installation folder, but can be set to anything else. 

The ``resources.json`` file contains the formatting used to generate the PlantUML code -- an array of options. 
Everything that is possible within these formats can be found on the [PlantUML Webpage](https://plantuml.com/). 
Some of these are given formatting parameters, which you can address with ``{x}`` where x is the somanieth parameter. 
* ``DOCUMENT_HEADER`` contains all code that is to be added prior to generating anything else (e.g. formatting settings). 
  * ``{0}`` is the name of the assembly file. 
* ``DOCUMENT_FOOTER`` contains all code that is to be added after generating everything else. 
* ``COLOR_CLASS`` represents the color of the class item, the 3rd parameter of the ``CLASS_PREFIX``. 
* ``COLOR_INTERFACE`` Same story, but for interfaces. 
* ``COLOR_ABSTRACT_CLASS`` Same story, but for abstract classes. 
* ``CLASS_TOKEN`` The token used for classes, the first paramter of ``CLASS_PREFIX``.
* ``INTERFACE_TOKEN`` Same story, but for interfaces. 
* ``ABSTRACT_CLASS_TOKEN`` Same story, but for abstract classes. 
* ``CLASS_PREFIX`` Code that is written prior to generating the inners of a class. 
  * ``{0}`` The class token (either ``CLASS_TOKEN``, ``INTERFACE_TOKEN``, or ``ABSTRACT_CLASS_TOKEN``).
  * ``{1}`` The name of the class.
  * ``{2}`` The GUID (unique identifier) of the class. 
  * ``{3}`` The color of the class (either ``COLOR_CLASS``, ``COLOR_INTERFACE``, or ``COLOR_ABSTRACT_CLASS``).
* ``CLASS_SUFFIX`` The code that is added after generating the inners of a class. 
* ``FIELD_FORMAT`` Format used to represent a field -- this is only used, if the field's type is primitive or when the type is not present in the compiled assembly, otherwise, an arrow (``USES_FIELD_FORMAT``) is drawn. 
  * ``{0}`` Accessibility token of a field (either ``TOKEN_PRIVATE``, ``TOKEN_PUBLIC``, or ``TOKEN_PROTECTED``).
  * ``{1}`` The field's typename.
* ``PROPERTY_FORMAT`` The codeused to represent a property -- this is only used when the field's type is primitive or when the type is not present in the compiled assembly, otherwise an arrow (``USES_PROPERTY_FORMAT``) is drawn. 
  * ``{0}`` Accessibility token of the get field (if unavailable, it shows the ``TOKEN_MISSING``). 
  * ``{1}`` Accessibility token of the set field. 
  * ``{2}`` Name of the property. 
  * ``{3}`` The property's type.
* ``METHOD_PREFIX`` Code written prior to writing a method's parameters. 
  * ``{0}`` The method's accessibility token. 
  * ``{1}`` The method's name.
* ``METHOD_SUFFIX`` Code written after writing a method's parameters. 
  * ``{0}``  The method's return type. 
* ``PARAMETER_FORMAT`` format of a a single parameter -- note, the last element (i.e. the last comma) is removed after the final parameter. 
  * ``{0}`` The parameter's name. 
  * ``{1}`` The parameter's type.   
* ``INHERITS_FORMAT`` format used to draw the is-relation arrow for inheritance -- note the element that is mentioned first is tried to be placed above the latter. 
  * ``{0}`` The GUID of the super class. 
  * ``{1}`` The GUID of the inheriting class.
* ``IMPLEMENTS_FORMAT`` format used to draw the is-relation arrow for implementing interfaces -- note the element that is mentioned first is tried to be placed above the latter. 
  * ``{0}`` The GUID of the interface. 
  * ``{1}`` The GUID of the implementing class.
* ``USES_FIELD_FORMAT`` format used to draw the has-relation arrow for fields -- note the element that is mentioned first is tried to be placed above the latter. 
  * ``{0}`` The GUID of the class that is implementing. 
  * ``{1}`` The GUID of the class that is implemented. 
* ``USES_PROPERTY_FORMAT`` format used to draw the has-relation arrow for properties -- note the element that is mentioned first is tried to be placed above the latter.
  * ``{0}`` The GUID of the class that is implementing. 
  * ``{1}`` The GUID of the class that is implemented. 
* ``TOKEN_PUBLIC``, ``TOKEN_PRIVATE``, ``TOKEN_PROTECTED``, ``TOKEN_MISSING`` are tokens used to describe the accessibility of e.g. fields.
