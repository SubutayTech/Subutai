using Subutai.Domain.Model;
using Subutai.Repository.SqlRepository.Repositories;
using Subutai.Repository.SqlRepository.Contexts;
using FluentAssertions;
using Moq;
using Microsoft.EntityFrameworkCore;
using FluentAssertions.Execution;

namespace Subutai.Repository.SqlRepository.Tests;
public sealed class ProjectEntityRepositoryTests
{
    readonly ISubutaiContext _context;
    readonly ProjectEntityRepository _repository;

    public ProjectEntityRepositoryTests()
    {
        var context = ContextGenerator.Generate();
        _context = context;
        _repository = new ProjectEntityRepository(_context);
    }

    #region add method test 
    [Fact]
    public async Task AddAsync_ShouldAddProjectEntity()
    {
        // Arrange
        var projectEntity = new ProjectEntity();

        // Act
        await _repository.AddAsync(projectEntity);

        // Assert
        _context.Projects.Should().Contain(projectEntity);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnProjectEntity()
    {
        // Arrange
        var projectId = 1;
        var projectName = "Test Project";
        var projectDescription = "Test Description";
        var projectEntity = new ProjectEntity()
        {
            Id = projectId,
            Name = projectName,
            Description = projectDescription
        };

        // Act
        var result = await _repository.AddAsync(projectEntity);

        // Assert
        using (new AssertionScope())
        {
            result.Should().Be(projectEntity);
            result.Id.Should().Be(projectId);
            result.Name.Should().Be(projectName);
            result.Description.Should().Be(projectDescription);
        }
    }

    [Fact]
    public async Task AddAsync_ShouldThrowArgumentNullException_WhenEntityIsNull()
    {
        // Act
        var act = async () => await _repository.AddAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AddAsync_ShouldHandleException()
    {
        // Arrange
        var mockContext = new Mock<ISubutaiContext>();
        var mockDbSet = new Mock<DbSet<ProjectEntity>>();

        mockContext.Setup(x => x.Projects).Returns(mockDbSet.Object);
        mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var repository = new ProjectEntityRepository(mockContext.Object);
        var projectEntity = new ProjectEntity();

        // Act
        var act = async () => await repository.AddAsync(projectEntity);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    #endregion

    #region update method test
    [Fact]
    public async Task UpdateAsync_ShouldUpdateProjectEntity()
    {
        // Arrange
        var existingEntity = new ProjectEntity
        {
            Id = 1,
            Name = "Old Name"
        };

        _context.Projects.Add(existingEntity);
        await _context.SaveChangesAsync();

        var updateEntity = new ProjectEntity
        {
            Id = existingEntity.Id,
            Name = "New Name"
        };

        // Act
        var result = await _repository.UpdateAsync(updateEntity);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(existingEntity.Id);
        result.Name.Should().Be(updateEntity.Name);
    }

    #endregion
}