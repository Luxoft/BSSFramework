using Framework.GenericQueryable.Default;

namespace Framework.GenericQueryable.Tests
{
    public class MainTests
    {
        [Fact]
        public async Task DefaultGenericQueryable_AsyncEvaluated_ResultCorrected()
        {
            // Arrange
            var baseSource = new[] { 1, 2, 3 };
            var qSource = baseSource.AsDefaultGenericQueryable();

            // Act
            var result = await qSource.ToGenericListAsync();

            //Assert
            result.Should().BeEquivalentTo(baseSource);
        }

        [Fact]
        public void DefaultGenericQueryable_SyncEvaluated_ResultCorrected()
        {
            // Arrange
            var baseSource = new[] { 1, 2, 3 };
            var qSource = baseSource.AsDefaultGenericQueryable();

            // Act
            var result = qSource.ToList();

            //Assert
            result.Should().BeEquivalentTo(baseSource);
        }
    }
}
