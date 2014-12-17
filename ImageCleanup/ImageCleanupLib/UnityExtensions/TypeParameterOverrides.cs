namespace ImageCleanupLib.UnityExtensions
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Use this to send parameters and have them automatically map to the correct constructor order in the resolving class
    /// </summary>
    public class TypeParameterOverrides : ResolverOverride
    {
        #region Fields

        private readonly IDictionary<Type, InjectionParameterValue> parameterValues;

        #endregion

        #region Constructors and Destructors

        public TypeParameterOverrides(params object[] parameterValues)
        {
            this.parameterValues = new Dictionary<Type, InjectionParameterValue>();
            foreach (var parameterValue in parameterValues)
            {
                if (this.parameterValues.ContainsKey(parameterValue.GetType()))
                {
                    throw new NotSupportedException(
                        "TypeParameterOverrides does not support multiple properties with the same type. Please use another override.");
                }

                this.parameterValues.Add(parameterValue.GetType(), InjectionParameterValue.ToParameter(parameterValue));
            }
        }

        #endregion

        #region Public Methods and Operators

        public override IDependencyResolverPolicy GetResolver(IBuilderContext context, Type dependencyType)
        {
            if ((this.parameterValues.Count < 1) || (!this.parameterValues.ContainsKey(dependencyType)))
            {
                return null;
            }

            var value = this.parameterValues[dependencyType];
            return value.GetResolverPolicy(dependencyType);
        }

        #endregion
    }
}