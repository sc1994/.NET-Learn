using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace DapperModel
{
    public abstract class BaseModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public abstract string PrimaryKey { get; }

        /// <summary>
        /// 自增键
        /// </summary>
        public abstract string IdentityKey { get; }

        /// <summary>
        /// 库名
        /// </summary>
        public abstract string DbName { get; }

        /// <summary>
        /// 表名
        /// </summary>
        public abstract string TableName { get; }

        /// <summary>
        /// 链接字符串
        /// </summary>
        public abstract string ConnectionString { get; }
    }

    public class Where<T> where T : BaseModel
    {
        internal Where() { }

        internal readonly IList<WhereDictionary> Wheres = new List<WhereDictionary>();

        public static Where<T> Init() => new Where<T>();

        public Where<T> Or<TField>(Expression<Func<T, TField>> expression, RelationEnum relation, TField value)
        {
            if (Wheres.Count <= 0)
                throw new Exception("首个条件不能为OR关系");

            Wheres.Add(new WhereDictionary
            {
                FieldDictionary = ParseExpression<T>.GetFieldDictionary(expression),
                Coexist = CoexistEnum.Or,
                Relation = relation,
                Value = value
            });
            return this;
        }

        public Where<T> Or<TField>(Expression<Func<T, TField>> expression, RelationEnum relation, IEnumerable<TField> value)
        {
            if (Wheres.Count <= 0)
                throw new Exception("首个条件不能为OR关系");

            if (relation != RelationEnum.In)
                throw new Exception($"{typeof(T)}和 array 之间不存在对等关系");

            Wheres.Add(new WhereDictionary
            {
                FieldDictionary = ParseExpression<T>.GetFieldDictionary(expression),
                Coexist = CoexistEnum.Or,
                Relation = relation,
                Value = value
            });
            return this;
        }

        public Where<T> And<TField>(Expression<Func<T, TField>> expression, RelationEnum relation, TField value)
        {
            Wheres.Add(new WhereDictionary
            {
                FieldDictionary = ParseExpression<T>.GetFieldDictionary(expression),
                Coexist = CoexistEnum.And,
                Relation = relation,
                Value = value
            });
            return this;
        }

        public Where<T> And<TField>(Expression<Func<T, TField>> expression, RelationEnum relation, IEnumerable<TField> value)
        {
            if (relation != RelationEnum.In)
                throw new Exception($"{typeof(T)}和 array 之间不存在对等关系");

            Wheres.Add(new WhereDictionary
            {
                FieldDictionary = ParseExpression<T>.GetFieldDictionary(expression),
                Coexist = CoexistEnum.Or,
                Relation = relation,
                Value = value
            });
            return this;
        }
    }

    public class InitWhere<T> : Where<T> where T : BaseModel
    {
        public static IList<WhereDictionary> GetWhere(Where<T> where)
        {
            return where.Wheres;
        }
    }

    public class Update<T> where T : BaseModel
    {
        internal Update() { }

        internal readonly IList<UpdateDictionary> Updates = new List<UpdateDictionary>();

        public Update<T> Add<TField>(Expression<Func<T, TField>> expression, TField value)
        {
            Updates.Add(new UpdateDictionary
            {
                FieldDictionary = ParseExpression<T>.GetFieldDictionary(expression),
                Value = value
            });
            return this;
        }
    }

    public class InitUpdate<T> : Update<T> where T : BaseModel
    {
        public static Update<T> Init() => new InitUpdate<T>();

        public static IList<UpdateDictionary> GetUpdate(Update<T> update)
        {
            return update.Updates;
        }
    }

    public class Order<T> where T : BaseModel
    {
        internal Order() { }

        internal readonly IList<OrderDictionary> Orders = new List<OrderDictionary>();

        public Order<T> Add(Expression<Func<T, object>> expression, SortEnum sort)
        {
            Orders.Add(new OrderDictionary
            {
                FieldDictionary = ParseExpression<T>.GetFieldDictionary(expression),
                Sort = sort
            });
            return this;
        }
    }

    public class InitOrder<T> : Order<T> where T : BaseModel
    {
        public static Order<T> Init() => new InitOrder<T>();

        public static IList<OrderDictionary> GetOrder(Order<T> order)
        {
            return order.Orders;
        }
    }

    public class Show<T> where T : BaseModel
    {
        internal Show() { }

        internal readonly IList<FieldDictionary> Shows = new List<FieldDictionary>();

        public Show<T> Add(Expression<Func<T, object>> expression)
        {
            Shows.Add(ParseExpression<T>.GetFieldDictionary(expression));
            return this;
        }
    }

    public class InitShow<T> : Show<T> where T : BaseModel
    {
        public static Show<T> Init() => new InitShow<T>();

        public static IList<FieldDictionary> GetShow(Show<T> show)
        {
            return show.Shows;
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

    public class UpdateDictionary
    {
        public FieldDictionary FieldDictionary { get; set; }

        public object Value { get; set; }
    }

    public class OrderDictionary
    {
        public FieldDictionary FieldDictionary { get; set; }

        // ReSharper disable once MemberHidesStaticFromOuterClass
        public SortEnum Sort { get; set; }
    }



    /// <summary>
    /// 参数解析
    /// </summary>
    public class ParseExpression<T>
    {
        public static FieldDictionary GetFieldDictionary<TField>(Expression<Func<T, TField>> expression)
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
}
