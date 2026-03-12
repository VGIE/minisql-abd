using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using DbManager;

namespace DbManager
{
     public class Condition
    {
        public const string LessThan = "<";
        public const string GreaterThan = ">";
        public const string Equal = "=";
        public const string LessOrEqual = "<=";
        public const string GreaterOrEqual = ">=";
        public const string NotEqual = "!=";
        public string ColumnName { get; private set; }
        public string Operator { get; private set; }
        public string LiteralValue { get; private set; }

        public Condition(string column, string op, string literalValue)
        {
            //TODO DEADLINE 1A: Initialize member variables
            ColumnName = column;
            Operator = op;
            LiteralValue = literalValue;
            
        }


        public bool IsTrue(string value, ColumnDefinition.DataType type)
        {
            //TODO DEADLINE 1A: return true if the condition is true for this value
            //Depending on the type of the column, the comparison should be different:
            //"ab" < "cd
            //"9" > "10"
            //9 < 10
            //Convert first the strings to the appropriate type and then compare (depending on the operator of the condition)

            if(type == ColumnDefinition.DataType.Int)
            {
                int e1 = int.Parse(value);
                int e2 = int.Parse(LiteralValue);
                if (Operator == LessThan)
                {
                    return e1 < e2;
                }
                  if (Operator == GreaterThan)
                {
                    return e1 > e2;
                }
                  if (Operator == Equal)
                {
                    return e1 == e2;
                }
                
                  if (Operator == LessOrEqual)
                {
                    return e1 <= e2;
                }
                  if (Operator == GreaterOrEqual)
                {
                    return e1 >= e2;
                }
                  if (Operator == NotEqual)
                {
                    return e1 != e2;
                }
                return false;
            }

            if (type == ColumnDefinition.DataType.String)
            {
                int aux = string.Compare(value, LiteralValue);
                if (Operator == LessThan)
                {
                    return aux <0;
                }
                if (Operator == GreaterThan)
                {
                    return aux > 0;
                }
                if (Operator == Equal)
                {
                    return aux ==0;
                }
                if (Operator == NotEqual)
                {
                    return aux !=0;
                }
                return false;
            }
            return false;
        }
    }
}