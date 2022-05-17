using PlatformService.Models;

namespace PlatformService.Data
{
    public static  class PrepDb
    {
        public static  void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetRequiredService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data");
                context.Platforms.AddRange(new Platform() { Name = "DotNet", Publisher = "Microsoft", Cost = "free" },
                                            new Platform() { Name = "Sql Server Express", Publisher = "Microsoft", Cost = "free" },
                                            new Platform() { Name = "Cloud Native Computing Foundation", Publisher = "Microsoft", Cost = "free" }
                    );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> Already have data");
            }
        }
    }
}
