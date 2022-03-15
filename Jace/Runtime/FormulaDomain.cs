using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Jace;
using Jace.Execution;
using UnityEngine;

namespace Contra.Core.DataTypes
{
    public abstract class FormulaDomain : ScriptableObject, IDictionary<string, double>
    {
        protected CalculationEngine Jace;

        protected virtual void OnEnable()
        {
            Jace = new CalculationEngine(new JaceOptions()
            {
                CultureInfo = CultureInfo.InvariantCulture,
                ExecutionMode = ExecutionMode.Compiled, //TODO: interpreted for switch
                CaseSensitive = true,
                DefaultConstants = false,
                DefaultFunctions = false,
            });
        }

        public Func<IDictionary<string, double>, double> Build(Formula formula)
        {
            return !string.IsNullOrEmpty(formula.expression) ? Jace.Build(formula.expression) : null;
        }

        /**
         * Only used for Editor, dont use it in code
         */
        public bool IsValid(Formula formula)
        {
            try
            {
                var value = Jace.Calculate(formula.expression, this);
                if (!double.IsNaN(value) && !double.IsPositiveInfinity(value) && !double.IsNegativeInfinity(value))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
        protected virtual void OnDisable()
        {
            Jace = null;
        }

        
        
        #region idictionary must implement to use variables
        public abstract bool TryGetValue(string key, out double value);
        public abstract ICollection<string> Keys { get; }
        #endregion
        #region IDictionary not implemented, not used for Jace
        public IEnumerator<KeyValuePair<string, double>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<string, double> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, double> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, double>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, double> item)
        {
            throw new NotImplementedException();
        }

        public int Count => throw new NotImplementedException();
        public bool IsReadOnly => throw new NotImplementedException();
        public void Add(string key, double value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }



        public double this[string key]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public ICollection<double> Values =>throw new NotImplementedException();
        #endregion

    }
}