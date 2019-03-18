using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFDemo.Models
{
    public partial class DemoContext : DbContext
    {
        public DemoContext()
        {
        }

        public DemoContext(DbContextOptions<DemoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TouTiaoInformation> TouTiaoInformation { get; set; }
        public virtual DbSet<TouTiaoInformationContent> TouTiaoInformationContent { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("User ID=root;Password=sun940622;DataBase=Demo;Server=localhost;Port=3306;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TouTiaoInformation>(entity =>
            {
                entity.HasKey(e => e.Tiid)
                    .HasName("PRIMARY");

                entity.Property(e => e.Tiid)
                    .HasColumnName("TIId")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.TiappCode)
                    .IsRequired()
                    .HasColumnName("TIAppCode")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TicatId1)
                    .IsRequired()
                    .HasColumnName("TICatId1")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TicatName1)
                    .IsRequired()
                    .HasColumnName("TICatName1")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TicatPathKey)
                    .IsRequired()
                    .HasColumnName("TICatPathKey")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TicommentCount)
                    .HasColumnName("TICommentCount")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TicoverUrl)
                    .IsRequired()
                    .HasColumnName("TICoverUrl")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TidanmakuCount)
                    .HasColumnName("TIDanmakuCount")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TidataType)
                    .IsRequired()
                    .HasColumnName("TIDataType")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Tidescription)
                    .IsRequired()
                    .HasColumnName("TIDescription")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TidislikeCount)
                    .HasColumnName("TIDislikeCount")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TidurationMin)
                    .IsRequired()
                    .HasColumnName("TIDurationMin")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TifavoriteCount)
                    .HasColumnName("TIFavoriteCount")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Tifree)
                    .HasColumnName("TIFree")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TilikeCount)
                    .HasColumnName("TILikeCount")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TimediaType)
                    .HasColumnName("TIMediaType")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TimemberOnly)
                    .IsRequired()
                    .HasColumnName("TIMemberOnly")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TinewsId)
                    .IsRequired()
                    .HasColumnName("TINewsId")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TipartList)
                    .IsRequired()
                    .HasColumnName("TIPartList")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TiposterId)
                    .IsRequired()
                    .HasColumnName("TIPosterId")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TiposterScreenName)
                    .IsRequired()
                    .HasColumnName("TIPosterScreenName")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TipublishDate)
                    .HasColumnName("TIPublishDate")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'1900-01-01 00:00:00'");

                entity.Property(e => e.TipublishDateStr)
                    .IsRequired()
                    .HasColumnName("TIPublishDateStr")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TishareCount)
                    .HasColumnName("TIShareCount")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Titags)
                    .IsRequired()
                    .HasColumnName("TITags")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Tiurl)
                    .IsRequired()
                    .HasColumnName("TIUrl")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TiviewCount)
                    .HasColumnName("TIViewCount")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<TouTiaoInformationContent>(entity =>
            {
                entity.HasKey(e => e.Ticid)
                    .HasName("PRIMARY");

                entity.Property(e => e.Ticid)
                    .HasColumnName("TICId")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Ticcontent)
                    .IsRequired()
                    .HasColumnName("TICContent")
                    .HasColumnType("varchar(4095)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Tichtml)
                    .IsRequired()
                    .HasColumnName("TICHtml")
                    .HasColumnType("varchar(4095)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TicimageUrls)
                    .IsRequired()
                    .HasColumnName("TICImageUrls")
                    .HasColumnType("varchar(1023)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Tictitle)
                    .IsRequired()
                    .HasColumnName("TICTitle")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Tuctiid)
                    .HasColumnName("TUCTIId")
                    .HasColumnType("bigint(20)")
                    .HasDefaultValueSql("'0'");
            });
        }
    }
}
