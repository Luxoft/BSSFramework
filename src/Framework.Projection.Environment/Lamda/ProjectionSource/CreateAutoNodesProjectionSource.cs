using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda
{
    internal class CreateAutoNodesProjectionSource : IProjectionSource
    {
        private readonly IProjectionSource baseSource;

        private readonly ProjectionLambdaEnvironment environment;

        public CreateAutoNodesProjectionSource([NotNull] IProjectionSource baseSource, [NotNull] ProjectionLambdaEnvironment environment)
        {
            this.baseSource = baseSource ?? throw new ArgumentNullException(nameof(baseSource));
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public IEnumerable<IProjection> GetProjections()
        {
            var builders = this.baseSource.GetProjections().ToBuilders();

            foreach (var projectionBuilder in builders)
            {
                if (projectionBuilder.Role == ProjectionRole.Default)
                {
                    var rootAutoProjection = new AutoProjectionFactory(this.environment, projectionBuilder).Create();

                    var addProperties = rootAutoProjection.Properties.Where(prop => prop.Role == ProjectionPropertyRole.AutoNode).ToList();

                    projectionBuilder.Properties.AddRange(addProperties);
                }
            }

            return builders.GetAllProjections();
        }
    }
}
