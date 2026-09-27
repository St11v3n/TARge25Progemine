
using Microsoft.EntityFrameworkCore;                
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;


namespace TARge25Shop 
{
    public class Program 
    {
        public static void Main(string[] args) 
        {
            var builder = WebApplication.CreateBuilder(args); 

           
            builder.Services.AddControllersWithViews();  

            builder.Services.AddScoped<IKindergartenServices, KindergartenServices>(); 
                                                                                        
            
            builder.Services.AddDbContext<KindergartenContext>(options =>                                  
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));     
                                                                                                           
            var app = builder.Build();  

           
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
  
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}