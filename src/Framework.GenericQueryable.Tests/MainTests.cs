using Framework.GenericQueryable.Default;

namespace Framework.GenericQueryable.Tests
{
    public class MainTests
    {
        [Fact]
        public async Task DefaultGenericQueryable_InvokeToListAsync_MethodInvoked()
        {
            // Arrange
            var baseSource = new[] { 1, 2, 3 };
            var qSource = baseSource.AsDefaultGenericQueryable();

            // Act
            var result = await qSource.GenericToListAsync();

            //Assert
            result.Should().BeEquivalentTo(baseSource);
        }

        [Fact]
        public void DefaultGenericQueryable_InvokeToList_MethodInvoked()
        {
            // Arrange
            var baseSource = new[] { 1, 2, 3 };
            var qSource = baseSource.AsDefaultGenericQueryable();

            // Act
            var result = qSource.ToList();

            //Assert
            result.Should().BeEquivalentTo(baseSource);
        }

        [Fact]
        public async Task DefaultGenericQueryable_InvokeSingleOrDefaultAsync_CollisionResolved()
        {
            // Arrange
            var baseSource = 1;
            var qSource = new[] { baseSource }.AsDefaultGenericQueryable();

            // Act
            var result = await qSource.GenericSingleOrDefaultAsync(_ => true);

            //Assert
            result.Should().Be(baseSource);
        }

        [Fact]
        public async Task DefaultGenericQueryable_InvokeFetch_FetchIgnored()
        {
            // Arrange
            var baseSource = "abc";
            var qSource = new[] { baseSource }.AsDefaultGenericQueryable();

            // Act
            var result = await qSource.WithFetch(nameof(string.Length))
                                      .GenericSingleOrDefaultAsync(_ => true);

            //Assert
            result.Should().Be(baseSource);
        }
    }
}
