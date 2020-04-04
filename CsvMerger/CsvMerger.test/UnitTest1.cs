using AutoFixture;
using AutoFixture.AutoMoq;
using CsvMerger.Data;
using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using CsvMerger.Services.ServiceLayers;
using Moq;
using CsvMerger.Services.Interfaces;
//using CsvMerger.Data;

namespace CsvMerger.test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //var x =
            
            Assert.False(false);
        }

        [Fact]
        public void DoesRuleExist_WhenExists_ReturnTrue()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var rules = new List<int[]>();
            int[] arr = {0,1};
            rules.Add(arr);

            var csvSetTotest = fixture.Build<CsvSet>()
                .With(p => p.MapRules, rules)
                .Create();

            var sut = fixture.Create<Helper>();
            var result = sut.DoesRuleExist(csvSetTotest, 1);

            Assert.True(result);

            //var dat = new CsvSet();
        }

        [Fact]
        public void ValidateRowSplitter_ReturnsTrue()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            string testString = "Hello,World";

            var sut = fixture.Create<MapSetService>();

            var stringArray = sut.RowSplitter(testString);

            //foreach(var s in stringArray)
            //{
            //    Console.WriteLine(s);
            //}

            bool result = false;

            if (stringArray.Length > 0)
            {
                result = true;
            }

            Assert.True(result);
        }

        [Fact]
        public void ValidatesSets_ReturnsTrue()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var moqHelper = fixture.Freeze<Mock<IHelper>>();

            moqHelper.Setup(m => m.ValidateSets(It.IsAny<string>(), It.IsAny<string[]>())).Returns(true);
            var result = moqHelper.Object.ValidateSets("stuff", new string[] { "stuff" });

            Assert.True(result);
            //var sut = fixture.Create<App>();

        }
    }
}
