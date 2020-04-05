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
    public class FileLineReaderTests
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
        public void CountLines_UsesFileProvider_ReturnsExpectedRowCount()
        {
            //Use autoFixture to create mocks for these interfaces.
            var mockFileStreamProvider = _fixture.Freeze<Mock<IFileStreamProvider>>();
            var mockRowProcessor = _fixture.Freeze<Mock<IRowProcessor>>();

            //Use autoFixture to create the FileLineReader so it automatically gets the interfaces
            //the constructor is expecting.
            var sut = _fixture.Create<FileLineReader>();

            //Setup the mock method to return a fake stream.
            mockFileStreamProvider
                .Setup(m => m.GetReadStream(It.IsAny<string>()))
                .Returns(() => new StreamReader(FakeMemoryStream()));

            //Exercise the sut
            var result = sut.CountLines("");

            mockFileStreamProvider
                .Verify(m => m.GetReadStream(It.IsAny<string>()), Times.Exactly(1));

            mockRowProcessor
                .Verify(m => m.RowSplitter(It.IsAny<string>()), Times.Never);

            Assert.Equal(1, result);
        }

        [Fact]
        public void LineReader_CallsExpectedInterfaces()
        {
            //Use autoFixture to create mocks for these interfaces.
            var mockFileStreamProvider = _fixture.Freeze<Mock<IFileStreamProvider>>();
            var mockRowProcessor = _fixture.Freeze<Mock<IRowProcessor>>();
            var mockPercentageCounter = _fixture.Freeze<Mock<IPercentageCounter>>();

            //Use autoFixture to create the FileLineReader so it automatically gets the interfaces
            //the constructor is expecting.
            var sut = _fixture.Create<FileLineReader>();

            //Setup the mock method to return a fake stream.
            mockFileStreamProvider
                .Setup(m => m.GetReadStream(It.IsAny<string>()))
                .Returns(() => new StreamReader(FakeMemoryStream()));

            var filePath = _fixture.Create<string>();
            var stringArr = _fixture.Create<string[]>();
            var rules = _fixture.CreateMany<int[]>().ToList();

            //Exercise the sut
            var result = sut.LineReader(filePath, stringArr, rules);

            //Called twice: once for countlines, once to get stream for loop
            mockFileStreamProvider
                .Verify(m => m.GetReadStream(It.IsAny<string>()), Times.Exactly(2));

            mockRowProcessor
                .Verify(m => m.RowSplitter(It.IsAny<string>()), Times.Once);

            mockRowProcessor
                .Verify(m => m.RowMapper(It.IsAny<List<int[]>>(), It.IsAny<string[]>(), It.IsAny<string[]>()), Times.Once);

            mockPercentageCounter
                .Verify(m => m.CalcPercent(), Times.Once);

        }
    }
}
