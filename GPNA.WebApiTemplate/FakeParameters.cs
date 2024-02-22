using Bogus;
using GPNA.Converters.TagValues;
using System;
using System.Collections.Generic;

namespace GPNA.gRPCServer
{
    public class FakeParameters
    {
        //private static Queue<TagValueDouble> Storage = new();
        public static List<TagValueDouble> StorageListDouble = new();
        public static List<TagValueBool> StorageListBool = new();

        public static void GenerateTagValueDouble(int count)
        {
            StorageListDouble = new List<TagValueDouble>(new Faker<TagValueDouble>()
                .RuleFor(x => x.TagId, f => f.Random.Int(1, 40))
                .RuleFor(x => x.OpcQuality, f => 192)
                .RuleFor(x => x.DateTime, DateTime.Now)
                .RuleFor(x => x.DateTimeUtc, DateTime.UtcNow)
                .RuleFor(x => x.TimeStampUtc, DateTime.UtcNow)
                .RuleFor(x => x.Value, f => f.Random.Double(0, 100))
                .RuleFor(x => x.Tagname, f => f.Random.String2(1, 30)).Generate(count));
            Queue<TagValueDouble> _storage = new Queue<TagValueDouble>(StorageListDouble);
        }

        public static void GenerateTagValueBool(int count)
        {
            StorageListBool = new List<TagValueBool>(new Faker<TagValueBool>()
                .RuleFor(x => x.TagId, f => f.Random.Int(1, 40))
                .RuleFor(x => x.OpcQuality, f => 192)
                .RuleFor(x => x.DateTime, DateTime.Now)
                .RuleFor(x => x.DateTimeUtc, DateTime.UtcNow)
                .RuleFor(x => x.TimeStampUtc, DateTime.UtcNow)
                .RuleFor(x => x.Value, f => f.Random.Bool())
                .RuleFor(x => x.Tagname, f => f.Random.String2(1, 30)).Generate(count));
            Queue<TagValueBool> _storage = new Queue<TagValueBool>(StorageListBool);
        }
    }

}