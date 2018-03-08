using System.ComponentModel;

namespace DapperModel
{
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
        [Description("IN {0}")]
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
        [Description("LIKE '%'+{0}+'%'")]
        Like,
        /// <summary>
        /// 右匹配
        /// </summary>
        [Description("LIKE {0}+'%'")]
        RightLike,
        /// <summary>
        /// 左匹配
        /// </summary>
        [Description("LIKE '%'+{0}")]
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
