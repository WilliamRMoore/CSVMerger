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

            var sut = fixture.Create<ServiceoOchestrator>();
            var result = sut.DoesRuleExist(csvSetTotest, 1);

            Assert.True(result);
        }

        [Fact]
        public void ValidateRowSplitter_ReturnsTrue()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            string testString = "Hello,World";

            var sut = fixture.Create<RowProcessor>();

            var stringArray = sut.RowSplitter(testString);

            bool result = false;

            if (stringArray[0].Equals("Hello") && stringArray[1].Equals("World"))
            {
                result = true;
            }

            Assert.True(result);
        }

        [Fact]
        public void ValidateRowMapper_ReturnsTrue()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var rules = new List<int[]>();
            int[] rule = { 0, 1 };
            int[] rule2 = { 1, 0 };
            rules.Add(rule);
            rules.Add(rule2);
            string[] attributes = { "these", "are", "the", "attributes" };
            string[] resultArray = new string[2];
            bool result = false;

            var sut = fixture.Create<RowProcessor>();

            resultArray = sut.RowMapper(rules, attributes, resultArray);

            if(resultArray[0].Equals("are") && resultArray[1].Equals("these"))
            {
                result = true;
            }

            Assert.True(result);
        }

        [Fact]
        public void ValidatesSets_ReturnsTrue()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var moqHelper = fixture.Freeze<Mock<IServiceOrchestrator>>();

            moqHelper.Setup(m => m.ValidateSets(It.IsAny<string>(), It.IsAny<string[]>())).Returns(true);
            var result = moqHelper.Object.ValidateSets("stuff", new string[] { "stuff" });

            Assert.True(result);
            //var sut = fixture.Create<App>();

        }
    }
}
