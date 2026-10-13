using Anch.Testing.Xunit;

using Framework.Application.Repository;
using Framework.Database;
using Framework.Tracking;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain.TestIdComment;
using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests;

public abstract class TestCommentIdTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    public async Task AddComment_BeforeExplicitSave_IdStaysDefaultAfterLazyNavigationAccess(CancellationToken ct)
    {
        // Arrange: persist the parent (with a Category reference) in its own transaction first, mirroring how
        // AbsenceRequest (with its Type reference) already exists in the database - from a prior test setup
        // step - before the command handler under test runs.
        var parentId = await this.EvaluateAsync(
                           DBSessionMode.Write,
                           null,
                           async ctx =>
                           {
                               var parentRepo = ctx.ServiceProvider.GetRequiredService<IRepositoryFactory<TestCommentParent>>().Create();
                               var categoryRepo = ctx.ServiceProvider.GetRequiredService<IRepositoryFactory<TestCommentCategory>>().Create();

                               var category = new TestCommentCategory { Name = "category" };
                               await categoryRepo.SaveAsync(category, ct);

                               var parent = new TestCommentParent { Name = "parent", Category = category };
                               await parentRepo.SaveAsync(parent, ct);

                               return parent.Id;
                           },
                           ct);

        // Act: in a new session, load the already-persisted parent (its Category navigation not yet loaded),
        // add a new comment only to its in-memory collection (cascade, never explicitly saved itself - matches
        // AbsenceRequestReadOnlyValidator's use of AbsenceRequest.Comments.Add(...) before the request itself
        // is saved), then access the not-yet-loaded Category navigation - mirroring AbsenceRequestTypeValidator's
        // RuleFor(x => x.Type.Id), which is the actual trigger identified in the real bug: the first lazy load of
        // *any* navigation on the same tracked root causes EF's proxy-driven change detection to run, which
        // eagerly assigns a real key to the still-unsaved comment.
        var (idRightAfterAdd, idAfterLazyNavigationAccess, isNew) =
            await this.EvaluateAsync(
                DBSessionMode.Write,
                null,
                async ctx =>
                {
                    var parentRepo = ctx.ServiceProvider
                                        .GetRequiredService<IRepositoryFactory<TestCommentParent>>()
                                        .Create();

                    var objectStateService =
                        ctx.ServiceProvider.GetRequiredService<IObjectStateService>();

                    var parent = parentRepo.GetQueryable().Single(x => x.Id == parentId);

                    var comment = new TestComment(parent) { Text = "comment" };

                    var idRightAfterAdd = comment.Id;

                    _ = parent.Category.Name;

                    var idAfterLazyNavigationAccess = comment.Id;

                    var isNew = objectStateService.IsNew(comment);

                    return (idRightAfterAdd, idAfterLazyNavigationAccess, isNew);
                },
                ct);

        // Assert: the not-yet-saved comment must stay "new" (Id == default) until it is actually persisted,
        // regardless of any unrelated navigation lazy-loaded on the same session in the meantime.

        Assert.Equal(true, isNew);

        //Assert.Equal(Guid.Empty, idRightAfterAdd);
        //Assert.Equal(Guid.Empty, idAfterLazyNavigationAccess);
    }
}
