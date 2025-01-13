using Subutai.Domain.Model;
using Subutai.Repository.SqlRepository.Repositories;
using Subutai.Repository.SqlRepository.Contexts;
using FluentAssertions;
using Moq;
using Microsoft.EntityFrameworkCore;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

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
            Name = "New Name",
            Reference = "new reference",
            DateStarted = DateTimeOffset.UtcNow,         
        };

        // Act
        var result = await _repository.UpdateAsync(updateEntity);

        // Assert
        using (new AssertionScope())
        {
        result.Should().NotBeNull();
        result.Id.Should().Be(existingEntity.Id);
        result.Name.Should().Be(updateEntity.Name);
        result.Reference.Should().Be(updateEntity.Reference);
        result.DateCompleted.Should().Be(updateEntity.DateCompleted);
        result.DateStarted.Should().Be(updateEntity.DateStarted);
        result.DepartmentId.Should().Be(updateEntity.DepartmentId);
        result.UpdatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        }
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowArgumentException_WhenEntityNotFound()
    {
    // Arrange
    var existingtEntity = new ProjectEntity { Id = 1, Description="first Name" };
    var nonExistingtEntity = new ProjectEntity{Id = 99, Description ="Second name"};
    _context.Projects.Add(existingtEntity);
    await _context.SaveChangesAsync();

    // Act
    var act = async () => await _repository.UpdateAsync(nonExistingtEntity);

    // Assert
    await act.Should().ThrowAsync<ArgumentException>().WithMessage("Entity not found");
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnProjectEntityCreatedTimeImmutable()
    {
        // Arrange
        var projectId = 1;
        var projectName = "Test Project";
        var projectDescription = "Test Description";
        var projectUpdateTime = DateTimeOffset.UtcNow;
        var projectCreatedTime = DateTimeOffset.UtcNow.AddDays(-1);
        var newEntity = new ProjectEntity()      
        {
            Id = projectId,
            Name = projectName,
            Description = projectDescription,
            UpdatedAt = projectUpdateTime,
            CreatedAt = projectCreatedTime,  

        };
        _context.Projects.Add(newEntity);
        await _context.SaveChangesAsync(); 
        var SecondEntity = new ProjectEntity()
        {   Id = projectId,
            Name = "Second project",
            Description = projectDescription,
            UpdatedAt = projectUpdateTime,
            CreatedAt = DateTimeOffset.UtcNow
        };

        // Act
        await _repository.UpdateAsync(SecondEntity);
        var result = await _context.Projects.FindAsync(projectId);

         // Assert
         using (new AssertionScope())
         {
        result.Should().NotBeNull();
        result!.CreatedAt.Should().Be(projectCreatedTime, because: "CreatedAt should remain unchanged after an update");
        result.Name.Should().Be(SecondEntity.Name);
        result.Description.Should().Be(SecondEntity.Description);
        result.UpdatedAt.Should().NotBe(projectUpdateTime);   
         }
    }
    #endregion
    #region delete method test
    
    [Fact]
    public async Task DeleteAsync_ShouldThrowArgumentException_WhenEntityNotFound()
    {

        // Arrange
        var firstEntity = new ProjectEntity(){ Id = 1, Name = "firstEntity"};
        var deleteEntity = new ProjectEntity(){ Id = 2, Name = "deletedEntiy"};

        _context.Projects.Add(firstEntity);
        await _context.SaveChangesAsync();

        // Act     
        var act = async() => await _repository.DeleteAsync(deleteEntity);

        // Assert
        using(new AssertionScope())
        {await act.Should().ThrowAsync<ArgumentException>().WithMessage("Entity is Not Found");}
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnProject_DeletedTimeControl()
    {

        // Arrange
        var entityID = 1;
        var entityName = "firstEntity";
        var deletedTime = DateTimeOffset.UtcNow;

        
        var firstEntity = new ProjectEntity(){ Id = entityID,Name = entityName};
        await _context.Projects.AddAsync(firstEntity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.DeleteAsync(firstEntity);

        // Assert
        using(new AssertionScope())
        {
            result.Should().NotBeNull();
            result.DeletedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }
    }
    #endregion
}