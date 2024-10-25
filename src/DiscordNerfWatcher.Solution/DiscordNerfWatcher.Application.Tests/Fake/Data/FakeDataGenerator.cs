using AutoBogus;
using Bogus;

namespace DiscordNerfWatcher.Application.Tests.Fake.Data
{
    public static class FakeDataGenerator
    {

        public static Faker<FakeClassData> FakeClassData() => new AutoFaker<FakeClassData>()
            .RuleFor(d => d.FakeId, f => f.Random.Guid())
            .RuleFor(d => d.FakeBool, f => f.Random.Bool())
            .RuleFor(d => d.FakeDecimal, f => f.Random.Decimal())
            .RuleFor(d => d.FakeInteger, f => f.Random.Int())
            .RuleFor(d => d.FakeString, f => f.Random.String());

    }
}
