using System.Reflection;

namespace KarateClub.Application.Security
{
    public static class Permissions
    {
        public static class Users
        {
            public const string View = "Users.View";
            public const string Create = "Users.Create";
            public const string Update = "Users.Update";
            public const string Delete = "Users.Delete";
        }

        public static class Instructors
        {
            public const string View = "Instructors.View";
            public const string Create = "Instructors.Create";
            public const string Update = "Instructors.Update";
            public const string Delete = "Instructors.Delete";
        }

        public static class BeltRanks
        {
            public const string View = "BeltRanks.View";
            public const string Create = "BeltRanks.Create";
            public const string Update = "BeltRanks.Update";
            public const string Delete = "BeltRanks.Delete";
        }

        public static class SubscriptionPeriods
        {
            public const string View = "SubscriptionPeriods.View";
            public const string Create = "SubscriptionPeriods.Create";
            public const string Update = "SubscriptionPeriods.Update";
            public const string Delete = "SubscriptionPeriods.Delete";
        }

        public static class BeltTests
        {
            public const string View = "BeltTests.View";
            public const string Create = "BeltTests.Create";
            public const string Update = "BeltTests.Update";
            public const string Delete = "BeltTests.Delete";
        }

        public static class Payments
        {
            public const string View = "Payments.View";
            public const string Create = "Payments.Create";
            public const string Update = "Payments.Update";
            public const string Delete = "Payments.Delete";
        }

        public static class Members
        {
            public const string View = "Members.View";
            public const string Create = "Members.Create";
            public const string Update = "Members.Update";
            public const string Delete = "Members.Delete";
        }

        public static IEnumerable<string> GetAll()
        {
            return typeof(Permissions).GetNestedTypes(BindingFlags.Public).SelectMany(type => type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Where(field => field.IsLiteral && !field.IsInitOnly).Select(field => field.GetRawConstantValue()!.ToString()!));
        }
    }
}