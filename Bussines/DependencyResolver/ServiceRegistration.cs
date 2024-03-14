using AutoMapper;
using Bussines.Abstract;
using Bussines.AutoMapper;
using Bussines.Concrete;
using Core.Helper.EmailHelper;
using Core.Helper.EmailHelper.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.Concrete.SQLserver;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Bussines.DependencyResolver
{
    public static class ServiceRegistration
    {
        public static void Run (this IServiceCollection services)
        {

            services.AddScoped<AppDbContext>();
           services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<ICategoryDAL, EFCategoryDAL>();
            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<IEmailHelper, EmailHelper>();
            services.AddScoped<ISizeDAL, EFSizeDAL>();
            services.AddScoped<ISizeService, SizeManager>();
            services.AddScoped<IProductDAL, EFProductDAL>();
            services.AddScoped<IProductServices, ProductManager>();
            services.AddScoped<IShippingMethodDAL, EFShippingMethodDAL>();
            services.AddScoped<IShippingMethodsServices, ShippingMethodManager>();
            services.AddScoped<IPaymentMethodDAL, EFPaymentMethodDAL>();
            services.AddScoped<IPaymentMethodServices, PaymentMethodManager>();
            services.AddScoped<IPictureDAL, EFPictureDAL>();
            services.AddScoped<IPictureServices, PictureManager>();
            

            //services.Configure<IdentityOptions>(opts =>
            //{
            //    opts.SignIn.RequireConfirmedEmail = true;
            //});
        
            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.SignIn.RequireConfirmedEmail = false;
                
            //    // Default Password settings.
            //    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";
            //    options.Password.RequireDigit = false;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequiredLength = 3;
            //    options.Password.RequiredUniqueChars = 1;
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                
            //});
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            
        
        }

    }
}
