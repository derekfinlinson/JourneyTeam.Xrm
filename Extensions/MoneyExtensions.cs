using System;
using Microsoft.Xrm.Sdk;

namespace Xrm
{
    public static partial class MoneyExtensions
    {
        /// <summary>
        /// Add Money value to existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="add">Money to add</param>
        /// <returns></returns>
        public static Money Add(this Money money, Money add)
        {
            var result = new Money(money?.Value ?? 0);

            if (add != null)
            {
                result.Value += add.Value;
            }

            return result;
        }

        /// <summary>
        /// Add double value to Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="add">Amount to add</param>
        /// <returns></returns>
        public static Money Add(this Money money, double? add)
        {
            var result = new Money(money?.Value ?? 0);

            if (add != null)
            {
                result.Value += (decimal)add;
            }

            return result;
        }

        /// <summary>
        /// Add decimal value to Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="add">Amount to add</param>
        /// <returns></returns>
        public static Money Add(this Money money, decimal? add)
        {
            var result = new Money(money?.Value ?? 0);

            if (add != null)
            {
                result.Value += add.Value;
            }

            return result;
        }

        /// <summary>
        /// Add integer value to Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="add">Amount to add</param>
        /// <returns></returns>
        public static Money Add(this Money money, int? add)
        {
            var result = new Money(money?.Value ?? 0);

            if (add != null)
            {
                result.Value += add.Value;
            }

            return result;
        }

        /// <summary>
        /// Subtract Money value from existing Money
        /// /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="add">Money to subtract</param>
        /// <returns></returns>
        public static Money Subtract(this Money money, Money subtract)
        {            
            var result = new Money(money?.Value ?? 0);

            if (subtract != null)
            {
                result.Value -= subtract.Value;
            }

            return result;
        }

        /// <summary>
        /// Subtract double value from Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="add">Amount to subtract</param>
        /// <returns></returns>
        public static Money Subtract(this Money money, double? subtract)
        {
            var result = new Money(money?.Value ?? 0);

            if (subtract != null)
            {
                result.Value -= (decimal)subtract;
            }

            return result;
        }

        /// <summary>
        /// Subtract decimal value from Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="add">Amount to subtract</param>
        /// <returns></returns>
        public static Money Subtract(this Money money, decimal? subtract)
        {
            var result = new Money(money?.Value ?? 0);

            if (subtract != null)
            {
                result.Value -= subtract.Value;
            }

            return result;
        }

        /// <summary>
        /// Subtract integer value from Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="add">Amount to subtract</param>
        /// <returns></returns>
        public static Money Subtract(this Money money, int? subtract)
        {
            var result = new Money(money?.Value ?? 0);

            if (subtract != null)
            {
                result.Value -= subtract.Value;
            }

            return result;
        }

        /// <summary>
        /// Multiply Money value with existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="multiply">Amount to multiply</param>
        /// <returns></returns>
        public static Money Multiply(this Money money, Money multiply)
        {
            var result = new Money(money?.Value ?? 0);

            if (multiply != null)
            {
                result.Value *= multiply.Value;
            }

            return result;
        }

        /// <summary>
        /// Multiply double value with existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="multiply">Amount to multiply</param>
        /// <returns></returns>
        public static Money Multiply(this Money money, double? multiply)
        {
            var result = new Money(money?.Value ?? 0);

            if (multiply != null)
            {
                result.Value *= (decimal)multiply;
            }

            return result;
        }

        /// <summary>
        /// Multiply decimal value with existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="multiply">Amount to multiply</param>
        /// <returns></returns>
        public static Money Multiply(this Money money, decimal? multiply)
        {
            var result = new Money(money?.Value ?? 0);

            if (multiply != null)
            {
                result.Value *= multiply.Value;
            }

            return result;
        }

        /// <summary>
        /// Multiply integer value with existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="multiply">Amount to multiply</param>
        /// <returns></returns>
        public static Money Multiply(this Money money, int? multiply)
        {
            var result = new Money(money?.Value ?? 0);

            if (multiply != null)
            {
                result.Value *= multiply.Value;
            }

            return result;
        }

        /// <summary>
        /// Divide Money value with existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="multiply">Amount to divide</param>
        /// <returns></returns>
        public static Money Divide(this Money money, Money divide)
        {
            var result = new Money(money?.Value ?? 0);

            if (divide != null)
            {
                result.Value /= divide.Value;
            }

            return result;
        }

        /// <summary>
        /// Divide double value with existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="multiply">Amount to divide</param>
        /// <returns></returns>
        public static Money Divide(this Money money, double? divide)
        {
            var result = new Money(money?.Value ?? 0);

            if (divide != null)
            {
                result.Value /= (decimal)divide;
            }

            return result;
        }

        /// <summary>
        /// Divide decimal value with existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="multiply">Amount to divide</param>
        /// <returns></returns>
        public static Money Divide(this Money money, decimal? divide)
        {
            var result = new Money(money?.Value ?? 0);

            if (divide != null)
            {
                result.Value /= divide.Value;
            }

            return result;
        }

        /// <summary>
        /// Divide integer value with existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="multiply">Amount to divide</param>
        /// <returns></returns>
        public static Money Divide(this Money money, int? divide)
        {
            var result = new Money(money?.Value ?? 0);

            if (divide != null)
            {
                result.Value /= divide.Value;
            }

            return result;
        }
    }
}