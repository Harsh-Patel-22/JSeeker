using Backend.Models;
using Backend.Models.Mapping;
using Backend.Models.Users;
using Backend.Models.Users.WorkRelated;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public static class DbSeeder {
    private static readonly Faker Faker = new("en");
    
    public static async Task SeedAsync(ApplicationDbContext context) {
        if(!context.Users.Any()) {
            await context.Users.AddRangeAsync(GenericSeeder.GenerateSeedData<User>(10));
            await context.SaveChangesAsync();
        }
        if(!context.Technologies.Any()) {
            await context.Technologies.AddRangeAsync(GenericSeeder.GenerateSeedData<Technology>(10));
            await context.SaveChangesAsync();
        }
        
        if(!context.Projects.Any()) {
            await context.Projects.AddRangeAsync(GenericSeeder.GenerateSeedData<Project>(20));
            await context.SaveChangesAsync();
        }
        if(!context.ProjectTechnologies.Any()) {
            var random = new Random();
            await context.ProjectTechnologies.AddRangeAsync(GenericSeeder.GenerateJoinData<Project, Technology, ProjectTechnology>(
                await context.Projects.ToListAsync(),
                await context.Technologies.ToListAsync(), 
                (project, technology) => new ProjectTechnology {
                    ProjectId = project.Id,
                    TechnologyId = technology.Id,
                    PercentUsage = (float) (random.NextDouble() * 100),
                }));
            await context.SaveChangesAsync();
        }
        
        if(!context.Addresses.Any()) {
            await context.Addresses.AddRangeAsync(GenericSeeder.GenerateSeedData<Address>(20));
            await context.SaveChangesAsync();
        }
        
        if(!context.Jobs.Any()) {
            await context.Jobs.AddRangeAsync(GenericSeeder.GenerateSeedData<Job>(10));
            await context.SaveChangesAsync();
        }
        
        if(!context.Applications.Any()) {
            await context.Applications.AddRangeAsync(GenericSeeder.GenerateSeedData<Application>(20));
            await context.SaveChangesAsync();
        }
        
        if(!context.Educations.Any()) {
            await context.Educations.AddRangeAsync(GenericSeeder.GenerateSeedData<Education>(20));
            await context.SaveChangesAsync();
        }
        
        if(!context.UserCredentials.Any()) {
            await context.UserCredentials.AddRangeAsync(GenericSeeder.GenerateSeedData<UserCredentials>(20));
            await context.SaveChangesAsync();
        }
    }
    
    private static class GenericSeeder {
        private static readonly Faker Faker = new("en");
        
        public static List<T> GenerateSeedData<T>(int count) where T: class, new() {
            
            var faker = new Faker<T>();
            
            var properties = typeof(T).GetProperties();

            foreach (var property in properties) {
                // TODO - Continue if it is not mapped or not required. Write the code to avoid it.
                //
                // if(property)
                
                var name =  property.Name.ToLower();
                var type = property.PropertyType;
                if (type == typeof(string)) {
                    if (name.Contains("email")) faker.RuleFor(property.Name, f => f.Internet.Email());
                    else if (name.Contains("phone")) faker.RuleFor(property.Name, f => f.Phone.PhoneNumber());
                    else if(name.Contains("first")) faker.RuleFor(property.Name, f => f.Name.FirstName());
                    else if (name.Contains("last")) faker.RuleFor(property.Name, f => f.Name.LastName());
                    else if (name.Contains("about") || name.Contains("description") ||
                             name.Contains("termsandconditions"))
                        faker.RuleFor(property.Name, f => f.Lorem.Sentences(3));
                    else faker.RuleFor(property.Name, f => f.Lorem.Word());
                }
                else if (type == typeof(int)) {
                    if(!name.Contains("id")) faker.RuleFor(property.Name, f => f.Random.Int());
                }
                else if (type == typeof(bool)) {
                    faker.RuleFor(property.Name, f => f.Random.Bool());
                }
                else if (type == typeof(DateOnly)) {
                    faker.RuleFor(property.Name, f => DateOnly.FromDateTime(f.Date.Past()));
                }
                else if (type == typeof(TimeOnly)) {
                    faker.RuleFor(property.Name, f => TimeOnly.FromDateTime(f.Date.Past()));
                }
                else if(type == typeof(Guid)) {
                    if(!name.Equals("id")) faker.RuleFor(property.Name, _ => Guid.NewGuid());
                }
            }
            return faker.Generate(count);
        }
        
        public static List<TJoin> GenerateJoinData<TLeft, TRight, TJoin>(
            List<TLeft> leftEntities,
            List<TRight> rightEntities,
            Func<TLeft, TRight, TJoin> joinFactory, 
            int minLeftLimit = 1,
            int maxLeftLimit = 5) {
            
            List<TJoin> returnList = new List<TJoin>();
            var random = new Random();

            foreach (var left in leftEntities) {

                var count = random.Next(minLeftLimit, maxLeftLimit + 1);
                var rights = rightEntities.OrderBy(_ => random.Next()).Take(count);
                
                foreach (var right in rights) {
                    returnList.Add(joinFactory(left, right));
                }
            }
            return returnList;
        }
    }
}