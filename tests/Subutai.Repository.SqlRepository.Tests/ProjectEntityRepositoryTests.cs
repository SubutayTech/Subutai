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
        var projectEntity = new ProjectEntity();

        // Act
        await _repository.UpdateAsync(projectEntity);

        // Assert
        _context.Projects.Should().Contain(projectEntity);

    }
    [Fact]
    public async Task UpdateAsync_ShouldThrowArgumentException_WhenEntityNotFound()
{
    // Arrange
    var nonExistentEntity = new ProjectEntity { Id = 999 };

    // Act
    var act = async () => await _repository.UpdateAsync(nonExistentEntity);

    // Assert
    await act.Should().ThrowAsync<ArgumentException>().WithMessage("Entity not found");
}
    [Fact]
    public async Task UpdateAsync_ShouldReturnProjectEntity()
    {
        
        // Arrange
        var projectId = 1;
        var projectName = "Test Project";
        var projectDescription = "Test Description";
        var projectUpdateTime = DateTimeOffset.UtcNow;
        var projectCreatedTime = DateTimeOffset.UtcNow.AddDays(-1);
        var projectEntity = new ProjectEntity()
        {
            Id = projectId,
            Name = projectName,
            Description = projectDescription,
            UpdatedAt = projectUpdateTime,
            CreatedAt = projectCreatedTime,
        };

        // Act
        var result = await _repository.UpdateAsync(projectEntity);

        // Assert
        using (new AssertionScope())
        {
            result.Should().Be(projectEntity);
            result.Id.Should().Be(projectId);
            result.Name.Should().Be(projectName);
            result.Description.Should().Be(projectDescription);
            result.UpdatedAt.Should().Be(projectUpdateTime);
            result.CreatedAt.Should().Be(projectCreatedTime);
        }
    }
    [Fact]
    public async Task UpdateAsync_ShouldThrowArgumentNullException_WhenEntityIsNull()
    {
        // Act
        var act = async () => await _repository.UpdateAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("entity");
    }
     [Fact]
    public async Task UpdateAsync_ShouldHandleException()
    {
        // Arrange
        var mockContext = new Mock<ISubutaiContext>();
        var mockDbSet = new Mock<DbSet<ProjectEntity>>();

        mockContext.Setup(x => x.Projects).Returns(mockDbSet.Object);
        mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB Save Failed"));

        var repository = new ProjectEntityRepository(mockContext.Object);
        var projectEntity = new ProjectEntity();

        // Act
        var act = async () => await repository.UpdateAsync(projectEntity);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Update Failed");
    }
    #endregion
}