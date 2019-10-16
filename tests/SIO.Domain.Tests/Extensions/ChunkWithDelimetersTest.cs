using System.Linq;
using FluentAssertions;
using SIO.Domain.Extensions;
using Xunit;

namespace SIO.Domain.Tests.Extensions
{
    public class ChunkWithDelimetersTest
    {
        private const string MOCK_TEXT = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Sit amet nisl suscipit adipiscing bibendum est ultricies. Eget nulla facilisi etiam dignissim diam quis enim lobortis scelerisque. Senectus et netus et malesuada. Elit duis tristique sollicitudin nibh sit amet commodo. Orci ac auctor augue mauris augue neque. Ullamcorper dignissim cras tincidunt lobortis feugiat vivamus at augue. Quisque id diam vel quam elementum pulvinar etiam. Condimentum id venenatis a condimentum vitae sapien pellentesque habitant. Mi eget mauris pharetra et ultrices neque ornare. Turpis egestas sed tempus urna et pharetra pharetra massa. Sit amet massa vitae tortor condimentum.";

        [Fact]
        public void When_Delimeter_Not_Found_On_First_Chunk_A_Chunk_Of_Max_Characters_Is_Taken()
        {
            var chunks = MOCK_TEXT.ChunkWithDelimeters(120, '.');
            chunks.First().Should().NotEndWith(".");
            chunks.First().Should().HaveLength(120);
        }

        [Fact]
        public void When_Delimeter_Found_On_First_Chunk_A_Chunk_With_Delmieter_Is_Taken()
        {
            var chunks = MOCK_TEXT.ChunkWithDelimeters(123, '.');
            chunks.First().Should().EndWith(".");
        }

        [Fact]
        public void When_Multiple_Delimeters_Found_On_First_Chunk_A_Chunk_With_The_Last_Delmieter_Is_Taken()
        {
            var chunks = MOCK_TEXT.ChunkWithDelimeters(126, '.', ',');
            chunks.First().Should().EndWith(".");
        }

        [Fact]
        public void When_Chunked_No_Text_Except_White_Space_Is_Removed()
        {
            var chunks = MOCK_TEXT.ChunkWithDelimeters(121, '.');

            // Replace whitespace as it could be removed from either side when chunking
            var expectedLength = MOCK_TEXT.Replace(" ", "").Length;
            var chunkedString = string.Join("", chunks).Replace(" ", "");

            chunkedString.Should().HaveLength(expectedLength);
        }
    }
}
