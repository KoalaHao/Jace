using System;
using System.Collections.Generic;
using UnityEngine;

namespace Contra.Core.DataTypes
{
    [Serializable]
    public class Formula
    {
        public string expression;
        public float fallback;
        protected Func<IDictionary<string, double>, double> Compiled;
        protected FormulaDomain domain;

        /**
         * Only used for Editor, dont use it elsewhere
         */
        internal bool IsValid(FormulaDomain testDomain)
        {
            return testDomain.IsValid(this);
        }

        public float Value
        {
            get
            {
                if (Compiled != null)
                    return (float) Compiled(domain);
                else if(string.IsNullOrEmpty(expression))
                    return fallback;
                else
                {
                    Debug.LogWarning($"formula {expression} was not compiled, fallback {fallback} was returned");
                    return fallback;
                }
            }
        }

        public void Compile(FormulaDomain formulaDomain)
        {
            if (string.IsNullOrEmpty(expression) || formulaDomain == null)
            {
                return;
            }
            domain = formulaDomain;
            try
            {
                Compiled = domain.Build(this);
            }
            catch (Exception)
            {
                Debug.LogError($"{expression} was not compiled successfully");
            }
        }
    }
}