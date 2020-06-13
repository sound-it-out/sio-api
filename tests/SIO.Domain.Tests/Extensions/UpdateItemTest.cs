using System.Linq;
using FluentAssertions;
using Xunit;

namespace SIO.Domain.Tests.Extensions
{
    internal class Test
    {
        public int Key { get; set; }
        public string Value { get; set; }

        public Test(int key, string value)
        {
            Key = key;
            Value = value;
        }
    }
    //public class UpdateItemTest
    //{
    //    [Fact]
    //    public void When_Updating_Items_They_Are_Changed()
    //    {
    //        var items = new Test[]
    //        {
    //            new Test(1, "1"),
    //            new Test(2, "2")
    //        };

    //        items.UpdateItem(
    //            condition: i => i.Key == 1,
    //            update: i => i.Value = "Changed"
    //        );

    //        items.First().Value.Should().Match("Changed");
    //    }

    //    [Fact]
    //    public void When_Updating_Items_Only_items_That_Match_Condition_Are_Changed()
    //    {
    //        var items = new Test[]
    //        {
    //            new Test(1, "1"),
    //            new Test(2, "2")
    //        };

    //        items.UpdateItem(
    //            condition: i => i.Key == 1,
    //            update: i => i.Value = "Changed"
    //        );

    //        items.Should().NotHaveSameCount(items.Where(i => i.Value == "Changed"));
    //    }
    //}
}
