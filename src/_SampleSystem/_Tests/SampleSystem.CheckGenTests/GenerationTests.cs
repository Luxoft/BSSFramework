﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

using FileInfo = Framework.DomainDriven.Generation.FileInfo;

namespace SampleSystem.CheckGenTests;

[TestClass]
public class GenerationTests
{
    [TestMethod]
    public void AuthorizationGeneration_CheckUpdatedFiles_ShouldBeNothing()
    {
        // Arrange
        var target = new Framework.Authorization.TestGenerate.ServerGenerators();

        // Act
        var generatedFiles = target.GenerateMain().ToList();

        // Assert
        ShouldBeNoNewAndModifiedFiles(generatedFiles);
    }

    [TestMethod]
    public void ConfigurationGeneration_CheckUpdatedFiles_ShouldBeNothing()
    {
        // Arrange
        var target = new Framework.Configuration.TestGenerate.ServerGenerators();

        // Act
        var generatedFiles = target.GenerateMain().ToList();

        // Assert
        ShouldBeNoNewAndModifiedFiles(generatedFiles);
    }

    [TestMethod]
    public void SampleSystemServerGeneration_CheckUpdatedFiles_ShouldBeNothing()
    {
        // Arrange
        var target = new CodeGenerate.ServerGenerators();

        // Act
        var generatedFiles = target.GenerateMain().ToList();

        // Assert
        ShouldBeNoNewAndModifiedFiles(generatedFiles);
    }

    private static void ShouldBeNoNewAndModifiedFiles(IReadOnlyCollection<FileInfo> generatedFiles)
    {
        var changedFiles = generatedFiles.Where(x => x.FileState == FileInfo.State.Modified).ToList();
        var newFiles = generatedFiles.Where(x => x.FileState == FileInfo.State.New).ToList();

        if (changedFiles.Any() || newFiles.Any())
        {
            Assert.Fail(
                $@"Repo do not have actual versions of autogenerated source code
New files are:
{GetAggregatedMessage(newFiles)}
Modified files are:
{GetAggregatedMessage(changedFiles)}");
        }
    }

    private static string GetAggregatedMessage(IReadOnlyCollection<FileInfo> source)
        => source.Any()
               ? source.Select(x => "\t" + x.AbsolutePath).Aggregate((total, next) => total + Environment.NewLine + next)
               : string.Empty;
}
