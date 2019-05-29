using System;
using Microsoft.Xrm.Sdk;

namespace Xrm
{
    public static class MoneyExtensions
    {
        /// <summary>
        /// Add Money value to existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="add">Money to add</param>
        /// <returns></returns>
        public static Money Add(this Money money, Money add)
        {
            if (add != null)
            {
                money.Value += add.Value;
            }

            return money;
        }

        /// <summary>
        /// Add double value to Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="add">Amount to add</param>
        /// <returns></returns>
        public static Money Add(this Money money, double? add)
        {
            if (add != null)
            {
                money.Value += (decimal)add.Value;
            }

            return money;
        }

        /// <summary>
        /// Add decimal value to Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="add">Amount to add</param>
        /// <returns></returns>
        public static Money Add(this Money money, decimal? add)
        {
            if (add != null)
            {
                money.Value += add.Value;
            }

            return money;
        }

        /// <summary>
        /// Add integer value to Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="add">Amount to add</param>
        /// <returns></returns>
        public static Money Add(this Money money, int? add)
        {
            if (add != null)
            {
                money.Value += add.Value;
            }

            return money;
        }

        /// <summary>
        /// Subtract Money value from existing Money
        /// /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="add">Money to subtract</param>
        /// <returns></returns>
        public static Money Subtract(this Money money, Money add)
        {            
            if (add != null)
            {
                money.Value -= add.Value;
            }

            return money;
        }

        /// <summary>
        /// Subtract double value from Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="add">Amount to subtract</param>
        /// <returns></returns>
        public static Money Subtract(this Money money, double? add)
        {
            if (add != null)
            {
                money.Value -= (decimal)add.Value;
            }

            return money;
        }

        /// <summary>
        /// Subtract decimal value from Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="add">Amount to subtract</param>
        /// <returns></returns>
        public static Money Subtract(this Money money, decimal? add)
        {
            if (add != null)
            {
                money.Value -= add.Value;
            }

            return money;
        }

        /// <summary>
        /// Subtract integer value from Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="add">Amount to subtract</param>
        /// <returns></returns>
        public static Money Subtract(this Money money, int? add)
        {
            if (add != null)
            {
                money.Value -= add.Value;
            }

            return money;
        }

        /// <summary>
        /// Multiply Money value with existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="multiply">Amount to multiply</param>
        /// <returns></returns>
        public static Money Multiply(this Money money, Money multiply)
        {
            if (multiply != null)
            {
                money.Value *= multiply.Value;
            }

            return money;
        }

        /// <summary>
        /// Multiply double value with existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="multiply">Amount to multiply</param>
        /// <returns></returns>
        public static Money Multiply(this Money money, double? multiply)
        {
            if (multiply != null)
            {
                money.Value *= (decimal)multiply.Value;
            }

            return money;
        }

        /// <summary>
        /// Multiply decimal value with existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="multiply">Amount to multiply</param>
        /// <returns></returns>
        public static Money Multiply(this Money money, decimal? multiply)
        {
            if (multiply != null)
            {
                money.Value *= multiply.Value;
            }

            return money;
        }

        /// <summary>
        /// Multiply integer value with existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="multiply">Amount to multiply</param>
        /// <returns></returns>
        public static Money Multiply(this Money money, int? multiply)
        {
            if (multiply != null)
            {
                money.Value *= multiply.Value;
            }

            return money;
        }

        /// <summary>
        /// Divide Money value with existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="multiply">Amount to divide</param>
        /// <returns></returns>
        public static Money Divide(this Money money, Money divide)
        {
            if (divide != null)
            {
                money.Value /= divide.Value;
            }

            return money;
        }

        /// <summary>
        /// Divide double value with existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="multiply">Amount to divide</param>
        /// <returns></returns>
        public static Money Divide(this Money money, double? divide)
        {
            if (divide != null)
            {
                money.Value /= (decimal)divide.Value;
            }

            return money;
        }

        /// <summary>
        /// Divide decimal value with existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="multiply">Amount to divide</param>
        /// <returns></returns>
        public static Money Divide(this Money money, decimal? divide)
        {
            if (divide != null)
            {
                money.Value /= divide.Value;
            }

            return money;
        }

        /// <summary>
        /// Divide integer value with existing Money
        /// </summary>
        /// <param name="money">Money value</param>
        /// <param name="multiply">Amount to divide</param>
        /// <returns></returns>
        public static Money Divide(this Money money, int? divide)
        {
            if (divide != null)
            {
                money.Value /= divide.Value;
            }

            return money;
        }
    }
}