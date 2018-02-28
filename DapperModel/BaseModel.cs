using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace DapperModel
{
    /// <summary>
    /// 参数解析
    /// </summary>
    public class ParametHelper<T>
    {
        public static FieldDictionary GetFieldDictionary(Expression<Func<T, object>> expression)
        {
            if (expression == null) return null;
            var result = new FieldDictionary();
            if (expression.Body is MemberExpression member)
            {
                result.Name = member.Member.Name;
                if (member.Expression is ParameterExpression parameter)
                {
                    result.Parent = parameter.Name;
                }
                else goto ERROR;
            }
            else if (expression.Body is UnaryExpression unary)
            {
                if (unary.Operand is MemberExpression unaryMember)
                {
                    result.Name = unaryMember.Member.Name;
                    if (unaryMember.Expression is ParameterExpression parameter)
                    {
                        result.Parent = parameter.Name;
                    }
                    else goto ERROR;
                }
                else goto ERROR;
            }

            return result;
            ERROR: throw new Exception($"没涉及过的表达式({nameof(expression)})类型: ({GetExpressionType(expression.Body)})");
        }

        /// <summary>
        /// 获取表达的类型描述
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static string GetExpressionType(Expression expression)
        {
            switch (expression)
            {
                case BinaryExpression _:
                    return "BinaryExpression";
                case BlockExpression _:
                    return "BlockExpression";
                case ConditionalExpression _:
                    return "ConditionalExpression";
                case ConstantExpression _:
                    return "ConstantExpression";
                case DebugInfoExpression _:
                    return "DebugInfoExpression";
                case DefaultExpression _:
                    return "DefaultExpression";
                case DynamicExpression _:
                    return "DynamicExpression";
                case GotoExpression _:
                    return "GotoExpression";
                case IndexExpression _:
                    return "IndexExpression";
                case InvocationExpression _:
                    return "InvocationExpression";
                case LabelExpression _:
                    return "LabelExpression";
                case LambdaExpression _:
                    return "LambdaExpression";
                case ListInitExpression _:
                    return "ListInitExpression";
                case LoopExpression _:
                    return "LoopExpression";
                case MemberExpression _:
                    return "MemberExpression";
                case MemberInitExpression _:
                    return "MemberInitExpression";
                case MethodCallExpression _:
                    return "MethodCallExpression";
                case NewArrayExpression _:
                    return "NewArrayExpression";
                case NewExpression _:
                    return "NewExpression";
                case ParameterExpression _:
                    return "ParameterExpression";
                case RuntimeVariablesExpression _:
                    return "RuntimeVariablesExpression";
                case SwitchExpression _:
                    return "GetExpressionType";
                case TryExpression _:
                    return "TryExpression";
                case TypeBinaryExpression _:
                    return "TypeBinaryExpression";
                case UnaryExpression _:
                    return "UnaryExpression";
                default:
                    throw new Exception(nameof(expression));
            }
        }
    }

    public class Where<T> where T : class
    {
        public readonly IList<WhereDictionary> Wheres = new List<WhereDictionary>();

        public Where<T> Or(Expression<Func<T, object>> expression, RelationEnum relation, object value)
        {
            if (Wheres.Count <= 0)
                throw new Exception("首个条件不能为OR关系");

            Wheres.Add(new WhereDictionary
            {
                FieldDictionary = ParametHelper<T>.GetFieldDictionary(expression),
                Coexist = CoexistEnum.Or,
                Relation = relation,
                Value = value
            });
            return this;
        }

        public Where<T> And(Expression<Func<T, object>> expression, RelationEnum relation, object value)
        {
            Wheres.Add(new WhereDictionary
            {
                FieldDictionary = ParametHelper<T>.GetFieldDictionary(expression),
                Coexist = CoexistEnum.And,
                Relation = relation,
                Value = value
            });
            return this;
        }
    }

    public class Sort<T> where T : class
    {
        public IList<OrderDictionary> Sorts { get; } = new List<OrderDictionary>();

        public Sort<T> Asc(Expression<Func<T, object>> expression)
        {
            Sorts.Add(new OrderDictionary
            {
                FieldDictionary = ParametHelper<T>.GetFieldDictionary(expression),
                Sort = SortEnum.Asc
            });
            return this;
        }

        public Sort<T> Desc(Expression<Func<T, object>> expression)
        {
            Sorts.Add(new OrderDictionary
            {
                FieldDictionary = ParametHelper<T>.GetFieldDictionary(expression),
                Sort = SortEnum.Desc
            });
            return this;
        }
    }

    public class Show<T> where T : class
    {
        public readonly IList<FieldDictionary> Shows = new List<FieldDictionary>();

        public Show<T> Add(Expression<Func<T, object>> expression)
        {
            Shows.Add(ParametHelper<T>.GetFieldDictionary(expression));
            return this;
        }
    }

    public class FieldDictionary
    {
        public string Parent { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class WhereDictionary
    {
        public FieldDictionary FieldDictionary { get; set; }

        public RelationEnum Relation { get; set; }

        public CoexistEnum Coexist { get; set; }

        public object Value { get; set; }
    }

    public class OrderDictionary
    {
        public FieldDictionary FieldDictionary { get; set; }

        // ReSharper disable once MemberHidesStaticFromOuterClass
        public SortEnum Sort { get; set; }
    }

    /// <summary>
    /// 排序关系
    /// </summary>
    public enum SortEnum
    {
        /// <summary>
        /// 正序
        /// </summary>
        [Description("ASC")]
        Asc,
        /// <summary>
        /// 倒序
        /// </summary>
        [Description("DESC")]
        Desc,
    }

    /// <summary>
    /// 键值关系
    /// </summary>
    public enum RelationEnum
    {
        /// <summary>
        /// 等于
        /// </summary>
        [Description("= {0}")]
        Equal,
        /// <summary>
        /// 不等于
        /// </summary>
        [Description("<> {0}")]
        NotEqual,
        /// <summary>
        /// in
        /// </summary>
        [Description("IN({0})")]
        In,
        /// <summary>
        /// NotIn
        /// </summary>
        [Description("NOT IN {0}")]
        NotIn,
        /// <summary>
        /// 大于
        /// </summary>
        [Description("> {0}")]
        Greater,
        /// <summary>
        /// 大于等于
        /// </summary>
        [Description(">= {0}")]
        GreaterEqual,
        /// <summary>
        /// 小于
        /// </summary>
        [Description("< {0}")]
        Less,
        /// <summary>
        /// 小于等于
        /// </summary>
        [Description("<= {0}")]
        LessEqual,
        /// <summary>
        /// 匹配
        /// </summary>
        [Description("LIKE %{0}%")]
        Like,
        /// <summary>
        /// 右匹配
        /// </summary>
        [Description("LIKE {0}%")]
        RightLike,
        /// <summary>
        /// 左匹配
        /// </summary>
        [Description("LIKE %{0}")]
        LeftLike,
        /// <summary>
        /// 是
        /// </summary>
        [Description("IS NULL")]
        IsNull,
        /// <summary>
        /// 不是
        /// </summary>
        [Description("IS NOT NULL")]
        IsNotNull
    }

    public enum CoexistEnum
    {
        /// <summary>
        /// AND 关系
        /// </summary>
        [Description("AND")]
        And,
        /// <summary>
        /// OR 关系
        /// </summary>
        [Description("OR")]
        Or
    }
}
