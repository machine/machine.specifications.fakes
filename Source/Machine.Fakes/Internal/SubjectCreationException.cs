using System;
using System.Text;
using Machine.Specifications;
using StructureMap;

namespace Machine.Fakes.Internal
{
	internal class SubjectCreationException : SpecificationException
	{
		public SubjectCreationException(
			Type targetType, 
			StructureMapException structureMapException) : base(Format(targetType, structureMapException))
		{
		}

		private static string Format(Type targetType, StructureMapException structureMapException)
		{
			var messageBuilder = new StringBuilder();
			messageBuilder.AppendFormat("Unable to create an instance of the target type {0}.", targetType.Name);
			messageBuilder.AppendLine();

			switch (structureMapException.ErrorCode)
			{
				case 207:
					messageBuilder.Append("The constructor threw an exception.");
					break;

				case 202:
					messageBuilder.Append("Please check that the type has at least a single public constructor!");
					break;
		
				default:
					return "";
			}

			return messageBuilder.ToString();
		}
	}
}