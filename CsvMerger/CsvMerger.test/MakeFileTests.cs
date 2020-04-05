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
using System.IO;
using System.Text;

namespace CsvMerger.test
{
    public class MakeFileTests
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());

        private MemoryStream FakeMemoryStream()
        {
            string fakeFileContents = "Hello world \n stuff";
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);

            MemoryStream fakeMemoryStream = new MemoryStream(fakeFileBytes);

            return fakeMemoryStream;
        }

        [Fact]
        public void MakeOutputFile_CallsExpectedInterfaces()
        {
            var mockPercentageCounter = _fixture.Freeze<Mock<IPercentageCounter>>();
            var mockFileStreamProvider = _fixture.Freeze<Mock<IFileStreamProvider>>();
            var mockFileLineWriter = _fixture.Freeze<Mock<IFileLineWriter>>();

            var sut = _fixture.Create<MakeFile>();

            mockFileStreamProvider
                .Setup(m => m.GetWriteStream(It.IsAny<string>()))
                .Returns(() => new StreamWriter(FakeMemoryStream()));

            var csvOutput = _fixture.Build<CsvSet>()
                .Create();

            sut.MakeOutputFile(csvOutput);

            mockPercentageCounter
                .Verify(m => m.SetTotalItems(It.IsAny<decimal>()), Times.Exactly(1));

            mockFileLineWriter
                .Verify(m => m.WriteLines(It.IsAny<string>(), csvOutput), Times.Once);

        }
    }
}
