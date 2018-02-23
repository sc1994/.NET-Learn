using System;
using DapperExtensions;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace DapperDemo
{
    interface IBaseDb<TModel, TEnum, TKey> where TModel : class, new()
    {
        bool IsExist(TKey key);

        bool IsExist(Where<TModel> wheres);

        long Count(Where<TModel> wheres);

        bool Update(TModel model, bool log = false);

        /// <summary>
        /// 依据自定义条件和自定义字段更新
        /// </summary>
        /// <param name="update"></param>
        /// <param name="wheres"></param>
        /// <param name="top">默认不限制更新条数</param>
        /// <returns></returns>
        bool Update(Dictionary<TEnum, object> update, Where<TModel> wheres, int top = 0);

        TKey Insert(TModel model);

        void InsertMany(IList<TModel> models);

        TModel Get(TKey key);

        TModel Get(Where<TModel> wheres);

        TModel Get(Show<TModel> shows, Where<TModel> wheres);

        IList<TModel> GetList(Where<TModel> wheres, Sort<TModel> orders= null);

        IList<TModel> GetList(Show<TModel> shows, Where<TModel> wheres, Sort<TModel> orders = null);

        IList<TModel> GetPage(Show<TModel> shows, Where<TModel> wheres, Sort<TModel> orders, int pageIndex, int pageSize);

        IList<TModel> GetPage();
    }

    class Where<T> where T : class, new()
    {
        private readonly IList<IFieldPredicate> _whereFields = new List<IFieldPredicate>();

        public Where<T> Add(Expression<Func<T, object>> expression, Operator op, object value, bool not = false)
        {
            _whereFields.Add(Predicates.Field(expression, op, value, not));
            return this;
        }
    }

    class Sort<T> where T : class, new()
    {
        private readonly IList<ISort> _sortFields = new List<ISort>();

        public Sort<T> Add(Expression<Func<T, object>> expression, bool ascending = true)
        {
            _sortFields.Add(Predicates.Sort(expression, ascending));
            return this;
        }
    }

    class Show<T> where T : class, new()
    {
        private readonly IList<string> _showFields = new List<string>();

        public Show<T> Add(Expression<Func<T, object>> expression)
        {
            if (expression == null) return this;
            if (expression.Body is MemberExpression member)
            {
                _showFields.Add(member.Member.Name);
            }
            else if (expression.Body is UnaryExpression unary)
            {
                if (unary.Operand is MemberExpression unaryMember)
                {
                    _showFields.Add(unaryMember.Member.Name);
                }
                else
                {
                    throw new Exception($"没涉及过的表达式({nameof(expression)})类型: ({SwitchExpression(expression.Body)})");
                }
            }
            else
            {
                throw new Exception($"没涉及过的表达式({nameof(expression)})类型: ({SwitchExpression(expression.Body)})");
            }

            return this;
        }

        /// <summary>
        /// 获取表达的类型描述
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static string SwitchExpression(Expression expression)
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
                    return "SwitchExpression";
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
