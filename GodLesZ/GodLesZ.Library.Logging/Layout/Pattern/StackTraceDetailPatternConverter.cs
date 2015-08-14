#if !NETCF

using System;
using System.Collections;
using System.Text;

using GodLesZ.Library.Logging.Util;
using GodLesZ.Library.Logging.Core;

namespace GodLesZ.Library.Logging.Layout.Pattern {
	/// <summary>
	/// Write the caller stack frames to the output
	/// </summary>
	/// <remarks>
	/// <para>
	/// Writes the <see cref="LocationInfo.StackFrames"/> to the output writer, using format:
	/// type3.MethodCall3(type param,...) > type2.MethodCall2(type param,...) > type1.MethodCall1(type param,...)
	/// </para>
	/// </remarks>
	/// <author>Adam Davies</author>
	internal class StackTraceDetailPatternConverter : StackTracePatternConverter {
		internal override string GetMethodInformation(System.Reflection.MethodBase method) {
			string returnValue = "";

			try {
				string param = "";
				string[] names = GetMethodParameterNames(method);
				StringBuilder sb = new StringBuilder();
				if (names != null && names.GetUpperBound(0) > 0) {
					for (int i = 0; i <= names.GetUpperBound(0); i++) {
						sb.AppendFormat("{0}, ", names[i]);
					}
				}

				if (sb.Length > 0) {
					sb.Remove(sb.Length - 2, 2);
					param = sb.ToString();
				}

				returnValue = base.GetMethodInformation(method) + "(" + param + ")";
			} catch (Exception ex) {
				LogLog.Error(declaringType, "An exception ocurred while retreiving method information.", ex);
			}

			return returnValue;
		}

		private string[] GetMethodParameterNames(System.Reflection.MethodBase methodBase) {
			ArrayList methodParameterNames = new ArrayList();
			try {
				System.Reflection.ParameterInfo[] methodBaseGetParameters = methodBase.GetParameters();

				int methodBaseGetParametersCount = methodBaseGetParameters.GetUpperBound(0);

				for (int i = 0; i <= methodBaseGetParametersCount; i++) {
					methodParameterNames.Add(methodBaseGetParameters[i].ParameterType + " " + methodBaseGetParameters[i].Name);
				}
			} catch (Exception ex) {
				LogLog.Error(declaringType, "An exception ocurred while retreiving method parameters.", ex);
			}

			return (string[])methodParameterNames.ToArray(typeof(string));
		}

		#region Private Static Fields

		/// <summary>
		/// The fully qualified type of the StackTraceDetailPatternConverter class.
		/// </summary>
		/// <remarks>
		/// Used by the internal logger to record the Type of the
		/// log message.
		/// </remarks>
		private readonly static Type declaringType = typeof(StackTracePatternConverter);

		#endregion Private Static Fields
	}
}
#endif