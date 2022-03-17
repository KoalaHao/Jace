using System;
using System.Collections.Generic;
using System.Globalization;
using Jace.Execution;
using UnityEngine;

namespace Jace.Runtime
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

        private static CalculationEngine _calcEngine;
        private static Dictionary<string, double> _tempVariable;
        public void CompileRaw()
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }
            _calcEngine ??= new CalculationEngine(new JaceOptions()
            {
                CultureInfo = CultureInfo.InvariantCulture,
                ExecutionMode = ExecutionMode.Compiled, //TODO: interpreted for switch
                CaseSensitive = true,
                DefaultConstants = false,
                DefaultFunctions = true,
            });
            try
            {
                Compiled = _calcEngine.Build(expression);
            }
            catch (Exception)
            {
                Debug.LogError($"{expression} was not compiled successfully");
            }
        }

        public float GetValueWithV(float v)
        {
            if (Compiled == null && !string.IsNullOrEmpty(expression))
            {
                CompileRaw();
            }
            if (Compiled != null)
            {
                _tempVariable ??= new Dictionary<string, double>();
                _tempVariable["v"] = v;
                return (float) Compiled(_tempVariable);
            }
            else if(string.IsNullOrEmpty(expression))
                return fallback;
            else
            {
                Debug.LogWarning($"formula {expression} was not compiled, fallback {fallback} was returned");
                return fallback;
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