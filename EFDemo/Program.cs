using System;
using Microsoft.EntityFrameworkCore;

namespace EFDemo
{
  /// <summary>
  /// 文档
  /// https://docs.microsoft.com/zh-cn/ef/core/
  /// </summary>
  class Program
  {
    static void Main(string[] args)
    {



      Console.WriteLine("Hello World!");
    }
  }

  class BloggingContext : DbContext
  {
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseMySql(@"User ID=root;Password=sun940622;DataBase=Demo;Server=localhost;Port=3306;");
    }
  }
}
