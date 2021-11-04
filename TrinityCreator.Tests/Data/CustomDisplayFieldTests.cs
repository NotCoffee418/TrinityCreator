using TrinityCreator.Shared.Data;
using Xunit;

namespace TrinityCreator.Tests.Data
{
    public class CustomDisplayFieldTests
    {
        [Theory]
        [InlineData("Item.CustomInt.SomeName")]
        [InlineData("Quest.CustomInt.SomeName")]
        [InlineData("Creature.CustomInt.SomeName")]
        [InlineData("Item.CustomFloat.SomeName")]
        [InlineData("Item.CustomText.SomeName")]
        [InlineData("Item.CustomText.SomeName with spaces")]
        [InlineData("Item.CustomText.SomeName.with.more.dots")]
        public void Create_ValidateCustomFieldName_SuccessfulCreate(string fullCustomFieldName)
        {
            CustomDisplayField result = CustomDisplayField.Create(
                "_", "_", fullCustomFieldName);
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("Shoe.CustomInt.SomeName")] // invalid creator
        [InlineData("quest.CustomInt.SomeName")] // case
        [InlineData("Creature.CustomBool.SomeName")] // Invalid type
        [InlineData("Item.Float.SomeName")] // without the word custom
        [InlineData("Item.CustomText")] // no display name
        public void Create_ValidateCustomFieldName_ExpectFail(string fullCustomFieldName)
        {
            CustomDisplayField result = CustomDisplayField.Create(
                "_", "_", fullCustomFieldName);
            Assert.Null(result);
        }
    }
}
