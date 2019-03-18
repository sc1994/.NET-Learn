using EFDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
            using (var db = new DemoContext())
            {
                db.TouTiaoInformation.Add(new TouTiaoInformation
                {
                    TiappCode = "1"
                });
                db.SaveChanges();

                var list = db.TouTiaoInformation
                             .Where(x => x.Tiid > 0)
                             .Select(x => new { x.Tiid, x.TiappCode });
                foreach (var item in list)
                {
                    Console.WriteLine($"{item.Tiid}-->{item.TiappCode}");
                }
            }

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
