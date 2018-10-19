using Autofac;
using Autofac.Integration.Mvc;
using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSepeti.Admin.Class
{
    public class BootStrapper
    {
        //Boot aşamasında çalışacak

        public static void RunConfig()
        {
            BuildAutoFac();
        }

        private static void BuildAutoFac()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<NewsRepository>().As<INewsRepository>();
            builder.RegisterType<PictureRepository>().As<IPictureRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>();
            builder.RegisterType<TagRepository>().As<ITagRepository>();
            builder.RegisterType<SliderRepository>().As<ISliderRepository>();
            builder.RegisterType<CommentRepository>().As<ICommentRepository>();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}