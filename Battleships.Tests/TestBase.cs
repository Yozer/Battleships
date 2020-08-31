using AutoFixture;
using AutoFixture.AutoNSubstitute;
using NUnit.Framework;

namespace Battleships.Tests
{
    public abstract class TestBase
    {
        protected IFixture Fixture { get; private set; }

        [SetUp]
        public void SetUpBase()
        {
            Fixture = new Fixture()
                .Customize(new AutoNSubstituteCustomization());
        }

        protected T Create<T>() => Fixture.Create<T>();
        protected T Freeze<T>() => Fixture.Freeze<T>();
    }
}