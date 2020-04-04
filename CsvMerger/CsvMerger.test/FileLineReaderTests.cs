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
        public void CountLines_UsesFileProvider()
        {
            var mockFileStreamProvider = _fixture.Freeze<Mock<IFileStreamProvider>>();
            var mockRowProcessor = _fixture.Freeze<Mock<IRowProcessor>>();
            var sut = _fixture.Create<FileLineReader>();

            mockFileStreamProvider
                .Setup(m => m.GetStream(It.IsAny<string>()))
                .Returns(() => new StreamReader(FakeMemoryStream()));

            var result = sut.CountLines("");

            Assert.Equal(1, result);
        }

    }
}
