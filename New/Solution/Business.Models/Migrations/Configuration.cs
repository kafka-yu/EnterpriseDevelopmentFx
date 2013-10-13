using NkjSoft.Framework.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;

namespace NkjSoft.DAL.Business.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<EFDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(EFDbContext context)
        {
#if DEBUG

            //setup the default data here.
            if (context.Set<Article>().Count() < 10)
            {
                var defType = new Guid("1c774bae-1b94-4608-92c8-499d7ffaeac3");
                var articleType = context.Set<ArticleType>().FirstOrDefault(p => p.Id == defType);
                if (articleType == null)
                {
                    articleType = new ArticleType()
                        {
                            Name = "培训机构",
                            Id = defType,
                        };

                    context.Set<ArticleType>()
                    .Add(articleType);
                }

                var defCategoryId = new Guid("1c774bae-1b94-4608-92c8-499d7ffaeac3");

                var cateogry = context.Set<ArticleCategory>()
                    .FirstOrDefault(p => p.Id == defCategoryId);

                if (cateogry == null)
                {
                    cateogry = new ArticleCategory()
                    {
                        Type = articleType,
                        Id = defCategoryId,
                        Name = "资讯",
                    };

                    context.Set<ArticleCategory>()
                        .Add(cateogry);
                }


                context.Set<Article>()
                .Add(new Article()
                {
                    Title = "测试",
                    Content = "测试" + DateTime.Now.ToShortTimeString(),
                    ArticleType = articleType,
                    Category = cateogry,
                });
#else 
                
#endif
            }
        }
    }
}
