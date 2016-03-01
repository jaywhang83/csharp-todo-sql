using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  public class ToDoTest : IDisposable
  {
    public ToDoTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
        int result = Task.GetAll().Count;

        Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_overrideTrueForSameDescription()
    {
      DateTime testDate = new DateTime(2016, 3, 1);
      Task firstTask = new Task("Mow the lawn", 1, testDate);
      Task secondTask = new Task("Mow the lawn", 1, testDate);

      Assert.Equal(firstTask, secondTask);
    }

    [Fact]
    public void Test_Save()
    {
      List<Task> initialTasks = Task.GetAll();
      Console.WriteLine("Initial Tasks:");
      foreach (Task task in initialTasks)
      {
        Console.WriteLine("ID: {0}, description: {1}", task.GetId(), task.GetDescription());
      }
      DateTime testDate = new DateTime(2016, 3, 1);
      Task testTask = new Task("Mow the lawn", 1, testDate);
      testTask.Save();

      List<Task> result = Task.GetAll();
      List<Task> testList = new List<Task>{testTask};
      Console.WriteLine("result tasks:");
      foreach (Task task in result)
      {
        Console.WriteLine("ID: {0}, description: {1}", task.GetId(), task.GetDescription());
      }

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      //Arrange
      DateTime testDate = new DateTime(2016, 3, 1);
      Task testTask = new Task("Mow the lawn", 1, testDate);

      //Act
      testTask.Save();

      List<Task> testSaveTasks = Task.GetAll();
      Console.WriteLine("testSaveTasks tasks:");
      foreach (Task task in testSaveTasks)
      {
        Console.WriteLine("ID: {0}, description: {1}", task.GetId(), task.GetDescription());
      }

      Task savedTask = Task.GetAll()[0];

      int result = savedTask.GetId();
      int testId = testTask.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsTaskInDatabase()
    {
      //Arrange
      DateTime testDate = new DateTime(2016, 3, 1); 
      Task testTask = new Task("Mow the lawn", 1, testDate);
      testTask.Save();

      //Act
      Task foundTask = Task.Find(testTask.GetId());

      //Assert
      Assert.Equal(testTask, foundTask);
    }

    public void Dispose()
    {
      Task.DeleteAll();
    }
  }
}
