using Microsoft.Xrm.Sdk.Query;

namespace Xrm
{
    public static partial class QueryExpressionExtensions
    {
        /// <summary>
        /// Convert FilterExpression to a FilterBuilder
        /// </summary>
        /// <param name="filter">FilterExpression</param>
        /// <returns>FilterBuilder</returns>
        public static FilterBuilder ToFilterBuilder(this FilterExpression filter)
        {
            var builder = new FilterBuilder(filter.FilterOperator.ToString().ToLower());

            foreach (var condition in filter.Conditions)
            {
                if (condition.Values.Count > 1)
                {
                    builder.WithCondition(condition.AttributeName, condition.Operator.ToFetchXmlOperator(), condition.Values);
                }
                else
                {
                    builder.WithCondition(condition.AttributeName, condition.Operator.ToFetchXmlOperator(), condition.Values[0]);
                }
            }

            foreach (var childFilter in filter.Filters)
            {
                var childBuilder = childFilter.ToFilterBuilder();

                builder.WithFilter(childBuilder);
            }

            return builder;
        }

        /// <summary>
        /// Convert ConditionOperator to matching FetchXML operator
        /// </summary>
        /// <param name="conditionOperator">ConditionOperator</param>
        /// <returns>FetchXML operator</returns>
        public static string ToFetchXmlOperator(this ConditionOperator conditionOperator)
        {
            switch (conditionOperator)
            {
                case ConditionOperator.Above:
                    return "above";                
                case ConditionOperator.AboveOrEqual:
                    return "eq-or-above";
                case ConditionOperator.BeginsWith:
                    return "begins-with";
                case ConditionOperator.Between:
                    return "between";
                case ConditionOperator.Contains:
                    return "like";
                case ConditionOperator.ChildOf:
                    return "child-of";
                case ConditionOperator.DoesNotBeginWith:
                    return "not-begin-with";
                case ConditionOperator.DoesNotContain:
                    return "not-like";
                case ConditionOperator.DoesNotEndWith:
                    return "not-end-width";
                case ConditionOperator.EndsWith:
                    return "ends-with";
                case ConditionOperator.NotLike:
                    return "not-like";
                case ConditionOperator.Equal:
                    return "eq";
                case ConditionOperator.EqualBusinessId:
                    return "eq-businessid";
                case ConditionOperator.EqualUserId:
                    return "eq-userid";
                case ConditionOperator.EqualUserLanguage:
                    return "eq-userlanguage";
                case ConditionOperator.EqualUserOrUserHierarchy:
                    return "eq-useroruserhierarchy";
                case ConditionOperator.EqualUserOrUserHierarchyAndTeams:
                    return "eq-useroruserhierarchyandteams";
                case ConditionOperator.EqualUserOrUserTeams:
                    return "eq-useroruserteams";
                case ConditionOperator.EqualUserTeams:
                    return "eq-userteams";
                case ConditionOperator.GreaterEqual:
                    return "ge";
                case ConditionOperator.GreaterThan:
                    return "gt";
                case ConditionOperator.In:
                    return "in";
                case ConditionOperator.InFiscalPeriod:
                    return "in-fiscal-period";
                case ConditionOperator.InFiscalPeriodAndYear:
                    return "in-fiscal-period-and-year";
                case ConditionOperator.InFiscalYear:
                    return "in-fiscal-year";
                case ConditionOperator.InOrAfterFiscalPeriodAndYear:
                    return "in-or-after-fiscal-period-and-year";
                case ConditionOperator.InOrBeforeFiscalPeriodAndYear:
                    return "in-or-before-fiscal-period-and-year";
                case ConditionOperator.Last7Days:
                    return "last-seven-days";
                case ConditionOperator.LastFiscalPeriod:
                    return "last-fiscal-period";
                case ConditionOperator.LastFiscalYear:
                    return "last-fiscal-year";
                case ConditionOperator.LastMonth:
                    return "last-month";
                case ConditionOperator.LastWeek:
                    return "last-week";
                case ConditionOperator.LastXDays:
                    return "last-x-days";
                case ConditionOperator.LastXFiscalPeriods:
                    return "last-x-fiscal-periods";
                case ConditionOperator.LastXFiscalYears:
                    return "last-x-fiscal-years";
                case ConditionOperator.LastXHours:
                    return "last-x-hours";
                case ConditionOperator.LastXMonths:
                    return "last-x-months";
                case ConditionOperator.LastXWeeks:
                    return "last-x-weeks";
                case ConditionOperator.LastXYears:
                    return "last-x-years";
                case ConditionOperator.LastYear:
                    return "last-year";
                case ConditionOperator.LessEqual:
                    return "le";
                case ConditionOperator.LessThan:
                    return "lt";
                case ConditionOperator.Like:
                    return "like";
                case ConditionOperator.Next7Days:
                    return "next-7-days";
                case ConditionOperator.NextFiscalPeriod:
                    return "next-fiscal-period";
                case ConditionOperator.NextFiscalYear:
                    return "next-fiscal-year";
                case ConditionOperator.NextMonth:
                    return "next-month";
                case ConditionOperator.NextWeek:
                    return "next-week";
                case ConditionOperator.NextXDays:
                    return "next-x-days";
                case ConditionOperator.NextXFiscalPeriods:
                    return "next-x-fiscal-periods";
                case ConditionOperator.NextXFiscalYears:
                    return "next-x-fiscal-years";
                case ConditionOperator.NextXHours:
                    return "next-x-hours";
                case ConditionOperator.NextXMonths:
                    return "next-x-months";
                case ConditionOperator.NextXWeeks:
                    return "next-x-weeks";
                case ConditionOperator.NextXYears:
                    return "next-x-years";
                case ConditionOperator.NextYear:
                    return "next-year";
                case ConditionOperator.NotBetween:
                    return "not-between";
                case ConditionOperator.NotEqual:
                    return "ne";
                case ConditionOperator.NotEqualBusinessId:
                    return "ne-businesid";
                case ConditionOperator.NotEqualUserId:
                    return "ne-userid";
                case ConditionOperator.NotIn:
                    return "not-in";
                case ConditionOperator.NotNull:
                    return "not-null";
                case ConditionOperator.NotOn:
                    return "not-on";
                case ConditionOperator.NotUnder:
                    return "not-under";
                case ConditionOperator.Null:
                    return "null";
                case ConditionOperator.OlderThanXDays:
                    return "older-than-x-days";
                case ConditionOperator.OlderThanXHours:
                    return "older-than-x-hours";
                case ConditionOperator.OlderThanXMinutes:
                    return "older-than-x-minutes";
                case ConditionOperator.OlderThanXMonths:
                    return "older-than-x-months";
                case ConditionOperator.OlderThanXWeeks:
                    return "older-than-x-weeks";
                case ConditionOperator.OlderThanXYears:
                    return "older-than-x-years";
                case ConditionOperator.On:
                    return "on";
                case ConditionOperator.OnOrAfter:
                    return "on-or-after";
                case ConditionOperator.OnOrBefore:
                    return "on-or-before";
                case ConditionOperator.ThisFiscalPeriod:
                    return "this-fiscal-period";
                case ConditionOperator.ThisFiscalYear:
                    return "this-fiscal-year";
                case ConditionOperator.ThisMonth:
                    return "this-month";
                case ConditionOperator.ThisWeek:
                    return "this-week";
                case ConditionOperator.ThisYear:
                    return "this-year";
                case ConditionOperator.Today:
                    return "today";
                case ConditionOperator.Tomorrow:
                    return "tomorrow";
                case ConditionOperator.Under:
                    return "under";
                case ConditionOperator.UnderOrEqual:
                    return "eq-or-under";
                case ConditionOperator.Yesterday:
                    return "yesterday";
                default:
                    return string.Empty;
            }
        }
    }
}