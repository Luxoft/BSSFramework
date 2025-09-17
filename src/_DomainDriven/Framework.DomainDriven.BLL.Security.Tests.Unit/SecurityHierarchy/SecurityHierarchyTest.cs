using CommonFramework;

using Framework.Core;
using Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;
using Framework.DomainDriven.Lock;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy;

/// <summary>
/// Summary description for UnitTest1
/// </summary>
[TestClass]
public class SecurityHierarchyTest
{
    [TestMethod]
    public void InitMockTest()
    {
        var context = new TestBllContext(new HierarchicalObjectBuilder().Build(0, 3, 2).GetAllElements(z => z.Children).ToList());


        var hierarchyObjects = context.HierarchyDomainDal.GetQueryable(LockRole.None).ToList();

        Assert.AreEqual(15, hierarchyObjects.Count);

        this.TestCorrectState(context);

    }

    [TestMethod]
    public void RemoveTest()
    {
        var values = new HierarchicalObjectBuilder().Build(0, 5, 3).GetAllElements(z => z.Children).ToList();
        var context = new TestBllContext(values);

        var dal = context.HierarchyDomainDal;
        var removing = dal.GetQueryable(LockRole.None).Take(2);

        foreach (var hierarchyObject in removing)
        {
            dal.Remove(hierarchyObject);
        }

        context.SyncHierarchyService.Sync(new HierarchyObject[0], removing.ToList());

        context.Flush();
        this.TestCorrectState(context);
    }

    [TestMethod]
    public void ChangeParentTest()
    {
        var root = new HierarchyObject() { Name = "root" };
        var child1 = new HierarchyObject(root) { Name = "1" };
        var child2 = new HierarchyObject(child1) { Name = "2" };
        var child3 = new HierarchyObject(child2) { Name = "3" };
        var child4 = new HierarchyObject(child3) { Name = "4" };
        var child5 = new HierarchyObject(child4) { Name = "5" };

        var values = new List<HierarchyObject>
                     {
                             root, child1, child2, child3, child4, child5
                     };

        var context = new TestBllContext(values);

        child3.Parent = child1;
        child1.AddChild(child3);
        child2.RemoveChild(child3);

        context.SyncHierarchyService.Sync(new[] { child3 }, new HierarchyObject[0]);

        context.Flush();

        this.TestCorrectState(context);
    }

    [TestMethod]
    public void AddTest()
    {
        var values = new HierarchicalObjectBuilder().Build(0, 5, 3).GetAllElements(z => z.Children).ToList();

        var context = new TestBllContext(values);

        var parent = values.Skip(3).Take(1).First();
        var newObject = new HierarchyObject(parent);

        context.HierarchyDomainDal.Save(newObject);

        context.SyncHierarchyService.Sync(new[] { newObject }, new HierarchyObject[0]);

        context.Flush();

        this.TestCorrectState(context);
    }

    private void TestCorrectState(TestBllContext context)
    {
        var hierarchyObjects = context.HierarchyDomainDal.GetQueryable(LockRole.None).ToList();
        var allLinks = context.DomainAncestorLinkDal.GetQueryable(LockRole.None).ToList();

        var dictionary = hierarchyObjects.ToDictionary(z => z.Id);

        //all info in links are actual
        foreach (var link in allLinks)
        {
            if (!dictionary.ContainsKey(link.Child.Id))
            {
                Assert.Fail($"{link.Child} not in db");
            }
            if (!dictionary.ContainsKey(link.Ancestor.Id))
            {
                Assert.Fail($"{link.Ancestor} not in db");
            }


            var child = dictionary[link.Child.Id];

            if (child.Parent == null)
            {
                continue;
            }
            var allParents = child.GetAllElements(z => z.Parent).ToList();
            var isFind = allParents.Any(z => z.Id == link.Ancestor.Id);
            Assert.IsTrue(isFind, $"Can't find ancestor{link.Id} from {link.Ancestor.Id}");
        }


        if (allLinks.GroupBy(z => new { childID = z.Child.Id, ancestorId = z.Ancestor.Id }).Any(z => z.Count() > 1))
        {
            Assert.Fail("Links has duplicate values");
        }

        foreach (var hierarchyObject in hierarchyObjects)
        {
            var parents = hierarchyObject
                          .GetAllElements(z => z.Parent)
                          .Select(z => new { ChildId = hierarchyObject.Id, AncestorId = z.Id })
                          .ToList();

            foreach (var parent in parents)
            {
                Assert.IsTrue(allLinks.Any(q => q.Child.Id == parent.ChildId && q.Ancestor.Id == parent.AncestorId), "all links can not contains links:({0},{1})", parent.ChildId, parent.AncestorId);
            }

            var children = hierarchyObject
                           .GetAllElements(z => z.Children)
                           .Select(z => new { AncestorId = hierarchyObject.Id, ChildId = z.Id , Child = z})
                           .ToList();

            foreach (var child in children)
            {
                Assert.IsTrue(allLinks.Any(q => q.Child.Id == child.ChildId && q.Ancestor.Id == child.AncestorId), "all links can not contains links:({0},{1})", child.ChildId, child.AncestorId);
            }
        }


    }

}

public class HierarchicalObjectBuilder
{
    public HierarchyObject Build(int cur, int max, int childrenCount, HierarchyObject master = null)
    {
        master = master ?? new HierarchyObject();
        if (cur == max)
        {
            return null;
        }

        var children = Enumerable.Repeat(1, childrenCount)
                                 .Select((index, _) => new HierarchyObject() { Name = new string('_', cur) + cur + index.ToString() })
                                 .Select(z => z.Self(q => q.Parent = master)).ToList();

        children.Foreach(master.AddChild);

        foreach (var hierarchicalObject in children)
        {
            this.Build(cur + 1, max, childrenCount, hierarchicalObject);
        }
        return master;
    }
}
