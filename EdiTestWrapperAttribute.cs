using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using JetBrains.Annotations;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery
{
    [SuppressMessage("ReSharper", "RedundantAttributeUsageProperty")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public abstract class EdiTestWrapperAttribute : Attribute, IEquatable<EdiTestWrapperAttribute>
    {
        public bool Equals([CanBeNull] EdiTestWrapperAttribute other)
        {
            if(ReferenceEquals(null, other)) return false;
            if(ReferenceEquals(this, other)) return true;
            return GetType() == other.GetType() && TryGetIdentity() == other.TryGetIdentity();
        }

        public override bool Equals([CanBeNull] object obj)
        {
            if(ReferenceEquals(null, obj)) return false;
            if(ReferenceEquals(this, obj)) return true;
            var other = obj as EdiTestWrapperAttribute;
            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            var identity = TryGetIdentity();
            if(identity == null)
                return GetType().GetHashCode();
            unchecked
            {
                return (identity.GetHashCode() * 397) ^ GetType().GetHashCode();
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}", GetType().Name);
            var identity = TryGetIdentity();
            if(identity != null)
                sb.AppendFormat("#{0}", identity);
            return sb.ToString();
        }

        [CanBeNull]
        protected virtual string TryGetIdentity()
        {
            return null;
        }
    }
}