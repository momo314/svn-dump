// .NET Compact Framework 1.0 has no support for reading assembly attributes
#if !NETCF

using System;
using System.Reflection;

using GodLesZ.Library.Logging.Util;
using GodLesZ.Library.Logging.Repository;
using GodLesZ.Library.Logging.Core;

namespace GodLesZ.Library.Logging.Config {
	/// <summary>
	/// Assembly level attribute to configure the <see cref="SecurityContextProvider"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This attribute may only be used at the assembly scope and can only
	/// be used once per assembly.
	/// </para>
	/// <para>
	/// Use this attribute to configure the <see cref="XmlConfigurator"/>
	/// without calling one of the <see cref="XmlConfigurator.Configure()"/>
	/// methods.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	[AttributeUsage(AttributeTargets.Assembly)]
	[Serializable]
	public sealed class SecurityContextProviderAttribute : ConfiguratorAttribute {
		#region Constructor

		/// <summary>
		/// Construct provider attribute with type specified
		/// </summary>
		/// <param name="providerType">the type of the provider to use</param>
		/// <remarks>
		/// <para>
		/// The provider specified must subclass the <see cref="SecurityContextProvider"/>
		/// class.
		/// </para>
		/// </remarks>
		public SecurityContextProviderAttribute(Type providerType)
			: base(100) /* configurator priority 100 to execute before the XmlConfigurator */
		{
			m_providerType = providerType;
		}

		#endregion

		#region Public Instance Properties

		/// <summary>
		/// Gets or sets the type of the provider to use.
		/// </summary>
		/// <value>
		/// the type of the provider to use.
		/// </value>
		/// <remarks>
		/// <para>
		/// The provider specified must subclass the <see cref="SecurityContextProvider"/>
		/// class.
		/// </para>
		/// </remarks>
		public Type ProviderType {
			get { return m_providerType; }
			set { m_providerType = value; }
		}

		#endregion Public Instance Properties

		#region Override ConfiguratorAttribute

		/// <summary>
		/// Configures the SecurityContextProvider
		/// </summary>
		/// <param name="sourceAssembly">The assembly that this attribute was defined on.</param>
		/// <param name="targetRepository">The repository to configure.</param>
		/// <remarks>
		/// <para>
		/// Creates a provider instance from the <see cref="ProviderType"/> specified.
		/// Sets this as the default security context provider <see cref="SecurityContextProvider.DefaultProvider"/>.
		/// </para>
		/// </remarks>
		override public void Configure(Assembly sourceAssembly, ILoggerRepository targetRepository) {
			if (m_providerType == null) {
				LogLog.Error(declaringType, "Attribute specified on assembly [" + sourceAssembly.FullName + "] with null ProviderType.");
			} else {
				LogLog.Debug(declaringType, "Creating provider of type [" + m_providerType.FullName + "]");

				SecurityContextProvider provider = Activator.CreateInstance(m_providerType) as SecurityContextProvider;

				if (provider == null) {
					LogLog.Error(declaringType, "Failed to create SecurityContextProvider instance of type [" + m_providerType.Name + "].");
				} else {
					SecurityContextProvider.DefaultProvider = provider;
				}
			}
		}

		#endregion

		#region Private Instance Fields

		private Type m_providerType = null;

		#endregion Private Instance Fields

		#region Private Static Fields

		/// <summary>
		/// The fully qualified type of the SecurityContextProviderAttribute class.
		/// </summary>
		/// <remarks>
		/// Used by the internal logger to record the Type of the
		/// log message.
		/// </remarks>
		private readonly static Type declaringType = typeof(SecurityContextProviderAttribute);

		#endregion Private Static Fields
	}
}

#endif // !NETCF