using System;
using EnsureThat;

namespace MyInfluxDbClient
{
    public class RetentionPolicyDuration : IEquatable<RetentionPolicyDuration>, IEquatable<string>
    {
        private readonly string _value;

        private RetentionPolicyDuration(string unit, int? value = null)
        {
            if (value.HasValue)
                Ensure.That(value.Value, nameof(value)).IsGt(0);

            _value = $"{value}{unit}";
        }

        public static RetentionPolicyDuration Minutes(int value)
        {
            return new RetentionPolicyDuration("m", value);
        }

        public static RetentionPolicyDuration Hours(int value)
        {
            return new RetentionPolicyDuration("h", value);
        }

        public static RetentionPolicyDuration Days(int value)
        {
            return new RetentionPolicyDuration("d", value);
        }

        public static RetentionPolicyDuration Weeks(int value)
        {
            return new RetentionPolicyDuration("w", value);
        }

        public static RetentionPolicyDuration Infinite()
        {
            return new RetentionPolicyDuration("INF");
        }

        public override string ToString()
        {
            return _value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is string
                ? Equals((string)obj)
                : Equals(obj as RetentionPolicyDuration);
        }

        public bool Equals(RetentionPolicyDuration other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals(other._value);
        }

        public bool Equals(string other)
        {
            return string.Equals(_value, other);
        }

        public static implicit operator string (RetentionPolicyDuration item)
        {
            return item.ToString();
        }
    }
}