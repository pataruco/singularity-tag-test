using Application.Api.Queries;
using Application.Core.Services.Interfaces;
using Application.Domain.Entities;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Snapshooter.NUnit;

namespace Application.Api.IntegrationTest;

public class Functional
{
  private Mock<IUserService> _mockUserService;
  private IRequestExecutorBuilder _requestExecutor;

  [SetUp]
  public void Setup()
  {
    _mockUserService = new Mock<IUserService>();
    _requestExecutor = new ServiceCollection()
        .AddSingleton(_mockUserService.Object)
        .AddGraphQLServer()
        .AddQueryType<Query>()
        .AddMutationType<Mutation>();
  }

  [Test]
  public async Task GetUser()
  {
    // arrange
    var mockUser = new User()
    {
      Name = "Martin"
    };

    _mockUserService.Setup(mock => mock.GetUser(It.IsAny<int>())).Returns(mockUser);

    // act
    var result = await _requestExecutor.ExecuteRequestAsync("{ user(id : 1) { name, id } }");

    // assert
    result.ToJson().MatchSnapshot();
    _mockUserService.Verify(mock => mock.GetUser(It.Is<int>(val => val == 1)), Times.Once);
  }
  [Test]
  public async Task GetUsers()
  {
    var users = new List<User>()
        {
            new User()
            {
                Name = "Martin"
            },
            new User()
            {
                Name = "Pedro"
            }

        }.AsQueryable();

    _mockUserService.Setup(mock => mock.GetUsers()).Returns(users);

    // act
    var result = await _requestExecutor.ExecuteRequestAsync("{ users { name, id } }");

    // assert
    result.ToJson().MatchSnapshot();
    _mockUserService.Verify(mock => mock.GetUsers(), Times.Once);
  }

  [Test]
  public async Task CreateUser()
  {
    // arrange
    var mockUser = new User()
    {
      Name = "Martin"
    };
    _mockUserService.Setup(mock => mock.CreateUser(It.IsAny<UserDTO>()).Result).Returns(mockUser);

    // act
    var result = await _requestExecutor.ExecuteRequestAsync("mutation { createUser(user: { name: \"Martin\" } ) { name, id } }");

    // assert
    result.ToJson().MatchSnapshot();
    _mockUserService.Verify(mock => mock.CreateUser(It.Is<UserDTO>(val => val.Name == "Martin")), Times.Once);
  }

  [Test]
  public async Task UpdateUser()
  {
    // arrange
    var mockUser = new User()
    {
      Id = 1,
      Name = "Martin"
    };
    _mockUserService.Setup(mock => mock.UpdateUser(It.IsAny<int>(), It.IsAny<UserDTO>()).Result).Returns(mockUser);

    // act
    var result = await _requestExecutor.ExecuteRequestAsync("mutation { updateUser(id: 1, user: { name: \"Johnson\"} ) { name, id } }");

    // assert
    result.ToJson().MatchSnapshot();
    _mockUserService.Verify(mock => mock.UpdateUser(
        It.Is<int>(val => val == 1),
      It.Is<UserDTO>(val => val.Name == "Johnson")), Times.Once);
  }

  [Test]
  public async Task DeleteUser()
  {
    // arrange
    var userId = 1;
    _mockUserService.Setup(mock => mock.DeleteUser(It.IsAny<int>()).Result).Returns(userId);

    // act
    var result = await _requestExecutor.ExecuteRequestAsync("mutation { deleteUser(id: 1) }");

    // assert
    result.ToJson().MatchSnapshot();
    _mockUserService.Verify(mock => mock.DeleteUser(
        It.Is<int>(val => val == userId)), Times.Once);
  }
}
