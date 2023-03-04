using Flora.Application.Common.Interfaces;
using Flora.Domain.Entities;
using Flora.Infrastructure.Persistence;
using Flora.Infrastructure.Persistence.Interceptors;
using Flora.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Flora.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(opts =>
        {
            opts.UseSqlServer(configuration["DbConnection"],
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            opts.EnableSensitiveDataLogging();
        });

        services.AddSingleton(new MongoClient(new MongoUrl(configuration["MongoDb:ConnectionString"])));
        // BsonSerializer.RegisterSerializer(new CategoryBsonSerializer());
        services.AddScoped<IMongoDbContext, MongoDbContext>();

        services.AddScoped<ApplicationDbContextInitializer>();
        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}

// public class CategoryBsonSerializer : IBsonSerializer<Category>
// {
//     object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
//     {
//         return Deserialize(context, args);
//     }
//
//     public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Category value)
//     {
//         var bsonWriter = context.Writer;
//         bsonWriter.WriteStartDocument();
//         bsonWriter.WriteName("id");
//         bsonWriter.WriteString(value.Id.ToString());
//
//         bsonWriter.WriteName("name");
//         bsonWriter.WriteString(value.Name);
//
//         bsonWriter.WriteName("parentId");
//         bsonWriter.WriteString(value.ParentId.ToString());
//
//         if (value.Parent is not null)
//         {
//             bsonWriter.WriteName("parent");
//             BsonSerializer.Serialize(bsonWriter, value.Parent);
//         }
//
//         if (value.Children.Any())
//         {
//             bsonWriter.WriteName("children");
//             BsonSerializer.Serialize(bsonWriter, value.Children);
//         }
//
//         if (value.Plants.Any())
//         {
//             bsonWriter.WriteName("plants");
//             BsonSerializer.Serialize(bsonWriter, value.Plants);
//         }
//
//         if (value.Characteristics.Any())
//         {
//             bsonWriter.WriteName("characteristics");
//             BsonSerializer.Serialize(bsonWriter, value.Characteristics);
//         }
//
//         bsonWriter.WriteEndDocument();
//     }
//
//     public Category Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
//     {
//         var bsonReader = context.Reader;
//         bsonReader.ReadStartDocument();
//
//         var category = new Category();
//
//         while (bsonReader.ReadBsonType() != BsonType.EndOfDocument)
//         {
//             var fieldName = bsonReader.ReadName(new Utf8NameDecoder());
//
//             switch (fieldName)
//             {
//                 case "id":
//                     category.Id = Guid.Parse(bsonReader.ReadString());
//                     break;
//                 case "name":
//                     category.Name = bsonReader.ReadString();
//                     break;
//                 case "parentId":
//                     category.ParentId = Guid.Parse(bsonReader.ReadString());
//                     break;
//                 case "parent":
//                     category.Parent = BsonSerializer.Deserialize<Category>(bsonReader);
//                     break;
//                 case "children":
//                     category.Children = BsonSerializer.Deserialize<List<Category>>(bsonReader);
//                     break;
//                 case "plants":
//                     category.Plants = BsonSerializer.Deserialize<List<Plant>>(bsonReader);
//                     break;
//                 case "characteristics":
//                     category.Characteristics = BsonSerializer.Deserialize<List<Characteristic>>(bsonReader);
//                     break;
//                 default:
//                     bsonReader.SkipValue();
//                     break;
//             }
//         }
//
//         bsonReader.ReadEndDocument();
//         return category;
//     }
//
//     public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
//     {
//         if(value is Category category) Serialize(context, args, category);
//     }
//
//     public Type ValueType => typeof(Category);
// }