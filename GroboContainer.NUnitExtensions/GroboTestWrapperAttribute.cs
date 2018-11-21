using System;
using System.Text;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions
{
    public abstract class GroboTestWrapperAttribute : Attribute, IEquatable<GroboTestWrapperAttribute>
    {
        public bool Equals([CanBeNull] GroboTestWrapperAttribute other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return GetType() == other.GetType() && TryGetIdentity() == other.TryGetIdentity();
        }

        public override bool Equals([CanBeNull] object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is GroboTestWrapperAttribute other && Equals(other);
        }

        public override int GetHashCode()
        {
            var identity = TryGetIdentity();
            if (identity == null)
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
            if (identity != null)
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